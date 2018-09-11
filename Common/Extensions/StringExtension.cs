using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Common.Extensions
{
    public static class StringExtension
    {
        public static string LeaveJustDigits(this string item)
        {
            try
            {
                Regex rgx = new Regex(@"[^0-9]");
                var str = rgx.Replace(item, "");

                return str;
            }
            catch
            {
                return item;
            }
        }

        public static string ClearEmail(this string email)
        {
            Regex rgx = new Regex(@"[^a-zA-Z0-9\._\-@]");
            var str = rgx.Replace(email, "");

            return str;
        }
    }
}
