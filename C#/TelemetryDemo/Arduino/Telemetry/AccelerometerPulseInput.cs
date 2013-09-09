using System;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;

namespace TheGoldenMule.Arduino.Telemetry
{
    /// <summary>
    /// Measures pulse input, not so straightforward...
    /// </summary>
    public class AccelerometerPulseInput
    {
        private readonly InterruptPort _port;
        private DateTime _time;

        public float PulseWidth
        {
            get;
            private set;
        }

        public AccelerometerPulseInput(Cpu.Pin pin)
        {
            _port = new InterruptPort(pin, false, Port.ResistorMode.Disabled, Port.InterruptMode.InterruptEdgeBoth);
            _port.OnInterrupt += new NativeEventHandler(OnInterrupt);
        }

        private void OnInterrupt(uint data1, uint state, DateTime time)
        {
            if (0 == state)
            {
                // end of pulse
                PulseWidth = (float)((((TimeSpan)(time - _time)).Ticks / 10000.0) - 5);
            }
            else
            {
                // start of pulse
                _time = time;
            }
        }
    }
}