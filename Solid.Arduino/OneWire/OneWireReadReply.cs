namespace Solid.Arduino.OneWire
{
    public struct OneWireReadReply
    {
        public int Bus { get; set; }
        public byte CorrelationByte1 { get; set; }
        public byte CorrelationByte2 { get; set; }
    }
}