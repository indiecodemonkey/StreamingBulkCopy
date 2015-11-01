- Implmentation of a streaming bulk copy form Jimmy Bogard's NSB Con 2014 presentation: "Scaling NServiceBus" (http://fast.wistia.net/embed/iframe/y56svovwnk?popover=true)
- The EnumerableDataReader class is copyied from this StackOverflow posting: http://stackoverflow.com/questions/2258310/get-an-idatareader-from-a-typed-list
- Another implementation of IDataReader: http://blogs.msdn.com/b/anthonybloesch/archive/2013/01/23/bulk-loading-data-with-idatareader-and-sqlbulkcopy.aspx
- Links to Microsoft's implementation of IDataReader: 
- https://msdn.microsoft.com/en-us/library/5awcy9t0(v=vs.71).aspx
- https://msdn.microsoft.com/en-us/library/aa720693(v=vs.71).aspx

benchmarks
==========
GetData
-------
- for import file w/out headers, one million inserts in 8.740 seconds

GetData<T> (uses reflection)
----------
- for import file w/out headers, one million inserts in 11.453 seconds