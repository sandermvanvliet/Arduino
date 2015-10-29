using System;

namespace Solid.Arduino
{
    public class OneWireReplyReceivedEventArgs: EventArgs
    {
        public OneWireReply Reply { get; set; }

        public OneWireReplyReceivedEventArgs(OneWireReply reply)
        {
            Reply = reply;
        }
    }
}