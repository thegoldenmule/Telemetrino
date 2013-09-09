using System;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using SecretLabs.NETMF.Hardware;
using SecretLabs.NETMF.Hardware.NetduinoPlus;

namespace TheGoldenMule.Arduino.Telemetry
{
    public class AccelerometerDemo
    {
        private SDWriter _writer;

        private AccelerometerPulseInput _xAxis;
        private AccelerometerPulseInput _yAxis;
        private AccelerometerPulseInput _tAxis;

        private InterruptPort _button;

        private bool _willToggle = false;
        private bool _log = false;

        public void Run()
        {
            Setup();

            while (Loop())
            {
                Thread.Sleep(1);
            }
        }

        private void Setup()
        {
            _writer = new SDWriter("Log.log");

            _xAxis = new AccelerometerPulseInput(Pins.GPIO_PIN_A0);
            _yAxis = new AccelerometerPulseInput(Pins.GPIO_PIN_A1);
            _tAxis = new AccelerometerPulseInput(Pins.GPIO_PIN_A2);

            _button = new InterruptPort(Pins.ONBOARD_SW1, false, Port.ResistorMode.Disabled, Port.InterruptMode.InterruptEdgeBoth);
            _button.OnInterrupt += new NativeEventHandler(OnButtonInterrupt);
        }

        private void OnButtonInterrupt(uint data1, uint data2, DateTime time)
        {
            // button is down
            if (0 == data2)
            {
                _willToggle = true;
            }
            else if (_willToggle)
            {
                _willToggle = false;

                _log = !_log;
            }
        }

        private bool Loop()
        {
            // only log if we've enabled it
            if (_log)
            {
                // read from inputs + write to SD card
                _writer.WriteLine(
                    _xAxis.PulseWidth.ToString() + ","
                    + _yAxis.PulseWidth.ToString() + ","
                    + _tAxis.PulseWidth.ToString());
            }

            return true;
        }

    }
}
