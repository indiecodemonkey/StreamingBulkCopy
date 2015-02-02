using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;

namespace StreamingBulkCopy
{
    public class StreamingBulkCopy
    {
        public static IEnumerable<Kit> GetKits()
        {
            //stream the file out first using LINQ
            //this method reads one line at a time, and returns an IEnumerable of string
            //it only every loads one line into memory at a time
            return File.ReadLines("D:\\Projects\\StreamingBulkCopy\\1000000_KitList.csv")
                .Select(line => line.Split(','))
                .Select(split => new Kit
                {
                    Sequence = split[0],
                    KitNumber = split[1],
                    KitType = split[2]
                });
        }

        public void WriteToDatabase(IDataReader dataReader)
        {
            using (var bulkCopy = new SqlBulkCopy(@"Data Source=US-SO-ACT-01027\SQLEXPRESS; Initial Catalog=StreamingBulkCopy; Integrated Security=True;"))
            {
                bulkCopy.DestinationTableName = "dbo.Kits";
                bulkCopy.EnableStreaming = true;
                bulkCopy.WriteToServer(dataReader);
            }
        }
    }

    //DTO
    public class Kit
    {
        public string Sequence { get; set; }
        public string KitNumber { get; set; }
        public string KitType { get; set; }
    }
}
