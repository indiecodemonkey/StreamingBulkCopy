using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace StreamingBulkCopy
{
    public class EnumerableDataReader<T> : IDataReader
    {
        private IEnumerator<T> enumerator;
        private T current;
        private bool enumeratorState;
        private readonly List<BaseField> baseFields = new List<BaseField>();

        public EnumerableDataReader(IEnumerable<T> collection, params string[] fieldNames)
        {
            VerifyCollectionAndGetEnumerator(collection);
            SetFields(fieldNames);
        }

        public EnumerableDataReader(IEnumerable<T> collection)
        {
            enumerator = VerifyCollectionAndGetEnumerator(collection);

            //we call this to forward the current enumerator 1 row past the headers row, so the headers are not imported
            enumeratorState = enumerator.MoveNext();

            var typeInfo = typeof(T).GetTypeInfo();
            var propertyInfoList = typeInfo.DeclaredProperties;
            var fieldNames = propertyInfoList.Select(propertyInfo => propertyInfo.Name).ToList();

            SetFields(fieldNames);
        }

        private IEnumerator<T> VerifyCollectionAndGetEnumerator(IEnumerable<T> collection)
        {
            if (collection == null)
                throw new ArgumentNullException("collection");

            enumerator = collection.GetEnumerator();

            if (enumerator == null)
                throw new NullReferenceException("collection does not implement GetEnumerator");

            return enumerator;
        }

        private void SetFields(ICollection<string> fields)
        {
            if (fields.Count > 0)
            {
                var type = typeof(T);
                foreach (var field in fields)
                {
                    var properyInfo = type.GetProperty(field);
                    if (properyInfo != null)
                        this.baseFields.Add(new Property(properyInfo));
                    else
                    {
                        var fieldInfo = type.GetField(field);
                        if (fieldInfo != null)
                            this.baseFields.Add(new Field(fieldInfo));
                        else
                            throw new NullReferenceException(string.Format("EnumerableDataReader<T>: Missing property or field '{0}' in Type '{1}'.", field, type.Name));
                    }
                }
            }
            else
                this.baseFields.Add(new Self());
        }

        #region IDisposable Members

        public void Dispose()
        {
            if (enumerator != null)
            {
                enumerator.Dispose();
                enumerator = null;
                current = default(T);
                enumeratorState = false;
            }
            closed = true;
        }

        #endregion

        #region IDataReader Members

        public void Close()
        {
            closed = true;
        }

        private bool closed = false;

        public int Depth
        {
            get { return 0; }
        }

        public DataTable GetSchemaTable()
        {
            var dt = new DataTable();
            foreach (var field in baseFields)
            {
                dt.Columns.Add(new DataColumn(field.Name, field.Type));
            }
            return dt;
        }

        public bool IsClosed
        {
            get { return closed; }
        }

        public bool NextResult()
        {
            return false;
        }

        public bool Read()
        {
            if (IsClosed)
                throw new InvalidOperationException("DataReader is closed");
            enumeratorState = enumerator.MoveNext();
            current = enumeratorState ? enumerator.Current : default(T);
            return enumeratorState;
        }

        public int RecordsAffected
        {
            get { return -1; }
        }

        #endregion

        #region IDataRecord Members

        public int FieldCount
        {
            get { return baseFields.Count; }
        }

        public Type GetFieldType(int i)
        {
            if (i < 0 || i >= baseFields.Count)
                throw new IndexOutOfRangeException();
            return baseFields[i].Type;
        }

        public string GetDataTypeName(int i)
        {
            return GetFieldType(i).Name;
        }

        public string GetName(int i)
        {
            if (i < 0 || i >= baseFields.Count)
                throw new IndexOutOfRangeException();
            return baseFields[i].Name;
        }

        public int GetOrdinal(string name)
        {
            for (int i = 0; i < baseFields.Count; i++)
                if (baseFields[i].Name == name)
                    return i;
            throw new IndexOutOfRangeException("name");
        }

        public bool IsDBNull(int i)
        {
            return GetValue(i) == null;
        }

        public object this[string name]
        {
            get { return GetValue(GetOrdinal(name)); }
        }

        public object this[int i]
        {
            get { return GetValue(i); }
        }

        public object GetValue(int i)
        {
            if (IsClosed || !enumeratorState)
                throw new InvalidOperationException("DataReader is closed or has reached the end of the enumerator");
            if (i < 0 || i >= baseFields.Count)
                throw new IndexOutOfRangeException();
            return baseFields[i].GetValue(current);
        }

        public int GetValues(object[] values)
        {
            int length = Math.Min(baseFields.Count, values.Length);
            for (int i = 0; i < length; i++)
                values[i] = GetValue(i);
            return length;
        }

        public bool GetBoolean(int i)
        {
            return (bool)GetValue(i);
        }

        public byte GetByte(int i)
        {
            return (byte)GetValue(i);
        }

        public char GetChar(int i)
        {
            return (char)GetValue(i);
        }

        public DateTime GetDateTime(int i)
        {
            return (DateTime)GetValue(i);
        }

        public decimal GetDecimal(int i)
        {
            return (decimal)GetValue(i);
        }

        public double GetDouble(int i)
        {
            return (double)GetValue(i);
        }

        public float GetFloat(int i)
        {
            return (float)GetValue(i);
        }

        public Guid GetGuid(int i)
        {
            return (Guid)GetValue(i);
        }

        public short GetInt16(int i)
        {
            return (short)GetValue(i);
        }

        public int GetInt32(int i)
        {
            return (int)GetValue(i);
        }

        public long GetInt64(int i)
        {
            return (long)GetValue(i);
        }

        public string GetString(int i)
        {
            return (string)GetValue(i);
        }

        public long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
        {
            throw new NotSupportedException();
        }

        public long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length)
        {
            throw new NotSupportedException();
        }

        public IDataReader GetData(int i)
        {
            throw new NotSupportedException();
        }

        #endregion

        #region Helper Classes

        private abstract class BaseField
        {
            public abstract Type Type { get; }
            public abstract string Name { get; }
            public abstract object GetValue(T instance);
            private static Dictionary<string, Func<T, object>> getterDictionary = new Dictionary<string, Func<T, object>>();

            protected static void AddGetter(Type classType, string fieldName, Func<T, object> getter)
            {
                getterDictionary.Add(string.Concat(classType.FullName, fieldName), getter);
            }

            protected static Func<T, object> GetGetter(Type classType, string fieldName)
            {
                Func<T, object> getter;
                if (getterDictionary.TryGetValue(string.Concat(classType.FullName, fieldName), out getter))
                    return getter;
                return null;
            }
        }

        private class Property : BaseField
        {
            private readonly PropertyInfo propertyInfo;
            private readonly Func<T, object> dynamicGetter;

            public Property(PropertyInfo info)
            {
                propertyInfo = info;
                dynamicGetter = CreateGetMethod(info);
            }

            public override Type Type
            {
                get { return propertyInfo.PropertyType; }
            }

            public override string Name
            {
                get { return propertyInfo.Name; }
            }

            public override object GetValue(T instance)
            {
                //return m_Info.GetValue(instance, null); // Reflection is slow
                return dynamicGetter(instance);
            }

            // Create dynamic method for faster access instead via reflection
            private Func<T, object> CreateGetMethod(PropertyInfo propertyInfo)
            {
                Type classType = typeof(T);
                Func<T, object> dynamicGetter = GetGetter(classType, propertyInfo.Name);
                if (dynamicGetter == null)
                {
                    ParameterExpression instance = Expression.Parameter(classType);
                    MemberExpression property = Expression.Property(instance, propertyInfo);
                    UnaryExpression convert = Expression.Convert(property, typeof(object));
                    dynamicGetter = (Func<T, object>)Expression.Lambda(convert, instance).Compile();
                    AddGetter(classType, propertyInfo.Name, dynamicGetter);
                }

                return dynamicGetter;
            }
        }

        private class Field : BaseField
        {
            private readonly FieldInfo fieldInfo;
            private readonly Func<T, object> dynamicGetter;

            public Field(FieldInfo info)
            {
                fieldInfo = info;
                dynamicGetter = CreateGetMethod(info);
            }

            public override Type Type
            {
                get { return fieldInfo.FieldType; }
            }

            public override string Name
            {
                get { return fieldInfo.Name; }
            }

            public override object GetValue(T instance)
            {
                //return m_Info.GetValue(instance); // Reflection is slow
                return dynamicGetter(instance);
            }

            // Create dynamic method for faster access instead via reflection
            private Func<T, object> CreateGetMethod(FieldInfo fieldInfo)
            {
                Type classType = typeof(T);
                Func<T, object> dynamicGetter = GetGetter(classType, fieldInfo.Name);
                if (dynamicGetter == null)
                {
                    ParameterExpression instance = Expression.Parameter(classType);
                    MemberExpression property = Expression.Field(instance, fieldInfo);
                    UnaryExpression convert = Expression.Convert(property, typeof(object));
                    dynamicGetter = (Func<T, object>)Expression.Lambda(convert, instance).Compile();
                    AddGetter(classType, fieldInfo.Name, dynamicGetter);
                }

                return dynamicGetter;
            }
        }

        private class Self : BaseField
        {
            private readonly Type type;

            public Self()
            {
                type = typeof(T);
            }

            public override Type Type
            {
                get { return type; }
            }

            public override string Name
            {
                get { return string.Empty; }
            }

            public override object GetValue(T instance)
            {
                return instance;
            }
        }
        #endregion
    }
}

