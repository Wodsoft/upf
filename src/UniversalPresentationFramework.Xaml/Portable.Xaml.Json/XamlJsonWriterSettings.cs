namespace System.Xaml.Json
{
	[EnhancedXaml]
	public class XamlJsonWriterSettings : XamlWriterSettings
	{
		public bool CloseOutput { get; set; }

		public bool UseNamespaces { get; set; }
	}
}
