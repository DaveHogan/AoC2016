using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2016
{
    public class Day5Part2
    {
        internal static void Execute()
        {
            using (MD5 md5Hash = MD5.Create())
            {
                var pw = new char[8];
                int i=0;
                int stopAfterXTimes = 8;
                while (true)
                {
                    var input = "uqwqemis" + i.ToString();
                    var hash = GetMd5Hash(md5Hash, input);
                    if (hash.StartsWith("00000"))
                    {
                        var pos = 0;
                        var success = int.TryParse(hash.Substring(5, 1).ToCharArray().First().ToString(), out pos);
                        if (success && pos < 8)
                        {
                            var seventh = hash.Substring(6, 1).ToCharArray().First();
                            if (pw[pos] == 0) // Use only the first result for each position
                            {
                                pw[pos] = seventh;
                                stopAfterXTimes--;
                            }
                            if (stopAfterXTimes == 0)
                                break;
                        }
                    }
                    i++;
                }

                Console.WriteLine(String.Join("",pw));

            }
        }

        static
            string GetMd5Hash(MD5 md5Hash, string input)
        {

            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }

       
    }
}
