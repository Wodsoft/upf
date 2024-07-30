using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Test
{
    public class MyObject : Control
    {
        public static readonly DependencyProperty TextAProperty = DependencyProperty.Register("TextA", typeof(string), typeof(MyObject));
        public static readonly DependencyProperty TextBProperty = DependencyProperty.Register("TextB", typeof(string), typeof(MyObject));
        public static readonly DependencyProperty TextCProperty = DependencyProperty.Register("TextC", typeof(string), typeof(MyObject));
        public static readonly DependencyProperty TextDProperty = DependencyProperty.Register("TextD", typeof(string), typeof(MyObject));

        public string? TextA { get => (string?)GetValue(TextAProperty); set => SetValue(TextAProperty, value); }
        public string? TextB { get => (string?)GetValue(TextBProperty); set => SetValue(TextBProperty, value); }
        public string? TextC { get => (string?)GetValue(TextCProperty); set => SetValue(TextCProperty, value); }
        public string? TextD { get => (string?)GetValue(TextDProperty); set => SetValue(TextDProperty, value); }

        public DependencyProperty? LastChangedProperty;
        public object? LastChangedNewValue, LastChangedOldValue;

        public MyObject()
        {

        }

        internal DependencyObject? FindTemplateChild(string name) => GetTemplateChild(name);

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            LastChangedProperty = e.Property;
            LastChangedOldValue = e.OldValue;
            LastChangedNewValue = e.NewValue;
            base.OnPropertyChanged(e);
        }

        internal ref readonly DependencyEffectiveValue InternalGetEffectiveValue(DependencyProperty dp) => ref base.GetEffectiveValue(dp);

        public static readonly DependencyProperty DefaultsProperty = DependencyProperty.Register("Defaults", typeof(IList<MyObject>), typeof(MyObject), new PropertyMetadata(new ObservableCollectionDefaultValueFactory<MyObject>()));
        public IList<MyObject> Defaults => (IList<MyObject>)GetValue(DefaultsProperty)!;
    }
}
