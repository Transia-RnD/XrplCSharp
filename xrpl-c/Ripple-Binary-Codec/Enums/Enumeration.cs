using System.Collections;
using System.Collections.Generic;

namespace Ripple.Core.Enums
{
    public class Enumeration<T> : IEnumerable<T> where T : EnumItem
    {
        private readonly Dictionary<string, T> _byText = new Dictionary<string, T>();
        private readonly Dictionary<int, T> _byOrdinal
            = new Dictionary<int, T>();
        private readonly List<T> _byDefinitionOrder = new List<T>(); 

        public T this[string name]
        {
            get
            {
                return _byText[name];
            }
            set { _byText[name] = value; }
        }

        public T this[int name]
        {
            get
            {
                return _byOrdinal[name];
            }
            set { _byOrdinal[name] = value; }
        }

        public virtual T AddEnum(T T)
        {
            _byOrdinal[T.Ordinal] = T;
            _byText[T.Name] = T;
            _byDefinitionOrder.Add(T);
            return T;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _byDefinitionOrder.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _byDefinitionOrder.GetEnumerator();
        }

        public bool Has(string key)
        {
            return _byText.ContainsKey(key);
        }
    }
}