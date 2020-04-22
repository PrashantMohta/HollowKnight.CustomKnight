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
        public static  Texture2D DefaultHornetTex { get; private set; }
        public static Texture2D DefaultBirthTex { get; private set; }
        public static Texture2D DefaultBaldurTex { get; private set; }
        public static Texture2D DefaultFlukeTex { get; private set; }
        public static Texture2D DefaultGrimmTex { get; private set; }
        public static Texture2D DefaultShieldTex { get; private set; }
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
        public static Texture2D BaldurTex { get; private set; }
        public static Texture2D FlukeTex { get; private set; }
        public static Texture2D GrimmTex { get; private set; }
        public static Texture2D ShieldTex { get; private set; }
        public static Texture2D WeaverTex { get; private set; }
        public static Texture2D WombTex { get; private set; }

        public static bool LoadComplete { get; private set; }

        public static void PullDefaultTextures()
        {
            if (HeroController.instance != null && 
                (DefaultKnightTex == null ||
                 DefaultSprintTex == null || 
                 DefaultUnnTex == null || 
                 DefaultWraithsTex == null || 
                 DefaultVoidTex == null ||
                 DefaultCloakTex == null ||
                 DefaultShriekTex == null ||
                 DefaultWingsTex == null ||
                 DefaultQuirrelTex == null ||
                 DefaultWebbedTex == null ||
                 DefaultDreamTex == null ||
                 DefaultDNTex == null ||
                 DefaultHornetTex == null ||
                 DefaultBirthTex == null ||
                 DefaultBaldurTex == null ||
                 DefaultFlukeTex == null ||
                 DefaultGrimmTex == null ||
                 DefaultShieldTex == null ||
                 DefaultWeaverTex == null ||
                 DefaultWombTex == null)
                )
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

                DefaultBaldurTex = _baldur.GetComponent<tk2dSprite>().GetCurrentSpriteDef().material.mainTexture as Texture2D;
                DefaultFlukeTex = _fluke.GetComponent<tk2dSprite>().GetCurrentSpriteDef().material.mainTexture as Texture2D;
                DefaultGrimmTex = _grimm.GetComponent<tk2dSprite>().GetCurrentSpriteDef().material.mainTexture as Texture2D;
                DefaultShieldTex = _shield.GetComponent<tk2dSprite>().GetCurrentSpriteDef().material.mainTexture as Texture2D;
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
                DefaultUnnTex != null &&
                DefaultWraithsTex != null && 
                DefaultVoidTex != null &&
                DefaultCloakTex != null &&
                DefaultShriekTex != null &&
                DefaultWingsTex != null &&
                DefaultQuirrelTex != null &&
                DefaultWebbedTex != null &&
                DefaultDreamTex != null &&
                DefaultDNTex != null &&
                DefaultHornetTex != null &&
                DefaultBirthTex != null &&
                DefaultBaldurTex != null &&
                DefaultFlukeTex != null &&
                DefaultGrimmTex != null &&
                DefaultShieldTex != null &&
                DefaultWeaverTex != null &&
                DefaultWombTex != null)
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
                CustomKnight.GameObjects["Hornet"].GetComponent<tk2dSprite>().GetCurrentSpriteDef().material.mainTexture = DefaultHornetTex;
                CustomKnight.GameObjects["Birth"].GetComponent<tk2dSprite>().GetCurrentSpriteDef().material.mainTexture = DefaultBirthTex;
                
                _baldur.GetComponent<tk2dSprite>().GetCurrentSpriteDef().material.mainTexture = DefaultBaldurTex;
                _fluke.GetComponent<tk2dSprite>().GetCurrentSpriteDef().material.mainTexture = DefaultFlukeTex;
                _grimm.GetComponent<tk2dSprite>().GetCurrentSpriteDef().material.mainTexture = DefaultGrimmTex;
                _shield.GetComponent<tk2dSprite>().GetCurrentSpriteDef().material.mainTexture = DefaultShieldTex;
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
                loader = new GameObject("Loader");
                loader.AddComponent<SpriteLoader>();
                DontDestroyOnLoad(loader);
            }
        }

        public static void ModifyHeroTextures(SaveGameData data = null)
        {
            if (!texRoutineRunning)
            {
                setTexRoutine = GameManager.instance.StartCoroutine(SetHeroTex());
                texRoutineRunning = true;
            }
        }

        private static GameObject _baldur;
        private static GameObject _fluke;
        private static GameObject _grimm;
        private static GameObject _shield;
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

            PlayMakerFSM spawnOrbitShield = charmEffects.LocateMyFSM("Spawn Orbit Shield");
            GameObject orbitShield = spawnOrbitShield.GetAction<SpawnObjectFromGlobalPool>("Spawn", 2).gameObject.Value;
            _shield = orbitShield.FindGameObjectInChildren("Shield");
            Texture2D tex =(Texture2D) _shield.GetComponent<tk2dSprite>().GetCurrentSpriteDef().material.mainTexture;
            Texture2D readable = DuplicateTexture(tex);
            byte[] bytes = readable.EncodeToPNG();
            File.WriteAllBytes(Path.Combine(Application.streamingAssetsPath, "Shield.png"), bytes);
            
            PlayMakerFSM weaverlingControl = charmEffects.LocateMyFSM("Weaverling Control");
            _weaver = weaverlingControl.GetAction<SpawnObjectFromGlobalPool>("Spawn", 0).gameObject.Value;

            PlayMakerFSM hatchlingSpawn = charmEffects.LocateMyFSM("Hatchling Spawn");
            _womb = hatchlingSpawn.GetAction<SpawnObjectFromGlobalPool>("Hatch", 2).gameObject.Value;

            LoadSprites();
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
                Destroy(SprintTex);
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

            if (HornetTex != null)
            {
                Destroy(HornetTex);
            }

            if (BirthTex != null)
            {
                Destroy(BirthTex);
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

            if (ShieldTex != null)
            {
                Destroy(ShieldTex);
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

        private void LoadSprites()
        {
            LoadComplete = false;
            
            if (!CustomKnight.KnightMissing)
            {
                byte[] bytes = File.ReadAllBytes((CustomKnight.DATA_DIR + "/" + CustomKnight.SKIN_FOLDER + "/" + CustomKnight.KNIGHT_PNG).Replace("\\", "/"));
                if (KnightTex != null)
                {
                    Destroy(KnightTex);
                }
                KnightTex = new Texture2D(1, 1);
                KnightTex.LoadImage(bytes);
            }
            
            if (!CustomKnight.SprintMissing)
            {
                byte[] bytes = File.ReadAllBytes((CustomKnight.DATA_DIR + "/" + CustomKnight.SKIN_FOLDER + "/" + CustomKnight.SPRINT_PNG).Replace("\\", "/"));
                if (SprintTex != null)
                {
                    Destroy(SprintTex);
                }
                SprintTex = new Texture2D(1, 1);
                SprintTex.LoadImage(bytes);
            }
            
            if (!CustomKnight.UnnMissing)
            {
                byte[] bytes = File.ReadAllBytes((CustomKnight.DATA_DIR + "/" + CustomKnight.SKIN_FOLDER + "/" + CustomKnight.UNN_PNG).Replace("\\", "/"));
                if (UnnTex != null)
                {
                    Destroy(UnnTex);
                }
                UnnTex = new Texture2D(1, 1);
                UnnTex.LoadImage(bytes);
            }
            
            if (!CustomKnight.WraithsMissing)
            {
                byte[] bytes = File.ReadAllBytes((CustomKnight.DATA_DIR + "/" + CustomKnight.SKIN_FOLDER + "/" + CustomKnight.WRAITHS_PNG).Replace("\\", "/"));
                if (WraithsTex != null)
                {
                    Destroy(WraithsTex);
                }
                WraithsTex = new Texture2D(1, 1);
                WraithsTex.LoadImage(bytes);
            }
            
            if (!CustomKnight.VoidMissing)
            {
                byte[] bytes = File.ReadAllBytes((CustomKnight.DATA_DIR + "/" + CustomKnight.SKIN_FOLDER + "/" + CustomKnight.VOID_PNG).Replace("\\", "/"));
                if (VoidTex != null)
                {
                    Destroy(VoidTex);
                }
                VoidTex = new Texture2D(1, 1);
                VoidTex.LoadImage(bytes);
            }
            
            if (!CustomKnight.VSMissing)
            {
                byte[] bytes = File.ReadAllBytes((CustomKnight.DATA_DIR + "/" + CustomKnight.SKIN_FOLDER + "/" + CustomKnight.VS_PNG).Replace("\\", "/"));
                if (VSTex != null)
                {
                    Destroy(VSTex);
                }
                VSTex = new Texture2D(1, 1);
                VSTex.LoadImage(bytes);
            }
            
            if (!CustomKnight.HUDMissing)
            {
                byte[] bytes = File.ReadAllBytes((CustomKnight.DATA_DIR + "/" + CustomKnight.SKIN_FOLDER + "/" + CustomKnight.HUD_PNG).Replace("\\", "/"));
                if (HudTex != null)
                {
                    Destroy(HudTex);
                }
                HudTex = new Texture2D(1, 1); 
                HudTex.LoadImage(bytes);
            }
            
            if (!CustomKnight.FullMissing)
            {
                byte[] bytes = File.ReadAllBytes((CustomKnight.DATA_DIR + "/" + CustomKnight.SKIN_FOLDER + "/" + CustomKnight.FULL_PNG).Replace("\\", "/"));
                if (OrbFull != null)
                {
                    Destroy(OrbFull);
                }
                OrbFull = new Texture2D(1, 1);
                OrbFull.LoadImage(bytes);
            }
            
            if (!CustomKnight.GeoMissing)
            {
                byte[] bytes = File.ReadAllBytes((CustomKnight.DATA_DIR + "/" + CustomKnight.SKIN_FOLDER + "/" + CustomKnight.GEO_PNG).Replace("\\", "/"));
                if (GeoTex != null)
                {
                    Destroy(GeoTex);
                }
                GeoTex = new Texture2D(1, 1);
                GeoTex.LoadImage(bytes);
            }
            
            if (!CustomKnight.DreamMissing)
            {
                byte[] bytes = File.ReadAllBytes((CustomKnight.DATA_DIR + "/" + CustomKnight.SKIN_FOLDER + "/" + CustomKnight.DREAM_PNG).Replace("\\", "/"));
                if (DreamTex != null)
                {
                    Destroy(DreamTex);
                }
                DreamTex = new Texture2D(1, 1);
                DreamTex.LoadImage(bytes);
            }
            
            if (!CustomKnight.CloakMissing)
            {
                byte[] bytes = File.ReadAllBytes((CustomKnight.DATA_DIR + "/" + CustomKnight.SKIN_FOLDER + "/" + CustomKnight.CLOAK_PNG).Replace("\\", "/"));
                if (CloakTex != null)
                {
                    Destroy(CloakTex);
                }
                CloakTex = new Texture2D(1, 1);
                CloakTex.LoadImage(bytes);
            }
            
            if (!CustomKnight.ShriekMissing)
            {
                byte[] bytes = File.ReadAllBytes((CustomKnight.DATA_DIR + "/" + CustomKnight.SKIN_FOLDER + "/" + CustomKnight.SHRIEK_PNG).Replace("\\", "/"));
                if (ShriekTex != null)
                {
                    Destroy(ShriekTex);
                }
                ShriekTex = new Texture2D(1, 1);
                ShriekTex.LoadImage(bytes);
            }
            
            if (!CustomKnight.WingsMissing)
            {
                byte[] bytes = File.ReadAllBytes((CustomKnight.DATA_DIR + "/" + CustomKnight.SKIN_FOLDER + "/" + CustomKnight.WINGS_PNG).Replace("\\", "/"));
                if (WingsTex != null)
                {
                    Destroy(WingsTex);
                }
                WingsTex = new Texture2D(1, 1);
                WingsTex.LoadImage(bytes);
            }
            
            if (!CustomKnight.QuirrelMissing)
            {
                byte[] bytes = File.ReadAllBytes((CustomKnight.DATA_DIR + "/" + CustomKnight.SKIN_FOLDER + "/" + CustomKnight.QUIRREL_PNG).Replace("\\", "/"));
                if (QuirrelTex != null)
                {
                    Destroy(QuirrelTex);
                }
                QuirrelTex = new Texture2D(1, 1);
                QuirrelTex.LoadImage(bytes);
            }
            
            if (!CustomKnight.WebbedMissing)
            {
                byte[] bytes = File.ReadAllBytes((CustomKnight.DATA_DIR + "/" + CustomKnight.SKIN_FOLDER + "/" + CustomKnight.WEBBED_PNG).Replace("\\", "/"));
                if (WebbedTex != null)
                {
                    Destroy(WebbedTex);
                }
                WebbedTex = new Texture2D(1, 1);
                WebbedTex.LoadImage(bytes);
            }

            if (!CustomKnight.DNMissing)
            {
                byte[] bytes = File.ReadAllBytes((CustomKnight.DATA_DIR + "/" + CustomKnight.SKIN_FOLDER + "/" + CustomKnight.DN_PNG).Replace("\\", "/"));
                if (DNTex != null)
                {
                    Destroy(DNTex);
                }

                DNTex = new Texture2D(1, 1);
                DNTex.LoadImage(bytes);
            }

            if (!CustomKnight.HornetMissing)
            {
                byte[] bytes = File.ReadAllBytes((CustomKnight.DATA_DIR + "/" + CustomKnight.SKIN_FOLDER + "/" + CustomKnight.HORNET_PNG).Replace("\\", "/"));
                if (HornetTex != null)
                {
                    Destroy(HornetTex);
                }
                HornetTex = new Texture2D(1, 1);
                HornetTex.LoadImage(bytes);
            }
            
            if (!CustomKnight.BirthMissing)
            {
                byte[] bytes = File.ReadAllBytes((CustomKnight.DATA_DIR + "/" + CustomKnight.SKIN_FOLDER + "/" + CustomKnight.BIRTH_PNG).Replace("\\", "/"));
                if (BirthTex != null)
                {
                    Destroy(BirthTex);
                }
                BirthTex = new Texture2D(1, 1);
                BirthTex.LoadImage(bytes);    
            }
            
            if (!CustomKnight.BaldurMissing)
            {
                byte[] bytes = File.ReadAllBytes((CustomKnight.DATA_DIR + "/" + CustomKnight.SKIN_FOLDER + "/" + CustomKnight.BALDUR_PNG).Replace("\\", "/"));
                if (BaldurTex != null)
                {
                    Destroy(BaldurTex);
                }
                BaldurTex = new Texture2D(1, 1);
                BaldurTex.LoadImage(bytes);
            }
            
            if (!CustomKnight.FlukeMissing)
            {
                byte[] bytes = File.ReadAllBytes((CustomKnight.DATA_DIR + "/" + CustomKnight.SKIN_FOLDER + "/" + CustomKnight.FLUKE_PNG).Replace("\\", "/"));
                if (FlukeTex != null)
                {
                    Destroy(FlukeTex);
                }
                FlukeTex = new Texture2D(1, 1);
                FlukeTex.LoadImage(bytes);
            }
            
            if (!CustomKnight.GrimmMissing)
            {
                byte[] bytes = File.ReadAllBytes((CustomKnight.DATA_DIR + "/" + CustomKnight.SKIN_FOLDER + "/" + CustomKnight.GRIMM_PNG).Replace("\\", "/"));
                if (GrimmTex != null)
                {
                    Destroy(GrimmTex);
                }
                GrimmTex = new Texture2D(1, 1);
                GrimmTex.LoadImage(bytes);
            }
            
            if (!CustomKnight.ShieldMissing)
            {
                byte[] bytes = File.ReadAllBytes((CustomKnight.DATA_DIR + "/" + CustomKnight.SKIN_FOLDER + "/" + CustomKnight.SHIELD_PNG).Replace("\\", "/"));
                if (ShieldTex != null)
                {
                    Destroy(ShieldTex);
                }
                ShieldTex = new Texture2D(1, 1);
                ShieldTex.LoadImage(bytes);
            }
            
            if (!CustomKnight.WeaverMissing)
            {
                byte[] bytes = File.ReadAllBytes((CustomKnight.DATA_DIR + "/" + CustomKnight.SKIN_FOLDER + "/" + CustomKnight.WEAVER_PNG).Replace("\\", "/"));
                if (WeaverTex != null)
                {
                    Destroy(WeaverTex);
                }
                WeaverTex = new Texture2D(1, 1);
                WeaverTex.LoadImage(bytes);
            }
            
            if (!CustomKnight.WombMissing)
            {
                byte[] bytes = File.ReadAllBytes((CustomKnight.DATA_DIR + "/" + CustomKnight.SKIN_FOLDER + "/" + CustomKnight.WOMB_PNG).Replace("\\", "/"));
                if (WombTex != null)
                {
                    Destroy(WombTex);
                }
                WombTex = new Texture2D(1, 1);
                WombTex.LoadImage(bytes);
            }
            
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

            HeroController.instance.GetComponent<tk2dSprite>().GetCurrentSpriteDef().material.mainTexture = CustomKnight.KnightMissing ? DefaultKnightTex : KnightTex;
            HeroController.instance.GetComponent<tk2dSpriteAnimator>().GetClipByName("Sprint").frames[0].spriteCollection.spriteDefinitions[0].material.mainTexture = CustomKnight.SprintMissing ? DefaultSprintTex : SprintTex;
            HeroController.instance.GetComponent<tk2dSpriteAnimator>().GetClipByName("Slug Up").frames[0].spriteCollection.spriteDefinitions[0].material.mainTexture = CustomKnight.UnnMissing ? DefaultUnnTex : UnnTex;
            
            CustomKnight.GameObjects["Cloak"].GetComponent<tk2dSprite>().GetCurrentSpriteDef().material.mainTexture = CustomKnight.CloakMissing ? DefaultCloakTex : CloakTex;
            CustomKnight.GameObjects["Shriek"].GetComponent<tk2dSprite>().GetCurrentSpriteDef().material.mainTexture = CustomKnight.ShriekMissing ? DefaultShriekTex : ShriekTex;
            CustomKnight.GameObjects["Wings"].GetComponent<tk2dSprite>().GetCurrentSpriteDef().material.mainTexture = CustomKnight.WingsMissing ? DefaultWingsTex : WingsTex;
            CustomKnight.GameObjects["Quirrel"].GetComponent<tk2dSprite>().GetCurrentSpriteDef().material.mainTexture = CustomKnight.QuirrelMissing ? DefaultQuirrelTex : QuirrelTex;
            CustomKnight.GameObjects["Webbed"].GetComponent<tk2dSprite>().GetCurrentSpriteDef().material.mainTexture = CustomKnight.WebbedMissing ? DefaultWebbedTex : WebbedTex;
            CustomKnight.GameObjects["Dream"].GetComponent<tk2dSprite>().GetCurrentSpriteDef().material.mainTexture = CustomKnight.DreamMissing ? DefaultDreamTex : DreamTex;
            CustomKnight.GameObjects["DN"].GetComponent<tk2dSprite>().GetCurrentSpriteDef().material.mainTexture = CustomKnight.DNMissing ? DefaultDNTex : DNTex;
            CustomKnight.GameObjects["Hornet"].GetComponent<tk2dSprite>().GetCurrentSpriteDef().material.mainTexture = CustomKnight.HornetMissing ? DefaultHornetTex : HornetTex;
            CustomKnight.GameObjects["Birth"].GetComponent<tk2dSprite>().GetCurrentSpriteDef().material.mainTexture = CustomKnight.BirthMissing ? DefaultBirthTex : BirthTex;

            _baldur.GetComponent<tk2dSprite>().GetCurrentSpriteDef().material.mainTexture = CustomKnight.BaldurMissing ? DefaultBaldurTex : BaldurTex;
            _fluke.GetComponent<tk2dSprite>().GetCurrentSpriteDef().material.mainTexture = CustomKnight.FlukeMissing ? DefaultFlukeTex : FlukeTex;
            _grimm.GetComponent<tk2dSprite>().GetCurrentSpriteDef().material.mainTexture = CustomKnight.GrimmMissing ? DefaultGrimmTex : GrimmTex;
            _shield.GetComponent<tk2dSprite>().GetCurrentSpriteDef().material.mainTexture = CustomKnight.ShieldMissing ? DefaultShieldTex : ShieldTex;
            _weaver.GetComponent<tk2dSprite>().GetCurrentSpriteDef().material.mainTexture = CustomKnight.WeaverMissing ? DefaultWeaverTex : WeaverTex;
            _womb.GetComponent<tk2dSprite>().GetCurrentSpriteDef().material.mainTexture = CustomKnight.WombMissing ? DefaultWombTex : WombTex;
            
            foreach (Transform child in HeroController.instance.gameObject.transform)
            {
                if (child.name == "Spells")
                {
                    foreach (Transform spellsChild in child)
                    {
                        if (spellsChild.name == "Scr Heads" || spellsChild.name == "Scr Base")
                        {
                            spellsChild.gameObject.GetComponent<tk2dSprite>().GetCurrentSpriteDef().material.mainTexture = CustomKnight.WraithsMissing ? DefaultWraithsTex : WraithsTex;
                        }
                        else if (spellsChild.name == "Scr Heads 2" || spellsChild.name == "Scr Base 2")
                        {
                            spellsChild.gameObject.GetComponent<tk2dSprite>().GetCurrentSpriteDef().material.mainTexture = CustomKnight.VoidMissing ? DefaultVoidTex : VoidTex;
                        }
                    }
                }
                else if (child.name == "Focus Effects")
                {
                    foreach (Transform focusChild in child)
                    {
                        if (focusChild.name == "Heal Anim")
                        {
                            focusChild.gameObject.GetComponent<tk2dSprite>().GetCurrentSpriteDef().material.mainTexture = CustomKnight.VSMissing ? DefaultVSTex : VSTex;
                            break;
                        }
                    }
                }
            }
            foreach (tk2dSprite i in GameCameras.instance.hudCanvas.GetComponentsInChildren<tk2dSprite>())
            {
                if (i.name == "Health 1")
                {
                    i.GetCurrentSpriteDef().material.mainTexture = CustomKnight.HUDMissing ? DefaultHudTex : HudTex;
                    break;
                }
            }
            foreach (SpriteRenderer i in GameCameras.instance.hudCanvas.GetComponentsInChildren<SpriteRenderer>(true))
            {
                if (i.name == "Orb Full")
                {
                    Texture2D tex = CustomKnight.FullMissing ? DefaultOrbFull : OrbFull;
                    i.sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
                }
                else if (i.name == "Pulse Sprite")
                {
                    if (i.gameObject != null) Destroy(i.gameObject);
                }
            }
            
            texRoutineRunning = false;
        }

        private static void Log(object message) => Modding.Logger.Log("[Sprite Loader] " + message);
    }
}
