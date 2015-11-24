namespace Solid.Arduino.OneWire
{
    public enum OneWireCommand : byte
    {
        SearchNormalRequest = 0x40,
        SearchNormalReply = 0x42,
        SearchAlarmsRequest = 0x44,
        SearchAlarmsReply = 0x45,
        ConfigRequest = 0x41
    }
}