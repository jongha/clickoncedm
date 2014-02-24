using ClickOnceDMLib.Path;
using ClickOnceDMLib.Structs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;

namespace ClickOnceDMLib.Process
{
    public class TicketProcess
    {
        private Object lockObj = new Object();

        public List<Ticket> GetTickets()
        {
            lock (lockObj)
            {
                IOrderedEnumerable<string> files = from file in Directory.GetFiles(PathInfo.Ticket)
                                                   where DateTime.Compare(DateTime.Now.AddSeconds(-10), new FileInfo(file).LastWriteTime) > 0
                                                   orderby file ascending
                                                   select file;

                List<Ticket> tickets = new List<Ticket>();
                foreach (string file in files)
                {
                    Ticket ticket = null;
                    using (FileStream stream = new FileStream(PathInfo.CombinePath(PathInfo.Ticket, file), FileMode.Open))
                    {
                        DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(Ticket));
                        ticket = (Ticket)serializer.ReadObject(stream);
                    }

                    if (ticket != null)
                    {
                        tickets.Add(ticket);
                    }
                }

                return tickets;
            }
        }

        public bool EmptyTicket()
        {
            try
            {
                foreach (Ticket ticket in GetTickets())
                {
                    lock (lockObj)
                    {
                        File.Delete(System.IO.Path.Combine(PathInfo.Ticket, ticket.FileName));
                    }
                }
                return true;
            }
            catch (Exception e)
            {
                LogProcess.Error(e.Message);
            }

            return false;
        }

        public bool RemoveTicket(Ticket ticket)
        {
            lock (lockObj)
            {
                try
                {
                    File.Move(
                        System.IO.Path.Combine(PathInfo.Ticket, ticket.FileName),
                        System.IO.Path.Combine(PathInfo.Trash, ticket.FileName)
                        );
                    return true;
                }
                catch (Exception e)
                {
                    LogProcess.Error(e.Message);
                }

                return false;
            }
        }

        public Ticket SaveTicket(Ticket ticket)
        {
            lock (lockObj)
            {
                ticket.FileName = DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + Guid.NewGuid().ToString("N").Substring(0, 16) + ".ticket";

                try
                {
                    using (FileStream stream = new FileStream(System.IO.Path.Combine(PathInfo.Ticket, ticket.FileName), FileMode.CreateNew))
                    {
                        DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(Ticket));

                        ticket.Source = ticket.Source.EncryptedSource;
                        serializer.WriteObject(stream, ticket);
                    }

                    return ticket;
                }
                catch (Exception e)
                {
                    LogProcess.Error(e.Message);

                    return null;
                }
            }
        }
    }
}
