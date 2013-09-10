using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace TekConf.Web
{
    public class GravatarImage
    {
        private const string _url = "http://www.gravatar.com/avatar.php?gravatar_id=";

        /// <summary&gr;
        /// Get the URL of the image
        /// </summary&gr;
        /// <param name="email"&gr;The email address</param&gr;
        /// <param name="size"&gr;The size of the image (1 - 600)</param&gr;
        /// <param name="rating"&gr;The MPAA style rating(g, pg, r, x)</param&gr;
        /// <returns>The image URL</returns&gr;
        public string GetURL(string email, int size, string rating)
        {
            email = email.ToLower();
            email = getMd5Hash(email);

            if (size < 1 | size > 600)
            {
                throw new ArgumentOutOfRangeException("size",
                    "The image size should be between 20 and 80");
            }

            rating = rating.ToLower();
            if (rating != "g" & rating != "pg" & rating != "r" & rating != "x")
            {
                throw new ArgumentOutOfRangeException("rating",
                    "The rating can only be one of the following: g, pg, r, x");
            }

            return _url + email + "&s=" + size.ToString() + "&r=" + rating;
        }

        /// <summary&gr;
        /// Hash an input string and return the hash as a 32 character hexadecimal string
        /// </summary&gr;
        /// <param name="input"&gr;The string to hash</param&gr;
        /// <returns&gr;The MD5 hash</returns&gr;
        public string getMd5Hash(string input)
        {
            // Create a new instance of the MD5CryptoServiceProvider object.
            MD5 md5Hasher = MD5.Create();

            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            return sBuilder.ToString();  // Return the hexadecimal string.
        }
    }
}