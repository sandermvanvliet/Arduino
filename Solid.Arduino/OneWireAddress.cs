using System.Linq;

namespace Solid.Arduino
{
    public class OneWireAddress
    {
        private readonly byte[] _address;

        public OneWireAddress(byte[] address)
        {
            _address = address;
        }

        public override string ToString()
        {
            return string.Join("", _address.Select(a => $"{a:X2}"));
        }
    }
}