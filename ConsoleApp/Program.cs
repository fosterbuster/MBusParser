using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using MBus;
using MBus.DataRecord.DataRecordHeader.DataInformationBlock;
using MBus.Extensions;
using MBus.Header;
using MBus.Header.ELL;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var gmm = new Configuration("4005".HexStringToBytes());
            var bb = "54".HexStringToBytes().FirstOrDefault();
            var ttt = new DataInformationField(bb);

            byte mask = 0b0000_1111;
            var hhh = ttt.DataField;
            var tets = gmm.EncryptionScheme;
            var keys = new Dictionary<int, byte[]>();
            string line;

            // Read the file and display it line by line.  
            System.IO.StreamReader file =
                new System.IO.StreamReader(@"C:\Users\gamad\source\git\MBusParser\MbusParser\keys.txt");
            file.ReadLine();
            while ((line = file.ReadLine()) != null)
            {
                var elem = line.Split(',');
                if (elem.Last() == "0") continue;

                var key = int.Parse(elem.First()); 
                var decryptionKey = elem.Last().Replace("0x", "").HexStringToBytes();
                keys.Add(key, decryptionKey);
            }


            file.Close();

            var parser = new MBusParser();

            System.IO.StreamReader wmbusPackets =
               new System.IO.StreamReader(@"C:\Users\gamad\source\git\MBusParser\MbusParser\wmbus.txt");
            var mbus = "";
            wmbusPackets.ReadLine();

            var list = new List<MBusTelegram>();
            int counter = 0;
            Stopwatch stopwatch = new Stopwatch();
            while ((mbus = wmbusPackets.ReadLine()) != null)
            {
                if (mbus.Contains("}") || mbus.Contains("{")) continue;
                var bytes = mbus.HexStringToBytes();

                stopwatch.Start();
                var header = parser.ParseHeader(bytes);
                stopwatch.Stop();
                var hmm = keys.TryGetValue(header.SerialNumber, out var key);
                if (!hmm) continue;
                Console.WriteLine(header.ManufacturerName);
                stopwatch.Start();
                var parsed = parser.ParsePayload(header, bytes, key);
                stopwatch.Stop();
                list.Add(parsed);
                counter++;

                if (counter == 100000) break;
            }

            wmbusPackets.Close();
            /*

            68 FF FF 68 08 01 72 ----
            0  1  2  3  4  5  6  7

            08 01 72 ----
            0  1  2  3
            */

        }
    }


}
