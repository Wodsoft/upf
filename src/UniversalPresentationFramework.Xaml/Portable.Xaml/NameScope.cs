//
// Copyright (C) 2010 Novell Inc. http://novell.com
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xaml.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Xaml.Markup;
using System.Xaml.Schema;

namespace System.Xaml
{
    internal class NameScope : INameScope
    {
        private readonly Dictionary<string, object> _table = new Dictionary<string, object>();
        // It is an external read-only namescope.
        private readonly INameScope _external;

        public NameScope() { }

        public NameScope(INameScope external)
        {
            this._external = external;
        }

        public object FindName(string name)
        {
            object obj = _external != null ? _external.FindName(name) : null;
            if (obj != null)
                return obj;
            return _table.TryGetValue(name, out obj) ? obj : null;
        }

        public void RegisterName(string name, object scopedElement)
        {
            _table.Add(name, scopedElement);
        }

        public void UnregisterName(string name)
        {
            _table.Remove(name);
        }
    }
}
