using System;

namespace Galeri.Web.Utilities
{
    public class UtilityManager
    {
        private static readonly string[] DosyaTipleri = { "excel", "sheet", "pdf", "word", "presentation", "octet-stream", "image", "text", "audio", "video" };
        private static readonly string[] DosyaIconlari = { "fa fa-file-excel-o", "fa fa-file-excel-o", "fa fa-file-pdf-o", "fa fa-file-word-o", "fa fa-file-powerpoint-o", "fa fa-file-archive-o", "fa fa-image", "fa fa-file-text", "fa fa-sound-o", "fa fa-play" };
        private static readonly string[] DosyaClass = { "bg-green-2", "bg-green-2", "bg-pdf", "bg-blue", "bg-orange", "bg-archive", "bg-archive", "bg-archive", "bg-sound", "bg-sound" };

        public static byte[] ByteBirlestir(byte[] previous, byte[] next)
        {
            byte[] output = new byte[previous.Length + next.Length];
            Buffer.BlockCopy(previous, 0, output, 0, previous.Length);
            Buffer.BlockCopy(next, 0, output, previous.Length, next.Length);
            return output;
        }

        public static string SetIcon(string contentType)
        {
            for (int i = 0; i < DosyaTipleri.Length; i++)
            {
                if (contentType.Contains(DosyaTipleri[i]))
                {
                    return DosyaIconlari[i];
                }
            }
            return "fa fa-file-o";
        }

        public static string BytesToString(long byteCount)
        {
            string[] suf = { "B", "KB", "MB", "GB", "TB", "PB", "EB" };

            long bytes = Math.Abs(byteCount);
            int place = Convert.ToInt32(Math.Floor(Math.Log(bytes, 1024)));
            double num = Math.Round(bytes / Math.Pow(1024, place), 1);
            return (Math.Sign(byteCount) * num) + suf[place];
        }

        public static string SetClass(string contentType)
        {
            for (int i = 0; i < DosyaTipleri.Length; i++)
            {
                if (contentType.Contains(DosyaTipleri[i]))
                {
                    return DosyaClass[i];
                }
            }
            return "bg-green-2";
        }
    }
}