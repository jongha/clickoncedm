using ClickOnceDMLib.Path;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

namespace ClickOnceDMLib.Structs
{
    [DataContract]
    public class Ticket
    {
        [DataMember]
        public string FileName;

        [DataMember]
        public string SenderName;

        [DataMember]
        public string SenderAddress;

        [DataMember]
        public string Subject;

        [DataMember]
        public string Body;

        private Source source;

        public Source DecryptedSource
        {
            get
            {
                return this.source.DecryptedSource;
            }
        }

        public Source EncryptedSource
        {
            get
            {
                return this.source.EncryptedSource;
            }
        }

        [DataMember]
        public Source Source
        {
            get
            {
                return this.source;
            }
            set
            {
                this.source = value;
            }
        }
    }
}
