using System.Linq;

namespace Solid.Arduino
{
    public class OneWireAddress
    {
        private readonly int[] _address;

        private OneWireAddress(int[] address)
        {
            _address = address;
        }

        public override string ToString()
        {
            return string.Join("", _address.Select(a => $"{a:X2}"));
        }

        public static OneWireAddress GetAddressFromBytes(int[] buff)
        {
            var address = new int[8];

            address[0] = buff[0] + (buff[1] << 7 & 0x7F);
            address[1] = (buff[1] >> 1) + (buff[2] << 6 & 0x7F);
            address[2] = (buff[2] >> 2) + (buff[3] << 5 & 0x7F);
            address[3] = (buff[3] >> 3) + (buff[4] << 4 & 0x7F);
            address[4] = (buff[4] >> 4) + (buff[5] << 3 & 0x7F);
            address[5] = (buff[5] >> 5) + (buff[6] << 2 & 0x7F);
            address[6] = (buff[6] >> 6) + (buff[7] << 1 & 0x7F);
            address[7] = buff[8] + (buff[9] << 7 & 0x7F);

            return new OneWireAddress(address);
        }
    }
}