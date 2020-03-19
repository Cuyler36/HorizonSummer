using System;
using System.IO;

namespace HorizonSummer.ConsoleApp
{
    internal class Program
    {
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

            HashInfo selectedInfo = null;
            foreach (var info in Checksums.VersionHashInfoList)
            {
                var valid = true;
                for (var i = 0; i < 4; i++)
                {
                    if (info.RevisionMagic[i] != BitConverter.ToUInt32(data, i * 4))
                    {
                        valid = false;
                        break;
                    }
                }
                if (valid)
                {
                    selectedInfo = info;
                    break;
                }
            }

            if (selectedInfo != null)
            {
                Console.WriteLine($"Save File Revision {selectedInfo.RevisionId} detected!");
                HashSet thisFileSet = selectedInfo[(uint)data.Length];
                if (thisFileSet != null)
                {
                    Console.WriteLine($"{thisFileSet.FileName} detected!");
                    foreach (var hashRegion in thisFileSet)
                    {
                        UpdateAndPrint(data, hashRegion.HashOffset, hashRegion.BeginOffset, hashRegion.Size);
                    }
                }
                else
                {
                    ShowErrorMessage();
                    return;
                }
            }
            else
            {
                ShowErrorMessage();
                return;
            }

            File.WriteAllBytes(args[0], data);
            Console.WriteLine("The file's hashes were successfully updated!");
            Console.ReadLine();
        }

        private static void ShowErrorMessage()
        {
            Console.WriteLine("The file supplied isn't supported!");
            Console.WriteLine("Supported Files:\n\tmain.dat\n\tpersonal.dat\n\tpostbox.dat\n\tphoto_island_studio.dat\n\tprofile.dat");
            Console.WriteLine("Supplied files must first be decrypted via HorizonCrypt.");
            Console.ReadLine();
        }

        private static void UpdateAndPrint(in byte[] data, int hashOffset, int startOffset, uint size)
        {
            var currHash = BitConverter.ToUInt32(data, hashOffset);
            var genHash = Checksums.Murmur3.UpdateMurmur32(data, hashOffset, startOffset, size);
            Console.WriteLine($"Updated hash @ 0x{hashOffset:X} [0x{startOffset:X} - 0x{(startOffset + size):X}] from 0x{currHash:X8} to 0x{genHash:X8}");
        }
    }
}
