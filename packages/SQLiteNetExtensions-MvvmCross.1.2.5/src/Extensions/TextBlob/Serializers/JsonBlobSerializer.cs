using System;
using Newtonsoft.Json;

namespace SQLiteNetExtensions.Extensions.TextBlob.Serializers
{
    public class JsonBlobSerializer : ITextBlobSerializer
    {
        public string Serialize(object element)
        {
            return JsonConvert.SerializeObject(element);
        }

        public object Deserialize(string text, Type type)
        {
            return JsonConvert.DeserializeObject(text, type);
        }
    }
}