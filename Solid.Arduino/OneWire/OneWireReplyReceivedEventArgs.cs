using System;

namespace Solid.Arduino.OneWire
{
    public class OneWireReplyReceivedEventArgs : EventArgs
    {
        public OneWireReplyReceivedEventArgs(OneWireSearchReply searchReply)
        {
            SearchReply = searchReply;
        }

        public OneWireReplyReceivedEventArgs(OneWireReadReply readReply)
        {
            ReadReply = readReply;
        }

        public OneWireSearchReply SearchReply { get; private set; }

        public OneWireReadReply ReadReply { get; private set; }
    }
}