using System.Collections.Generic;
using System.Linq;

namespace Solid.Arduino
{
    public class OneWireMessageParser
    {
        /* OneWire SEARCH reply
             * ------------------------------
             * 0  START_SYSEX (0xF0)
             * 1  OneWire Command (0x73)
             * 2  search reply command (0x42|0x45) //0x42 normal search reply. 0x45 reply to a SEARCH_ALARMS request
             * 3  pin (0-127)
             * 4  bit 0-6   [optional] //address bytes encoded using 8 times 7 bit for 7 bytes of 8 bit
             * 5  bit 7-13  [optional] //1.address[0] = byte[0]    + byte[1]<<7 & 0x7F
             * 6  bit 14-20 [optional] //1.address[1] = byte[1]>>1 + byte[2]<<6 & 0x7F
             * 7  ....                 //...
             * 11 bit 49-55			   //1.address[6] = byte[6]>>6 + byte[7]<<1 & 0x7F
             * 12 bit 56-63            //1.address[7] = byte[8]    + byte[9]<<7 & 0x7F

             * 13 bit 64-69            //2.address[0] = byte[9]>>1 + byte[10]<<6 &0x7F
             * n  ... // as many bytes as needed (don't exceed MAX_DATA_BYTES though)
             * n+1  END_SYSEX (0xF7)
             */

        public static OneWireReply Parse(int[] buffer, int size)
        {
            var reply = new OneWireReply
            {
                Command = (byte)buffer[1],
                SearchReply = (SearchReply)buffer[2],
                Bus = (byte)buffer[3],
                Sensors = new List<OneWireAddress>()
            };

            // 287B3E5E06000044
            // 286C365E060000A4

            var headerSize = 4;
            var addressSize = 8;

            var sensorBytesLength = size - headerSize;

            var numberOfSensors = sensorBytesLength / addressSize;

            var sensorBytes = buffer
                .Skip(headerSize)
                .Take(sensorBytesLength)
                .Select(a => (byte)a)
                .ToArray();

            var decodedSensorBytes = new Encoder7BitClass().readBinary(numberOfSensors * addressSize, sensorBytes);

            for (var pointer = 0; pointer < numberOfSensors; pointer++)
            {
                reply.Sensors.Add(
                    new OneWireAddress(decodedSensorBytes.Skip(pointer * addressSize).Take(addressSize).ToArray()));
            }

            return reply;
        }
    }
}