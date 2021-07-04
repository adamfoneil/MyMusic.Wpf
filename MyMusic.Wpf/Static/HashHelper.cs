using System;
using System.Security.Cryptography;
using System.Text;

namespace MyMusic.Wpf.Static
{
    public static class HashHelper
    {
        public static string Md5(string input)
        {
            using (var md5 = MD5.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(input);
                return Convert.ToBase64String(md5.ComputeHash(bytes));
            }
        }
    }
}
