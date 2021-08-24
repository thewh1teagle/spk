using PacketDotNet;
using SharpPcap;
using System;
using System.Collections;


namespace spk_dot
{

    public delegate void CallBack(object sender, PacketCapture e);

    class Sniffer
    {

        public void Sniff(ICaptureDevice device, string filter, CallBack onPacketCallback)
        {

            // using var device = 1//devices[i];

            //Register our handler function to the 'packet arrival' event
            device.OnPacketArrival +=
                new PacketArrivalEventHandler(onPacketCallback);

            //Open the device for capturing
            int readTimeoutMilliseconds = 1000;
            device.Open(DeviceModes.Promiscuous, readTimeoutMilliseconds);

            // tcpdump filter to capture only TCP/IP packets
            device.Filter = filter;

            Console.WriteLine();
            Console.WriteLine
                ("-- The following tcpdump filter will be applied: \"{0}\"",
                filter);
            Console.WriteLine
                ("-- Listening on {0}, hit 'Ctrl-C' to exit...",
                device.Description);

            // Start capture packets
            device.Capture();

        }

    }
}