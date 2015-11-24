using System;
using System.Threading;
using Solid.Arduino;
using Solid.Arduino.Firmata;
using Solid.Arduino.OneWire;

namespace Degree.Arduino.Test
{
    internal class Program
    {
        private static readonly AutoResetEvent ResetEvent = new AutoResetEvent(false);

        private static void Main(string[] args)
        {
            var connection = new MonoSerialConnection(args[0], SerialBaudRate.Bps_57600);
            var session = new ArduinoSession(connection, 250);

            session.MessageReceived += (sender, eventArgs) => HandleMessageReceived(eventArgs);

            session.OneWireReplyReceived += (sender, eventArgs) => HandleOneWireReplyReceived(eventArgs);

            ResetEvent.WaitOne();

            Console.WriteLine("Setting digital pinmode");
            session.SetDigitalPinMode(2, PinMode.OneWire);

            Console.WriteLine("Sending 1-Wire search");
            session.SendOneWireSearch();

            Console.ReadLine();
            connection.Close();

            Console.WriteLine("Done!");
        }

        private static void HandleOneWireReplyReceived(OneWireReplyReceivedEventArgs eventArgs)
        {
            Console.WriteLine(@"OneWire reply:");
            foreach (var address in eventArgs.Reply.Sensors)
            {
                Console.WriteLine("\t" + address);
            }
        }

        private static void HandleMessageReceived(FirmataMessageEventArgs eventArgs)
        {
            if (eventArgs.Value.Type == MessageType.ProtocolVersion)
            {
                ResetEvent.Set();
            }

            Console.WriteLine(@"Message: {0}", eventArgs.Value.Type);
        }
    }
}
