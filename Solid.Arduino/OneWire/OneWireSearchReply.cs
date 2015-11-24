using System.Collections.Generic;

namespace Solid.Arduino.OneWire
{
    /// <summary>
    /// Wraps the OneWire SearchReply message to a OneWire command
    /// </summary>
    public struct OneWireSearchReply
    {
        /// <summary>
        /// Raw data in rest of message
        /// </summary>
        public byte[] Data { get; set; }
        /// <summary>
        /// The command that triggered this SearchReply
        /// </summary>
        public OneWireCommand Command { get; set; }
        /// <summary>
        /// The type of search that was requested
        /// (0x42|0x45) //0x42 normal search SearchReply. 0x45 SearchReply to a SEARCH_ALARMS request
        /// </summary>
        public SearchReply SearchReply { get; set; }
        /// <summary>
        /// The OneWire bus identifier (or pin)
        /// </summary>
        public byte Bus { get; set; }

        public List<OneWireAddress> Sensors { get; set; }
    }
}