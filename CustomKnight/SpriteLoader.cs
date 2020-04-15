using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Modding;
using System.Linq;
using CustomKnight.Canvas;
using HutongGames.PlayMaker.Actions;
using ModCommon;
using ModCommon.Util;
using UnityEngine.SceneManagement;

namespace CustomKnight
{
    internal class SpriteLoader : MonoBehaviour
    {
        private static GameObject loader;

        private static bool texRoutineRunning;
        private static Coroutine setTexRoutine;

        public static Texture2D DefaultKnightTex { get; private set; }
        public static Texture2D DefaultSprintTex { get; private set; }
        public static Texture2D DefaultWraithsTex { get; private set; }
        public static Texture2D DefaultVoidTex { get; private set; }
        public static Texture2D DefaultVSTex { get; private set; }
        public static Texture2D DefaultHudTex { get; private set; }
        public static Texture2D DefaultOrbFull { get; private set; }
        public static Texture2D DefaultDreamTex { get; private set; }
        public static Texture2D DefaultUnnTex { get; private set; }
        public static Texture2D DefaultCloakTex { get; private set; }
        public static Texture2D DefaultShriekTex { get; private set; }
        public static Texture2D DefaultWingsTex { get; private set; }
        public static Texture2D DefaultQuirrelTex { get; private set; }
        public static Texture2D DefaultWebbedTex { get; private set; }
        public static Texture2D DefaultDNTex { get; private set; }
        //public static Texture2D DefaultP4Tex { get; private set; }
        public static  Texture2D DefaultHornetTex { get; private set; }
        public static Texture2D DefaultBirthTex { get; private set; }
        public static Texture2D DefaultSkullLTex { get; private set; }
        public static Texture2D DefaultSkullRTex { get; private set; }
        public static Texture2D DefaultBaldurTex { get; private set; }
        public static Texture2D DefaultFlukeTex { get; private set; }
        public static Texture2D DefaultGrimmTex { get; private set; }
        public static Texture2D DefaultWeaverTex { get; private set; }
        public static Texture2D DefaultWombTex { get; private set; }

        public static Texture2D KnightTex { get; private set; }
        public static Texture2D SprintTex { get; private set; }
        public static Texture2D WraithsTex { get; private set; }
        public static Texture2D VoidTex { get; private set; }
        public static Texture2D VSTex { get; private set; }
        public static Texture2D HudTex { get; private set; }
        public static Texture2D OrbFull { get; private set; }
        public static Texture2D GeoTex { get; private set; }
        public static Texture2D DreamTex { get; private set; }
        public static Texture2D UnnTex { get; private set; }
        public static Texture2D CloakTex { get; private set; }
        public static Texture2D ShriekTex { get; private set; }
        public static Texture2D WingsTex { get; private set; }
        public static Texture2D QuirrelTex { get; private set; }
        public static Texture2D WebbedTex { get; private set; }
        public static Texture2D DNTex { get; private set; }
        public static Texture2D HornetTex { get; private set; }
        public static Texture2D BirthTex { get; private set; }
        public static Texture2D SkullLTex { get; private set; }
        public static Texture2D SkullRTex { get; private set; }
        public static Texture2D BaldurTex { get; private set; }
        public static Texture2D FlukeTex { get; private set; }
        public static Texture2D GrimmTex { get; private set; }
        public static Texture2D WeaverTex { get; private set; }
        public static Texture2D WombTex { get; private set; }

        public static bool LoadComplete { get; private set; }

        public static void PullDefaultTextures()
        {
            if (HeroController.instance != null && (DefaultKnightTex == null || DefaultSprintTex == null || DefaultWraithsTex == null || DefaultVoidTex == null))
            {
                DefaultKnightTex = HeroController.instance.GetComponent<tk2dSprite>().GetCurrentSpriteDef().material.mainTexture as Texture2D;
                DefaultSprintTex = HeroController.instance.GetComponent<tk2dSpriteAnimator>().GetClipByName("Sprint").frames[0].spriteCollection.spriteDefinitions[0].material.mainTexture as Texture2D;
                DefaultUnnTex = HeroController.instance.GetComponent<tk2dSpriteAnimator>().GetClipByName("Slug Up").frames[0].spriteCollection.spriteDefinitions[0].material.mainTexture as Texture2D;

                DefaultCloakTex = CustomKnight.GameObjects["Cloak"].GetComponent<tk2dSprite>().GetCurrentSpriteDef().material.mainTexture as Texture2D;
                DefaultShriekTex = CustomKnight.GameObjects["Shriek"].GetComponent<tk2dSprite>().GetCurrentSpriteDef().material.mainTexture as Texture2D;
                DefaultWingsTex = CustomKnight.GameObjects["Wings"].GetComponent<tk2dSprite>().GetCurrentSpriteDef().material.mainTexture as Texture2D;
                DefaultQuirrelTex = CustomKnight.GameObjects["Quirrel"].GetComponent<tk2dSprite>().GetCurrentSpriteDef().material.mainTexture as Texture2D;
                DefaultWebbedTex = CustomKnight.GameObjects["Webbed"].GetComponent<tk2dSprite>().GetCurrentSpriteDef().material.mainTexture as Texture2D;
                DefaultDreamTex = CustomKnight.GameObjects["Dream"].GetComponent<tk2dSprite>().GetCurrentSpriteDef().material.mainTexture as Texture2D;
                DefaultDNTex = CustomKnight.GameObjects["DN"].GetComponent<tk2dSprite>().GetCurrentSpriteDef().material.mainTexture as Texture2D;
                DefaultHornetTex = CustomKnight.GameObjects["Hornet"].GetComponent<tk2dSprite>().GetCurrentSpriteDef().material.mainTexture as Texture2D;
                DefaultBirthTex = CustomKnight.GameObjects["Birth"].GetComponent<tk2dSprite>().GetCurrentSpriteDef().material.mainTexture as Texture2D;

                DefaultSkullLTex = CustomKnight.GameObjects["Skull Left"].GetComponent<SpriteRenderer>().sprite.texture;
                DefaultSkullRTex = CustomKnight.GameObjects["Skull Right"].GetComponent<SpriteRenderer>().sprite.texture;
                
                DefaultBaldurTex = _baldur.GetComponent<tk2dSprite>().GetCurrentSpriteDef().material.mainTexture as Texture2D;
                DefaultFlukeTex = _fluke.GetComponent<tk2dSprite>().GetCurrentSpriteDef().material.mainTexture as Texture2D;
                DefaultGrimmTex = _grimm.GetComponent<tk2dSprite>().GetCurrentSpriteDef().material.mainTexture as Texture2D;
                DefaultWeaverTex = _weaver.GetComponent<tk2dSprite>().GetCurrentSpriteDef().material.mainTexture as Texture2D;
                DefaultWombTex = _womb.GetComponent<tk2dSprite>().GetCurrentSpriteDef().material.mainTexture as Texture2D;
                
                foreach (Transform child in HeroController.instance.gameObject.transform)
                {
                    if (child.name == "Spells")
                    {
                        foreach (Transform spellsChild in child)
                        {
                            if (spellsChild.name == "Scr Heads")
                            {
                                DefaultWraithsTex = spellsChild.gameObject.GetComponent<tk2dSprite>().GetCurrentSpriteDef().material.mainTexture as Texture2D;
                            }
                            else if (spellsChild.name == "Scr Heads 2")
                            {
                                DefaultVoidTex = spellsChild.gameObject.GetComponent<tk2dSprite>().GetCurrentSpriteDef().material.mainTexture as Texture2D;
                            }
                        }
                    }
                    else if (child.name == "Focus Effects")
                    {
                        foreach (Transform focusChild in child)
                        {
                            if (focusChild.name == "Heal Anim")
                            {
                                DefaultVSTex = focusChild.gameObject.GetComponent<tk2dSprite>().GetCurrentSpriteDef().material.mainTexture as Texture2D;
                                break;
                            }
                        }
                    }
                }
                foreach (tk2dSprite i in GameCameras.instance.hudCanvas.GetComponentsInChildren<tk2dSprite>())
                {
                    if (i.name == "Health 1")
                    {
                        DefaultHudTex = i.GetCurrentSpriteDef().material.mainTexture as Texture2D;
                        break;
                    }
                }
                foreach (SpriteRenderer i in GameCameras.instance.hudCanvas.GetComponentsInChildren<SpriteRenderer>(true))
                {
                    if (i.name == "Orb Full")
                    {
                        DefaultOrbFull = i.sprite.texture;
                    }
                }
            }
        }
        
        public static void UnloadAll()
        {    
            if (loader != null)
            {
                Destroy(loader);
            }

            if (HeroController.instance != null && 
                DefaultKnightTex != null && 
                DefaultSprintTex != null && 
                DefaultWraithsTex != null && 
                DefaultVoidTex != null &&
                DefaultVSTex != null &&
                DefaultHudTex != null && 
                DefaultOrbFull != null &&
                 DefaultUnnTex != null)
            {
                HeroController.instance.GetComponent<tk2dSprite>().GetCurrentSpriteDef().material.mainTexture = DefaultKnightTex;
                HeroController.instance.GetComponent<tk2dSpriteAnimator>().GetClipByName("Sprint").frames[0].spriteCollection.spriteDefinitions[0].material.mainTexture = DefaultSprintTex;
                HeroController.instance.GetComponent<tk2dSpriteAnimator>().GetClipByName("Slug Up").frames[0].spriteCollection.spriteDefinitions[0].material.mainTexture = DefaultUnnTex;

                CustomKnight.GameObjects["Cloak"].GetComponent<tk2dSprite>().GetCurrentSpriteDef().material.mainTexture = DefaultCloakTex;
                CustomKnight.GameObjects["Shriek"].GetComponent<tk2dSprite>().GetCurrentSpriteDef().material.mainTexture = DefaultShriekTex;
                CustomKnight.GameObjects["Wings"].GetComponent<tk2dSprite>().GetCurrentSpriteDef().material.mainTexture = DefaultWingsTex;
                CustomKnight.GameObjects["Quirrel"].GetComponent<tk2dSprite>().GetCurrentSpriteDef().material.mainTexture = DefaultQuirrelTex;
                CustomKnight.GameObjects["Webbed"].GetComponent<tk2dSprite>().GetCurrentSpriteDef().material.mainTexture = DefaultWebbedTex;
                CustomKnight.GameObjects["Dream"].GetComponent<tk2dSprite>().GetCurrentSpriteDef().material.mainTexture = DefaultDreamTex;
                CustomKnight.GameObjects["DN"].GetComponent<tk2dSprite>().GetCurrentSpriteDef().material.mainTexture = DefaultDNTex;
                //CustomKnight.GameObjects["P4"].GetComponent<tk2dSprite>().GetCurrentSpriteDef().material.mainTexture = DefaultP4Tex;
                CustomKnight.GameObjects["Hornet"].GetComponent<tk2dSprite>().GetCurrentSpriteDef().material.mainTexture = DefaultHornetTex;
                CustomKnight.GameObjects["Birth"].GetComponent<tk2dSprite>().GetCurrentSpriteDef().material.mainTexture = DefaultBirthTex;
                
                Vector2 skullLPivot = CustomKnight.GameObjects["Skull Left"].GetComponent<SpriteRenderer>().sprite.pivot; 
                CustomKnight.GameObjects["Skull Left"].GetComponent<SpriteRenderer>().sprite = Sprite.Create(DefaultSkullLTex, new Rect(0, 0, DefaultSkullLTex.width, DefaultSkullLTex.height), skullLPivot);
                Vector2 skullRPivot = CustomKnight.GameObjects["Skull Right"].GetComponent<SpriteRenderer>().sprite.pivot; 
                CustomKnight.GameObjects["Skull Right"].GetComponent<SpriteRenderer>().sprite = Sprite.Create(DefaultSkullRTex, new Rect(0, 0, DefaultSkullRTex.width, DefaultSkullRTex.height), skullRPivot);

                _baldur.GetComponent<tk2dSprite>().GetCurrentSpriteDef().material.mainTexture = DefaultBaldurTex;
                _fluke.GetComponent<tk2dSprite>().GetCurrentSpriteDef().material.mainTexture = DefaultFlukeTex;
                _grimm.GetComponent<tk2dSprite>().GetCurrentSpriteDef().material.mainTexture = DefaultGrimmTex;
                _weaver.GetComponent<tk2dSprite>().GetCurrentSpriteDef().material.mainTexture = DefaultWeaverTex;
                _womb.GetComponent<tk2dSprite>().GetCurrentSpriteDef().material.mainTexture = DefaultWombTex;

                foreach (Transform child in HeroController.instance.gameObject.transform)
                {
                    if (child.name == "Spells")
                    {
                        foreach (Transform spellsChild in child)
                        {
                            if (spellsChild.name == "Scr Heads" || spellsChild.name == "Scr Base")
                            {
                                spellsChild.gameObject.GetComponent<tk2dSprite>().GetCurrentSpriteDef().material.mainTexture = DefaultWraithsTex;
                            }
                            else if (spellsChild.name == "Scr Heads 2" || spellsChild.name == "Scr Base 2")
                            {
                                spellsChild.gameObject.GetComponent<tk2dSprite>().GetCurrentSpriteDef().material.mainTexture = DefaultVoidTex;
                            }
                        }
                    }
                    else if (child.name == "Focus Effects")
                    {
                        foreach (Transform focusChild in child)
                        {
                            if (focusChild.name == "Heal Anim")
                            {
                                focusChild.gameObject.GetComponent<tk2dSprite>().GetCurrentSpriteDef().material.mainTexture = DefaultVSTex;
                                break;
                            }
                        }
                    }
                }
                foreach (tk2dSprite i in GameCameras.instance.hudCanvas.GetComponentsInChildren<tk2dSprite>())
                {
                    if (i.name == "Health 1")
                    {
                        i.GetCurrentSpriteDef().material.mainTexture = DefaultHudTex;
                        break;
                    }
                }
                foreach (SpriteRenderer i in GameCameras.instance.hudCanvas.GetComponentsInChildren<SpriteRenderer>(true))
                {
                    if (i.name == "Orb Full")
                    {
                        i.sprite = Sprite.Create(DefaultOrbFull, new Rect(0, 0, DefaultOrbFull.width, DefaultOrbFull.height), new Vector2(0.5f, 0.5f));
                    }
                }
            }

            Log("E");
            if (texRoutineRunning && GameManager.instance != null)
            {
                GameManager.instance.StopCoroutine(setTexRoutine);
                texRoutineRunning = false;
            }

            DestroyObjects();
        }

        public static void Load()
        {
            if (loader == null)
            {
                Log("Creating new Loader");
                loader = new GameObject("Loader");
                loader.AddComponent<SpriteLoader>();
                DontDestroyOnLoad(loader);
            }
        }

        public static void ModifyHeroTextures(SaveGameData data = null)
        {
            if (!texRoutineRunning)
            {
                Log("Running SetHeroTex");
                setTexRoutine = GameManager.instance.StartCoroutine(SetHeroTex());
                texRoutineRunning = true;
            }
        }

        private static GameObject _baldur;
        private static GameObject _fluke;
        private static GameObject _grimm;
        private static GameObject _weaver;
        private static GameObject _womb;
        public IEnumerator Start()
        {
            yield return new WaitWhile(() => HeroController.instance == null);

            GameObject charmEffects = HeroController.instance.gameObject.FindGameObjectInChildren("Charm Effects");

            _baldur = charmEffects.FindGameObjectInChildren("Blocker Shield").FindGameObjectInChildren("Shell Anim");
            
            PlayMakerFSM poolFlukes = charmEffects.LocateMyFSM("Pool Flukes");
            _fluke = poolFlukes.GetAction<CreateGameObjectPool>("Pool Normal", 0).prefab.Value;

            PlayMakerFSM spawnGrimmchild = charmEffects.LocateMyFSM("Spawn Grimmchild");
            _grimm = spawnGrimmchild.GetAction<SpawnObjectFromGlobalPool>("Spawn", 2).gameObject.Value;

            PlayMakerFSM weaverlingControl = charmEffects.LocateMyFSM("Weaverling Control");
            _weaver = weaverlingControl.GetAction<SpawnObjectFromGlobalPool>("Spawn", 0).gameObject.Value;

            PlayMakerFSM hatchlingSpawn = charmEffects.LocateMyFSM("Hatchling Spawn");
            _womb = hatchlingSpawn.GetAction<SpawnObjectFromGlobalPool>("Hatch", 2).gameObject.Value;

            StartCoroutine(LoadSprites());

            UnityEngine.SceneManagement.SceneManager.activeSceneChanged += OnSceneChanged;
        }

        private void OnSceneChanged(Scene prevScene, Scene nextScene)
        {
            if (nextScene.name == "Dream_Final_Boss" || nextScene.name == "GG_Radiance")
            {
                Log("Modifying Skull Pieces...");
                GameObject skullLeft = GameObject.Find("Boss Control").FindGameObjectInChildren("Absolute Radiance").FindGameObjectInChildren("Death").FindGameObjectInChildren("Knight Split").FindGameObjectInChildren("hollow_knight_skull_left");
                Log("skullLeft null? " + (skullLeft == null));
                Log("SkullLTex null? " + (SkullLTex == null));
                Log("Creating L Rect");
                Rect skullLRect = new Rect(0, 0, SkullLTex.width, SkullLTex.height);
                Log("Creating L Pivot");
                Vector2 skullLPivot = skullLeft.GetComponent<SpriteRenderer>().sprite.pivot;
                Log("Changing L Sprite");
                skullLeft.GetComponent<SpriteRenderer>().sprite = Sprite.Create(SkullLTex, skullLRect, skullLPivot);
                Log("Getting R");
                GameObject skullRight = GameObject.Find("Boss Control").FindGameObjectInChildren("Absolute Radiance").FindGameObjectInChildren("Death").FindGameObjectInChildren("Knight Split").FindGameObjectInChildren("hollow_knight_skull_left (1)");
                Log("Creating R Rect");
                Rect skullRRect = new Rect(0, 0, SkullRTex.width, SkullRTex.height);
                Log("Creating R Pivot");
                Vector2 skullRPivot = skullRight.GetComponent<SpriteRenderer>().sprite.pivot;
                Log("Changing R Sprite");
                skullRight.GetComponent<SpriteRenderer>().sprite = Sprite.Create(SkullRTex, skullRRect, skullRPivot);
                Log("Modified Skull Pieces!");
            }
        }

        public static Texture2D DuplicateTexture(Texture2D source)
        {
            RenderTexture renderTex = RenderTexture.GetTemporary(
                source.width,
                source.height,
                0,
                RenderTextureFormat.Default,
                RenderTextureReadWrite.Linear);

            Graphics.Blit(source, renderTex);
            RenderTexture previous = RenderTexture.active;
            RenderTexture.active = renderTex;
            Texture2D readableTex = new Texture2D(source.width, source.height);
            readableTex.ReadPixels(new Rect(0, 0, renderTex.width, renderTex.height), 0, 0);
            readableTex.Apply();
            RenderTexture.active = previous;
            RenderTexture.ReleaseTemporary(renderTex);
            return readableTex;
        }
        
        private static void DestroyObjects()
        {
            if (KnightTex != null)
            {
                Destroy(KnightTex);
            }

            if (SprintTex != null)
            {
                Destroy(KnightTex);
            }

            if (WraithsTex != null)
            {
                Destroy(WraithsTex);
            }

            if (VoidTex != null)
            {
                Destroy(VoidTex);
            }

            if (VSTex != null)
            {
                Destroy(VSTex);
            }

            if (HudTex != null)
            {
                Destroy(HudTex);
            }

            if (OrbFull != null)
            {
                Destroy(OrbFull);
            }

            if (GeoTex != null)
            {
                Destroy(GeoTex);
            }

            if (DreamTex != null)
            {
                Destroy(DreamTex);
            }

            if (CloakTex != null)
            {
                Destroy(CloakTex);
            }

            if (ShriekTex != null)
            {
                Destroy(ShriekTex);
            }

            if (WingsTex != null)
            {
                Destroy(WingsTex);
            }

            if (QuirrelTex != null)
            {
                Destroy(QuirrelTex);
            }
            
            if (WebbedTex != null)
            {
                Destroy(WebbedTex);
            }

            if (DNTex != null)
            {
                Destroy(DNTex);
            }

            /*if (P4Tex != null)
            {
                Destroy(P4Tex);
            }*/

            if (HornetTex != null)
            {
                Destroy(HornetTex);
            }

            if (BirthTex != null)
            {
                Destroy(BirthTex);
            }

            if (SkullLTex != null)
            {
                Destroy(SkullLTex);
            }

            if (SkullRTex != null)
            {
                Destroy(SkullRTex);
            }

            if (BaldurTex != null)
            {
                Destroy(BaldurTex);
            }
            
            if (FlukeTex != null)
            {
                Destroy(FlukeTex);
            }
            
            if (GrimmTex != null)
            {
                Destroy(GrimmTex);
            }

            if (WeaverTex != null)
            {
                Destroy(WeaverTex);
            }

            if (WombTex != null)
            {
                Destroy(WombTex);
            }
            
            LoadComplete = false;
        }

        private IEnumerator LoadSprites()
        {
            Modding.Logger.Log("[CustomKnight] - Starting texture load");

            LoadComplete = false;
            
            if (!CustomKnight.KnightMissing)
            {
                WWW knight = new WWW(("file:///" + CustomKnight.DATA_DIR + "/" + CustomKnight.SKIN_FOLDER + "/" + CustomKnight.KNIGHT_PNG).Replace("\\", "/"));
                yield return knight;
                if (KnightTex != null)
                {
                    Destroy(KnightTex);
                }
                KnightTex = knight.textureNonReadable;
            }
            
            if (!CustomKnight.SprintMissing)
            {
                WWW sprint = new WWW(("file:///" + CustomKnight.DATA_DIR + "/" + CustomKnight.SKIN_FOLDER + "/" + CustomKnight.SPRINT_PNG).Replace("\\", "/"));
                yield return sprint;
                if (SprintTex != null)
                {
                    Destroy(SprintTex);
                }
                SprintTex = sprint.textureNonReadable;
            }
            
            if (!CustomKnight.WraithsMissing)
            {
                WWW wraiths = new WWW(("file:///" + CustomKnight.DATA_DIR + "/" + CustomKnight.SKIN_FOLDER + "/" + CustomKnight.WRAITHS_PNG).Replace("\\", "/"));
                yield return wraiths;
                if (WraithsTex != null)
                {
                    Destroy(WraithsTex);
                }
                WraithsTex = wraiths.textureNonReadable;
            }
            
            if (!CustomKnight.VoidMissing)
            {
                WWW @void = new WWW(("file:///" + CustomKnight.DATA_DIR + "/" + CustomKnight.SKIN_FOLDER + "/" + CustomKnight.VOID_PNG).Replace("\\", "/"));
                yield return @void;
                if (VoidTex != null)
                {
                    Destroy(VoidTex);
                }
                VoidTex = @void.textureNonReadable;
            }
            
            if (!CustomKnight.VSMissing)
            {
                WWW vs = new WWW(("file:///" + CustomKnight.DATA_DIR + "/" + CustomKnight.SKIN_FOLDER + "/" + CustomKnight.VS_PNG).Replace("\\", "/"));
                yield return vs;
                if (VSTex != null)
                {
                    Destroy(VSTex);
                }
                VSTex = vs.textureNonReadable;
            }
            
            if (!CustomKnight.HUDMissing)
            {
                WWW hud = new WWW(("file:///" + CustomKnight.DATA_DIR + "/" + CustomKnight.SKIN_FOLDER + "/" + CustomKnight.HUD_PNG).Replace("\\", "/"));
                yield return hud;
                if (HudTex != null)
                {
                    Destroy(HudTex);
                }
                HudTex = hud.textureNonReadable;
            }
            
            if (!CustomKnight.FullMissing)
            {
                WWW orb = new WWW(("file:///" + CustomKnight.DATA_DIR + "/" + CustomKnight.SKIN_FOLDER + "/" + CustomKnight.FULL_PNG).Replace("\\", "/"));
                yield return orb;
                if (OrbFull != null)
                {
                    Destroy(OrbFull);
                }
                OrbFull = orb.textureNonReadable;
            }
            
            if (!CustomKnight.GeoMissing)
            {
                WWW geo= new WWW(("file:///" + CustomKnight.DATA_DIR + "/" + CustomKnight.SKIN_FOLDER + "/" + CustomKnight.GEO_PNG).Replace("\\", "/"));
                yield return geo;
                if (GeoTex != null)
                {
                    Destroy(GeoTex);
                }
                GeoTex = geo.textureNonReadable;
            }
            
            if (!CustomKnight.DreamMissing)
            {
                WWW dream = new WWW(("file:///" + CustomKnight.DATA_DIR + "/" + CustomKnight.SKIN_FOLDER + "/" + CustomKnight.DREAM_PNG).Replace("\\", "/"));
                yield return dream;
                if (DreamTex != null)
                {
                    Destroy(DreamTex);
                }
                DreamTex = dream.textureNonReadable;
            }
            
            if (!CustomKnight.CloakMissing)
            {
                WWW cloak = new WWW(("file:///" + CustomKnight.DATA_DIR + "/" + CustomKnight.SKIN_FOLDER + "/" + CustomKnight.CLOAK_PNG).Replace("\\", "/"));
                yield return cloak;
                if (CloakTex != null)
                {
                    Destroy(CloakTex);
                }
                CloakTex = cloak.textureNonReadable;
            }
            
            if (!CustomKnight.ShriekMissing)
            {
                WWW shriek = new WWW(("file:///" + CustomKnight.DATA_DIR + "/" + CustomKnight.SKIN_FOLDER + "/" + CustomKnight.SHRIEK_PNG).Replace("\\", "/"));
                yield return shriek;
                if (ShriekTex != null)
                {
                    Destroy(ShriekTex);
                }
                ShriekTex = shriek.textureNonReadable;
            }
            
            if (!CustomKnight.WingsMissing)
            {
                WWW wings = new WWW(("file:///" + CustomKnight.DATA_DIR + "/" + CustomKnight.SKIN_FOLDER + "/" + CustomKnight.WINGS_PNG).Replace("\\", "/"));
                yield return wings;
                if (WingsTex != null)
                {
                    Destroy(WingsTex);
                }
                WingsTex = wings.textureNonReadable;
            }
            
            if (!CustomKnight.QuirrelMissing)
            {
                WWW quirrel = new WWW(("file:///" + CustomKnight.DATA_DIR + "/" + CustomKnight.SKIN_FOLDER + "/" + CustomKnight.QUIRREL_PNG).Replace("\\", "/"));
                yield return quirrel;
                if (QuirrelTex != null)
                {
                    Destroy(QuirrelTex);
                }
                QuirrelTex = quirrel.textureNonReadable;
            }
            
            if (!CustomKnight.WebbedMissing)
            {
                WWW webbed = new WWW(("file:///" + CustomKnight.DATA_DIR + "/" + CustomKnight.SKIN_FOLDER + "/" + CustomKnight.WEBBED_PNG).Replace("\\", "/"));
                yield return webbed;
                if (WebbedTex != null)
                {
                    Destroy(WebbedTex);
                }
                WebbedTex = webbed.textureNonReadable;
            }
            
            if (!CustomKnight.DNMissing)
            {
                WWW dn = new WWW(("file:///" + CustomKnight.DATA_DIR + "/" + CustomKnight.SKIN_FOLDER + "/" + CustomKnight.DN_PNG).Replace("\\", "/"));
                yield return dn;
                if (DNTex != null)
                {
                    Destroy(DNTex);
                }
                DNTex = dn.textureNonReadable;
            }
            
            if (!CustomKnight.HornetMissing)
            {
                WWW hornet = new WWW(("file:///" + CustomKnight.DATA_DIR + "/" + CustomKnight.SKIN_FOLDER + "/" + CustomKnight.HORNET_PNG).Replace("\\", "/"));
                yield return hornet;
                if (HornetTex != null)
                {
                    Destroy(HornetTex);
                }
                HornetTex = hornet.textureNonReadable;
            }
            
            if (!CustomKnight.BirthMissing)
            {
                WWW birth = new WWW(("file:///" + CustomKnight.DATA_DIR + "/" + CustomKnight.SKIN_FOLDER + "/" + CustomKnight.BIRTH_PNG).Replace("\\", "/"));
                yield return birth;
                if (BirthTex != null)
                {
                    Destroy(BirthTex);
                }
                BirthTex = birth.textureNonReadable;
            }
            
            if (!CustomKnight.BaldurMissing)
            {
                WWW baldur = new WWW(("file:///" + CustomKnight.DATA_DIR + "/" + CustomKnight.SKIN_FOLDER + "/" + CustomKnight.BALDUR_PNG).Replace("\\", "/"));
                yield return baldur;
                if (BaldurTex != null)
                {
                    Destroy(BaldurTex);
                }
                BaldurTex = baldur.textureNonReadable;
            }
            
            if (!CustomKnight.FlukeMissing)
            {
                WWW fluke = new WWW(("file:///" + CustomKnight.DATA_DIR + "/" + CustomKnight.SKIN_FOLDER + "/" + CustomKnight.FLUKE_PNG).Replace("\\", "/"));
                yield return fluke;
                if (FlukeTex != null)
                {
                    Destroy(FlukeTex);
                }
                FlukeTex = fluke.textureNonReadable;
            }
            
            if (!CustomKnight.GrimmMissing)
            {
                WWW grimm = new WWW(("file:///" + CustomKnight.DATA_DIR + "/" + CustomKnight.SKIN_FOLDER + "/" + CustomKnight.GRIMM_PNG).Replace("\\", "/"));
                yield return grimm;
                if (GrimmTex != null)
                {
                    Destroy(GrimmTex);
                }
                GrimmTex = grimm.textureNonReadable;
            }
            
            if (!CustomKnight.WeaverMissing)
            {
                WWW weaver = new WWW(("file:///" + CustomKnight.DATA_DIR + "/" + CustomKnight.SKIN_FOLDER + "/" + CustomKnight.WEAVER_PNG).Replace("\\", "/"));
                yield return weaver;
                if (WeaverTex != null)
                {
                    Destroy(WeaverTex);
                }
                WeaverTex = weaver.textureNonReadable;
            }
            
            if (!CustomKnight.WombMissing)
            {
                WWW womb = new WWW(("file:///" + CustomKnight.DATA_DIR + "/" + CustomKnight.SKIN_FOLDER + "/" + CustomKnight.WOMB_PNG).Replace("\\", "/"));
                yield return womb;
                if (WombTex != null)
                {
                    Destroy(WombTex);
                }
                WombTex = womb.textureNonReadable;
            }

            Log("Texture load done");
            ModifyHeroTextures();

            LoadComplete = true;
            Destroy(gameObject);
        }

        private static IEnumerator SetHeroTex()
        {
            while (!LoadComplete || HeroController.instance == null)
            {
                yield return null;
            }

            PullDefaultTextures();

            if (!CustomKnight.KnightMissing)
            {
                HeroController.instance.GetComponent<tk2dSprite>().GetCurrentSpriteDef().material.mainTexture = KnightTex;
            }

            if (!CustomKnight.SprintMissing)
            {
                HeroController.instance.GetComponent<tk2dSpriteAnimator>().GetClipByName("Sprint").frames[0].spriteCollection.spriteDefinitions[0].material.mainTexture = SprintTex;
            }

            if (!CustomKnight.UnnMissing)
            {
                HeroController.instance.GetComponent<tk2dSpriteAnimator>().GetClipByName("Slug Up").frames[0].spriteCollection.spriteDefinitions[0].material.mainTexture = UnnTex;
            }

            if (!CustomKnight.CloakMissing)
            {
                CustomKnight.GameObjects["Cloak"].GetComponent<tk2dSprite>().GetCurrentSpriteDef().material.mainTexture = CloakTex;
            }

            if (!CustomKnight.ShriekMissing)
            {
                CustomKnight.GameObjects["Shriek"].GetComponent<tk2dSprite>().GetCurrentSpriteDef().material.mainTexture = ShriekTex;    
            }

            if (!CustomKnight.WingsMissing)
            {
                CustomKnight.GameObjects["Wings"].GetComponent<tk2dSprite>().GetCurrentSpriteDef().material.mainTexture = WingsTex;    
            }

            if (!CustomKnight.QuirrelMissing)
            {
                CustomKnight.GameObjects["Quirrel"].GetComponent<tk2dSprite>().GetCurrentSpriteDef().material.mainTexture = QuirrelTex;    
            }

            if (!CustomKnight.WebbedMissing)
            {
                CustomKnight.GameObjects["Webbed"].GetComponent<tk2dSprite>().GetCurrentSpriteDef().material.mainTexture = WebbedTex;    
            }

            if (!CustomKnight.DreamMissing)
            {
                CustomKnight.GameObjects["Dream"].GetComponent<tk2dSprite>().GetCurrentSpriteDef().material.mainTexture = DreamTex;    
            }

            if (!CustomKnight.DNMissing)
            {
                CustomKnight.GameObjects["DN"].GetComponent<tk2dSprite>().GetCurrentSpriteDef().material.mainTexture = DNTex;    
            }

            if (!CustomKnight.HornetMissing)
            {
                CustomKnight.GameObjects["Hornet"].GetComponent<tk2dSprite>().GetCurrentSpriteDef().material.mainTexture = HornetTex;    
            }

            if (!CustomKnight.BirthMissing)
            {
                CustomKnight.GameObjects["Birth"].GetComponent<tk2dSprite>().GetCurrentSpriteDef().material.mainTexture = BirthTex;    
            }
            
            /*Vector2 skullLPivot = CustomKnight.GameObjects["Skull Left"].GetComponent<SpriteRenderer>().sprite.pivot;
            skullLPivot = new Vector2(SkullLTex.width / 2.0f, SkullLTex.height / 2.0f); 
            Log("Skull L Pivot: " + skullLPivot);
            CustomKnight.GameObjects["Skull Left"].GetComponent<SpriteRenderer>().sprite = Sprite.Create(SkullLTex, new Rect(0, 0, SkullLTex.width, SkullLTex.height), skullLPivot);
            Vector2 skullRPivot = CustomKnight.GameObjects["Skull Right"].GetComponent<SpriteRenderer>().sprite.pivot;
            skullRPivot = new Vector2(SkullRTex.width / 2.0f, SkullRTex.height / 2.0f);
            Log("Skull R Pivot: " + skullRPivot);
            CustomKnight.GameObjects["Skull Right"].GetComponent<SpriteRenderer>().sprite = Sprite.Create(SkullRTex, new Rect(0, 0, SkullRTex.width, SkullRTex.height), skullRPivot);*/

            if (!CustomKnight.BaldurMissing)
            {
                _baldur.GetComponent<tk2dSprite>().GetCurrentSpriteDef().material.mainTexture = BaldurTex;    
            }

            if (!CustomKnight.FlukeMissing)
            {
                _fluke.GetComponent<tk2dSprite>().GetCurrentSpriteDef().material.mainTexture = FlukeTex;    
            }

            if (!CustomKnight.GrimmMissing)
            {
                _grimm.GetComponent<tk2dSprite>().GetCurrentSpriteDef().material.mainTexture = GrimmTex;    
            }

            if (!CustomKnight.WeaverMissing)
            {
                _weaver.GetComponent<tk2dSprite>().GetCurrentSpriteDef().material.mainTexture = WeaverTex;    
            }

            if (!CustomKnight.WombMissing)
            {
                _womb.GetComponent<tk2dSprite>().GetCurrentSpriteDef().material.mainTexture = WombTex;    
            }
            
            foreach (Transform child in HeroController.instance.gameObject.transform)
            {
                if (child.name == "Spells")
                {
                    foreach (Transform spellsChild in child)
                    {
                        if (spellsChild.name == "Scr Heads" || spellsChild.name == "Scr Base")
                        {
                            if (!CustomKnight.WraithsMissing)
                            {
                                spellsChild.gameObject.GetComponent<tk2dSprite>().GetCurrentSpriteDef().material.mainTexture = WraithsTex;    
                            }
                        }
                        else if (spellsChild.name == "Scr Heads 2" || spellsChild.name == "Scr Base 2")
                        {
                            if (!CustomKnight.VoidMissing)
                            {
                                spellsChild.gameObject.GetComponent<tk2dSprite>().GetCurrentSpriteDef().material.mainTexture = VoidTex;    
                            }
                        }
                    }
                }
                else if (child.name == "Focus Effects")
                {
                    foreach (Transform focusChild in child)
                    {
                        if (focusChild.name == "Heal Anim")
                        {
                            if (!CustomKnight.VSMissing)
                            {
                                focusChild.gameObject.GetComponent<tk2dSprite>().GetCurrentSpriteDef().material.mainTexture = VSTex;
                            }
                            break;
                        }
                    }
                }
            }
            foreach (tk2dSprite i in GameCameras.instance.hudCanvas.GetComponentsInChildren<tk2dSprite>())
            {
                if (i.name == "Health 1")
                {
                    if (!CustomKnight.HUDMissing)
                    {
                        i.GetCurrentSpriteDef().material.mainTexture = HudTex;
                    }
                    break;
                }
            }
            foreach (SpriteRenderer i in GameCameras.instance.hudCanvas.GetComponentsInChildren<SpriteRenderer>(true))
            {
                if (i.name == "Orb Full")
                {
                    if (!CustomKnight.FullMissing)
                    {
                        i.sprite = Sprite.Create(OrbFull, new Rect(0, 0, OrbFull.width, OrbFull.height), new Vector2(0.5f, 0.5f));    
                    }
                }
                else if (i.name == "Pulse Sprite")
                {
                    if (i.gameObject != null) Destroy(i.gameObject);
                }
            }
            
            Log("Setting texRoutineRunning false");
            texRoutineRunning = false;
        }

        private static void Log(object message) => Modding.Logger.Log("[Sprite Loader] " + message);
    }
}
