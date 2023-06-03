using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomKnight
{
    // user side
    public class SkinSettings
    {
        public Dictionary<string, string> selectedAlternates = new Dictionary<string, string>();

        public SkinSettings()
        {
            foreach (var kvp in SkinManager.Skinables)
            {
                var name = kvp.Value.name + ".png";
                selectedAlternates[name] = name;
            }
        }
    }

    // author side
    public class SkinConfig
    {
        public bool dungFilter = true;
        public bool wraithsFilter = false;
        public string flashColor = "";
        public bool detectAlts = true;
        public Dictionary<string, List<string>> alternates = new Dictionary<string, List<string>> (); 
        public SkinConfig()
        {
            foreach( var kvp in SkinManager.Skinables)
            {
                var name = kvp.Value.name + ".png";
                alternates[name] = new List<string> { name };
            }
        }
    }

}
