using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Linq.Expressions;

namespace StreamingBulkCopy
{
    public class StreamingBulkCopy<T> where T : new()
    {
        private readonly string importFile;
        public static readonly Func<T> instance = Expression.Lambda<Func<T>>(Expression.New(typeof(T))).Compile();

        public StreamingBulkCopy(string importFile)
        {
            this.importFile = importFile;
        }

        public void WriteToDatabase()
        {
            using (var bulkCopy = new SqlBulkCopy(@"Data Source=(LocalDB)\MSSQLLocalDB; Initial Catalog=StreamingBulkCopy; Integrated Security=True;"))
            {
                //var data = sbk.GetData();
                var dataReader = new EnumerableDataReader<T>(BuildDtosFromImportFile());

                bulkCopy.DestinationTableName = "dbo.Data";
                bulkCopy.EnableStreaming = true; //this can only be set to true when streaming data from an IDataReader

                bulkCopy.WriteToServer(dataReader);
            }
        }

        private static class New<T> where T : new()
        {
            public static readonly Func<T> Instance = Expression.Lambda<Func<T>>(Expression.New(typeof(T))).Compile();
        }

        private static T NewT()
        {
            //apparently, using compiled expressions is the quickest way to create new instances of T
            //http://stackoverflow.com/questions/6582259/fast-creation-of-objects-instead-of-activator-createinstancetype

            //this is benchmarked around 1800ms for classes
            //return new T();

            //this is benchmarked at 50ms for classes
            //return instance.Invoke();

            //using the static New<T> class, which once the expression is compiled, caches it,  is the fastest
            return New<T>.Instance();

            //my benchmarks found that using instance.Invoke was slower than new T, but the cached, compiled expression was the fastest
            //new T:			11.160 seconds
            //instance.Invoke()	11.253 seconds
            //New<T> class:     10.851 seconds
        }

        private IEnumerable<T> BuildDtosFromImportFile()
        {

            var listOfT = new List<T>();
            var lines = File.ReadLines(importFile);
            var propertyInfoOfT = typeof(T).GetProperties();

            foreach (var line in lines)
            {
                var data = line.Split(',');
                var i = 0;
                var newT = NewT();

                foreach (var propertyInfo in propertyInfoOfT)
                {
                    propertyInfo.SetValue(newT, data[i], null);
                    i++;
                }
                listOfT.Add(newT);
            }
            return listOfT;            
        }

        private IEnumerable<Data> GetData()
        {
            //stream the file out first using LINQ
            //File.ReadLines method reads one line at a time, and returns an IEnumerable of string
            //it only every loads one line into memory at a time
            //https://msdn.microsoft.com/en-us/library/dd383503(v=vs.110).aspx: "The ReadLines and ReadAllLines methods differ as follows: When you use ReadLines, you can start enumerating the collection of strings before the whole collection is returned; when you use ReadAllLines, you must wait for the whole array of strings be returned before you can access the array. Therefore, when you are working with very large files, ReadLines can be more efficient."
            return File.ReadLines(importFile)
                .Select(line => line.Split(','))
                .Select(split => new Data { Field1 = split[0], Field2 = split[1], Field3 = split[2] });
        }
    }
}

