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
        public List<Ticket> GetTickets()
        {
            IOrderedEnumerable<string> files = from file in Directory.GetFiles(PathInfo.Ticket)
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
        
        public bool EmptyTicket()
        {
            lock (typeof(TicketProcess))
            {
                try
                {
                    foreach (Ticket ticket in GetTickets())
                    {
                        File.Delete(System.IO.Path.Combine(PathInfo.Ticket, ticket.FileName));
                    }
                    return true;
                }
                catch { }

                return false;
            }
        }

        public bool RemoveTicket(Ticket ticket)
        {
            lock (typeof(TicketProcess))
            {
                try
                {
                    File.Move(
                        System.IO.Path.Combine(PathInfo.Ticket, ticket.FileName),
                        System.IO.Path.Combine(PathInfo.Trash, ticket.FileName)
                        );
                    return true;
                }
                catch { }

                return false;
            }
        }

        public Ticket SaveTicket(Ticket ticket)
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
            catch
            {
                return null;
            }
        }
    }
}
