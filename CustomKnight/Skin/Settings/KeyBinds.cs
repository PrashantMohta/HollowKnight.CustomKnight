using InControl;

namespace CustomKnight
{
    /// <summary>
    /// Stores Keybinds
    /// </summary>
    public class KeyBinds : PlayerActionSet
    {
        /// <summary>
        /// Keybind to open the skin list
        /// </summary>
        public PlayerAction OpenSkinList;

        /// <summary>
        /// Keybind to Reload the skin
        /// </summary>
        public PlayerAction ReloadSkins;

        /// <summary>
        /// Ctor
        /// </summary>
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