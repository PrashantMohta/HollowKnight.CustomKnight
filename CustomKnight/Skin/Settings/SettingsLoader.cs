using Newtonsoft.Json;
using System.IO;

namespace CustomKnight
{
    internal class SettingsLoader<T> where T : new()
    {
        public SettingsLoader() { }

        public void Save(string filePath, T instance)
        {
            File.WriteAllText(filePath, JsonConvert.SerializeObject(instance, Formatting.Indented));
        }
        public T Load(string filePath)
        {
            //load from file or create defaults
            T instance;
            try
            {
                if(!File.Exists(filePath))
                {
                    CustomKnight.Instance.Log($"{filePath} not found!, creating defaults!");
                    instance = new T();
                    Save(filePath, instance);
                }
                instance = JsonConvert.DeserializeObject<T>(File.ReadAllText(filePath));
            }
            catch (Exception ex)
            {
                CustomKnight.Instance.Log(ex);
                instance = new T();
                Save(filePath, instance);
            }
            return instance;
        }
    }
}
