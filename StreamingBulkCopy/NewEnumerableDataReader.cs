using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;

namespace StreamingBulkCopy
{
    public class NewEnumerableDataReader<TResult> : IDataReader
    {
        private readonly IEnumerable<TResult> items;
        //private readonly PropertyMapping[] propertyMappings;
        private bool disposed;
        private IEnumerator<TResult> enumerator;
        private readonly Dictionary<int, PropertyInfo> ordinalToPropertyInfo = new Dictionary<int, PropertyInfo>();

        public NewEnumerableDataReader(IEnumerable<TResult> items)
        {
            if (null == items)
                throw new ArgumentNullException("items");

            this.items = items;

            //we're making the assumption here that the items collection passed to us is in the same order as TRsult.GetProps is going to return to us
            var properties = typeof(TResult).GetProperties();
            var i = 0;
            foreach (var property in properties)
            {
                ordinalToPropertyInfo.Add(i, property);
                i++;
            }
        }

        //need to be able to get the ordinal by the field name?
        public int GetOrdinal(string name)
        {
            this.EnsureNotDisposed();
            var ordinal = ordinalToPropertyInfo.FirstOrDefault(x => x.Value.Name == name).Key;
            return ordinal;
        }

        public object GetValue(int i)
        {
            this.EnsureNotDisposed();
            PropertyInfo propertyInfo;
            ordinalToPropertyInfo.TryGetValue(0, out propertyInfo);
            var value = propertyInfo.GetValue(this.enumerator.Current);
            return value;
        }

        public string GetName(int i)
        {
            this.EnsureNotDisposed();
            PropertyInfo propertyInfo;
            ordinalToPropertyInfo.TryGetValue(i, out propertyInfo);
            return propertyInfo.Name;
        }

        public int FieldCount
        {
            get
            {
                this.EnsureNotDisposed();
                //return this.propertyMappings.Length; //this returns 3 on the bulk-writer examples
                return this.ordinalToPropertyInfo.Count;
            }
        }

        public TResult Current
        {
            get
            {
                this.EnsureNotDisposed();
                return (null != this.enumerator) ? this.enumerator.Current : default(TResult);
            }
        }

        public bool Read()
        {
            this.EnsureNotDisposed();

            if (null == this.enumerator)
            {
                this.enumerator = this.items.GetEnumerator();
            }

            return this.enumerator.MoveNext();
        }

        public bool IsDBNull(int i)
        {
            this.EnsureNotDisposed();

            object value = this.GetValue(i);
            return (null == value);
        }

        public void Dispose()
        {
            if (null != this.enumerator)
            {
                this.enumerator.Dispose();
                this.enumerator = null;
            }

            this.disposed = true;
        }

        public void Close()
        {
            this.Dispose();
        }

        private void EnsureNotDisposed()
        {
            if (this.disposed)
            {
                throw new ObjectDisposedException("EnumerableDataReader");
            }
        }

        #region Not used by NewEnumerableDataReader

        public string GetDataTypeName(int i)
        {
            throw new NotSupportedException();
        }

        public Type GetFieldType(int i)
        {
            throw new NotSupportedException();
        }

        public int GetValues(object[] values)
        {
            throw new NotSupportedException();
        }

        public bool GetBoolean(int i)
        {
            throw new NotSupportedException();
        }

        public byte GetByte(int i)
        {
            throw new NotSupportedException();
        }

        public long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
        {
            throw new NotSupportedException();
        }

        public char GetChar(int i)
        {
            throw new NotSupportedException();
        }

        public long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length)
        {
            throw new NotSupportedException();
        }

        public Guid GetGuid(int i)
        {
            throw new NotSupportedException();
        }

        public short GetInt16(int i)
        {
            throw new NotSupportedException();
        }

        public int GetInt32(int i)
        {
            throw new NotSupportedException();
        }

        public long GetInt64(int i)
        {
            throw new NotSupportedException();
        }

        public float GetFloat(int i)
        {
            throw new NotSupportedException();
        }

        public double GetDouble(int i)
        {
            throw new NotSupportedException();
        }

        public string GetString(int i)
        {
            throw new NotSupportedException();
        }

        public decimal GetDecimal(int i)
        {
            throw new NotSupportedException();
        }

        public DateTime GetDateTime(int i)
        {
            throw new NotSupportedException();
        }

        public IDataReader GetData(int i)
        {
            throw new NotSupportedException();
        }

        object IDataRecord.this[int i]
        {
            get { throw new NotSupportedException(); }
        }

        object IDataRecord.this[string name]
        {
            get { throw new NotSupportedException(); }
        }

        public DataTable GetSchemaTable()
        {
            throw new NotSupportedException();
        }

        public bool NextResult()
        {
            throw new NotSupportedException();
        }

        [SuppressMessage("Microsoft.Design", "CA1065:DoNotRaiseExceptionsInUnexpectedLocations")]
        public int Depth
        {
            get { throw new NotSupportedException(); }
        }

        [SuppressMessage("Microsoft.Design", "CA1065:DoNotRaiseExceptionsInUnexpectedLocations")]
        public bool IsClosed
        {
            get { throw new NotSupportedException(); }
        }

        [SuppressMessage("Microsoft.Design", "CA1065:DoNotRaiseExceptionsInUnexpectedLocations")]
        public int RecordsAffected
        {
            get { throw new NotSupportedException(); }
        }

        #endregion
    }
}
