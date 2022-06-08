using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Fido2NetLib
{
    public sealed class HexStringConverter : JsonConverter<byte[]>
    {
        public override byte[] Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            // what am i doing here?
            if (!reader.HasValueSequence)
            {
                return Convert.FromHexString(reader.ValueSpan.ToString());
            }
            else
            {
                return Convert.FromHexString(reader.GetString());
            }
        }

        public override void Write(Utf8JsonWriter writer, byte[] value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(Convert.ToHexString(value));
        }
    }
}
