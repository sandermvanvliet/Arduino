using System.Linq;
using Degree.Arduino.Test;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Solid.Arduino.Test
{
    [TestClass]
    public class EncodingTests
    {
        [TestMethod]
        public void OneAddressRoundtrip()
        {
            // 28 7B 3E 5E 06 00 00 44
            var encoder = new Encoder7BitClass();
            encoder.startBinaryWrite();
            encoder.writeBinary(0x28);
            encoder.writeBinary(0x7B);
            encoder.writeBinary(0x3E);
            encoder.writeBinary(0x5E);
            encoder.writeBinary(0x06);
            encoder.writeBinary(0x00);
            encoder.writeBinary(0x00);
            encoder.writeBinary(0x44);
            encoder.endBinaryWrite();

            var buffer = encoder.Buffer;
            var array = buffer.Select(b => (int)b).ToArray();
            var address =OneWireAddress.GetAddressFromBytes(array);

            Assert.AreEqual("287B3E5E06000044", address.ToString());
            var result = encoder.readBinary(8, buffer);
            Assert.AreEqual("287B3E5E06000044", Dump(result));
        }

        [TestMethod]
        public void OneAddressRoundtripAlternate()
        {
            // 28 6C 36 5E 06 00 00 A4
            var encoder = new Encoder7BitClass();
            encoder.startBinaryWrite();
            encoder.writeBinary(0x28);
            encoder.writeBinary(0x6C);
            encoder.writeBinary(0x36);
            encoder.writeBinary(0x5E);
            encoder.writeBinary(0x06);
            encoder.writeBinary(0x00);
            encoder.writeBinary(0x00);
            encoder.writeBinary(0x24); // Weird as fuck, 0x44 is encoded as 0x24
            encoder.endBinaryWrite();

            var buffer = encoder.Buffer;
            var array = buffer.Select(b => (int)b).ToArray();
            var address = OneWireAddress.GetAddressFromBytes(array);

            Assert.AreEqual("286C365E06000024", address.ToString());
            var result = encoder.readBinary(8, buffer);
            Assert.AreEqual("286C365E06000024", Dump(result));
        }

        [TestMethod]
        public void TwoAddressesRoundTrip()
        {
            var encoder = new Encoder7BitClass();
            encoder.startBinaryWrite();
            encoder.writeBinary(0x28);
            encoder.writeBinary(0x6C);
            encoder.writeBinary(0x36);
            encoder.writeBinary(0x5E);
            encoder.writeBinary(0x06);
            encoder.writeBinary(0x00);
            encoder.writeBinary(0x00);
            encoder.writeBinary(0x24); // Weird as fuck, 0x44 is encoded as 0x24
            encoder.writeBinary(0x28);
            encoder.writeBinary(0x7B);
            encoder.writeBinary(0x3E);
            encoder.writeBinary(0x5E);
            encoder.writeBinary(0x06);
            encoder.writeBinary(0x00);
            encoder.writeBinary(0x00);
            encoder.writeBinary(0x44);
            encoder.endBinaryWrite();

            var buffer = encoder.Buffer;
            
            var result = encoder.readBinary(16, buffer);
            Assert.AreEqual("286C365E06000024287B3E5E06000044", Dump(result));
        }
        [TestMethod]
        public void TwoAddressesRoundTripAlternate()
        {
            var encoder = new Encoder7BitClass();
            encoder.startBinaryWrite();
            encoder.writeBinary(0x28);
            encoder.writeBinary(0x7B);
            encoder.writeBinary(0x3E);
            encoder.writeBinary(0x5E);
            encoder.writeBinary(0x06);
            encoder.writeBinary(0x00);
            encoder.writeBinary(0x00);
            encoder.writeBinary(0x44);
            encoder.writeBinary(0x28);
            encoder.writeBinary(0x6C);
            encoder.writeBinary(0x36);
            encoder.writeBinary(0x5E);
            encoder.writeBinary(0x06);
            encoder.writeBinary(0x00);
            encoder.writeBinary(0x00);
            encoder.writeBinary(0x24); // Weird as fuck, 0x44 is encoded as 0x24
            encoder.endBinaryWrite();

            var buffer = encoder.Buffer;

            var result = encoder.readBinary(16, buffer);
            Assert.AreEqual("287B3E5E06000044286C365E06000024", Dump(result));
        }

        private string Dump(byte[] buff)
        {
            return string.Join("", buff.Select(a => $"{a:X2}"));
        }
    }
}