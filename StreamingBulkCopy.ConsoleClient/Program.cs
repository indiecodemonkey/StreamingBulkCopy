using System;

namespace StreamingBulkCopy.ConsoleClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("starting bulk import");
            
            var startTime = DateTime.Now;
            var sbk = new StreamingBulkCopy();
            sbk.WriteToDatabase(new MyDataReader());
            var endTime = DateTime.Now;
            
            Console.WriteLine("bulk import finished in: {0} seconds", (endTime - startTime).TotalSeconds);
            Console.ReadLine();
        }
    }
}
