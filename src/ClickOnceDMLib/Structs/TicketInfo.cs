using ClickOnceDMLib.Path;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;

namespace ClickOnceDMLib.Structs
{
    public class Ticket
    {
        public string Name;
        public SourceInfo Source
        {
            get
            {
                SourceInfo sourceInfo = new SourceInfo();
                using (FileStream stream = new FileStream(PathInfo.GetTicketFile(this.Name), FileMode.Open))
                {
                    DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(SourceInfo));
                    sourceInfo = (SourceInfo)serializer.ReadObject(stream);
                }

                return sourceInfo;
            }
        }
    }

    public class TicketInfo
    {
        public List<SourceInfo> GetSources()
        {
            List<Ticket> tickets = new List<Ticket>();
            foreach (string file in Directory.GetFiles(PathInfo.Ticket))
            {
                tickets.Add(new Ticket() { Name = file });
            }

            var l = from fi in tickets
                    orderby fi.Name ascending
                    select fi.Source.DecryptedObject();

            return l.ToList<SourceInfo>();
        }

        public bool SaveTicket(string fileName, SourceInfo sourceInfo)
        {
            try
            {
                using (FileStream stream = new FileStream(System.IO.Path.Combine(PathInfo.Ticket, fileName), FileMode.CreateNew))
                {
                    DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(SourceInfo));
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
