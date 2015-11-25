using System;

namespace Solid.Arduino.OneWire
{
    public class OneWireReplyReceivedEventArgs : EventArgs
    {
        public OneWireReplyReceivedEventArgs(OneWireSearchReply searchReply)
        {
            SearchReply = searchReply;
            Type = OneWireReplyType.SearchReply;
        }

        public OneWireReplyReceivedEventArgs(OneWireReadReply readReply)
        {
            ReadReply = readReply;
            Type = OneWireReplyType.ReadReply;
        }

        public OneWireReplyType Type { get; private set; }

        public OneWireSearchReply SearchReply { get; private set; }

        public OneWireReadReply ReadReply { get; private set; }
    }
}