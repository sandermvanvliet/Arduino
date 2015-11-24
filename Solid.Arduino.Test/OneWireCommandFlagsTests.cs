
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
            Assert.AreEqual("00000001", ToByte(OneWireCommand.Reset));
        }


        [TestMethod]
        public void Skip()
        {
            Assert.AreEqual("00000010", ToByte(OneWireCommand.Skip));
        }

        [TestMethod]
        public void Select()
        {
            Assert.AreEqual("00000100", ToByte(OneWireCommand.Select));
        }

        [TestMethod]
        public void Read()
        {
            Assert.AreEqual("00001000", ToByte(OneWireCommand.Read));
        }

        [TestMethod]
        public void Delay()
        {
            Assert.AreEqual("00010000", ToByte(OneWireCommand.Delay));
        }

        [TestMethod]
        public void Write()
        {
            Assert.AreEqual("00100000", ToByte(OneWireCommand.Write));
        }

        [TestMethod]
        public void ResetAndSelect()
        {
            Assert.AreEqual("00000101", ToByte(OneWireCommand.Reset | OneWireCommand.Select));
        }

        private string ToByte(OneWireCommand flags)
        {
            return Convert.ToString((byte)flags, 2).PadLeft(8, '0');
        }
    }
}