using System;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;

namespace ClickOnceDMLib.Structs
{
    [DataContract]
    public class Queue
    {
        private string name;
        private string email;

        [DataMember]
        public string Name
        {
            get
            {
                return this.name;
            }
            set
            {
                this.name = value;
            }
        }

        [DataMember]
        public string Email
        {
            get
            {
                return this.email;
            }
            set
            {
                // email address format valication
                if (Regex.IsMatch(value, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$"))
                {
                    this.email = value;
                }
                else
                {
                    throw new FormatException();
                }
            }
        }
    }
}
