using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomKnight
{
    internal class SettingsLoader<T> where T : new()
    {
        public SettingsLoader(){}

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
                instance = JsonConvert.DeserializeObject<T>(File.ReadAllText(filePath));
            }
            catch
            {
                instance = new T();
                Save(filePath, instance);
            }
            return instance;
        }
    }
}
