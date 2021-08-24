using CommandLine;
using System.Collections.Generic;

namespace spk_dot
{
    class Options
    {

        [Option('p', "port", Required = true, HelpText = "Port", SetName = "config")]
        public int Port { get; set; }

        [Option('s', "secret", Required = true, HelpText = "secret", SetName = "config")]
        public string Secret { get; set; }



        [Option('e', "exec",
        Required = true,
        HelpText = "command to execute after the correct knock", SetName = "config")]
        public string Exec { get; set; }

        [Option('t', "threshold",
        Required = true,
        HelpText = "knock packet time threshold", SetName = "config")]
        public float Threshold { get; set; }


        [Option('k', "key",
        Required = true,
        HelpText = "Path to private key", SetName = "config")]
        public string KeyPath { get; set; }
            

        [Option('i', "interface, number",
        Required = false,
        HelpText = "interface name (number), get all interfaces by -l option", SetName = "config")]
        public int InterfaceIdx { get; set; }


        [Option('l', "list",
        Required = false,
        HelpText = "list all availaible interfaces", SetName = "help")]
        public bool ListInterfaces { get; set; }
    }
}