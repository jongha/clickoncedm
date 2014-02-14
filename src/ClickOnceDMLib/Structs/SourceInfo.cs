using ClickOnceDMLib.Crypto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace ClickOnceDMLib.Structs
{
    [DataContract]
    public class SourceInfo
    {
        private const string encryptKey = "8d48c686d84d46fdb1bff9a79d935430";
        private string source;
        private string value;
        private string connectionString;

        public SourceInfo EncryptedObject()
        {
            this.source = AES.Encrypt(source, encryptKey);
            this.value = AES.Encrypt(value, encryptKey);
            this.connectionString = AES.Encrypt(connectionString, encryptKey);

            return this;
        }

        public SourceInfo DecryptedObject()
        {
            this.source = AES.Decrypt(source, encryptKey);
            this.value = AES.Decrypt(value, encryptKey);
            this.connectionString = AES.Decrypt(connectionString, encryptKey);

            return this;
        }

        [DataMember]
        public string Source
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

        [DataMember]
        public string Value
        {
            get
            {
                return this.value;
            }
            set
            {
                this.value = value;
            }
        }

        [DataMember]
        public string ConnectionString
        {
            get
            {
                return this.connectionString;
            }
            set
            {
                this.connectionString = value;
            }
        }
    }
}