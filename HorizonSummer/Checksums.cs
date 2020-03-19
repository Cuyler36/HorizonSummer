using System;
using System.Collections;
using System.Collections.Generic;

namespace HorizonSummer
{
    public struct HashRegion
    {
        public readonly int HashOffset;
        public readonly int BeginOffset;
        public readonly uint Size;

        public HashRegion(int hashOfs, int begOfs, uint size)
        {
            HashOffset = hashOfs;
            BeginOffset = begOfs;
            Size = size;
        }
    }

    public sealed class HashSet : IEnumerable<HashRegion>
    {
        public readonly string FileName;
        public readonly uint FileSize;
        public readonly HashRegion[] HashRegions;

        public IEnumerator<HashRegion> GetEnumerator() => (HashRegions as IEnumerable<HashRegion>).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => HashRegions.GetEnumerator();

        public HashSet(string fileName, uint fileSize, in HashRegion[] regions)
        {
            FileName = fileName;
            FileSize = fileSize;
            HashRegions = regions;
        }
    }

    public sealed class HashInfo
    {
        public readonly uint RevisionId; // Custom to us
        public readonly uint[] RevisionMagic; // First 16 bytes of files (4 uints)

        private readonly Dictionary<uint, HashSet> _internalHashDict = new Dictionary<uint, HashSet>();

        public HashSet this[uint index]
        {
            get
            {
                if (_internalHashDict.ContainsKey(index))
                    return _internalHashDict[index];
                return null;
            }
            set
            {
                if (_internalHashDict.ContainsKey(index))
                    _internalHashDict[index] = value;
                else
                    _internalHashDict.Add(index, value);
            }
        }

        public HashInfo(uint revisionId, in uint[] magic, in HashSet[] hashSets)
        {
            RevisionId = revisionId;
            RevisionMagic = magic;
            foreach (var hashSet in hashSets)
                this[hashSet.FileSize] = hashSet;
        }
    }


    public static class Checksums
    {
        #region REVISION 1.0.0

        private const uint REVISION_100_ID = 0;
        private const int MAIN_SAVE_SIZE = 0xAC0938;
        private const int PERSONAL_SAVE_SIZE = 0x6BC50;
        private const int POSTBOX_SAVE_SIZE = 0xB44580;
        private const int PHOTO_STUDIO_ISLAND_SIZE = 0x263B4;
        private const int PROFILE_SIZE = 0x69508;

        #endregion

        #region REVISION 1.1.0

        private const uint REVISION_110_ID = 1;
        private const int REV_110_MAIN_SAVE_SIZE = 0xAC2AA0;
        private const int REV_110_PERSONAL_SAVE_SIZE = 0x6BED0;
        private const int REV_110_POSTBOX_SAVE_SIZE = 0xB44590;
        private const int REV_110_PHOTO_STUDIO_ISLAND_SIZE = 0x263C0;
        private const int REV_110_PROFILE_SIZE = 0x69560;

        #endregion

        public static readonly List<HashInfo> VersionHashInfoList = new List<HashInfo>
        {
            new HashInfo(REVISION_100_ID, new uint[]{ 0x67, 0x6F, 0x02, 0x02 }, new HashSet[] {
                new HashSet("main.dat", MAIN_SAVE_SIZE, new HashRegion[] {
                    new HashRegion(0x000108, 0x00010C, 0x1D6D4C),
                    new HashRegion(0x1D6E58, 0x1D6E5C, 0x323384),
                    new HashRegion(0x4FA2E8, 0x4FA2EC, 0x035AC4),
                    new HashRegion(0x52FDB0, 0x52FDB4, 0x03607C),
                    new HashRegion(0x565F38, 0x565F3C, 0x035AC4),
                    new HashRegion(0x59BA00, 0x59BA04, 0x03607C),
                    new HashRegion(0x5D1B88, 0x5D1B8C, 0x035AC4),
                    new HashRegion(0x607650, 0x607654, 0x03607C),
                    new HashRegion(0x63D7D8, 0x63D7DC, 0x035AC4),
                    new HashRegion(0x6732A0, 0x6732A4, 0x03607C),
                    new HashRegion(0x6A9428, 0x6A942C, 0x035AC4),
                    new HashRegion(0x6DEEF0, 0x6DEEF4, 0x03607C),
                    new HashRegion(0x715078, 0x71507C, 0x035AC4),
                    new HashRegion(0x74AB40, 0x74AB44, 0x03607C),
                    new HashRegion(0x780CC8, 0x780CCC, 0x035AC4),
                    new HashRegion(0x7B6790, 0x7B6794, 0x03607C),
                    new HashRegion(0x7EC918, 0x7EC91C, 0x035AC4),
                    new HashRegion(0x8223E0, 0x8223E4, 0x03607C),
                    new HashRegion(0x858460, 0x858464, 0x2684D4)
                }),
                new HashSet("personal.dat", PERSONAL_SAVE_SIZE, new HashRegion[]
                {
                    new HashRegion(0x00108, 0x0010C, 0x35AC4),
                    new HashRegion(0x35BD0, 0x35BD4, 0x3607C)
                }),
                new HashSet("postbox.dat", POSTBOX_SAVE_SIZE, new HashRegion[]
                {
                    new HashRegion(0x000100, 0x00104, 0xB4447C)
                }),
                new HashSet("photo_studio_island.dat", PHOTO_STUDIO_ISLAND_SIZE, new HashRegion[]
                {
                    new HashRegion(0x000100, 0x00104, 0x262B0)
                }),
                new HashSet("profile.dat", PROFILE_SIZE, new HashRegion[]
                {
                    new HashRegion(0x000100, 0x00104, 0x69404)
                }),
            }),
            new HashInfo(REVISION_110_ID, new uint[]{ 0x6D, 0x78, 0x02, 0x00010002 }, new HashSet[] {
                new HashSet("main.dat", REV_110_MAIN_SAVE_SIZE, new HashRegion[] {
                    new HashRegion(0x000110, 0x000114, 0x1D6D5C),
                    new HashRegion(0x1D6E70, 0x1D6E74, 0x323C0C),
                    new HashRegion(0x4FAB90, 0x4FAB94, 0x035AFC),
                    new HashRegion(0x530690, 0x530694, 0x0362BC),
                    new HashRegion(0x566A60, 0x566A64, 0x035AFC),
                    new HashRegion(0x59C560, 0x59C564, 0x0362BC),
                    new HashRegion(0x5D2930, 0x5D2934, 0x035AFC),
                    new HashRegion(0x608430, 0x608434, 0x0362BC),
                    new HashRegion(0x63E800, 0x63E804, 0x035AFC),
                    new HashRegion(0x674300, 0x674304, 0x0362BC),
                    new HashRegion(0x6AA6D0, 0x6AA6D4, 0x035AFC),
                    new HashRegion(0x6E01D0, 0x6E01D4, 0x0362BC),
                    new HashRegion(0x7165A0, 0x7165A4, 0x035AFC),
                    new HashRegion(0x74C0A0, 0x74C0A4, 0x0362BC),
                    new HashRegion(0x782470, 0x782474, 0x035AFC),
                    new HashRegion(0x7B7F70, 0x7B7F74, 0x0362BC),
                    new HashRegion(0x7EE340, 0x7EE344, 0x035AFC),
                    new HashRegion(0x823E40, 0x823E44, 0x0362BC),
                    new HashRegion(0x85A100, 0x85A104, 0x26899C)
                }),
                new HashSet("personal.dat", REV_110_PERSONAL_SAVE_SIZE, new HashRegion[]
                {
                    new HashRegion(0x00110, 0x00114, 0x35AFC),
                    new HashRegion(0x35C10, 0x35C14, 0x362BC)
                }),
                new HashSet("postbox.dat", REV_110_POSTBOX_SAVE_SIZE, new HashRegion[]
                {
                    new HashRegion(0x000100, 0x00104, 0xB4448C)
                }),
                new HashSet("photo_studio_island.dat", REV_110_PHOTO_STUDIO_ISLAND_SIZE, new HashRegion[]
                {
                    new HashRegion(0x000100, 0x00104, 0x262BC)
                }),
                new HashSet("profile.dat", REV_110_PROFILE_SIZE, new HashRegion[]
                {
                    new HashRegion(0x000100, 0x00104, 0x6945C)
                }),
            }),
        };

        public static class Murmur3
        {
            private static uint Murmur32_Scramble(uint k)
            {
                k = (k * 0x16A88000) | ((k * 0xCC9E2D51) >> 17);
                k *= 0x1B873593;
                return k;
            }

            public static uint GetMurmur3Hash(in byte[] data, int offset, uint size, uint seed = 0)
            {
                uint checksum = seed;
                if (size > 3)
                {
                    for (var i = 0; i < (size / sizeof(uint)); i++)
                    {
                        var val = BitConverter.ToUInt32(data, offset);
                        checksum ^= Murmur32_Scramble(val);
                        checksum = (checksum >> 19) | (checksum << 13);
                        checksum = checksum * 5 + 0xE6546B64;
                        offset += 4;
                    }
                }

                var remainder = size % sizeof(uint);
                if (remainder != 0)
                {
                    uint val = BitConverter.ToUInt32(data, (int)((offset + size) - remainder));
                    for (var i = 0; i < (sizeof(uint) - remainder); i++)
                        val >>= 8;
                    checksum ^= Murmur32_Scramble(val);
                }

                checksum ^= size;
                checksum ^= checksum >> 16;
                checksum *= 0x85EBCA6B;
                checksum ^= checksum >> 13;
                checksum *= 0xC2B2AE35;
                checksum ^= checksum >> 16;
                return checksum;
            }

            public static uint UpdateMurmur32(in byte[] data, int hashOffset, int readOffset, uint readSize)
            {
                var newHash = GetMurmur3Hash(data, readOffset, readSize);
                Array.Copy(BitConverter.GetBytes(newHash), 0, data, hashOffset, 4);
                return newHash;
            }

            public static bool VerifyMurmur32(in byte[] data, int hashOffset, int readOffset, uint readSize)
                => BitConverter.ToUInt32(data, hashOffset) == GetMurmur3Hash(data, readOffset, readSize);
        }
    }
}
