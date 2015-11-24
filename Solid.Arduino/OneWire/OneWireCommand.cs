using System;

namespace Solid.Arduino.OneWire
{
    [Flags]
    public enum OneWireCommand
    {
        // bit 0 = reset, bit 1 = skip, bit 2 = select, bit 3 = read, bit 4 = delay, bit 5 = write
        None = 0,
        Reset = 1,
        Skip = 2,
        Select = 4,
        Read = 8,
        Delay = 16,
        Write = 32
    }
}