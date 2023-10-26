using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wodsoft.UI.Media;

namespace Wodsoft.UI
{
    public class UIElement : Visual
    {
        #region Layout

        public void Measure(Size availableSize)
        {
            throw new NotImplementedException();
        }

        public void Arrange(Rect finalRect)
        {
            throw new NotImplementedException();
        }

        protected virtual Size MeasureCore(Size availableSize)
        {
            throw new NotImplementedException();
        }

        protected virtual void ArrangeCore(Rect finalRect)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
