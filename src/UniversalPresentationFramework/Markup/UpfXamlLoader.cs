using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;
using System.Xaml;
using System.Xml;

namespace Wodsoft.UI.Markup
{
    internal static class UpfXamlLoader
    {
        public static object Load(System.Xaml.XamlReader xamlReader, bool skipJournaledProperties)
        {
            XamlObjectWriterSettings settings = new XamlObjectWriterSettings();
            settings.IgnoreCanConvert = true;
            settings.PreferUnconvertedDictionaryKeys = true;
            settings.XamlSetValueHandler = (sender, e) =>
            {
                if (sender is DependencyObject d && e.Member is UpfXamlMember upfXamlMember)
                {
                    d.SetValue(upfXamlMember.DependencyProperty, e.Value);
                    e.Handled = true;
                }
            };
            object result = Load(xamlReader, null, skipJournaledProperties, null, settings);
            //EnsureXmlNamespaceMaps(result, xamlReader.SchemaContext);
            return result;
        }

        private static object Load(System.Xaml.XamlReader xamlReader, IXamlObjectWriterFactory? writerFactory,
            bool skipJournaledProperties, object? rootObject, XamlObjectWriterSettings settings)
        {
            XamlObjectWriter xamlWriter;
            if (writerFactory == null)
                xamlWriter = new System.Xaml.XamlObjectWriter(xamlReader.SchemaContext, settings);
            else
                xamlWriter = writerFactory.GetXamlObjectWriter(settings);

            IXamlLineInfo? xamlLineInfo = null;
            //try
            //{
            //Handle Line Numbers
            xamlLineInfo = xamlReader as IXamlLineInfo;
            bool shouldPassLineNumberInfo = false;
            if (xamlLineInfo != null && xamlLineInfo.HasLineInfo && xamlWriter.ShouldProvideLineInfo)
            {
                shouldPassLineNumberInfo = true;
            }

            //IStyleConnector styleConnector = rootObject as IStyleConnector;
            TransformNodes(xamlReader, xamlWriter,
                false /*onlyLoadOneNode*/,
                skipJournaledProperties,
                shouldPassLineNumberInfo ? xamlLineInfo : null, xamlWriter
                //stack, styleConnector
                );
            xamlWriter.Close();
            if (xamlWriter.Result is DependencyObject d)
                NameScope.SetNameScope(d, xamlWriter.RootNameScope);
            return xamlWriter.Result;
            //}
            //catch (Exception e)
            //{
            //    //// Don't wrap critical exceptions or already-wrapped exceptions.
            //    //if (MS.Internal.CriticalExceptions.IsCriticalException(e) || !XamlReader.ShouldReWrapException(e, baseUri))
            //    //{
            //    //    throw;
            //    //}
            //    //XamlReader.RewrapException(e, xamlLineInfo, baseUri);
            //    return null;    // this should never be executed
            //}
        }

        private static void TransformNodes(System.Xaml.XamlReader xamlReader, System.Xaml.XamlObjectWriter xamlWriter,
                                         bool onlyLoadOneNode,
                                         bool skipJournaledProperties,
                                         IXamlLineInfo? xamlLineInfo, IXamlLineInfoConsumer xamlLineInfoConsumer
            //XamlContextStack<WpfXamlFrame> stack,
            //IStyleConnector styleConnector
            )
        {
            while (xamlReader.Read())
            {
                if (xamlLineInfo != null)
                {
                    if (xamlLineInfo.LineNumber != 0)
                    {
                        xamlLineInfoConsumer.SetLineInfo(xamlLineInfo.LineNumber, xamlLineInfo.LinePosition);
                    }
                }
                switch (xamlReader.NodeType)
                {
                    case XamlNodeType.StartObject:
                        {
                            xamlWriter.WriteNode(xamlReader);
                            break;
                        }
                    case XamlNodeType.EndObject:
                        {
                            xamlWriter.WriteNode(xamlReader);
                            break;
                        }
                    //case System.Xaml.XamlNodeType.NamespaceDeclaration:
                    //    xamlWriter.WriteNode(xamlReader);
                    //    if (stack.Depth == 0 || stack.CurrentFrame.Type != null)
                    //    {
                    //        stack.PushScope();
                    //        // Need to create an XmlnsDictionary.  
                    //        // Look up stack to see if we have one earlier
                    //        //  If so, use that.  Otherwise new a xmlnsDictionary                        
                    //        WpfXamlFrame iteratorFrame = stack.CurrentFrame;
                    //        while (iteratorFrame != null)
                    //        {
                    //            if (iteratorFrame.XmlnsDictionary != null)
                    //            {
                    //                stack.CurrentFrame.XmlnsDictionary =
                    //                    new XmlnsDictionary(iteratorFrame.XmlnsDictionary);
                    //                break;
                    //            }
                    //            iteratorFrame = (WpfXamlFrame)iteratorFrame.Previous;
                    //        }
                    //        if (stack.CurrentFrame.XmlnsDictionary == null)
                    //        {
                    //            stack.CurrentFrame.XmlnsDictionary =
                    //                     new XmlnsDictionary();
                    //        }
                    //    }
                    //    stack.CurrentFrame.XmlnsDictionary.Add(xamlReader.Namespace.Prefix, xamlReader.Namespace.Namespace);
                    //    break;
                    //case System.Xaml.XamlNodeType.StartObject:
                    //    WriteStartObject(xamlReader, xamlWriter, stack);
                    //    break;
                    //case System.Xaml.XamlNodeType.GetObject:
                    //    xamlWriter.WriteNode(xamlReader);
                    //    // If there wasn't a namespace node before this get object, need to pushScope.
                    //    if (stack.CurrentFrame.Type != null)
                    //    {
                    //        stack.PushScope();
                    //    }
                    //    stack.CurrentFrame.Type = stack.PreviousFrame.Property.Type;
                    //    break;
                    //case System.Xaml.XamlNodeType.EndObject:
                    //    xamlWriter.WriteNode(xamlReader);
                    //    // Freeze if required
                    //    if (stack.CurrentFrame.FreezeFreezable)
                    //    {
                    //        Freezable freezable = xamlWriter.Result as Freezable;
                    //        if (freezable != null && freezable.CanFreeze)
                    //        {
                    //            freezable.Freeze();
                    //        }
                    //    }
                    //    DependencyObject dependencyObject = xamlWriter.Result as DependencyObject;
                    //    if (dependencyObject != null && stack.CurrentFrame.XmlSpace.HasValue)
                    //    {
                    //        XmlAttributeProperties.SetXmlSpace(dependencyObject, stack.CurrentFrame.XmlSpace.Value ? "default" : "preserve");
                    //    }
                    //    stack.PopScope();
                    //    break;
                    //case System.Xaml.XamlNodeType.StartMember:
                    //    // ObjectWriter should NOT process PresentationOptions:Freeze directive since it is Unknown
                    //    // The space directive node stream should not be written because it induces object instantiation,
                    //    // and the Baml2006Reader can produce space directives prematurely.
                    //    if (!(xamlReader.Member.IsDirective && xamlReader.Member == XamlReaderHelper.Freeze) &&
                    //        xamlReader.Member != XmlSpace.Value &&
                    //        xamlReader.Member != XamlLanguage.Space)
                    //    {
                    //        xamlWriter.WriteNode(xamlReader);
                    //    }

                    //    stack.CurrentFrame.Property = xamlReader.Member;
                    //    if (skipJournaledProperties)
                    //    {
                    //        if (!stack.CurrentFrame.Property.IsDirective)
                    //        {
                    //            System.Windows.Baml2006.WpfXamlMember wpfMember = stack.CurrentFrame.Property as System.Windows.Baml2006.WpfXamlMember;
                    //            if (wpfMember != null)
                    //            {
                    //                DependencyProperty prop = wpfMember.DependencyProperty;

                    //                if (prop != null)
                    //                {
                    //                    FrameworkPropertyMetadata metadata = prop.GetMetadata(stack.CurrentFrame.Type.UnderlyingType) as FrameworkPropertyMetadata;
                    //                    if (metadata != null && metadata.Journal == true)
                    //                    {
                    //                        // Ignore the BAML for this member, unless it declares a value that wasn't journaled - namely a binding or a dynamic resource
                    //                        int count = 1;
                    //                        while (xamlReader.Read())
                    //                        {
                    //                            switch (xamlReader.NodeType)
                    //                            {
                    //                                case System.Xaml.XamlNodeType.StartMember:
                    //                                    count++;
                    //                                    break;
                    //                                case System.Xaml.XamlNodeType.StartObject:
                    //                                    XamlType xamlType = xamlReader.Type;
                    //                                    XamlType bindingBaseType = xamlType.SchemaContext.GetXamlType(typeof(BindingBase));
                    //                                    XamlType dynamicResourceType = xamlType.SchemaContext.GetXamlType(typeof(DynamicResourceExtension));
                    //                                    if (count == 1 && (xamlType.CanAssignTo(bindingBaseType) || xamlType.CanAssignTo(dynamicResourceType)))
                    //                                    {
                    //                                        count = 0;
                    //                                        WriteStartObject(xamlReader, xamlWriter, stack);
                    //                                    }
                    //                                    break;
                    //                                case System.Xaml.XamlNodeType.EndMember:
                    //                                    count--;
                    //                                    if (count == 0)
                    //                                    {
                    //                                        xamlWriter.WriteNode(xamlReader);
                    //                                        stack.CurrentFrame.Property = null;
                    //                                    }
                    //                                    break;
                    //                                case System.Xaml.XamlNodeType.Value:
                    //                                    DynamicResourceExtension value = xamlReader.Value as DynamicResourceExtension;
                    //                                    if (value != null)
                    //                                    {
                    //                                        WriteValue(xamlReader, xamlWriter, stack, styleConnector);
                    //                                    }
                    //                                    break;
                    //                            }
                    //                            if (count == 0)
                    //                                break;
                    //                        }

                    //                        System.Diagnostics.Debug.Assert(count == 0, "Mismatch StartMember/EndMember");
                    //                    }
                    //                }
                    //            }
                    //        }
                    //    }
                    //    break;
                    //case System.Xaml.XamlNodeType.EndMember:
                    //    WpfXamlFrame currentFrame = stack.CurrentFrame;
                    //    XamlMember currentProperty = currentFrame.Property;
                    //    // ObjectWriter should not process PresentationOptions:Freeze directive nodes since it is unknown
                    //    // The space directive node stream should not be written because it induces object instantiation,
                    //    // and the Baml2006Reader can produce space directives prematurely.
                    //    if (!(currentProperty.IsDirective && currentProperty == XamlReaderHelper.Freeze) &&
                    //        currentProperty != XmlSpace.Value &&
                    //        currentProperty != XamlLanguage.Space)
                    //    {
                    //        xamlWriter.WriteNode(xamlReader);
                    //    }
                    //    currentFrame.Property = null;
                    //    break;
                    //case System.Xaml.XamlNodeType.Value:
                    //    WriteValue(xamlReader, xamlWriter, stack, styleConnector);
                    //    break;
                    default:
                        xamlWriter.WriteNode(xamlReader);
                        break;
                }

                //Only do this loop for one node if loadAsync
                if (onlyLoadOneNode)
                {
                    return;
                }
            }
        }
    }
}
