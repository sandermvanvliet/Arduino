using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Solid.Arduino.OneWire;

namespace Solid.Arduino.Test
{
    [TestClass]
    public class WhenParsingAnOneWireReply
    {
        /* OneWire SEARCH reply
             * ------------------------------
             * 0  START_SYSEX (0xF0)
             * 1  OneWire Command (0x73)
             * 2  search reply command (0x42|0x45) //0x42 normal search reply. 0x45 reply to a SEARCH_ALARMS request
             * 3  pin (0-127)
             * 4  bit 0-6   [optional] //address bytes encoded using 8 times 7 bit for 7 bytes of 8 bit
             * 5  bit 7-13  [optional] //1.address[0] = byte[0]    + byte[1]<<7 & 0x7F
             * 6  bit 14-20 [optional] //1.address[1] = byte[1]>>1 + byte[2]<<6 & 0x7F
             * 7  ....                 //...
             * 11 bit 49-55			   //1.address[6] = byte[6]>>6 + byte[7]<<1 & 0x7F
             * 12 bit 56-63            //1.address[7] = byte[8]    + byte[9]<<7 & 0x7F

             * 13 bit 64-69            //2.address[0] = byte[9]>>1 + byte[10]<<6 &0x7F
             * n  ... // as many bytes as needed (don't exceed MAX_DATA_BYTES though)
             * n+1  END_SYSEX (0xF7)
             */

        [TestMethod]
        public void GivenTheReplyContainsNoAddressesTheSensorCountIsZero()
        {
            var messageBuffer = new[] { 0xF0, 0x73, 0x42, 0x02 };

            var reply = OneWireMessageParser.Parse(messageBuffer, messageBuffer.Length);

            Assert.AreEqual(0, reply.Sensors.Count);
        }

        [TestMethod]
        public void GivenTheReplyContainsOneAddresTheSensorCountIsOne()
        {
            var messageBuffer = new[]
            {
                0xF0, 0x73, 0x42, 0x02
            }
            .Concat(Encoded())
            .ToArray();

            var reply = OneWireMessageParser.Parse(messageBuffer, messageBuffer.Length);

            Assert.AreEqual("287B3E5E06000044", reply.Sensors.Single().ToString());
        }

        [TestMethod]
        public void GivenTheReplyContainsMultipleAddressesTheyAreAllParsed()
        {
            var messageBuffer = new[]
            {
                0xF0, 0x73, 0x42, 0x02
            }
            .Concat(Encoded(4))
            .ToArray();

            var reply = OneWireMessageParser.Parse(messageBuffer, messageBuffer.Length);

            Assert.AreEqual(4, reply.Sensors.Count);
        }

        public int[] Encoded(int number = 1)
        {
            var encoder = new Encoder7BitClass();
            encoder.startBinaryWrite();
            for(int i = 0; i < number; i++)
            {
                encoder.writeBinary(0x28);
                encoder.writeBinary(0x7B);
                encoder.writeBinary(0x3E);
                encoder.writeBinary(0x5E);
                encoder.writeBinary(0x06);
                encoder.writeBinary(0x00);
                encoder.writeBinary(0x00);
                encoder.writeBinary(0x44);
            }
            encoder.endBinaryWrite();

            return encoder.Buffer.Select(b => (int)b).ToArray();
        }
    }
}