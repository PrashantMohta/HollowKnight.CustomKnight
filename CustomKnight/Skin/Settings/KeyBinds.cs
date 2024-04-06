using InControl;

namespace CustomKnight
{
    public class KeyBinds : PlayerActionSet
    {
        public PlayerAction OpenSkinList;
        public KeyBinds()
        {
            OpenSkinList = CreatePlayerAction("OpenSkinList");
            DefaultBinds();
        }

        private void DefaultBinds()
        {
            OpenSkinList.AddDefaultBinding(Key.Delete);
        }
    }
}