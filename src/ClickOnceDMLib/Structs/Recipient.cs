using ClickOnceDMLib.Process;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;

namespace ClickOnceDMLib.Structs
{
    [DataContract]
    public class Recipient
    {
        [DataMember]
        public string Name;

        [DataMember]
        public string Address;

        public Recipient(string name, string address)
        {
            if (Regex.IsMatch(address, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$"))
            {

                this.Name = name;
                this.Address = address;
            }
            else
            {
                Exception e = new FormatException();

                LogProcess.Error(e, address);

                throw e;
            }
        }
    }
}
