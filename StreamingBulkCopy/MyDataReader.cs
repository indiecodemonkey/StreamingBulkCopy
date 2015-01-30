using System;
using System.Data;
using System.IO;

namespace StreamingBulkCopy
{
    public class MyDataReader : IDataReader
    {
        protected StreamReader Stream { get; set; }
        protected object[] Values;
        protected bool Eof { get; set; }
        protected string CurrentRecord { get; set; }
        protected int CurrentIndex { get; set; }

        public MyDataReader()
        {
            Stream = new StreamReader( "D:\\Projects\\StreamingBulkCopy\\1000000_KitList.csv");
            Values = new object[this.FieldCount];
        }

        public int GetValues(object[] values)
        {
            Fill(values);
            Array.Copy(values, Values, this.FieldCount);
            return this.FieldCount;
        }

        private void Fill(object[] values)
        {
            //To simplify the implementation, lets assume here that the table have just 3         
            //columns: the primary key, and 2 string columns. And the file is fixed column formatted 
            //and have 2 columns: the first with width 12 and the second with width 40. Said that, we can do as follows

            //values[0] = null;
            //values[1] = CurrentRecord.Substring(0, 12).Trim();
            //values[2] = CurrentRecord.Substring(12, 40).Trim();

            var currentRecordArray = CurrentRecord.Split(',');
            values[0] = currentRecordArray[0].Trim();
            values[1] = currentRecordArray[1].Trim();
            values[2] = currentRecordArray[2].Trim();

            // by default, the first position of the array hold the value that will be  
            // inserted at the first column of the table, and so on
            // lets assume here that the primary key is auto-generated
            // if the file is xml we could parse the nodes instead of Substring operations
        } 

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public string GetName(int i)
        {
            return Values[i].ToString();
        }

        public string GetDataTypeName(int i)
        {
            return "String";
        }

        public Type GetFieldType(int i)
        {
            throw new NotImplementedException();
        }

        public object GetValue(int i)
        {
            return Values[i];
        }

        public int GetOrdinal(string name)
        {
            throw new NotImplementedException();
        }

        public bool GetBoolean(int i)
        {
            throw new NotImplementedException();
        }

        public byte GetByte(int i)
        {
            throw new NotImplementedException();
        }

        public long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
        {
            throw new NotImplementedException();
        }

        public char GetChar(int i)
        {
            throw new NotImplementedException();
        }

        public long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length)
        {
            throw new NotImplementedException();
        }

        public Guid GetGuid(int i)
        {
            throw new NotImplementedException();
        }

        public short GetInt16(int i)
        {
            throw new NotImplementedException();
        }

        public int GetInt32(int i)
        {
            throw new NotImplementedException();
        }

        public long GetInt64(int i)
        {
            throw new NotImplementedException();
        }

        public float GetFloat(int i)
        {
            throw new NotImplementedException();
        }

        public double GetDouble(int i)
        {
            throw new NotImplementedException();
        }

        public string GetString(int i)
        {
            return Values[i].ToString();
        }

        public decimal GetDecimal(int i)
        {
            throw new NotImplementedException();
        }

        public DateTime GetDateTime(int i)
        {
            throw new NotImplementedException();
        }

        public IDataReader GetData(int i)
        {
            if (i == 0)
                return this;

            return null;
        }

        public bool IsDBNull(int i)
        {
            return false;
        }

        public int FieldCount
        {
            //assuming the table has 3 columns 
            get { return 3; }
        }

        object IDataRecord.this[int i]
        {
            get { return Values[i]; }
        }

        object IDataRecord.this[string name]
        {
            get { throw new NotImplementedException(); }
        }

        public void Close()
        {
            Array.Clear(Values, 0, Values.Length);
            Stream.Close();
            Stream.Dispose();
        }

        public DataTable GetSchemaTable()
        {
            throw new NotImplementedException();
        }

        public bool NextResult()
        {
            return false;
        }

        public bool Read()
        {
            CurrentRecord = Stream.ReadLine();
            Eof = CurrentRecord == null;

            if (!Eof)
            {
                Fill(Values);
                CurrentIndex++;
            }

            return !Eof;
        }

        public int Depth
        {
            get { return 0; }
    }

        public bool IsClosed
        {
            get { return Eof; } 
        }

        public int RecordsAffected
        {
            get { return -1; }
        }
    }
}