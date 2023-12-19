using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Data
{
    internal class DependencyPropertyBinding : PropertyBinding
    {
        private readonly DependencyObject _d;
        private readonly DependencyProperty _dp;

        public DependencyPropertyBinding(DependencyObject d, DependencyProperty dp)
        {
            _d = d;
            _dp = dp;
            d.DependencyPropertyChanged += PropertyChanged;
        }

        private void PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.Property == _dp)
                NotifyValueChange();
        }

        public override bool CanSet => !_dp.ReadOnly;

        public override bool CanGet => true;

        public override object? GetValue()
        {
            return _d.GetValue(_dp);
        }

        public override void SetValue(object? value)
        {
            _d.SetValue(_dp, value);
        }

        protected override void OnDispose()
        {
            _d.DependencyPropertyChanged -= PropertyChanged;
        }
    }
}
