﻿using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Solid.Arduino
{
    /// <summary>
    ///     Mono specific <see cref="SerialPort">SerialPort</see> implementation
    /// </summary>
    public class MonoSerialConnection : ISerialConnection
    {
        private readonly CancellationTokenSource cancellationTokenSource;
        private readonly SerialPort serialPort;
        private bool isDisposed;
        private Task serialReadTask;
        private readonly Queue<byte> internalBuffer;

        public MonoSerialConnection(string portName, SerialBaudRate baudRate)
        {
            PortName = portName;
            BaudRate = (int)baudRate;

            cancellationTokenSource = new CancellationTokenSource();
            serialPort = new SerialPort(portName, (int)baudRate);

            internalBuffer = new Queue<byte>();
        }

        public void Dispose()
        {
            if (isDisposed)
            {
                return;
            }

            isDisposed = true;

            if (serialPort != null && serialPort.IsOpen)
            {
                serialPort.Close();
            }
        }

        public event SerialDataReceivedEventHandler DataReceived;
        public int BaudRate { get; set; }
        public string PortName { get; set; }
        public bool IsOpen { get { return serialPort.IsOpen; } }
        public string NewLine { get; set; }

        public int BytesToRead
        {
            get { return internalBuffer.Count; }
        }

        public void Open()
        {
            serialPort.Open();

            serialReadTask = new TaskFactory(cancellationTokenSource.Token).StartNew(SerialRead, cancellationTokenSource.Token);
        }

        private async Task SerialRead()
        {
            var buffer = new byte[32];
            while (true)
            {
                var bytesRead = await serialPort.BaseStream.ReadAsync(buffer, 0, buffer.Length, cancellationTokenSource.Token);

                if (bytesRead > 0)
                {
                    foreach (var b in buffer.Take(bytesRead))
                    {
                        internalBuffer.Enqueue(b);
                    }

                    RaiseBytesRead();
                }
            }
        }

        private void RaiseBytesRead()
        {
            DataReceived?.Invoke(this, null);
        }

        public void Close()
        {
            cancellationTokenSource.Cancel();

            serialReadTask.Wait(1500);

            Dispose();
        }

        public int ReadByte()
        {
            return internalBuffer.Dequeue();
        }

        public void Write(string text)
        {
            serialPort.Write(text);
        }

        public void Write(byte[] buffer, int offset, int count)
        {
            serialPort.Write(buffer, offset, count);
        }

        public void WriteLine(string text)
        {
            serialPort.WriteLine(text);
        }
    }
}
