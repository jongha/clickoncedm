﻿using ClickOnceDMLib.Path;
using ClickOnceDMLib.Structs;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Text.RegularExpressions;

namespace ClickOnceDMLib.Process
{
    public class QueueProcess
    {
        private const int BLOCKCOUNT = 500;
        private Object lockObj = new Object();

        public void SaveQueue(Queue queue)
        {
            lock (lockObj)
            {
                if (queue.RecipientData.Count() > 0)
                {
                    string fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + Guid.NewGuid().ToString("N") + ".queue";

                    using (FileStream stream = new FileStream(System.IO.Path.Combine(PathInfo.Queue, fileName), FileMode.CreateNew))
                    {
                        DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(Queue));
                        serializer.WriteObject(stream, queue);
                    }
                }
            }
        }

        public bool RemoveQueue(string queueName)
        {
            lock (lockObj)
            {
                try
                {
                    File.Move(
                        System.IO.Path.Combine(PathInfo.Queue, queueName),
                        System.IO.Path.Combine(PathInfo.Trash, queueName)
                        );

                    return true;
                }
                catch { }

                return false;
            }
        }

        public void BuildQueueFromTicket(TicketProcess ticketProcess, int blobkCount)
        {
            if (blobkCount <= 0)
            {
                blobkCount = BLOCKCOUNT;
            }
            List<Ticket> tickets = ticketProcess.GetTickets();

            if (tickets.Count() == 0)
            {
                return;
            }


            Ticket ticket = tickets[0];
            Source source = ticket.Source;

            try
            {
                source = source.DecryptedSource;
                string value = source.Value;
                string connectionString = source.ConnectionString;

                if (source.Provider == "System.Data.SqlClient")
                {
                    Database db = new SqlDatabase(source.ConnectionString);
                    DataSet ds = db.ExecuteDataSet(CommandType.Text, source.Value);
                    if (ds != null && ds.Tables.Count > 0)
                    {
                        QueueProcess queueProcess = new QueueProcess();
                        List<Recipient> recipients = new List<Recipient>();

                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            string address = dr["address"].ToString();
                            string name = dr["name"].ToString();

                            try
                            {
                                recipients.Add(new Recipient(name, address));
                            }
                            catch (Exception e)
                            {
                                LogProcess.Error(e, address + ", " + name);
                                continue;
                            }

                            if (recipients.Count >= blobkCount)
                            {
                                queueProcess.SaveQueue(new Queue()
                                {
                                    RecipientData = recipients.ToArray(),
                                    TicketData = ticket
                                });
                                recipients = new List<Recipient>();
                            }
                        }


                        if (recipients.Count() > 0)
                        {
                            queueProcess.SaveQueue(new Queue()
                            {
                                RecipientData = recipients.ToArray(),
                                TicketData = ticket
                            });

                            recipients = new List<Recipient>();
                        }
                    }
                }
                else if (source.Provider == "System.String[]")
                {
                    QueueProcess queueProcess = new QueueProcess();
                    List<Recipient> recipients = new List<Recipient>();

                    foreach (string recipient in source.Value.Trim().Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        string address = recipient.Trim();
                        recipients.Add(new Recipient(address, address));

                        if (recipients.Count >= blobkCount)
                        {
                            queueProcess.SaveQueue(new Queue()
                            {
                                RecipientData = recipients.ToArray(),
                                TicketData = ticket
                            });
                            recipients = new List<Recipient>();
                        }
                    }

                    queueProcess.SaveQueue(new Queue()
                    {
                        RecipientData = recipients.ToArray(),
                        TicketData = ticket
                    });
                    recipients = new List<Recipient>();
                }
                else
                {
                    throw new NotSupportedException();
                }
            }
            catch (Exception e)
            {
                LogProcess.Error(e);
            }

            ticketProcess.RemoveTicket(ticket);
        }

        private IOrderedEnumerable<string> GetQueueFiles()
        {
            lock (lockObj)
            {
                return from file in Directory.GetFiles(PathInfo.Queue)
                       where DateTime.Compare(DateTime.Now.AddSeconds(-10), new FileInfo(file).LastWriteTime) > 0
                       orderby file ascending
                       select file;
            }
        }

        public Queue GetQueue()
        {
            IOrderedEnumerable<string> files = GetQueueFiles();

            Queue queue = new Queue();

            if (files.Count() > 0)
            {
                string file = files.First();
                FileInfo fileInfo = new FileInfo(file);
                string queueName = System.IO.Path.GetFileName(file);

                if (fileInfo.Length > 0)
                {
                    using (FileStream stream = new FileStream(PathInfo.CombinePath(PathInfo.Queue, queueName), FileMode.Open))
                    {
                        try
                        {
                            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(Queue));
                            queue = (Queue)serializer.ReadObject(stream);
                        }
                        catch (Exception e)
                        {
                            LogProcess.Error(e);
                        }
                    }
                }
                else
                {
                    queue = null;
                }

                RemoveQueue(queueName);
            }
            else
            {
                queue = null;
            }

            return queue;
        }
    }
}
