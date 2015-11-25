using System;
using System.Linq;
using System.Threading;
using Solid.Arduino;
using Solid.Arduino.Firmata;
using Solid.Arduino.OneWire;

namespace Degree.Arduino.Test
{
    internal class Program
    {
        private static readonly AutoResetEvent ResetEvent = new AutoResetEvent(false);
        private static byte[] sensorAddress;

        private static void Main(string[] args)
        {
            var connection = new MonoSerialConnection(args[0], SerialBaudRate.Bps_57600);
            var session = new ArduinoSession(connection, 250);

            session.MessageReceived += (sender, eventArgs) => HandleMessageReceived(eventArgs);

            session.OneWireReplyReceived += (sender, eventArgs) => HandleOneWireReplyReceived(eventArgs);

            Console.WriteLine("Waiting for FirmwareResponse");
            ResetEvent.WaitOne();

            Console.WriteLine("Setting digital pinmode");
            session.SetDigitalPinMode(2, PinMode.OneWire);

            Console.WriteLine("Sending 1-Wire search");
            session.SendOneWireSearch();

            Console.WriteLine("Waiting for OneWire search reply");
            ResetEvent.WaitOne();

            Console.WriteLine("Sending sensor read");
            session.SensOneWireSensorRead(sensorAddress);

            Console.ReadLine();
            connection.Close();

            Console.WriteLine("Done!");
        }

        private static void HandleOneWireReplyReceived(OneWireReplyReceivedEventArgs eventArgs)
        {
            if (eventArgs.Type == OneWireReplyType.SearchReply)
            {
                Console.WriteLine(@"OneWire SearchReply:");
                foreach (var address in eventArgs.SearchReply.Sensors)
                {
                    Console.WriteLine("\t" + address);
                }

                sensorAddress = eventArgs.SearchReply.Sensors.First().Raw;

                ResetEvent.Set();
            }
            else if (eventArgs.Type == OneWireReplyType.ReadReply)
            {
                Console.WriteLine(@"OneWire ReadReply:");
                Console.WriteLine("\tReply on bus " + eventArgs.ReadReply.Bus);
            }
        }

        private static void HandleMessageReceived(FirmataMessageEventArgs eventArgs)
        {
            Console.WriteLine(@"Message: {0}", eventArgs.Value.Type);

            if (eventArgs.Value.Type == MessageType.FirmwareResponse)
            {
                Console.WriteLine("Signalling ResetEvent");
                ResetEvent.Set();
            }
        }
    }
}