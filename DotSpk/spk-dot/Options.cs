using CommandLine;
using System.Collections.Generic;

namespace spk_dot
{
    class Options
    {

        [Option('p', "port", Required = true, HelpText = "Port", SetName = "config")]
        public IEnumerable<string> Ports { get; set; }

        [Option('e', "exec",
        Required = true,
        HelpText = "command to execute after the correct knock", SetName = "config")]
        public string Exec { get; set; }

        [Option('t', "threshold",
        Required = true,
        HelpText = "knock packet time threshold", SetName = "config")]
        public int SeqTimeout { get; set; }


        [Option('i', "interface, number",
        Required = false,
        HelpText = "interface name (number), get all interfaces by -l option", SetName = "config")]
        public int InterfaceIdx { get; set; }


        [Option('l', "list",
        Required = true,
        HelpText = "list all availaible interfaces", SetName = "help")]
        public bool ListInterfaces { get; set; }
    }
}