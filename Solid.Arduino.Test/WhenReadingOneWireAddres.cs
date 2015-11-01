using System.Collections.Generic;
using System.Linq;
using Degree.Arduino.Test;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Solid.Arduino.Test
{
    [TestClass]
    public class WhenReadingOneWireAddres
    {
        [TestMethod]
        public void GivenASingleAddressInTheBuffer()
        {
            var buff = new[]
            {
                0,
                0,
                0,
                0, // end of header
                40,
                118,
                121,
                113,
                101,
                0,
                0,
                0,
                68,
                0,
            };

            Assert.AreEqual("287B3E5E06000044",
                GetAddressesFrom(buff, buff.Length).First().ToString());
        }

        [TestMethod]
        public void GivenATwoAddressesInTheBuffer()
        {
            var buff = new[]
            {
                0,
                0,
                0,
                0, // end of header
                40,
                118,
                121,
                113,
                101,
                0,
                0,
                0,
                68,
                0,
                40,
                118,
                121,
                113,
                101,
                0,
                0,
                0,
                69,
                0,
            };

            Assert.AreEqual("287B3E5E06000044",
                GetAddressesFrom(buff, buff.Length).First().ToString());
            Assert.AreEqual("287B3E5E06000045",
                GetAddressesFrom(buff, buff.Length).Skip(1).First().ToString());
        }

        [TestMethod]
        public void RealWorld()
        {
            var rawbuff = new[]
            {
                240, 115, 66, 2, 40, 88, 89, 113, 101, 0, 0, 0, 36, 81, 108, 115, 99, 75, 1, 0, 0, 8, 1, 0
            };

            var buff = rawbuff.Skip(4).ToArray();

            var addresses = new Encoder7BitClass().readBinary(16, buff.Select(b => (byte)b).ToArray());
            
            Assert.AreEqual("286C365E060000A4", Dump(addresses.Take(8)));
            Assert.AreEqual("287B3E5E06000044", Dump(addresses.Skip(8).Take(8)));
        }

        private string Dump(IEnumerable<byte> buff)
        {
            return string.Join("", buff.Select(a => $"{a:X2}"));
        }

        private List<OneWireAddress> GetAddressesFrom(int[] _messageBuffer, int messageBufferIndex)
        {
            return OneWireMessageParser.Parse(_messageBuffer, messageBufferIndex).Sensors;
        }
    }
}