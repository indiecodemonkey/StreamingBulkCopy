Implmentation of a streaming bulk copy form Jimmy Bogard's NSB Con 2014 presentation: "Scaling NServiceBus" (http://fast.wistia.net/embed/iframe/y56svovwnk?popover=true)
The EnumerableDataReader class is copyied from this StackOverflow posting: http://stackoverflow.com/questions/2258310/get-an-idatareader-from-a-typed-list

metrics
=======
- for import file w/out headers, one million inserts in 8.224 seconds
- for import file with headers, one million inserts in 7.975 seconds

todo
====
- use : this() for constructors chaining on the EnumerableDataReader to get rid of duplicate constructor code