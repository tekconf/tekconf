using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Net;
using FluentMongo.Linq;
using ServiceStack.Text;
using TekConf.UI.Api;

namespace CodeStock
{
    class Program
    {
        static void Main(string[] args)
        {
            
        }
    }


   

    public static class StringExtensions
    {
        public static string SafeSubstring(this string input, int startIndex, int length)
        {
            // Todo: Check that startIndex + length does not cause an arithmetic overflow
            if (input.Length >= (startIndex + length))
            {
                return input.Substring(startIndex, length);
            }
            else
            {
                if (input.Length > startIndex)
                {
                    return input.Substring(startIndex);
                }
                else
                {
                    return string.Empty;
                }
            }
        }
    }

}