using System;
using System.Configuration;
using System.IO;

namespace BTreeReader
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string path = ConfigurationManager.AppSettings["path"];
                if (String.IsNullOrEmpty(path))
                    throw new ArgumentException("Unknown file path.");
                if (!File.Exists(path))
                    throw new ArgumentException("File is not exist.");
                using (StreamReader sr = new StreamReader(path))
                {
                    Tree tree = Common.BuildTree(sr);
                    tree.Print();
                    Console.ReadKey();
                }
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
    }
}
