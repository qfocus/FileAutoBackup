using System;
using System.IO;
using System.Linq;

namespace AutoBackup
{
    internal class QQCrypto
    {
        private static int[] UKey =
            {
        0x77, 0x48, 0x32, 0x73, 0xDE, 0xF2, 0xC0, 0xC8, 0x95, 0xEC, 0x30, 0xB2,
        0x51, 0xC3, 0xE1, 0xA0, 0x9E, 0xE6, 0x9D, 0xCF, 0xFA, 0x7F, 0x14, 0xD1,
        0xCE, 0xB8, 0xDC, 0xC3, 0x4A, 0x67, 0x93, 0xD6, 0x28, 0xC2, 0x91, 0x70,
        0xCA, 0x8D, 0xA2, 0xA4, 0xF0, 0x08, 0x61, 0x90, 0x7E, 0x6F, 0xA2, 0xE0,
        0xEB, 0xAE, 0x3E, 0xB6, 0x67, 0xC7, 0x92, 0xF4, 0x91, 0xB5, 0xF6, 0x6C,
        0x5E, 0x84, 0x40, 0xF7, 0xF3, 0x1B, 0x02, 0x7F, 0xD5, 0xAB, 0x41, 0x89,
        0x28, 0xF4, 0x25, 0xCC, 0x52, 0x11, 0xAD, 0x43, 0x68, 0xA6, 0x41, 0x8B,
        0x84, 0xB5, 0xFF, 0x2C, 0x92, 0x4A, 0x26, 0xD8, 0x47, 0x6A, 0x7C, 0x95,
        0x61, 0xCC, 0xE6, 0xCB, 0xBB, 0x3F, 0x47, 0x58, 0x89, 0x75, 0xC3, 0x75,
        0xA1, 0xD9, 0xAF, 0xCC, 0x08, 0x73, 0x17, 0xDC, 0xAA, 0x9A, 0xA2, 0x16,
        0x41, 0xD8, 0xA2, 0x06, 0xC6, 0x8B, 0xFC, 0x66, 0x34, 0x9F, 0xCF, 0x18,
        0x23, 0xA0, 0x0A, 0x74, 0xE7, 0x2B, 0x27, 0x70, 0x92, 0xE9, 0xAF, 0x37,
        0xE6, 0x8C, 0xA7, 0xBC, 0x62, 0x65, 0x9C, 0xC2, 0x08, 0xC9, 0x88, 0xB3,
        0xF3, 0x43, 0xAC, 0x74, 0x2C, 0x0F, 0xD4, 0xAF, 0xA1, 0xC3, 0x01, 0x64,
        0x95, 0x4E, 0x48, 0x9F, 0xF4, 0x35, 0x78, 0x95, 0x7A, 0x39, 0xD6, 0x6A,
        0xA0, 0x6D, 0x40, 0xE8, 0x4F, 0xA8, 0xEF, 0x11, 0x1D, 0xF3, 0x1B, 0x3F,
        0x3F, 0x07, 0xDD, 0x6F, 0x5B, 0x19, 0x30, 0x19, 0xFB, 0xEF, 0x0E, 0x37,
        0xF0, 0x0E, 0xCD, 0x16, 0x49, 0xFE, 0x53, 0x47, 0x13, 0x1A, 0xBD, 0xA4,
        0xF1, 0x40, 0x19, 0x60, 0x0E, 0xED, 0x68, 0x09, 0x06, 0x5F, 0x4D, 0xCF,
        0x3D, 0x1A, 0xFE, 0x20, 0x77, 0xE4, 0xD9, 0xDA, 0xF9, 0xA4, 0x2B, 0x76,
        0x1C, 0x71, 0xDB, 0x00, 0xBC, 0xFD, 0xC,  0x6C, 0xA5, 0x47, 0xF7, 0xF6,
        0x00, 0x79, 0x4A, 0x11};

        private static int[] MagicList =
        {
            27 ,28 ,31 ,36 ,43 ,52 ,63 ,76 ,91 ,108,127,148,171,196,223,252,
            27 ,60 ,95 ,132,171,212,255,44 ,91 ,140,191,244,43 ,100,159,220,
            27 ,92 ,159,228,43 ,116,191,12 ,91 ,172,255,84 ,171,4  ,95 ,188,
            27 ,124,223,68 ,171,20 ,127,236,91 ,204,63 ,180,43 ,164,31 ,156,
            27 ,156,31 ,164,43 ,180,63 ,204,91 ,236,127,20 ,171,68 ,223,124,
            27 ,188,95 ,4  ,171,84 ,255,172,91 ,12 ,191,116,43 ,228,159,92 ,
            27 ,220,159,100,43 ,244,191,140,91 ,44 ,255,212,171,132,95 ,60 ,
            27 ,252,223,196,171,148,127,108,91 ,76 ,63 ,52 ,43 ,36 ,31 ,28
        };

        private int[] decodeKey;

        private FileInfo fileinfo;
        private String originalExt;


        public QQCrypto(FileInfo file)
        {
            this.fileinfo = file;
            if (Path.GetExtension(file.FullName).ToLower().Contains("flac"))
            {
                this.originalExt = "flac";
            }
            else
            {
                this.originalExt = "mp3";
            }
            ChooseKey();
        }


        private void ChooseKey()
        {
            if (Path.GetExtension(fileinfo.Name) != ".mflac")
            {
                decodeKey = UKey;
                return;
            }

            decodeKey = new int[256];

            FileStream fStream = new FileStream(fileinfo.FullName, FileMode.Open);
            BinaryReader bsfReader = new BinaryReader(fStream);
            byte[] buffer1 = new byte[128];
            byte[] buffer2 = new byte[128];
            bsfReader.Read(buffer1, 0, 128);
            bsfReader.Read(buffer2, 0, 128);
            bool findKey = false;
            for (; ; )
            {
                if (Enumerable.SequenceEqual(buffer1, buffer2))
                {
                    findKey = true;
                    break;
                }
                Buffer.BlockCopy(buffer2, 0, buffer1, 0, buffer2.Length);
                var readSize = bsfReader.Read(buffer2, 0, 128);
                if (readSize < 1)
                {
                    break;
                }
            }

            if (findKey)
            {
                //Only 64 Bytes in UKey are valid.
                for (int i = 0; i < 64; i++)
                {
                    decodeKey[MagicList[i]] = buffer2[i];
                }
            }
            bsfReader.Close();
            fStream.Close();
        }


        public void Dump(String destinationPath)
        {
            string newName = string.Format("{0}.{1}", Path.GetFileNameWithoutExtension(fileinfo.FullName), this.originalExt);

            string desFileName = Path.Combine(destinationPath, newName);

            if (File.Exists(desFileName))
            {
                File.Delete(desFileName);
            }

            FileStream fsreadFile = new FileStream(fileinfo.FullName, FileMode.Open);

            FileStream fswriteFile = new FileStream(desFileName, FileMode.CreateNew);
            BinaryReader bsreadFile = new BinaryReader(fsreadFile);
            BinaryWriter bswriteFile = new BinaryWriter(fswriteFile);

            byte[] buffer = new byte[8192];
            int readSize = 0;
            int offset = 0;
            do
            {
                readSize = bsreadFile.Read(buffer, 0, 8192);
                for (int i = 0; i < 8192; i++)
                {
                    buffer[i] ^= GetKey(offset + i);
                }
                offset += readSize;
                bswriteFile.Write(buffer);
            } while (readSize > 0);

            bswriteFile.Close();
            bsreadFile.Close();
            fswriteFile.Close();
        }

        private byte GetKey(int v)
        {
            if (v >= 0)
            {
                if (v > 0x7fff)
                    v %= 0x7fff;
            }
            else
            {
                v = 0;
            }

            return Convert.ToByte(decodeKey[(v * v + 80923) % 256]);
        }
    }
}
