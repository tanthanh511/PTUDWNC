﻿using System.Text.RegularExpressions;

namespace TatBlog.WebApi.Models
{
    public static class ExtensionMethods
    {
        public static string GenerateSlug(this string phrase)
        {
            string str = RemoveAccent(phrase.ToString()).ToLower();

            // Invalid chars           
            str = Regex.Replace(str, @"[^a-z0-9\s-]", "");

            // Convert multiple spaces into one space   
            str = Regex.Replace(str, @"\s+", " ").Trim();

            // Cut and trim 
            str = str.Substring(0, str.Length <= 45 ? str.Length : 45).Trim();
            str = Regex.Replace(str, @"\s", "-"); // hyphens

            return str;
        }

        public static string RemoveAccent(this string txt)
        {
            byte[] bytes = System.Text.Encoding.GetEncoding("Cyrillic").GetBytes(txt);
            return System.Text.Encoding.ASCII.GetString(bytes);
        }
    }
}
