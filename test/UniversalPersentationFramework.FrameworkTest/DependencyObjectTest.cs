using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Test
{
    public class DependencyObjectTest
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
    }
}
