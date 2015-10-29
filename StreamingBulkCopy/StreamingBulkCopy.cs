using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;

namespace StreamingBulkCopy
{
    public class StreamingBulkCopy
    {
        private readonly string importFile;

        public StreamingBulkCopy(string importFile)
        {
            this.importFile = importFile;
        }

        public IEnumerable<Data> GetData()
        {
            //stream the file out first using LINQ
            //this method reads one line at a time, and returns an IEnumerable of string
            //it only every loads one line into memory at a time
            return File.ReadLines(importFile)
                .Select(line => line.Split(','))
                .Select(split => new Data
                {
                    Field1 = split[0],
                    Field2 = split[1],
                    Field3 = split[2]
                });
        }

        public void WriteToDatabase(IDataReader dataReader)
        {
            using (var bulkCopy = new SqlBulkCopy(@"Data Source=(LocalDB)\MSSQLLocalDB; Initial Catalog=StreamingBulkCopy; Integrated Security=True;"))
            {
                bulkCopy.DestinationTableName = "dbo.Data";
                bulkCopy.EnableStreaming = true;
                bulkCopy.WriteToServer(dataReader);
            }
        }
    }
}

