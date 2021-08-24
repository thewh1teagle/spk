using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PacketDotNet;
using SharpPcap;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace spk_dot
{
    public class KnockPacket
    {
        public long timestamp { get; set; }
        public string secret { get; set; }
    }

    class KnockSrv
    {
        static RSA privateRsa;
        static string secret;
        static float threshold;
        static int port;
        static string execCommand;

        private static bool CorrectKnoc(KnockPacket result, String secret, float threshold)
        {
            long knockTimestamp = Convert.ToInt64(result.timestamp);
            String knockSecret = result.secret;
            long currentTimestamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();
            if (currentTimestamp - knockTimestamp - threshold <= 0 && knockSecret == secret)
            {
                return true;
            }
            return false;
        }

        private static void OnPacketArrival(object sender, PacketCapture e)
        {
            Console.WriteLine("new packet!");
            // var time = e.Header.Timeval.Date;
            int packetSize = e.Data.Length;
            if (packetSize > 1000) // ignore if packet > 1kb
            {
                return;
            }
            // var destinationPort = e.GetPacket().GetPacket().Extract<UdpPacket>().DestinationPort;

            var rawCapture = e.GetPacket();
            var packet = Packet.ParsePacket(rawCapture.LinkLayerType, rawCapture.Data);
            var data = packet.PayloadPacket.PayloadPacket.PayloadData;
            try
            {
                var decrypted = RsaHelper.Decrypt(privateRsa, data);
                KnockPacket result = JsonConvert.DeserializeObject<KnockPacket>(decrypted);
                if (CorrectKnoc(result, KnockSrv.secret, threshold))
                {
                    Console.WriteLine("Correct knock!!");
                    Commander.RunCommand(KnockSrv.execCommand, false);
                }
                else
                {
                    Console.WriteLine("not Correct knock :(");
                }
            } catch
            {
                Console.WriteLine("Wrong private key");
                return;
            }            
        }

        public static void start(string privateKeyPath, ICaptureDevice device, string secret, string execCommand, int port, float threshold)
        {
            // RSA pubRsa = RsaHelper.ReadPubKeyFromFile("public.pem");
            // String text = "{\"secret\":\"123\"}";
            // dynamic result = JsonConvert.DeserializeObject<dynamic>(text);
            // byte[] enc = RsaHelper.Encrypt(pubRsa, text);
            // String decrypted = RsaHelper.Decrypt(privateRsa, enc);
            // Console.WriteLine($"decrypted: {decrypted}");
            KnockSrv.privateRsa = RsaHelper.ReadPrivateKeyFromFile(privateKeyPath);
            KnockSrv.threshold = threshold;
            KnockSrv.secret = secret;
            KnockSrv.port = port;
            KnockSrv.execCommand = execCommand;
            Sniffer sniffer = new Sniffer();
            String filterStr = Sniffer.GenerateFilterString(port, "udp");
            sniffer.Sniff(device, filterStr, OnPacketArrival);
        }
    }
}
