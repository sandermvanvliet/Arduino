namespace Solid.Arduino
{
    public struct OneWireReply
    {
        public byte[] Data { get; set; }
        public byte Command { get; set; }
        public byte SearchReply { get; set; }
        public byte Bus { get; set; }
    }
}