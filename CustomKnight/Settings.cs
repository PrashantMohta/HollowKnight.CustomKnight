using Modding;

namespace CustomKnight
{
    public class SaveModSettings  {
        public string DefaultSkin {get; set;} = "Default";
     }

    public class GlobalModSettings 
    {

        public bool Preloads {get; set;} = true;
        public string DefaultSkin {get; set;} = "Default";
        public float PanelY {get; set;} = 80f;
        public int PanelWidth {get; set;} = 250;
        public int PanelHeight {get; set;} = 1080;
        public int NameLength {get; set;} = 10;
        public int OptionSize {get; set;} = 35;

    }

    public class Settings {
        
    }
}