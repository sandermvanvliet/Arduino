using System.Collections.Generic;
using System.Linq;
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

            var a = new int[8];
            var b = new int[8];

            a[0] = buff[0] + (buff[1] << 7 & 0x7F);
            a[1] = (buff[1] >> 1) + (buff[2] << 6 & 0x7F);
            a[2] = (buff[2] >> 2) + (buff[3] << 5 & 0x7F);
            a[3] = (buff[3] >> 3) + (buff[4] << 4 & 0x7F);
            a[4] = (buff[4] >> 4) + (buff[5] << 3 & 0x7F);
            a[5] = (buff[5] >> 5) + (buff[6] << 2 & 0x7F);
            a[6] = (buff[6] >> 6) + (buff[7] << 1 & 0x7F);
            a[7] = buff[8] + (buff[9] << 7 & 0x7F);
            b[0] = (buff[9] >> 1) + (buff[10] << 6 & 0x7F);
            b[1] = (buff[10] >> 1) + (buff[11] << 6 & 0x7F);
            b[2] = (buff[11] >> 2) + (buff[12] << 5 & 0x7F);
            b[3] = (buff[12] >> 3) + (buff[13] << 4 & 0x7F);
            b[4] = (buff[13] >> 4) + (buff[14] << 3 & 0x7F);
            b[5] = (buff[14] >> 5) + (buff[15] << 2 & 0x7F);
            b[6] = (buff[15] >> 6) + (buff[16] << 1 & 0x7F);
            b[7] = buff[17] + (buff[18] << 7 & 0x7F);

            //var sensors = GetAddressesFrom(buff, buff.Length);
            Assert.AreEqual("286C365E06000024", Dump(a));
            Assert.AreEqual("287B3E5E06000044", Dump(b));
        }

        private string Dump(int[] buff)
        {
            return string.Join("", buff.Select(a => $"{a:X2}"));
        }

        private List<OneWireAddress> GetAddressesFrom(int[] _messageBuffer, int messageBufferIndex)
        {
            return OneWireMessageParser.Parse(_messageBuffer, messageBufferIndex).Sensors;
        }
    }
}