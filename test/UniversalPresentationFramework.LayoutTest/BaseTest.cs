using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly: CollectionBehavior(DisableTestParallelization = true)]
namespace Wodsoft.UI.Test
{
    public class BaseTest
    {
        protected T LoadWpfXaml<T>(string xaml)
        {
            return (T)System.Windows.Markup.XamlReader.Parse(xaml);
        }

        protected T LoadUpfXaml<T>(string xaml)
        {
            return (T)Wodsoft.UI.Markup.XamlReader.Parse(xaml);
        }

        protected T LoadUpfWithWpfXaml<T>(string xaml)
        {
            return LoadUpfXaml<T>(ConvertToUpfXaml(xaml));
        }

        protected string ConvertToUpfXaml(string xaml)
        {
            return xaml.Replace("http://schemas.microsoft.com/winfx/2006/xaml/presentation", "http://schemas.wodsoft.com/upf/presentation");
        }
    }
}
