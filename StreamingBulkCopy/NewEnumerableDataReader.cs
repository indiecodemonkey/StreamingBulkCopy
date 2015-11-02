using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;

namespace StreamingBulkCopy
{
    public class NewEnumerableDataReader<T> : IDataReader
    {
        private readonly IEnumerable<T> items;
        private bool disposed;
        private IEnumerator<T> enumerator;
        private readonly Dictionary<int, PropertyInfo> ordinalToPropertyInfo = new Dictionary<int, PropertyInfo>();

        public NewEnumerableDataReader(IEnumerable<T> items)
        {
            if (null == items)
                throw new ArgumentNullException("items");

            this.items = items;

            //we're making the assumption here that the items collection passed to us is in the same order as TRsult.GetProps is going to return to us
            var properties = typeof(T).GetProperties();
            var i = 0;
            foreach (var property in properties)
            {
                ordinalToPropertyInfo.Add(i, property);
                i++;
            }
        }

        public object GetValue(int i)
        {
            this.EnsureNotDisposed();
            PropertyInfo propertyInfo;
            if(!ordinalToPropertyInfo.TryGetValue(i, out propertyInfo))
                throw new InvalidOperationException(string.Format("Cannot GetValue for '{0}' because the key does not exist in ordinalToPropertyInfo", i));
            var value = propertyInfo.GetValue(this.enumerator.Current);
            return value;
        }

        //this icsn't currently being used
        public string GetName(int i)
        {
            this.EnsureNotDisposed();
            PropertyInfo propertyInfo;
            if(!ordinalToPropertyInfo.TryGetValue(i, out propertyInfo))
                throw new InvalidOperationException(string.Format("Cannot GetName for '{0}' because the key does not exist in ordinalToPropertyInfo", i));
            return propertyInfo.Name;
        }

        //this isn't currently being used
        public int GetOrdinal(string name)
        {
            this.EnsureNotDisposed();
            //var ordinal = ordinalToPropertyInfo.FirstOrDefault(x => x.Value.Name == name).Key;

            //TODO: either create a new dictionary of PropertyName/Ordinal, or test this code to see if looking up a key in dictionary by value will work
            //since the values being populated into ordinalToPropertyInfo are coming from dto's created in StreamingBulkCopy, you can't have a single class with matching property names,
            //that won't compile, so we should not have to worry about non-unique values in the ordinalToPropertyInfo dictionary
            if (ordinalToPropertyInfo.All(x => x.Value.Name != name))
                throw new InvalidOperationException(string.Format("Cannot get the key for value '{0}' in ordinalToPropertyInfo", name));

            var ordinal = ordinalToPropertyInfo.First(x => x.Value.Name == name).Key;
            return ordinal;
        }

        public int FieldCount
        {
            get
            {
                this.EnsureNotDisposed();
                return this.ordinalToPropertyInfo.Count;
            }
        }

        public T Current
        {
            get
            {
                this.EnsureNotDisposed();
                return (null != this.enumerator) ? this.enumerator.Current : default(T);
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

        #region not used

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
