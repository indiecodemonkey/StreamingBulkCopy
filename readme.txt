TODO
====
- get IEnumerable<Kit> GetKits() feeding the DataReader implementation
- MyDataReader changes/optimzations
	- FieldCount should return the count of columns in the file being read in from the stream.
	- support different data types besdies string
	- any additional speed that can be pulled out of the optimization
	- support for different formats... .csv, pipe deliminited, etc...
- 