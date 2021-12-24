using Modding;

namespace CustomKnight
{
    public class SaveModSettings  {
        public string DefaultSkin {get; set;} = null;
     }

    public class GlobalModSettings 
    {

        public string Version {get; set;} = "";
        public bool Preloads {get; set;} = true;
        public string DefaultSkin {get; set;} = "Default";
        public int NameLength {get; set;} = 20;

        public bool showMovedText {get; set;} = true;
        public bool SwapperEnabled {get; set;} = true;
    }

}