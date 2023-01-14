using System;
using System.IO;

namespace NvidiaCleaner.Lib
{
    class NVC
    {
        //Calculates the directory size by checking each and every files' size's in directory and sub-directories.
        public static long GetDirectorySize(string path)
        {
            string[] a = Directory.GetFiles(path, "*.*", SearchOption.AllDirectories);
            long b = 0;
            foreach (var name in a)
            {
                FileInfo info = new FileInfo(name);
                b += info.Length;
            }
            return b;
        }

        //Byte to desired significance converter. KB, MB, GB only.
        public static double SizeConvert(long b, string desired)
        {
            long sizeKB = b / 1024;
            int sizeMB = (int)sizeKB / 1024;
            double sizeGB = sizeMB / 1024.0;

            switch (desired)
            {
                case "KB":
                    return sizeKB;
                case "MB":
                    return sizeMB;
                case "GB":
                    return sizeGB;
                default:
                    return 0.0;
            }
        }

    }
}
