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

        public void SaveQueue(List<Queue> queue)
        {
            if (queue.Count > 0)
            {
                string fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + Guid.NewGuid().ToString("N") + ".queue";

                using (FileStream stream = new FileStream(System.IO.Path.Combine(PathInfo.Queue, fileName), FileMode.CreateNew))
                {
                    DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(List<Queue>));
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
            List<Ticket> tickets = ticketProcess.GetTickets();

            if (tickets.Count() == 0)
            {
                return;
            }


            Ticket ticket = tickets[0];
            Source source = ticket.Source;

            try
            {
                source = source.DecryptedObject();
                string value = source.Value;
                string connectionString = source.ConnectionString;

                if (source.Provider == "System.Data.SqlClient")
                {
                    Database db = new SqlDatabase(source.ConnectionString);
                    DataSet ds = db.ExecuteDataSet(CommandType.Text, source.Value);
                    if (ds != null && ds.Tables.Count > 0)
                    {
                        QueueProcess queueProcess = new QueueProcess();
                        List<Queue> queue = new List<Queue>();

                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            string email = dr["email"].ToString();
                            string name = dr["name"].ToString();

                            try
                            {
                                queue.Add(new Queue()
                                {
                                    Name = name,
                                    Email = email
                                });
                            }
                            catch(Exception e)
                            {
                                LogProcess.WriteLog(e);
                                continue;
                            }

                            if (queue.Count >= BLOCKCOUNT)
                            {
                                queueProcess.SaveQueue(queue);
                                queue = new List<Queue>();
                            }
                        }

                        if (queue.Count() > 0)
                        {
                            queueProcess.SaveQueue(queue);
                            queue = new List<Queue>();
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

        public List<Queue> GetQueue()
        {
            IOrderedEnumerable<string> files = from file in Directory.GetFiles(PathInfo.Queue)
                                               orderby file ascending
                                               select file;

            List<Queue> queues = new List<Queue>();

            if (files.Count() > 0)
            {
                string queueName = System.IO.Path.GetFileName(files.First());

                using (FileStream stream = new FileStream(PathInfo.CombinePath(PathInfo.Queue, queueName), FileMode.Open))
                {
                    DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(List<Queue>));
                    queues = (List<Queue>)serializer.ReadObject(stream);
                }

                RemoveQueue(queueName);
            }

            return queues.ToList<Queue>();
        }
    }
}
