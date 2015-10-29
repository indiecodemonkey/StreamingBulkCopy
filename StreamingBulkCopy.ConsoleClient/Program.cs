using System;

namespace StreamingBulkCopy.ConsoleClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("starting bulk import");
            
            var startTime = DateTime.Now;

            var sbk = new StreamingBulkCopy("D:\\Projects\\StreamingBulkCopy\\StreamingBulkCopy.ConsoleClient\\1000000_Data_wout_headers.csv");
            //var sbk = new StreamingBulkCopy("D:\\Projects\\StreamingBulkCopy\\StreamingBulkCopy.ConsoleClient\\1000000_Data_w_headers.csv");

            //var data = sbk.GetData();
            var data = sbk.GetData<Data>();

            var enumDataReader = new EnumerableDataReader<Data>(data);
            //var enumDataReader = new EnumerableDataReader<Data>(data, new[] { "Field1", "Field2", "Field3" });

            sbk.WriteToDatabase(enumDataReader);
            
            Console.WriteLine("bulk import finished in: {0} seconds", (DateTime.Now - startTime).TotalSeconds);
            Console.ReadLine();
        }
    }
}

