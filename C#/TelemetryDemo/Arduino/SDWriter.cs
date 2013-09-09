using System;
using System.IO;
using System.Collections;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using SecretLabs.NETMF.Hardware;
using SecretLabs.NETMF.Hardware.NetduinoPlus;

namespace TheGoldenMule.Arduino
{
    /// <summary>
    /// Writes messages to an SD card.
    /// </summary>
    public class SDWriter
    {
        /// <summary>
        /// Represents a log message.
        /// </summary>
        private struct LogMessage
        {
            public string message;
            public bool line;
        }

        private readonly string _fileName;
        private readonly ArrayList _logs = new ArrayList();

        private OutputPort _ledPort;

        public SDWriter()
            : this(string.Empty, true)
        {
            // nothing here
        }

        /// <summary>
        /// The contructor requires a file name.
        /// </summary>
        /// <param name="fileName"></param>
        public SDWriter(string fileName, bool ledOnWrite = true)
        {
            if (string.Empty == fileName)
            {
                _fileName = DateTime.Now.ToString("yyyy.M.dd-h.mm.ss") + ".log";
            }
            else
            {
                _fileName = fileName;
            }

            _ledPort = new OutputPort(Pins.ONBOARD_LED, false);
        }

        /// <summary>
        /// Writes a message to the SD card.
        /// </summary>
        /// <param name="message"></param>
        public void Write(string message)
        {
            AddLog(new LogMessage
            {
                message = message,
                line = false
            });
        }

        /// <summary>
        /// Writes a line to the SD card.
        /// </summary>
        /// <param name="message"></param>
        public void WriteLine(string message)
        {
            AddLog(new LogMessage
            {
                message = message,
                line = true
            });
        }

        /// <summary>
        /// Makes sure that everything has been written to the SD card.
        /// </summary>
        public void Flush()
        {
            WriteBuffer();
        }

        /// <summary>
        /// Add a log.
        /// </summary>
        /// <param name="message"></param>
        private void AddLog(LogMessage message)
        {
            _logs.Add(message);

            if (_logs.Count > 8)
            {
                WriteBuffer();
            }
        }

        /// <summary>
        /// Writes all logs to the card and clears the log buffer. The LED
        /// is also turned on on write.
        /// </summary>
        private void WriteBuffer()
        {
            _ledPort.Write(true);

            using (var fs = new FileStream(@"SD\" + _fileName, FileMode.Append, FileAccess.Write))
            {
                StreamWriter writer = new StreamWriter(fs);
                foreach (var log in _logs)
                {
                    LogMessage message = (LogMessage) log;
                    if (message.line)
                    {
                        writer.WriteLine(message.message);
                    }
                    else
                    {
                        writer.Write(message.message);
                    }
                }
                writer.Close();
            }

            _logs.Clear();

            _ledPort.Write(false);
        }
    }
}