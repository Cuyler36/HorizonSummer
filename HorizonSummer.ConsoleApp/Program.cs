using System;
using System.IO;

namespace HorizonSummer.ConsoleApp
{
    internal class Program
    {
        private const int MAIN_SAVE_SIZE = 0xAC0938;
        private const int PERSONAL_SAVE_SIZE = 0x6BC50;
        private const int POSTBOX_SAVE_SIZE = 0xB44580;

        static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                Console.WriteLine("Enter the path to a New Horizons decrypted save file:");
                args = new string[] { Console.ReadLine().Replace("\"", "") };
            }

            if (!File.Exists(args[0]))
            {
                Console.WriteLine("A file path must be supplied to use this program.");
                Console.ReadLine();
                return;
            }

            var data = File.ReadAllBytes(args[0]);
            switch (data.Length)
            {
                case MAIN_SAVE_SIZE:
                    {
                        Console.WriteLine("main.dat detected!");
                        UpdateAndPrint(data, 0x000108, 0x00010C, 0x1D6D4C);
                        UpdateAndPrint(data, 0x1D6E58, 0x1D6E5C, 0x323384);
                        UpdateAndPrint(data, 0x4FA2E8, 0x4FA2EC, 0x035AC4);
                        UpdateAndPrint(data, 0x52FDB0, 0x52FDB4, 0x03607C);
                        UpdateAndPrint(data, 0x565F38, 0x565F3C, 0x035AC4);
                        UpdateAndPrint(data, 0x59BA00, 0x59BA04, 0x03607C);
                        UpdateAndPrint(data, 0x5D1B88, 0x5D1B8C, 0x035AC4);
                        UpdateAndPrint(data, 0x607650, 0x607654, 0x03607C);
                        UpdateAndPrint(data, 0x63D7D8, 0x63D7DC, 0x035AC4);
                        UpdateAndPrint(data, 0x6732A0, 0x6732A4, 0x03607C);
                        UpdateAndPrint(data, 0x6A9428, 0x6A942C, 0x035AC4);
                        UpdateAndPrint(data, 0x6DEEF0, 0x6DEEF4, 0x03607C);
                        UpdateAndPrint(data, 0x715078, 0x71507C, 0x035AC4);
                        UpdateAndPrint(data, 0x74AB40, 0x74AB44, 0x03607C);
                        UpdateAndPrint(data, 0x780CC8, 0x780CCC, 0x035AC4);
                        UpdateAndPrint(data, 0x7B6790, 0x7B6794, 0x03607C);
                        UpdateAndPrint(data, 0x7EC918, 0x7EC91C, 0x035AC4);
                        UpdateAndPrint(data, 0x8223E0, 0x8223E4, 0x03607C);
                        UpdateAndPrint(data, 0x858460, 0x858464, 0x2684D4);
                        break;
                    }

                case PERSONAL_SAVE_SIZE:
                    {
                        Console.WriteLine("personal.dat detected!");
                        UpdateAndPrint(data, 0x00108, 0x0010C, 0x35AC4);
                        UpdateAndPrint(data, 0x35BD0, 0x35BD4, 0x3607C);
                        break;
                    }

                case POSTBOX_SAVE_SIZE:
                    {
                        Console.WriteLine("postbox.dat detected!");
                        UpdateAndPrint(data, 0x100, 0x104, 0xB4447C);
                        break;
                    }

                default:
                    {
                        Console.WriteLine("The file supplied isn't supported!");
                        Console.WriteLine("Supported Files:\n\tmain.dat\n\tpersonal.dat\n\tpostbox.dat");
                        Console.WriteLine("Supplied files must first be decrypted via HorizonCrypt.");
                        Console.ReadLine();
                        return;
                    }
            }

            File.WriteAllBytes(args[0], data);
            Console.WriteLine("The file's hashes were successfully updated!");
            Console.ReadLine();
        }

        private static void UpdateAndPrint(in byte[] data, int hashOffset, int startOffset, uint size)
        {
            var currHash = BitConverter.ToUInt32(data, hashOffset);
            var genHash = Checksums.UpdateMurmur32(data, hashOffset, startOffset, size);
            Console.WriteLine($"Updated hash @ 0x{hashOffset:X} [0x{startOffset:X} - 0x{(startOffset + size):X}] from 0x{currHash:X8} to 0x{genHash:X8}");
        }
    }
}
