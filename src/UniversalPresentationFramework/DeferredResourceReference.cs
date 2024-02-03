//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Wodsoft.UI
//{
//    internal class DeferredResourceReference : DeferredReference
//    {
//        private readonly ResourceDictionary _dictionary;
//        private readonly object _keyOrValue;

//        internal DeferredResourceReference(ResourceDictionary dictionary, object key)
//        {
//            _dictionary = dictionary;
//            _keyOrValue = key;
//        }

//        public override object GetValue()
//        {
//            // If the _value cache is invalid fetch the value from
//            // the dictionary else just retun the cached value
//            if (_dictionary != null)
//            {
//                bool canCache;
//                object value = _dictionary.GetValue(_keyOrValue, out canCache);
//                if (canCache)
//                {
//                    // Note that we are replacing the _keyorValue field
//                    // with the value and deleting the _dictionary field.
//                    _keyOrValue = value;
//                    RemoveFromDictionary();
//                }

//                // tell any listeners (e.g. ResourceReferenceExpressions)
//                // that the value has been inflated
//                OnInflated();

//                return value;
//            }

//            return _keyOrValue;
//        }
//    }
//}
