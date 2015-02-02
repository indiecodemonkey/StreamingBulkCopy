using System;
using System.Linq;

namespace StreamingBulkCopy.ConsoleClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("starting bulk import");
            
            var startTime = DateTime.Now;
            //var sbk = new StreamingBulkCopy();
            //sbk.WriteToDatabase(new MyDataReader());

            var sbk = new StreamingBulkCopy();
            var kits = StreamingBulkCopy.GetKits();
            
            //this is assuming headers are NOT included in the file, if they are, we should not have to specify header names
            var enumDataReader = new EnumerableDataReader<Kit>(kits, new[] { "Sequence", "KitNumber", "KitType" });
            sbk.WriteToDatabase(enumDataReader);

            var endTime = DateTime.Now;
            
            Console.WriteLine("bulk import finished in: {0} seconds", (endTime - startTime).TotalSeconds);
            Console.ReadLine();
        }
    }
}

