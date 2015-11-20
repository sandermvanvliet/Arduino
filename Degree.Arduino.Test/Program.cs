using System;
using Solid.Arduino;
using Solid.Arduino.Firmata;

namespace Degree.Arduino.Test
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var connection = new MonoSerialConnection(args[0], SerialBaudRate.Bps_57600);
            var session = new ArduinoSession(connection, 250);

            session.MessageReceived += (sender, eventArgs) => HandleMessageReceived(eventArgs);

            session.OneWireReplyReceived += (sender, eventArgs) => HandleOneWireReplyReceived(eventArgs);
            session.SetDigitalPinMode(2, PinMode.OneWire);

            session.SendOneWireSearch();

            Console.ReadLine();
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
            Console.WriteLine(@"Message: {0}", eventArgs.Value.Type);
        }
    }
}