using System.IO;

namespace Degree.Arduino.Test
{
    public class Encoder7BitClass
    {
        private int previous;
        private int shift;
        private readonly MemoryStream Firmata;

        public Encoder7BitClass()
        {
            Firmata = new MemoryStream();
            previous = 0;
            shift = 0;
        }

        public byte[] Buffer => Firmata.GetBuffer();

        private void Write(int data)
        {
            Firmata.WriteByte((byte)data);
        }

        public void startBinaryWrite()
        {
            shift = 0;
        }

        public void endBinaryWrite()
        {
            if (shift > 0)
            {
                Write(previous);
            }
        }

        public void writeBinary(byte data)
        {
            if (shift == 0)
            {
                Write(data & 0x7f);
                shift++;
                previous = data >> 7;
            }
            else
            {
                Write(((data << shift) & 0x7f) | previous);
                if (shift == 6)
                {
                    Write(data >> 1);
                    shift = 0;
                }
                else
                {
                    shift++;
                    previous = data >> (8 - shift);
                }
            }
        }

        public byte[] readBinary(int outBytes, byte[] inData)
        {
            byte[] outData = new byte[outBytes];

            for (int i = 0; i < outBytes; i++)
            {
                int j = i << 3;
                int pos = j / 7;
                byte shift = (byte)(j % 7);
                outData[i] = (byte)((inData[pos] >> shift) | ((inData[pos + 1] << (7 - shift)) & 0xFF));
            }

            return outData;
        }
    }
}