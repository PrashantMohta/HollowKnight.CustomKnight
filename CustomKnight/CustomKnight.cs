using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using CustomKnight.Canvas;
using HutongGames.PlayMaker.Actions;
using ModCommon;
using Modding;
using On.TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Object = System.Object;
using Random = UnityEngine.Random;

namespace CustomKnight
{
    public class CustomKnight : Mod, ITogglableMod
    {
        public const string KNIGHT_PNG = "Knight.png";
        public const string SPRINT_PNG = "Sprint.png";
        public const string WRAITHS_PNG = "Wraiths.png";
        public const string VOID_PNG = "VoidSpells.png";
        public const string VS_PNG = "VS.png";
        public const string HUD_PNG = "Hud.png";
        public const string FULL_PNG = "OrbFull.png";
        public const string GEO_PNG = "Geo.png";
        public const string DN_PNG = "Dreamnail.png";
        public const string DREAM_PNG = "DreamArrival.png";
        public const string UNN_PNG = "Unn.png";
        public const string WINGS_PNG = "Wings.png";
        public const string QUIRREL_PNG = "Quirrel.png";
        public const string WEBBED_PNG = "Webbed.png";
        public const string CLOAK_PNG = "Cloak.png";
        public const string SHRIEK_PNG = "Shriek.png";
        public const string HORNET_PNG = "Hornet.png";
        public const string BIRTH_PNG = "Birthplace.png";
        public const string BALDUR_PNG = "Baldur.png";
        public const string FLUKE_PNG = "Fluke.png";
        public const string GRIMM_PNG = "Grimm.png";
        public const string SHIELD_PNG = "Shield.png";
        public const string WEAVER_PNG = "Weaver.png";
        public const string WOMB_PNG = "Hatchling.png";
        public const string SKINS_FOLDER = "CustomKnight";
        public static string SKIN_FOLDER;

        public static bool KnightMissing;
        public static bool SprintMissing;
        public static bool WraithsMissing;
        public static bool VoidMissing;
        public static bool VSMissing;
        public static bool HUDMissing;
        public static bool FullMissing;
        public static bool GeoMissing;
        public static bool DNMissing;
        public static bool DreamMissing;
        public static bool UnnMissing;
        public static bool WingsMissing;
        public static bool QuirrelMissing;
        public static bool WebbedMissing;
        public static bool CloakMissing;
        public static bool ShriekMissing;
        public static bool HornetMissing;
        public static bool BirthMissing;
        public static bool BaldurMissing;
        public static bool FlukeMissing;
        public static bool GrimmMissing;
        public static bool ShieldMissing;
        public static bool WeaverMissing;
        public static bool WombMissing;

        public static readonly string DATA_DIR = Path.GetFullPath(Application.dataPath + "/Managed/Mods/" + SKINS_FOLDER);

        public static CustomKnight Instance { get; private set; }

        public override List<(string, string)> GetPreloadNames()
        {
            return new List<(string, string)>
            {
                ("Abyss_10", "higher_being/Dish Plat/Knight Dummy"),
                ("Abyss_12", "Scream 2 Get/Cutscene Knight"),
                ("Abyss_21", "Shiny Item DJ/Knight Cutscene"),
                ("Crossroads_50", "Quirrel Lakeside/Sit Region/Knight Sit"),
                ("Deepnest_East_12", "Hornet Blizzard Return Scene"),
                ("Deepnest_Spider_Town", "RestBench Spider/Webbed Knight"),
                ("Dream_Abyss", "End Cutscene/Dummy"),
                ("GG_Door_5_Finale", "abyss_door_5_cutscene_sequence/main_chars"),
                ("GG_Vengefly", "Boss Scene Controller/Dream Entry/Knight Dream Arrival"),
                ("RestingGrounds_07", "Dream Moth/Knight Dummy"),
            };
        }
        
        public static readonly Dictionary<string, GameObject> GameObjects = new Dictionary<string, GameObject>();

        public override void Initialize(Dictionary<string, Dictionary<string, GameObject>> preloadedObjects)
        {
            GUIController.Instance.BuildMenus();

            GC.Collect();
            
            ResetBools();
            
            if (preloadedObjects != null)
            {
                GameObjects.Add("Cloak", preloadedObjects["Abyss_10"]["higher_being/Dish Plat/Knight Dummy"]);
                GameObjects.Add("Shriek", preloadedObjects["Abyss_12"]["Scream 2 Get/Cutscene Knight"]);
                GameObjects.Add("Wings", preloadedObjects["Abyss_21"]["Shiny Item DJ/Knight Cutscene"]);
                GameObjects.Add("Quirrel", preloadedObjects["Crossroads_50"]["Quirrel Lakeside/Sit Region/Knight Sit"]);
                GameObjects.Add("Hornet", preloadedObjects["Deepnest_East_12"]["Hornet Blizzard Return Scene"]);
                GameObjects.Add("Webbed", preloadedObjects["Deepnest_Spider_Town"]["RestBench Spider/Webbed Knight"]);
                GameObjects.Add("Birth", preloadedObjects["Dream_Abyss"]["End Cutscene/Dummy"]);
                GameObjects.Add("Dream", preloadedObjects["GG_Vengefly"]["Boss Scene Controller/Dream Entry/Knight Dream Arrival"]);
                GameObjects.Add("DN", preloadedObjects["RestingGrounds_07"]["Dream Moth/Knight Dummy"]);
            }

            Instance = this;
            
            if (!Directory.Exists(DATA_DIR))
            {
                Directory.CreateDirectory(DATA_DIR);
            }

            if (Directory.GetDirectories(DATA_DIR).Length == 0)
            {
                Log("There are no Custom Knight skin folders in the Custom Knight directory.");
                return;
            }

            if (SKIN_FOLDER == null)
            {
                int index = Random.Range(0, Directory.GetDirectories(DATA_DIR).Length - 1);
                SKIN_FOLDER = new DirectoryInfo(Directory.GetDirectories(DATA_DIR)[index]).Name;   
            }

            if (!File.Exists((DATA_DIR + "/" + SKIN_FOLDER + "/" + KNIGHT_PNG).Replace("\\", "/")))
            {
                Log($"Missing file {KNIGHT_PNG} from folder {SKIN_FOLDER}.");
                KnightMissing = true;
            }
            
            if (!File.Exists((DATA_DIR + "/" + SKIN_FOLDER + "/" + SPRINT_PNG).Replace("\\", "/")))
            {
                Log($"Missing file {SPRINT_PNG} from folder {SKIN_FOLDER}.");
                SprintMissing = true;
            }
            
            if (!File.Exists((DATA_DIR + "/" + SKIN_FOLDER + "/" + WRAITHS_PNG).Replace("\\", "/")))
            {
                Log($"Missing file {WRAITHS_PNG} from folder {SKIN_FOLDER}.");
                WraithsMissing = true;
            }
            
            if (!File.Exists((DATA_DIR + "/" + SKIN_FOLDER + "/" + VOID_PNG).Replace("\\", "/")))
            {
                Log($"Missing file {VOID_PNG} from folder {SKIN_FOLDER}.");
                VoidMissing = true;
            }
            
            if (!File.Exists((DATA_DIR + "/" + SKIN_FOLDER + "/" + VS_PNG).Replace("\\", "/")))
            {
                Log($"Missing file {VS_PNG} from folder {SKIN_FOLDER}.");
                VSMissing = true;
            }
            
            if (!File.Exists((DATA_DIR + "/" + SKIN_FOLDER + "/" + HUD_PNG).Replace("\\", "/")))
            {
                Log($"Missing file {HUD_PNG} from folder {SKIN_FOLDER}.");
                HUDMissing = true;
            }
            
            if (!File.Exists((DATA_DIR + "/" + SKIN_FOLDER + "/" + FULL_PNG).Replace("\\", "/")))
            {
                Log($"Missing file {FULL_PNG} from folder {SKIN_FOLDER}.");
                FullMissing = true;
            }
            
            if (!File.Exists((DATA_DIR + "/" + SKIN_FOLDER + "/" + GEO_PNG).Replace("\\", "/")))
            {
                Log($"Missing file {GEO_PNG} from folder {SKIN_FOLDER}.");
                GeoMissing = true;
            }
            
            if (!File.Exists((DATA_DIR + "/" + SKIN_FOLDER + "/" + DN_PNG).Replace("\\", "/")))
            {
                Log($"Missing file {DN_PNG} from folder {SKIN_FOLDER}.");
                DNMissing = true;
            }
            
            if (!File.Exists((DATA_DIR + "/" + SKIN_FOLDER + "/" + DREAM_PNG).Replace("\\", "/")))
            {
                Log($"Missing file {DREAM_PNG} from folder {SKIN_FOLDER}.");
                DreamMissing = true;
            }
            
            if (!File.Exists((DATA_DIR + "/" + SKIN_FOLDER + "/" + UNN_PNG).Replace("\\", "/")))
            {
                Log($"Missing file {UNN_PNG} from folder {SKIN_FOLDER}.");
                UnnMissing = true;
            }
            
            if (!File.Exists((DATA_DIR + "/" + SKIN_FOLDER + "/" + WINGS_PNG).Replace("\\", "/")))
            {
                Log($"Missing file {WINGS_PNG} from folder {SKIN_FOLDER}.");
                WingsMissing = true;
            }
            
            if (!File.Exists((DATA_DIR + "/" + SKIN_FOLDER + "/" + QUIRREL_PNG).Replace("\\", "/")))
            {
                Log($"Missing file {QUIRREL_PNG} from folder {SKIN_FOLDER}.");
                QuirrelMissing = true;
            }
            
            if (!File.Exists((DATA_DIR + "/" + SKIN_FOLDER + "/" + WEBBED_PNG).Replace("\\", "/")))
            {
                Log($"Missing file {WEBBED_PNG} from folder {SKIN_FOLDER}.");
                WebbedMissing = true;
            }
            
            if (!File.Exists((DATA_DIR + "/" + SKIN_FOLDER + "/" + CLOAK_PNG).Replace("\\", "/")))
            {
                Log($"Missing file {CLOAK_PNG} from folder {SKIN_FOLDER}.");
                CloakMissing = true;
            }
            
            if (!File.Exists((DATA_DIR + "/" + SKIN_FOLDER + "/" + SHRIEK_PNG).Replace("\\", "/")))
            {
                Log($"Missing file {SHRIEK_PNG} from folder {SKIN_FOLDER}.");
                ShriekMissing = true;
            }
            
            if (!File.Exists((DATA_DIR + "/" + SKIN_FOLDER + "/" + HORNET_PNG).Replace("\\", "/")))
            {
                Log($"Missing file {HORNET_PNG} from folder {SKIN_FOLDER}.");
                HornetMissing = true;
            }
            
            if (!File.Exists((DATA_DIR + "/" + SKIN_FOLDER + "/" + BIRTH_PNG).Replace("\\", "/")))
            {
                Log($"Missing file {BIRTH_PNG} from folder {SKIN_FOLDER}.");
                BirthMissing = true;
            }
            
            if (!File.Exists((DATA_DIR + "/" + SKIN_FOLDER + "/" + BALDUR_PNG).Replace("\\", "/")))
            {
                Log($"Missing file {BALDUR_PNG} from folder {SKIN_FOLDER}.");
                BaldurMissing = true;
            }

            if (!File.Exists((DATA_DIR + "/" + SKIN_FOLDER + "/" + FLUKE_PNG).Replace("\\", "/")))
            {
                Log($"Missing file {FLUKE_PNG} from folder {SKIN_FOLDER}.");
                FlukeMissing = true;
            }
            
            if (!File.Exists((DATA_DIR + "/" + SKIN_FOLDER + "/" + GRIMM_PNG).Replace("\\", "/")))
            {
                Log($"Missing file {GRIMM_PNG} from folder {SKIN_FOLDER}.");
                GrimmMissing = true;
            }
            
            if (!File.Exists((DATA_DIR + "/" + SKIN_FOLDER + "/" + SHIELD_PNG).Replace("\\", "/")))
            {
                Log($"Missing file {SHIELD_PNG} from folder {SKIN_FOLDER}.");
                ShieldMissing = true;
            }
            
            if (!File.Exists((DATA_DIR + "/" + SKIN_FOLDER + "/" + WEAVER_PNG).Replace("\\", "/")))
            {
                Log($"Missing file {WEAVER_PNG} from folder {SKIN_FOLDER}.");
                WeaverMissing = true;
            }
            
            if (!File.Exists((DATA_DIR + "/" + SKIN_FOLDER + "/" + WOMB_PNG).Replace("\\", "/")))
            {
                Log($"Missing file {WOMB_PNG} from folder {SKIN_FOLDER}.");
                WombMissing = true;
            }
            
            SpriteLoader.Load();

            On.GeoControl.Start += GeoControl_Start;
            ModHooks.Instance.AfterSavegameLoadHook += SpriteLoader.ModifyHeroTextures;
        }

        private void GeoControl_Start(On.GeoControl.orig_Start orig, GeoControl self)
        {
            if (SpriteLoader.GeoTex != null)
            {
                self.GetComponent<tk2dSprite>().GetCurrentSpriteDef().material.mainTexture = SpriteLoader.GeoTex;
            }
            orig(self);
        }

        private void ResetBools()
        {
            KnightMissing = false;
            SprintMissing = false;
            WraithsMissing = false;
            VoidMissing = false;
            VSMissing = false;
            HUDMissing = false;
            FullMissing = false;
            GeoMissing = false;
            DNMissing = false;
            DreamMissing = false;
            UnnMissing = false;
            WingsMissing = false;
            QuirrelMissing = false;
            WebbedMissing = false;
            CloakMissing = false;
            ShriekMissing = false;
            HornetMissing = false;
            BirthMissing = false;
            BaldurMissing = false;
            FlukeMissing = false;
            GrimmMissing = false;
            ShieldMissing = false;
            WeaverMissing = false;
            WombMissing = false;
        }
        
        public override string GetVersion() => "1.2.1";

        public void Unload()
        {
            Log("Unloading");
            ModHooks.Instance.AfterSavegameLoadHook -= SpriteLoader.ModifyHeroTextures;
            On.GeoControl.Start -= GeoControl_Start;
            SpriteLoader.UnloadAll();
            UnityEngine.Object.Destroy(GameObject.Find("Canvas"));
        }
    }
}