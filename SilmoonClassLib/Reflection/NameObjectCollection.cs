using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Specialized;

namespace Silmoon.Reflection
{
    [Serializable]
    public class NameObjectCollection : NameObjectCollectionBase
    {
        public NameObjectCollection()
        {

        }
        public void Add(string name, object value)
        {
            base.BaseAdd(name, value);
        }
        public void Remove(string name)
        {
            base.BaseRemove(name);
        }
        public void RemoveAt(int index)
        {
            base.BaseRemoveAt(index);
        }
        public void Clear()
        {
            base.BaseClear();
        }
        public object Get(int index)
        {
            return base.BaseGet(index);
        }
        public object Get(string name)
        {
            return base.BaseGet(name);
        }
        public string[] GetAllKeys()
        {
            return base.BaseGetAllKeys();
        }
        public object[] GetAllValues()
        {
            return base.BaseGetAllValues();
        }
        public object[] GetAllValues(Type type)
        {
            return base.BaseGetAllValues(type);
        }
        public string GetKey(int index)
        {
            return base.BaseGetKey(index);
        }
        public bool HasKeys()
        {
            return base.BaseHasKeys();
        }
        public void Set(int index, object value)
        {
            base.BaseSet(index, value);
        }
        public void Set(string name, object value)
        {
            base.BaseSet(name, value);
        }

        public object this[int index]
        {
            get
            {
                return base.BaseGet(index);
            }
            set
            {
                base.BaseSet(index, value);
            }
        }
        public object this[string name]
        {
            get
            {
                return base.BaseGet(name);
            }
            set
            {
                base.BaseSet(name, value);
            }
        }
    }
}
