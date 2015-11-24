
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Solid.Arduino.OneWire;

namespace Solid.Arduino.Test
{
    [TestClass]
    public class OneWireCommandFlagsTests
    {
        [TestMethod]
        public void Reset()
        {
            Assert.AreEqual("00000001", ToByte(OneWireCommandFlags.Reset));
        }


        [TestMethod]
        public void Skip()
        {
            Assert.AreEqual("00000010", ToByte(OneWireCommandFlags.Skip));
        }

        [TestMethod]
        public void Select()
        {
            Assert.AreEqual("00000100", ToByte(OneWireCommandFlags.Select));
        }

        [TestMethod]
        public void Read()
        {
            Assert.AreEqual("00001000", ToByte(OneWireCommandFlags.Read));
        }

        [TestMethod]
        public void Delay()
        {
            Assert.AreEqual("00010000", ToByte(OneWireCommandFlags.Delay));
        }

        [TestMethod]
        public void Write()
        {
            Assert.AreEqual("00100000", ToByte(OneWireCommandFlags.Write));
        }

        [TestMethod]
        public void ResetAndSelect()
        {
            Assert.AreEqual("00000101", ToByte(OneWireCommandFlags.Reset | OneWireCommandFlags.Select));
        }

        private string ToByte(OneWireCommandFlags flags)
        {
            return Convert.ToString((byte)flags, 2).PadLeft(8, '0');
        }
    }
}