using Newtonsoft.Json;
using PacketDotNet;
using SharpPcap;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using CommandLine;

namespace spk_dot
{
    class Program
    {
        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args)
            .WithParsed<Options>(opts =>
            {
                if (opts.ListInterfaces)
                {
                    DeviceManager.PrintDevices();
                    System.Environment.Exit(0);
                }

                ICaptureDevice interfaceResult;
                if (opts.InterfaceIdx != null)
                {
                    interfaceResult = DeviceManager.GetInterfaceByIndex(opts.InterfaceIdx - 1); // uman index..
                }
                else
                {
                    interfaceResult = DeviceManager.GetDefaultDevice();
                }
            });

            // KnockSrv.start("private.pem");
        }
    }
}
