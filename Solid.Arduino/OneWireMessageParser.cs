using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Solid.Arduino
{
    public class OneWireMessageParser
    {
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
            var addressSize = 10;

            Debug.WriteLine("");
            Debug.WriteLine("var buff = new [] { ");
            buffer
                .Take(size)
                .ToList()
                .ForEach(b => Debug.Write(b + ","));
            Debug.WriteLine("}");


            reply.Sensors.Add(OneWireAddress.GetAddressFromBytes(buffer.Skip(headerSize).Take(addressSize).ToArray()));
            reply.Sensors.Add(OneWireAddress.GetAddressFromBytes(buffer.Skip(12).Take(addressSize).ToArray()));
            
            //for (var pointer = headerSize; pointer < size; pointer += 9)
            //{
            //    var buff = buffer.Skip(pointer).Take(addressSize).ToArray();

            //    if (buff.Count() == addressSize)
            //    {
            //        reply.Sensors.Add(OneWireAddress.GetAddressFromBytes(buff));
            //    }

            //}

            return reply;
        }
    }
}