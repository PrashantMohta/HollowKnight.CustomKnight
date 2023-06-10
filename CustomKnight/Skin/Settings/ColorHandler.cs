using Newtonsoft.Json;

namespace CustomKnight
{
    public class ColorHandler : JsonConverter
    {
        public ColorHandler()
        {
        }

        public override bool CanConvert(Type objectType)
        {
            return true;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            try
            {
                ColorUtility.TryParseHtmlString("#" + reader.Value, out Color loadedColor);
                return loadedColor;
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to parse color {objectType} : {ex.Message}");
                return new Color(1,1,1,1);
            }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            string val = ColorUtility.ToHtmlStringRGB((Color)value);
            writer.WriteValue(val);
        }
    }
}
