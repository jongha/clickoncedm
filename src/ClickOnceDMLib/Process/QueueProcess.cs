using ClickOnceDMLib.Path;
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

        public void SaveQueue(Queue queue)
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

        public bool RemoveQueue(string queueName)
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

        public void BuildQueueFromTicket(TicketProcess ticketProcess)
        {
            lock (typeof(QueueProcess))
            {
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
                                    LogProcess.WriteLog(e);
                                    continue;
                                }

                                if (recipients.Count >= BLOCKCOUNT)
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
                    else
                    {
                        throw new NotSupportedException();
                    }
                }
                catch { }

                ticketProcess.RemoveTicket(ticket);
            }
        }

        public Queue GetQueue()
        {
            lock (typeof(QueueProcess))
            {
                IOrderedEnumerable<string> files = from file in Directory.GetFiles(PathInfo.Queue)
                                                   orderby file ascending
                                                   select file;

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
                            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(Queue));
                            queue = (Queue)serializer.ReadObject(stream);
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
}
