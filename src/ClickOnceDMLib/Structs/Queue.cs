using System;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;

namespace ClickOnceDMLib.Structs
{
    [DataContract]
    public class Queue
    {
        private Recipient[] recipient;
        private Ticket ticket;

        [DataMember]
        public Ticket TicketData
        {
            get
            {
                return this.ticket;
            }
            set
            {
                Ticket ticket = value;

                this.ticket = new Ticket()
                {
                    SenderName = ticket.SenderName,
                    SenderAddress = ticket.SenderAddress,
                    Subject = ticket.Subject,
                    Body = ticket.Body
                };
            }
        }

        [DataMember]
        public Recipient[] RecipientData
        {
            get
            {
                return this.recipient;
            }
            set
            {
                this.recipient = value;
            }
        }
    }
}
