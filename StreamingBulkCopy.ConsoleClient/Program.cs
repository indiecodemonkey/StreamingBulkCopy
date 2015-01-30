using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StreamingBulkCopy.ConsoleClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("starting bulk import");
            
            var startTime = DateTime.Now;
            //var myDataReader = new MyDataReader();
            var sbk = new StreamingBulkCopy();
            sbk.WriteToDatabase(new MyDataReader());
            var endTime = DateTime.Now;
            
            Console.WriteLine("bulk import finished in: {0} seconds", (endTime - startTime).TotalSeconds);
            Console.ReadLine();
        }
    }
}
