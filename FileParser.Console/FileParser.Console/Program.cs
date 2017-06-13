using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileParser.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            
           
            string filePath = string.Empty;
            string sortField = string.Empty;
            string searchText = string.Empty;
            if (args != null)
            {
                for (var i=0; i<args.Length-1; i++)
                {
                    switch (args[i])
                    {
                        case "-file": filePath = args[i + 1]; break;
                        case "-sort": sortField = args[i + 1]; break;
                        case "-search": searchText = args[i + 1]; break;
                        default:
                            break;
                    }
                }

            }
            System.Console.WriteLine("-----------RESULT IS AS FOLLOW--------");
            System.Console.Write(FileParser.ParseFile(filePath,sortField,searchText));
            System.Console.Read();
        }
    }
}
