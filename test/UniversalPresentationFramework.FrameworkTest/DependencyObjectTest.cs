﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Test
{
    public class DependencyObjectTest : BaseTest
    {
        [Fact]
        public void GetSetTest()
        {
            MyObject d = new MyObject();
            d.TextA = "test value";
            Assert.Equal("test value", d.TextA);
            Assert.Null(d.LastChangedOldValue);
            Assert.Equal("test value", d.LastChangedNewValue);
            Assert.Equal(MyObject.TextAProperty, d.LastChangedProperty);
        }

        [Fact]
        public void InheritedTest()
        {
            Grid parent = new Grid();
            parent.DataContext = "test";
            Control child = new Control();
            parent.Children.Add(child);
            Assert.Equal(parent.DataContext, child.DataContext);
        }

        [Fact]
        public void DefaultValueFactoryTest()
        {
            MyObject d = new MyObject();
            var defaults = d.Defaults;
            ref readonly var effectiveValue = ref d.InternalGetEffectiveValue(MyObject.DefaultsProperty);
            Assert.Equal(DependencyEffectiveSource.None, effectiveValue.Source);
            defaults.Add(new MyObject());
            effectiveValue = ref d.InternalGetEffectiveValue(MyObject.DefaultsProperty);
            Assert.Equal(DependencyEffectiveSource.Local, effectiveValue.Source);
        }
    }
}
