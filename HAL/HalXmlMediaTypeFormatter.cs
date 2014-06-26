using System;
using System.Collections;
using System.Linq;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Reflection;
using System.Xml;

namespace HypertextRoots.HAL
{
    
    public class HalXmlMediaTypeFormatter : HalMediaTypeFormatterBase
    {
        public HalXmlMediaTypeFormatter()
            : base()
        {
            this.SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/hal+xml"));
        }

        public override object ReadFromStream(Type type, System.IO.Stream readStream, System.Net.Http.HttpContent content, IFormatterLogger formatterLogger)
        {
            var value = (LinkedResource)Activator.CreateInstance(type);
            
            var reader = XmlReader.Create(readStream);

            if (value is IEnumerable)
            {
                var collection = (ILinkedResourceCollection)value;

                reader.ReadStartElement("resource");
                //value.href = reader.GetAttribute("href");

                var innerType = type.BaseType.GetGenericArguments().First();

                while (reader.Read() && reader.LocalName == "resource")
                {
                    var innerResource = DeserializeInnerResource(reader, innerType);
                    collection.Add(innerResource);
                }
            }
            else
            {
                value = DeserializeInnerResource(reader, type);
            }

            reader.Close();

            return value;
        }

        public override void WriteToStream(Type type, object value, System.IO.Stream writeStream, System.Net.Http.HttpContent content)
        {
            var settings = new XmlWriterSettings();
            settings.Indent = false;
            settings.OmitXmlDeclaration = false;

            var writer = XmlWriter.Create(writeStream, settings);

            var resource = (LinkedResource)value;

            if (resource is IEnumerable)
            {
                writer.WriteStartElement("resource");
                //writer.WriteAttributeString("href", resource.href);

                foreach (LinkedResource innerResource in (IEnumerable)resource)
                {
                    SerializeInnerResource(writer, innerResource);
                }

                writer.WriteEndElement();
            }
            else
            {
                SerializeInnerResource(writer, resource);
            }

            writer.Flush();
            writer.Close();
        }

        private LinkedResource DeserializeInnerResource(XmlReader reader, Type innerType)
        {
            var resource = (LinkedResource)Activator.CreateInstance(innerType);
            
            reader.ReadStartElement("resource");
            //resource.href = reader.GetAttribute("href");

             var properties = resource.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p =>
                    p.Name != "href" &&
                    p.Name != "Links")
                .ToArray();

            while(reader.Read())
            {
                if(reader.LocalName == "link")
                {
                    var link = new Link();
                    link.href = reader.GetAttribute("href");
                    link.rel = reader.GetAttribute("rel");
                    link.name = reader.GetAttribute("name");

                    resource.Links.Add(link.name, link);
                }
                else if(reader.IsStartElement("resource"))
                {
                    var rel = reader.GetAttribute("rel");
                    var property = properties.FirstOrDefault(p => p.Name == rel);
                    if(property != null)
                    {
                        var propertyValue = DeserializeInnerResource(reader, property.PropertyType);
                        property.SetValue(resource, propertyValue);
                    }
                }
                else if(reader.IsStartElement())
                {
                    var property = properties.FirstOrDefault(p => p.Name == reader.LocalName);
                    if(property != null)
                    {
                        var propertyValue = reader.ReadElementContentAs(property.PropertyType, null);
                        property.SetValue(resource, propertyValue);
                    }
                }
            }

            return resource;
        }

        private void SerializeInnerResource(XmlWriter writer, LinkedResource resource, string rel = null)
        {
            writer.WriteStartElement("resource");
            //writer.WriteAttributeString("href", resource.href);
            if (rel != null)
            {
                writer.WriteAttributeString("rel", rel);
            }

            foreach (var link in resource.Links)
            {
                writer.WriteStartElement("link");
                writer.WriteAttributeString("rel", link.Value.rel);
                writer.WriteAttributeString("href", link.Value.href);
                writer.WriteEndElement();
            }

            var properties = resource.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p =>
                    p.Name != "href" &&
                    p.Name != "Links");

            foreach (var property in properties)
            {
                var propertyValue = property.GetValue(resource);
                if (property.PropertyType.IsPrimitive || property.PropertyType == typeof(string))
                {
                    writer.WriteElementString(property.Name, propertyValue == null? string.Empty: propertyValue.ToString());
                }
                else if(propertyValue is LinkedResource)
                {
                    SerializeInnerResource(writer, (LinkedResource)propertyValue, property.Name); 
                }
            }

            writer.WriteEndElement();
        }
        

    }
}
