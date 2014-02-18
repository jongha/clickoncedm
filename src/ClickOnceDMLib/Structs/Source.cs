using ClickOnceDMLib.Crypto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace ClickOnceDMLib.Structs
{
    public class Source
    {
        private const string encryptKey = "8d48c686d84d46fdb1bff9a79d935430";
        private string privider;
        private string value;
        private string connectionString;

        public Source DecryptedSource
        {
            get
            {
                this.privider = AES.Decrypt(privider, encryptKey);
                this.value = AES.Decrypt(value, encryptKey);
                this.connectionString = AES.Decrypt(connectionString, encryptKey);

                return this;
            }
        }

        public Source EncryptedSource
        {
            get
            {
                this.privider = AES.Encrypt(privider, encryptKey);
                this.value = AES.Encrypt(value, encryptKey);
                this.connectionString = AES.Encrypt(connectionString, encryptKey);

                return this;
            }
        }

        public string Provider
        {
            get
            {
                return this.privider;
            }
            set
            {
                this.privider = value;
            }
        }

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