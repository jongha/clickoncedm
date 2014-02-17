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
                tickets.Add(new Ticket() { Name = System.IO.Path.GetFileName(file) });
            }

            return tickets;
        }

        public List<Source> GetSources()
        {
            var l = from fi in GetTickets()
                    orderby fi.Name ascending
                    select fi.Source.DecryptedObject();

            return l.ToList<Source>();
        }

        public bool EmptyTicket()
        {
            try
            {
                foreach (Ticket ticket in GetTickets())
                {
                    File.Delete(System.IO.Path.Combine(PathInfo.Ticket, ticket.Name));
                }
                return true;
            }
            catch { }

            return false;
        }

        public bool RemoveTicket(Ticket ticket)
        {
            try
            {
                File.Move(
                    System.IO.Path.Combine(PathInfo.Ticket, ticket.Name),
                    System.IO.Path.Combine(PathInfo.Trash, ticket.Name)
                    );
                return true;
            }
            catch { }

            return false;
        }

        public bool SaveTicket(Source sourceInfo, out string fileName)
        {
            fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + Guid.NewGuid().ToString("N").Substring(0, 16) + ".ticket";

            try
            {
                using (FileStream stream = new FileStream(System.IO.Path.Combine(PathInfo.Ticket, fileName), FileMode.CreateNew))
                {
                    DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(Source));
                    serializer.WriteObject(stream, sourceInfo.EncryptedObject());
                }

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
