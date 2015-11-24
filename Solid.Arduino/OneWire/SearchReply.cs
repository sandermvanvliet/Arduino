namespace Solid.Arduino.OneWire
{
    /// <summary>
    /// SearchReply enumeration
    /// </summary>
    public enum SearchReply : byte
    {
        /// <summary>
        /// Perform a search for all sensors on the bus
        /// </summary>
        Normal = 0x42,
        /// <summary>
        /// Perform a search for sensors on the bus that have an alarm set
        /// </summary>
        Alarms = 0x45
    }
}