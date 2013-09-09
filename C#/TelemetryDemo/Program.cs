﻿using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using SecretLabs.NETMF.Hardware;
using SecretLabs.NETMF.Hardware.NetduinoPlus;

namespace TheGoldenMule.Arduino.Telemetry
{
    public class Program
    {
        public static void Main()
        {
            new AccelerometerDemo().Run();
        }
    }
}
