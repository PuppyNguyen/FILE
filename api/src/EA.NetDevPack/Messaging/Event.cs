using System;
using MediatR;

namespace EA.NetDevPack.Messaging
{
 
    public   class Event : Message, INotification
    {
        public DateTime Timestamp { get; private set; }

        public Event()
        {
            Timestamp = DateTime.Now;
        }
    }
}