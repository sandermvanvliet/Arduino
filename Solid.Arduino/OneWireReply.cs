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
        /// </summary>
        public byte SearchReply { get; set; }
        /// <summary>
        /// The OneWire bus identifier (or pin)
        /// </summary>
        public byte Bus { get; set; }
    }
}