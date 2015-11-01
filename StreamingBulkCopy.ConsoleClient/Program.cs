using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace StreamingBulkCopy.ConsoleClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("starting bulk import");
            
            var startTime = DateTime.Now;

            //1000000_Data_w_headers.csv
            var sbk = new StreamingBulkCopy<Data>("D:\\Projects\\StreamingBulkCopy\\StreamingBulkCopy.ConsoleClient\\1000000_Data_wout_headers.csv");

            //var data = sbk.GetData();
            var data = sbk.BuildDtosFromDataInImportFile();

            var dataReader = new EnumerableDataReader<Data>(data);
            //var dataReader = new EnumerableDataReader<Data>(data, new[] { "Field1", "Field2", "Field3" });

            sbk.WriteToDatabase(dataReader);
            
            Console.WriteLine("bulk import finished in: {0} seconds", (DateTime.Now - startTime).TotalSeconds);
            Console.ReadLine();
        }

        //var watch = new Stopwatch();
        //watch.Start();
        //do operation here
        //watch.Stop();
        //Console.WriteLine(watch.ElapsedMilliseconds);
    }
}

