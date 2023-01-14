using System;

namespace WinMan.Util
{
    public class Functions
    {
        public static string HumanReadableSize(long size)
        {
            string[] sizes = { "B", "KB", "MB", "GB", "TB" };
            int order = 0;
            while (size >= 1024 && order < sizes.Length - 1)
            {
                order++;
                size = size / 1024;
            }

            string result = String.Format("{0:0.##} {1}", size, sizes[order]);
            return result;
        }
    }
}
