using System;
using System.IO;
using Modding;
using UnityEngine;

namespace CustomKnight
{
    public class CustomKnight : Mod, ITogglableMod
    {
        public const string KNIGHT_PNG = "Knight.png";
        public const string SPRINT_PNG = "Sprint.png";
        public const string IMAGE_FOLDER = "CustomKnight";

        public static readonly string DATA_DIR = Path.GetFullPath(Application.dataPath + "/Managed/Mods/" + IMAGE_FOLDER);

        public override void Initialize()
        {
            if (!Directory.Exists(DATA_DIR))
            {
                Directory.CreateDirectory(DATA_DIR);
            }

            if (!File.Exists(DATA_DIR + "/" + KNIGHT_PNG) || !File.Exists(DATA_DIR + "/" + SPRINT_PNG))
            {
                Log("Could not find one or both of the required spritesheets");
                Log($"To use this mod, place two files \"{KNIGHT_PNG}\" and \"{SPRINT_PNG}\" in the folder \"{IMAGE_FOLDER}\"");
                return;
            }

            SpriteLoader.Load();

            ModHooks.Instance.AfterSavegameLoadHook += SpriteLoader.ModifyHeroTextures;
        }

        public override string GetVersion() => "1.0.0";

        public void Unload()
        {
            ModHooks.Instance.AfterSavegameLoadHook -= SpriteLoader.ModifyHeroTextures;
            SpriteLoader.UnloadAll();
        }
    }
}
