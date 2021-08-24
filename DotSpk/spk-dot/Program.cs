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

    
    class Program
    {
        static RSA privateRsa;


        private static bool CorrectKnoc(dynamic result, String secret, long threshold)
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
            dynamic result = JsonConvert.DeserializeObject<dynamic>(decrypted);
            if (CorrectKnoc(result, "123", (long)3))
            {
                Console.WriteLine("Correct knock!!");
            }
            



            // Console.WriteLine("{0}:{1}:{2},{3} Len={4}",
            //    time.Hour, time.Minute, time.Second, time.Millisecond, len);

        }

        static void Main(string[] args)
        {

            

            Console.WriteLine("Hello World!");
            privateRsa = RsaHelper.ReadPrivateKeyFromFile("private.pem");
            RSA pubRsa = RsaHelper.ReadPubKeyFromFile("public.pem");


            // String text = "{\"secret\":\"123\"}";
            // dynamic result = JsonConvert.DeserializeObject<dynamic>(text);
            String text = "hello";
            byte[] enc = RsaHelper.Encrypt(pubRsa, text);
            String decrypted = RsaHelper.Decrypt(privateRsa, enc);
            Console.WriteLine($"decrypted: {decrypted}");


            Sniffer sniffer = new Sniffer();
            ICaptureDevice defaultDev = DeviceManager.GetDefaultDevice();
            sniffer.Sniff(defaultDev, "port 1189", Device_OnPacketArrival);
        }
    }
}
