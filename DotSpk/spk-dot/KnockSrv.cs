using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PacketDotNet;
using SharpPcap;
using System;
using System.Security.Cryptography;
using System.Text;
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

        private static bool CorrectKnoc(KnockPacket result, String secret, long threshold)
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

        private static void Device_OnPacketArrival(object sender, PacketCapture e)
        {
            Console.WriteLine("new packet!");
            // var time = e.Header.Timeval.Date;
            // var len = e.Data.Length;
            var destinationPort = e.GetPacket().GetPacket().Extract<UdpPacket>().DestinationPort;
            var rawCapture = e.GetPacket();
            var packet = Packet.ParsePacket(rawCapture.LinkLayerType, rawCapture.Data);
            var data = packet.PayloadPacket.PayloadPacket.PayloadData;

            var decrypted = RsaHelper.Decrypt(privateRsa, data);
            KnockPacket result = JsonConvert.DeserializeObject<KnockPacket>(decrypted);
            if (CorrectKnoc(result, "123", (long)0))
            {
                Console.WriteLine("Correct knock!!");
            }
            else
            {
                Console.WriteLine("not Correct knock :(");
            }
        }

        public static void start(string privateKeyPath)
        {
            // RSA pubRsa = RsaHelper.ReadPubKeyFromFile("public.pem");
            // String text = "{\"secret\":\"123\"}";
            // dynamic result = JsonConvert.DeserializeObject<dynamic>(text);
            // byte[] enc = RsaHelper.Encrypt(pubRsa, text);
            // String decrypted = RsaHelper.Decrypt(privateRsa, enc);
            // Console.WriteLine($"decrypted: {decrypted}");

            privateRsa = RsaHelper.ReadPrivateKeyFromFile(privateKeyPath);
            Sniffer sniffer = new Sniffer();
            ICaptureDevice defaultDev = DeviceManager.GetDefaultDevice();
            sniffer.Sniff(defaultDev, "port 1189", Device_OnPacketArrival);
        }
    }
}
