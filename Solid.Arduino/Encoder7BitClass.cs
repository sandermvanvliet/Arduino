using System.IO;

namespace Solid.Arduino
{
    public class Encoder7BitClass
    {
        private int _previous;
        private int _shift;
        private readonly MemoryStream _firmata;

        public Encoder7BitClass()
        {
            _firmata = new MemoryStream();
            _previous = 0;
            _shift = 0;
        }

        public byte[] Buffer => _firmata.GetBuffer();

        private void Write(int data)
        {
            _firmata.WriteByte((byte)data);
        }

        public void startBinaryWrite()
        {
            _shift = 0;
        }

        public void endBinaryWrite()
        {
            if (_shift > 0)
            {
                Write(_previous);
            }
        }

        public void writeBinary(byte data)
        {
            if (_shift == 0)
            {
                Write(data & 0x7f);
                _shift++;
                _previous = data >> 7;
            }
            else
            {
                Write(((data << _shift) & 0x7f) | _previous);
                if (_shift == 6)
                {
                    Write(data >> 1);
                    _shift = 0;
                }
                else
                {
                    _shift++;
                    _previous = data >> (8 - _shift);
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