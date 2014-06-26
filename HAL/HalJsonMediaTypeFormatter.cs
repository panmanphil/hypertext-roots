using System;
using System.Globalization;
using System.IO;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace HypertextRoots.HAL
{
    public class HalJsonMediaTypeFormatter : HalMediaTypeFormatterBase
    {
        public HalJsonMediaTypeFormatter()
        {
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/hal+json"));
        }

        public override object ReadFromStream(Type type, Stream readStream, System.Net.Http.HttpContent content, IFormatterLogger formatterLogger)
        {
            using (var reader = new StreamReader(readStream))
            {
                return JsonConvert.DeserializeObject(reader.Read().ToString(CultureInfo.InvariantCulture), type, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            }

        }

        public override void WriteToStream(Type type, object value, Stream writeStream, System.Net.Http.HttpContent content)
        {
            var serializer = new JsonSerializer();
            using (var sw = new StreamWriter(writeStream))
            {
                using (JsonWriter writer = new JsonTextWriter(sw))
                {
                    writer.WriteRaw(JsonConvert.SerializeObject(value, new JsonSerializerSettings {}));
                }
            }
        }

        

    }
}
