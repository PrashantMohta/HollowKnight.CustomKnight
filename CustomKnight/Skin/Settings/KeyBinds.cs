using InControl;

namespace CustomKnight
{
    public class KeyBinds : PlayerActionSet
    {
        public PlayerAction OpenSkinList;
        public PlayerAction ReloadSkins;

        public KeyBinds()
        {
            OpenSkinList = CreatePlayerAction("OpenSkinList");
            ReloadSkins = CreatePlayerAction("ReloadSkins");
            DefaultBinds();
        }


        private void DefaultBinds()
        {
            OpenSkinList.AddDefaultBinding(Key.Delete);
            ReloadSkins.AddDefaultBinding(Key.None);
        }
    }
}