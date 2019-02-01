using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiBusinessLogic.Logics.Listener.DTO;

namespace WebApiBusinessLogic.Logics.Listener
{
    internal class IntOrArrayJsonConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            object retVal = new Object();

            if (reader.TokenType == JsonToken.StartArray)
            {
                retVal = serializer.Deserialize(reader, objectType);
            }
            else if (reader.TokenType == JsonToken.StartObject)
            {
                retVal = new Object();
            }
            return retVal;
        }

        public override bool CanConvert(Type objectType)
        {
            return false;
        }
    }
}
