namespace Solid.Arduino
{
    /// <summary>
    /// Wraps the OneWire reply message to a OneWire command
    /// </summary>
    public struct OneWireReply
    {
        /// <summary>
        /// Raw data in rest of message
        /// </summary>
        public byte[] Data { get; set; }
        /// <summary>
        /// The command that triggered this reply
        /// </summary>
        public byte Command { get; set; }
        /// <summary>
        /// The type of search that was requested
        /// (0x42|0x45) //0x42 normal search reply. 0x45 reply to a SEARCH_ALARMS request
        /// </summary>
        public SearchReply SearchReply { get; set; }
        /// <summary>
        /// The OneWire bus identifier (or pin)
        /// </summary>
        public byte Bus { get; set; }
    }
}