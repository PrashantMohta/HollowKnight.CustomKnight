using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;
using HutongGames.PlayMaker.Actions;
using ModCommon;
using ModCommon.Util;
using UnityEngine.Assertions.Must;

namespace CustomKnight
{
    internal class SpriteLoader : MonoBehaviour
    {
        private static GameObject loader;

        private static bool texRoutineRunning;
        private static Coroutine setTexRoutine;

        private static bool pullDefaultTexturesReady;
        private static bool pulledDefaultTexturesAlready;

        public static bool ChangedSkin;
        
        public static bool LoadComplete { get; private set; }

        public static void PullDefaultTextures()
        {
            bool hcNull = HeroController.instance == null;
            List<CustomKnight.CustomKnightTexture> textures = new List<CustomKnight.CustomKnightTexture>(CustomKnight.Textures.Values);
            foreach (CustomKnight.CustomKnightTexture texture in textures)
            {
                if (texture.defaultTex == null && !hcNull)
                {
                    pullDefaultTexturesReady = true;
                    break;
                }
            }
            
            if (pullDefaultTexturesReady)
            {
                pulledDefaultTexturesAlready = true;
                CustomKnight.Textures["Knight"].defaultTex = _knightMat.mainTexture as Texture2D;
                CustomKnight.Textures["Sprint"].defaultTex = _sprintMat.mainTexture as Texture2D;
                CustomKnight.Textures["Unn"].defaultTex = _unnMat.mainTexture as Texture2D;

                if (CustomKnight.settings.Preloads)
                {
                    CustomKnight.Textures["Cloak"].defaultTex = _cloakMat.mainTexture as Texture2D;
                    CustomKnight.Textures["Shriek"].defaultTex = _shriekMat.mainTexture as Texture2D;
                    CustomKnight.Textures["Wings"].defaultTex = _wingsMat.mainTexture as Texture2D;
                    CustomKnight.Textures["Quirrel"].defaultTex = _quirrelMat.mainTexture as Texture2D;
                    CustomKnight.Textures["Webbed"].defaultTex = _webbedMat.mainTexture as Texture2D;
                    CustomKnight.Textures["DreamArrival"].defaultTex = _dreamMat.mainTexture as Texture2D;
                    CustomKnight.Textures["Dreamnail"].defaultTex = _dnMat.mainTexture as Texture2D;
                    CustomKnight.Textures["Hornet"].defaultTex = _hornetMat.mainTexture as Texture2D;
                    CustomKnight.Textures["Birthplace"].defaultTex = _birthMat.mainTexture as Texture2D;
                }

                CustomKnight.Textures["Baldur"].defaultTex = _baldurMat.mainTexture as Texture2D;
                CustomKnight.Textures["Fluke"].defaultTex = _flukeMat.mainTexture as Texture2D;
                CustomKnight.Textures["Grimm"].defaultTex = _grimmMat.mainTexture as Texture2D;
                CustomKnight.Textures["Shield"].defaultTex = _shieldMat.mainTexture as Texture2D;
                CustomKnight.Textures["Weaver"].defaultTex = _weaverMat.mainTexture as Texture2D;
                CustomKnight.Textures["Hatchling"].defaultTex = _wombMat.mainTexture as Texture2D;

                foreach (Transform child in HeroController.instance.gameObject.transform)
                {
                    if (child.name == "Spells")
                    {
                        foreach (Transform spellsChild in child)
                        {
                            if (spellsChild.name == "Scr Heads")
                            {
                                CustomKnight.Textures["Wraiths"].defaultTex = _wraithsMat.mainTexture as Texture2D;
                            }
                            else if (spellsChild.name == "Scr Heads 2")
                            {
                                CustomKnight.Textures["VoidSpells"].defaultTex = _voidMat.mainTexture as Texture2D;
                            }
                        }
                    }
                    else if (child.name == "Focus Effects")
                    {
                        foreach (Transform focusChild in child)
                        {
                            if (focusChild.name == "Heal Anim")
                            {
                                CustomKnight.Textures["VS"].defaultTex = _vsMat.mainTexture as Texture2D;
                                break;
                            }
                        }
                    }
                }
                
                CustomKnight.Textures["Hud"].defaultTex = _hudMat.mainTexture as Texture2D;
                
                foreach (SpriteRenderer i in GameCameras.instance.hudCanvas.GetComponentsInChildren<SpriteRenderer>(true))
                {
                    if (i.name == "Orb Full")
                    {
                        CustomKnight.Textures["OrbFull"].defaultTex = i.sprite.texture;
                    }
                }

                GameObject dreamNail = GameManager.instance.inventoryFSM.gameObject.FindGameObjectInChildren("Dream Nail");
                var sr = dreamNail.GetComponent<SpriteRenderer>();
                CustomKnight.CustomKnightTexture invTexture = CustomKnight.Textures["Inventory"];
                invTexture.defaultTex = sr.sprite.texture;
                
                PlayMakerFSM charmShowIfCollected;
                Sprite brokenSprite;
                
                for (int charmNum = 1; charmNum <= 40; charmNum++)
                {
                    string charmName;
                    CustomKnight.CustomKnightTexture texture;

                    switch (charmNum)
                    {
                        case 23:
                            charmName = "Charm_" + charmNum + "_Fragile";
                            texture = CustomKnight.Textures[charmName];
                            texture.defaultCharmSprite = CharmIconList.Instance.spriteList[charmNum];

                            charmShowIfCollected = GameCameras.instance.hudCamera.gameObject.FindGameObjectInChildren(charmNum.ToString()).LocateMyFSM("charm_show_if_collected");
                            brokenSprite = charmShowIfCollected.GetAction<SetSpriteRendererSprite>("Glass HP", 2).sprite.Value as Sprite;
                            charmName = "Charm_" + charmNum + "_Broken";
                            texture = CustomKnight.Textures[charmName];
                            texture.defaultCharmSprite = brokenSprite;
                                
                            charmName = "Charm_" + charmNum + "_Unbreakable";
                            texture = CustomKnight.Textures[charmName];
                            texture.defaultCharmSprite = CharmIconList.Instance.unbreakableHeart;

                            break;
                        case 24:
                            charmName = "Charm_" + charmNum + "_Fragile";
                            texture = CustomKnight.Textures[charmName];
                            texture.defaultCharmSprite = CharmIconList.Instance.spriteList[charmNum];

                            charmShowIfCollected = GameCameras.instance.hudCamera.gameObject.FindGameObjectInChildren(charmNum.ToString()).LocateMyFSM("charm_show_if_collected");
                            brokenSprite = charmShowIfCollected.GetAction<SetSpriteRendererSprite>("Glass Geo", 2).sprite.Value as Sprite;
                            charmName = "Charm_" + charmNum + "_Broken";
                            texture = CustomKnight.Textures[charmName];
                            texture.defaultCharmSprite = brokenSprite;
                            
                            charmName = "Charm_" + charmNum + "_Unbreakable";
                            texture = CustomKnight.Textures[charmName];
                            texture.defaultCharmSprite = CharmIconList.Instance.unbreakableGreed;

                            break;
                        case 25:
                            charmName = "Charm_" + charmNum + "_Fragile";
                            texture = CustomKnight.Textures[charmName];
                            texture.defaultCharmSprite = CharmIconList.Instance.spriteList[charmNum];

                            charmShowIfCollected = GameCameras.instance.hudCamera.gameObject.FindGameObjectInChildren(charmNum.ToString()).LocateMyFSM("charm_show_if_collected");
                            brokenSprite = charmShowIfCollected.GetAction<SetSpriteRendererSprite>("Glass Attack", 2).sprite.Value as Sprite;
                            charmName = "Charm_" + charmNum + "_Broken";
                            texture = CustomKnight.Textures[charmName];
                            texture.defaultCharmSprite = brokenSprite;
                            
                            charmName = "Charm_" + charmNum + "_Unbreakable";
                            texture = CustomKnight.Textures[charmName];
                            texture.defaultCharmSprite = CharmIconList.Instance.unbreakableStrength;

                            break;
                        case 36:
                            charmShowIfCollected = GameCameras.instance.hudCamera.gameObject.FindGameObjectInChildren(charmNum.ToString()).LocateMyFSM("charm_show_if_collected");

                            CustomKnight.CustomKnightTexture kingsoulLeft = CustomKnight.Textures["Charm_" + charmNum + "_Left"]; 
                            kingsoulLeft.defaultCharmSprite = charmShowIfCollected.GetAction<SetSpriteRendererSprite>("R Queen", 0).sprite.Value as Sprite;

                            CustomKnight.CustomKnightTexture kingsoulRight = CustomKnight.Textures["Charm_" + charmNum + "_Right"]; 
                            kingsoulRight.defaultCharmSprite = charmShowIfCollected.GetAction<SetSpriteRendererSprite>("R King", 0).sprite.Value as Sprite;
                            
                            CustomKnight.CustomKnightTexture kingsoul = CustomKnight.Textures["Charm_" + charmNum + "_Full"]; 
                            kingsoul.defaultCharmSprite = charmShowIfCollected.GetAction<SetSpriteRendererSprite>("R Final", 0).sprite.Value as Sprite;
                            
                            CustomKnight.CustomKnightTexture voidHeart = CustomKnight.Textures["Charm_" + charmNum + "_Black"]; 
                            voidHeart.defaultCharmSprite = charmShowIfCollected.GetAction<SetSpriteRendererSprite>("R Shade", 0).sprite.Value as Sprite;
                            
                            break;
                        case 40:
                            CustomKnight.CustomKnightTexture gcLevel1 = CustomKnight.Textures["Charm_40_1"];
                            gcLevel1.defaultCharmSprite = CharmIconList.Instance.grimmchildLevel1;

                            CustomKnight.CustomKnightTexture gcLevel2 = CustomKnight.Textures["Charm_40_2"];
                            gcLevel2.defaultCharmSprite = CharmIconList.Instance.grimmchildLevel2;

                            CustomKnight.CustomKnightTexture gcLevel3 = CustomKnight.Textures["Charm_40_3"];
                            gcLevel3.defaultCharmSprite = CharmIconList.Instance.grimmchildLevel3;

                            CustomKnight.CustomKnightTexture gcLevel4 = CustomKnight.Textures["Charm_40_4"];
                            gcLevel4.defaultCharmSprite= CharmIconList.Instance.grimmchildLevel4;

                            CustomKnight.CustomKnightTexture melody = CustomKnight.Textures["Charm_40_5"];
                            melody.defaultCharmSprite = CharmIconList.Instance.nymmCharm;

                            break;
                        default:
                            charmName = "Charm_" + charmNum;
                            texture = CustomKnight.Textures[charmName];
                            texture.defaultCharmSprite = CharmIconList.Instance.spriteList[charmNum];

                            break;
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
            
            if (HeroController.instance != null)
            {
                _knightMat.mainTexture = CustomKnight.Textures["Knight"].defaultTex;
                _sprintMat.mainTexture = CustomKnight.Textures["Sprint"].defaultTex;
                _unnMat.mainTexture = CustomKnight.Textures["Unn"].defaultTex;

                if (CustomKnight.settings.Preloads)
                {
                    _cloakMat.mainTexture = CustomKnight.Textures["Cloak"].defaultTex;
                    _shriekMat.mainTexture = CustomKnight.Textures["Shriek"].defaultTex;
                    _wingsMat.mainTexture = CustomKnight.Textures["Wings"].defaultTex;
                    _quirrelMat.mainTexture = CustomKnight.Textures["Quirrel"].defaultTex;
                    _webbedMat.mainTexture = CustomKnight.Textures["Webbed"].defaultTex;
                    _dreamMat.mainTexture = CustomKnight.Textures["DreamArrival"].defaultTex;
                    _dnMat.mainTexture = CustomKnight.Textures["Dreamnail"].defaultTex;
                    _hornetMat.mainTexture = CustomKnight.Textures["Hornet"].defaultTex;
                    _birthMat.mainTexture = CustomKnight.Textures["Birthplace"].defaultTex;
                }
                
                _baldurMat.mainTexture = CustomKnight.Textures["Baldur"].defaultTex;
                _flukeMat.mainTexture = CustomKnight.Textures["Fluke"].defaultTex;
                _grimmMat.mainTexture = CustomKnight.Textures["Grimm"].defaultTex;
                _shieldMat.mainTexture = CustomKnight.Textures["Shield"].defaultTex;
                _weaverMat.mainTexture = CustomKnight.Textures["Weaver"].defaultTex;
                _wombMat.mainTexture = CustomKnight.Textures["Hatchling"].defaultTex;
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

            ChangedSkin = true;
        }

        private static Material _knightMat;
        private static Material _sprintMat;
        private static Material _unnMat;

        private static Material _cloakMat;
        private static Material _shriekMat;
        private static Material _wingsMat;
        private static Material _quirrelMat;
        private static Material _webbedMat;
        private static Material _dreamMat;
        private static Material _dnMat;
        private static Material _hornetMat;
        private static Material _birthMat;
        
        private static Material _baldurMat;
        private static Material _flukeMat;
        private static Material _grimmMat;
        private static Material _shieldMat;
        private static Material _weaverMat;
        private static Material _wombMat;

        private static Material _wraithsMat;
        private static Material _voidMat;
        private static Material _vsMat;
        private static Material _hudMat;
        public IEnumerator Start()
        {
            LoadComplete = false;
            
            yield return new WaitUntil(() => HeroController.instance != null);
            
            GameObject hc = HeroController.instance.gameObject;
            
            tk2dSpriteAnimator anim = hc.GetComponent<tk2dSpriteAnimator>();
            _knightMat = anim.GetClipByName("Idle").frames[0].spriteCollection.spriteDefinitions[0].material;
            _sprintMat = anim.GetClipByName("Sprint").frames[0].spriteCollection.spriteDefinitions[0].material;
            _unnMat = anim.GetClipByName("Slug Up").frames[0].spriteCollection.spriteDefinitions[0].material;

            if (CustomKnight.settings.Preloads)
            {
                _cloakMat = CustomKnight.GameObjects["Cloak"].GetComponent<tk2dSprite>().GetCurrentSpriteDef().material;
                _shriekMat = CustomKnight.GameObjects["Shriek"].GetComponent<tk2dSprite>().GetCurrentSpriteDef().material;
                _wingsMat = CustomKnight.GameObjects["Wings"].GetComponent<tk2dSprite>().GetCurrentSpriteDef().material;
                _quirrelMat = CustomKnight.GameObjects["Quirrel"].GetComponent<tk2dSprite>().GetCurrentSpriteDef().material;
                _webbedMat = CustomKnight.GameObjects["Webbed"].GetComponent<tk2dSprite>().GetCurrentSpriteDef().material;
                _dreamMat = CustomKnight.GameObjects["DreamArrival"].GetComponent<tk2dSprite>().GetCurrentSpriteDef().material;
                _dnMat = CustomKnight.GameObjects["Dreamnail"].GetComponent<tk2dSprite>().GetCurrentSpriteDef().material;
                _hornetMat = CustomKnight.GameObjects["Hornet"].GetComponent<tk2dSprite>().GetCurrentSpriteDef().material;
                _birthMat = CustomKnight.GameObjects["Birthplace"].GetComponent<tk2dSprite>().GetCurrentSpriteDef().material;
            }
            
            GameObject charmEffects = hc.FindGameObjectInChildren("Charm Effects");

            GameObject baldur = charmEffects.FindGameObjectInChildren("Blocker Shield").FindGameObjectInChildren("Shell Anim");
            _baldurMat = baldur.GetComponent<tk2dSprite>().GetCurrentSpriteDef().material;
            
            PlayMakerFSM poolFlukes = charmEffects.LocateMyFSM("Pool Flukes");
            GameObject fluke = poolFlukes.GetAction<CreateGameObjectPool>("Pool Normal", 0).prefab.Value;
            _flukeMat = fluke.GetComponent<tk2dSprite>().GetCurrentSpriteDef().material;

            PlayMakerFSM spawnGrimmchild = charmEffects.LocateMyFSM("Spawn Grimmchild");
            GameObject grimm = spawnGrimmchild.GetAction<SpawnObjectFromGlobalPool>("Spawn", 2).gameObject.Value;
            _grimmMat = grimm.GetComponent<tk2dSprite>().GetCurrentSpriteDef().material;

            PlayMakerFSM spawnOrbitShield = charmEffects.LocateMyFSM("Spawn Orbit Shield");
            GameObject orbitShield = spawnOrbitShield.GetAction<SpawnObjectFromGlobalPool>("Spawn", 2).gameObject.Value;
            GameObject shield = orbitShield.FindGameObjectInChildren("Shield");
            _shieldMat = shield.GetComponent<tk2dSprite>().GetCurrentSpriteDef().material;

            PlayMakerFSM weaverlingControl = charmEffects.LocateMyFSM("Weaverling Control");
            GameObject weaver = weaverlingControl.GetAction<SpawnObjectFromGlobalPool>("Spawn", 0).gameObject.Value;
            _weaverMat = weaver.GetComponent<tk2dSprite>().GetCurrentSpriteDef().material;

            PlayMakerFSM hatchlingSpawn = charmEffects.LocateMyFSM("Hatchling Spawn");
            GameObject hatchling = hatchlingSpawn.GetAction<SpawnObjectFromGlobalPool>("Hatch", 2).gameObject.Value;
            _wombMat = hatchling.GetComponent<tk2dSprite>().GetCurrentSpriteDef().material;

            foreach (Transform child in hc.transform)
            {
                if (child.name == "Spells")
                {
                    foreach (Transform spellsChild in child)
                    {
                        if (spellsChild.name == "Scr Heads" || spellsChild.name == "Scr Base")
                        {
                            _wraithsMat = spellsChild.gameObject.GetComponent<tk2dSprite>().GetCurrentSpriteDef().material;
                        }
                        else if (spellsChild.name == "Scr Heads 2" || spellsChild.name == "Scr Base 2")
                        {
                            _voidMat = spellsChild.gameObject.GetComponent<tk2dSprite>().GetCurrentSpriteDef().material;
                        }
                    }
                }
                else if (child.name == "Focus Effects")
                {
                    foreach (Transform focusChild in child)
                    {
                        if (focusChild.name == "Heal Anim")
                        {
                            _vsMat = focusChild.gameObject.GetComponent<tk2dSprite>().GetCurrentSpriteDef().material;
                            break;
                        }
                    }
                }
            }
            
            foreach (tk2dSprite i in GameCameras.instance.hudCanvas.GetComponentsInChildren<tk2dSprite>())
            {
                if (i.name == "Health 1")
                {
                    _hudMat = i.GetCurrentSpriteDef().material;
                    break;
                }
            }
            
            LoadSprites();
        }

        private static void DestroyObjects()
        {
            foreach (KeyValuePair<string, CustomKnight.CustomKnightTexture> pair in CustomKnight.Textures)
            {
                CustomKnight.CustomKnightTexture texture = pair.Value;
                if (texture.tex != null)
                {
                    Destroy(texture.tex);
                }
            }
            
            LoadComplete = false;
        }

        private void LoadSprites()
        {
            LoadComplete = false;
            
            foreach (KeyValuePair<string, CustomKnight.CustomKnightTexture> pair in CustomKnight.Textures)
            {
                CustomKnight.CustomKnightTexture texture = pair.Value;
                if (!texture.missing)
                {
                    byte[] texBytes = File.ReadAllBytes((CustomKnight.DATA_DIR + "/" + CustomKnight.SKIN_FOLDER + "/" + texture.fileName).Replace("\\", "/"));
                    if (texture.tex != null)
                    {
                        Destroy(texture.tex);
                    }

                    texture.tex = new Texture2D(2, 2);
                    texture.tex.LoadImage(texBytes, true);
                }
            }

            ModifyHeroTextures();

            LoadComplete = true;
            Destroy(gameObject);
        }

        private static IEnumerator SetHeroTex()
        {
            yield return new WaitWhile(() =>
                !LoadComplete || HeroController.instance == null || CharmIconList.Instance == null);

            if (!pulledDefaultTexturesAlready)
            {
                Log("Pulling Default Textures");
                PullDefaultTextures();
            }

            _knightMat.mainTexture = CustomKnight.Textures["Knight"].missing
                ? CustomKnight.Textures["Knight"].defaultTex
                : CustomKnight.Textures["Knight"].tex;
            _sprintMat.mainTexture = CustomKnight.Textures["Sprint"].missing
                ? CustomKnight.Textures["Sprint"].defaultTex
                : CustomKnight.Textures["Sprint"].tex;
            _unnMat.mainTexture = CustomKnight.Textures["Unn"].missing
                ? CustomKnight.Textures["Unn"].defaultTex
                : CustomKnight.Textures["Unn"].tex;

            if (CustomKnight.settings.Preloads)
            {
                _cloakMat.mainTexture = CustomKnight.Textures["Cloak"].missing
                    ? CustomKnight.Textures["Cloak"].defaultTex
                    : CustomKnight.Textures["Cloak"].tex;
                _shriekMat.mainTexture = CustomKnight.Textures["Shriek"].missing
                    ? CustomKnight.Textures["Shriek"].defaultTex
                    : CustomKnight.Textures["Shriek"].tex;
                _wingsMat.mainTexture = CustomKnight.Textures["Wings"].missing
                    ? CustomKnight.Textures["Wings"].defaultTex
                    : CustomKnight.Textures["Wings"].tex;
                _quirrelMat.mainTexture = CustomKnight.Textures["Quirrel"].missing
                    ? CustomKnight.Textures["Quirrel"].defaultTex
                    : CustomKnight.Textures["Quirrel"].tex;
                _webbedMat.mainTexture = CustomKnight.Textures["Webbed"].missing
                    ? CustomKnight.Textures["Webbed"].defaultTex
                    : CustomKnight.Textures["Webbed"].tex;
                _dreamMat.mainTexture = CustomKnight.Textures["DreamArrival"].missing
                    ? CustomKnight.Textures["DreamArrival"].defaultTex
                    : CustomKnight.Textures["DreamArrival"].tex;
                _dnMat.mainTexture = CustomKnight.Textures["Dreamnail"].missing
                    ? CustomKnight.Textures["Dreamnail"].defaultTex
                    : CustomKnight.Textures["Dreamnail"].tex;
                _hornetMat.mainTexture = CustomKnight.Textures["Hornet"].missing
                    ? CustomKnight.Textures["Hornet"].defaultTex
                    : CustomKnight.Textures["Hornet"].tex;
                _birthMat.mainTexture = CustomKnight.Textures["Birthplace"].missing
                    ? CustomKnight.Textures["Birthplace"].defaultTex
                    : CustomKnight.Textures["Birthplace"].tex;
            }

            _baldurMat.mainTexture = CustomKnight.Textures["Baldur"].missing
                ? CustomKnight.Textures["Baldur"].defaultTex
                : CustomKnight.Textures["Baldur"].tex;
            _flukeMat.mainTexture = CustomKnight.Textures["Fluke"].missing
                ? CustomKnight.Textures["Fluke"].defaultTex
                : CustomKnight.Textures["Fluke"].tex;
            _grimmMat.mainTexture = CustomKnight.Textures["Grimm"].missing
                ? CustomKnight.Textures["Grimm"].defaultTex
                : CustomKnight.Textures["Grimm"].tex;
            _shieldMat.mainTexture = CustomKnight.Textures["Shield"].missing
                ? CustomKnight.Textures["Shield"].defaultTex
                : CustomKnight.Textures["Shield"].tex;
            _weaverMat.mainTexture = CustomKnight.Textures["Weaver"].missing
                ? CustomKnight.Textures["Weaver"].defaultTex
                : CustomKnight.Textures["Weaver"].tex;
            _wombMat.mainTexture = CustomKnight.Textures["Hatchling"].missing
                ? CustomKnight.Textures["Hatchling"].defaultTex
                : CustomKnight.Textures["Hatchling"].tex;

            foreach (Transform child in HeroController.instance.gameObject.transform)
            {
                if (child.name == "Spells")
                {
                    foreach (Transform spellsChild in child)
                    {
                        if (spellsChild.name == "Scr Heads" || spellsChild.name == "Scr Base")
                        {
                            _wraithsMat.mainTexture = CustomKnight.Textures["Wraiths"].missing
                                ? CustomKnight.Textures["Wraiths"].defaultTex
                                : CustomKnight.Textures["Wraiths"].tex;
                        }
                        else if (spellsChild.name == "Scr Heads 2" || spellsChild.name == "Scr Base 2")
                        {
                            _voidMat.mainTexture = CustomKnight.Textures["VoidSpells"].missing
                                ? CustomKnight.Textures["VoidSpells"].defaultTex
                                : CustomKnight.Textures["VoidSpells"].tex;
                        }
                    }
                }
                else if (child.name == "Focus Effects")
                {
                    foreach (Transform focusChild in child)
                    {
                        if (focusChild.name == "Heal Anim")
                        {
                            _vsMat.mainTexture = CustomKnight.Textures["VS"].missing
                                ? CustomKnight.Textures["VS"].defaultTex
                                : CustomKnight.Textures["VS"].tex;
                            break;
                        }
                    }
                }
            }

            _hudMat.mainTexture = CustomKnight.Textures["Hud"].missing
                ? CustomKnight.Textures["Hud"].defaultTex
                : CustomKnight.Textures["Hud"].tex;

            foreach (SpriteRenderer i in GameCameras.instance.hudCanvas.GetComponentsInChildren<SpriteRenderer>(true))
            {
                if (i.name == "Orb Full")
                {
                    Texture2D tex = CustomKnight.Textures["OrbFull"].missing
                        ? CustomKnight.Textures["OrbFull"].defaultTex
                        : CustomKnight.Textures["OrbFull"].tex;
                    i.sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
                }
                else if (i.name == "Pulse Sprite")
                {
                    if (i.gameObject != null)
                    {
                        Destroy(i.gameObject);
                    }
                }
            }

            Log("Getting DN");
            GameObject dreamNail = GameManager.instance.inventoryFSM.gameObject.FindGameObjectInChildren("Dream Nail");
            Log("Getting SR");
            var sr = dreamNail.GetComponent<SpriteRenderer>();
            Log("Getting Inventory");
            CustomKnight.CustomKnightTexture invTexture = CustomKnight.Textures["Inventory"];
            Log("Getting InvTex: " + (invTexture == null));
            Texture2D invTex = invTexture.missing ? invTexture.defaultTex : invTexture.tex;
            Log("Getting Texturek_BackingField: " + (invTex == null));
            FieldInfo texInfo = typeof(Sprite).GetField("<texture>k__BackingField",
                BindingFlags.Instance | BindingFlags.NonPublic);
            
            ReflectionHelper.

        Log("Setting InvTex: " + (texInfo == null));
            texInfo.SetValue(sr.sprite, invTex);
            Log("Finale");

            string charmName;
            CustomKnight.CustomKnightTexture texture;

            PlayMakerFSM charmShowIfCollected;

            for (int charmNum = 1; charmNum <= 40; charmNum++)
            {
                switch (charmNum)
                {
                    case 23:
                        charmName = "Charm_" + charmNum + "_Fragile";
                        texture = CustomKnight.Textures[charmName];
                        Texture2D heartTex = texture.tex;
                        CharmIconList.Instance.spriteList[charmNum] = texture.missing ? texture.defaultCharmSprite : Sprite.Create(heartTex, new Rect(0, 0, heartTex.width, heartTex.height), new Vector2(0.5f, 0.5f));

                        charmShowIfCollected = GameCameras.instance.hudCamera.gameObject.FindGameObjectInChildren(charmNum.ToString()).LocateMyFSM("charm_show_if_collected");
                        charmName = "Charm_" + charmNum + "_Broken";
                        texture = CustomKnight.Textures[charmName];
                        if (texture.missing)
                        {
                            charmShowIfCollected.GetAction<SetSpriteRendererSprite>("Glass HP", 2).sprite.Value = texture.defaultCharmSprite;    
                        }
                        else
                        {
                            Texture2D tex = texture.tex;
                            Sprite sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
                            charmShowIfCollected.GetAction<SetSpriteRendererSprite>("Glass HP", 2).sprite.Value = sprite;
                        }

                        charmName = "Charm_" + charmNum + "_Unbreakable";
                        texture = CustomKnight.Textures[charmName];
                        Texture2D ubhTex = texture.tex;
                        CharmIconList.Instance.unbreakableHeart = texture.missing ? texture.defaultCharmSprite : Sprite.Create(ubhTex, new Rect(0, 0, ubhTex.width, ubhTex.height), new Vector2(0.5f, 0.5f));

                        break;
                    case 24:
                        charmName = "Charm_" + charmNum + "_Fragile";
                        texture = CustomKnight.Textures[charmName];
                        Texture2D greedTex = texture.tex;
                        CharmIconList.Instance.spriteList[charmNum] = texture.missing ? texture.defaultCharmSprite : Sprite.Create(greedTex, new Rect(0, 0, greedTex.width, greedTex.height), new Vector2(0.5f, 0.5f));
                        
                        charmShowIfCollected = GameCameras.instance.hudCamera.gameObject.FindGameObjectInChildren(charmNum.ToString()).LocateMyFSM("charm_show_if_collected");
                        charmName = "Charm_" + charmNum + "_Broken";
                        texture = CustomKnight.Textures[charmName];
                        if (texture.missing)
                        {
                            charmShowIfCollected.GetAction<SetSpriteRendererSprite>("Glass Geo", 2).sprite.Value = texture.defaultCharmSprite;    
                        }
                        else
                        {
                            Texture2D tex = texture.tex;
                            Sprite sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
                            charmShowIfCollected.GetAction<SetSpriteRendererSprite>("Glass Geo", 2).sprite.Value = sprite;
                        }
                        
                        charmName = "Charm_" + charmNum + "_Unbreakable";
                        texture = CustomKnight.Textures[charmName];
                        Texture2D ubgTex = texture.tex;
                        CharmIconList.Instance.unbreakableGreed = texture.missing ? texture.defaultCharmSprite : Sprite.Create(ubgTex, new Rect(0, 0, ubgTex.width, ubgTex.height), new Vector2(0.5f, 0.5f));
                        
                        break;
                    case 25:
                        charmName = "Charm_" + charmNum + "_Fragile";
                        texture = CustomKnight.Textures[charmName];
                        Texture2D strTex = texture.tex;
                        CharmIconList.Instance.spriteList[charmNum] = texture.missing ? texture.defaultCharmSprite : Sprite.Create(strTex, new Rect(0, 0, strTex.width, strTex.height), new Vector2(0.5f, 0.5f));
                        
                        charmShowIfCollected = GameCameras.instance.hudCamera.gameObject.FindGameObjectInChildren(charmNum.ToString()).LocateMyFSM("charm_show_if_collected");
                        charmName = "Charm_" + charmNum + "_Broken";
                        texture = CustomKnight.Textures[charmName];
                        if (texture.missing)
                        {
                            charmShowIfCollected.GetAction<SetSpriteRendererSprite>("Glass Attack", 2).sprite.Value = texture.defaultCharmSprite;    
                        }
                        else
                        {
                            Texture2D tex = texture.tex;
                            Sprite sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
                            charmShowIfCollected.GetAction<SetSpriteRendererSprite>("Glass Attack", 2).sprite.Value = sprite;
                        }
                        
                        charmName = "Charm_" + charmNum + "_Unbreakable";
                        texture = CustomKnight.Textures[charmName];
                        Texture2D ubsTex = texture.tex;
                        CharmIconList.Instance.unbreakableStrength = texture.missing ? texture.defaultCharmSprite : Sprite.Create(ubsTex, new Rect(0, 0, ubsTex.width, ubsTex.height), new Vector2(0.5f, 0.5f));

                        break;
                    case 36:
                        charmShowIfCollected = GameCameras.instance.hudCamera.gameObject.FindGameObjectInChildren(charmNum.ToString()).LocateMyFSM("charm_show_if_collected");

                        CustomKnight.CustomKnightTexture kingsoulLeft = CustomKnight.Textures["Charm_" + charmNum + "_Left"];
                        if (kingsoulLeft.missing)
                        {
                            charmShowIfCollected.GetAction<SetSpriteRendererSprite>("R Queen", 0).sprite.Value = kingsoulLeft.defaultCharmSprite;
                        }
                        else
                        {
                            Texture2D tex = kingsoulLeft.tex;
                            charmShowIfCollected.GetAction<SetSpriteRendererSprite>("R Queen", 0).sprite.Value = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
                        }

                        CustomKnight.CustomKnightTexture kingsoulRight = CustomKnight.Textures["Charm_" + charmNum + "_Right"];
                        if (kingsoulRight.missing)
                        {
                            charmShowIfCollected.GetAction<SetSpriteRendererSprite>("R King", 0).sprite.Value = kingsoulRight.defaultCharmSprite;
                        }
                        else
                        {
                            Texture2D tex = kingsoulRight.tex;
                            charmShowIfCollected.GetAction<SetSpriteRendererSprite>("R King", 0).sprite.Value = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
                        }
                            
                        CustomKnight.CustomKnightTexture kingsoul= CustomKnight.Textures["Charm_" + charmNum + "_Full"];
                        if (kingsoul.missing)
                        {
                            charmShowIfCollected.GetAction<SetSpriteRendererSprite>("R Final", 0).sprite.Value = kingsoul.defaultCharmSprite;
                        }
                        else
                        {
                            Texture2D tex = kingsoul.tex;
                            charmShowIfCollected.GetAction<SetSpriteRendererSprite>("R Final", 0).sprite.Value = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
                        }
                            
                        CustomKnight.CustomKnightTexture voidHeart = CustomKnight.Textures["Charm_" + charmNum + "_Black"];
                        if (voidHeart.missing)
                        {
                            charmShowIfCollected.GetAction<SetSpriteRendererSprite>("R Shade", 0).sprite.Value = voidHeart.defaultCharmSprite;
                        }
                        else
                        {
                            Texture2D tex = voidHeart.tex;
                            charmShowIfCollected.GetAction<SetSpriteRendererSprite>("R Shade", 0).sprite.Value = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
                        }
                        
                        break;
                    case 40:
                        texture = CustomKnight.Textures["Charm_" + charmNum + "_1"];
                        if (texture.missing)
                        {
                            CharmIconList.Instance.grimmchildLevel1 = texture.defaultCharmSprite;
                        }
                        else
                        {
                            Texture2D tex = texture.tex;
                            CharmIconList.Instance.grimmchildLevel1 = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
                        }
                        
                        texture = CustomKnight.Textures["Charm_" + charmNum + "_2"];
                        if (texture.missing)
                        {
                            CharmIconList.Instance.grimmchildLevel2 = texture.defaultCharmSprite;
                        }
                        else
                        {
                            Texture2D tex = texture.tex;
                            CharmIconList.Instance.grimmchildLevel2 = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
                        }
                        
                        texture = CustomKnight.Textures["Charm_" + charmNum + "_3"];
                        if (texture.missing)
                        {
                            CharmIconList.Instance.grimmchildLevel3 = texture.defaultCharmSprite;
                        }
                        else
                        {
                            Texture2D tex = texture.tex;
                            CharmIconList.Instance.grimmchildLevel3 = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
                        }
                        
                        texture = CustomKnight.Textures["Charm_" + charmNum + "_4"];
                        if (texture.missing)
                        {
                            CharmIconList.Instance.grimmchildLevel4 = texture.defaultCharmSprite;
                        }
                        else
                        {
                            Texture2D tex = texture.tex;
                            CharmIconList.Instance.grimmchildLevel4 = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
                        }

                        texture = CustomKnight.Textures["Charm_" + charmNum + "_5"];
                        if (texture.missing)
                        {
                            CharmIconList.Instance.nymmCharm = texture.defaultCharmSprite;
                        }
                        else
                        {
                            Texture2D tex = texture.tex;
                            CharmIconList.Instance.nymmCharm = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
                        }

                        break;
                    default:
                        charmName = "Charm_" + charmNum;
                        texture = CustomKnight.Textures[charmName];
                        Texture2D charmTex = texture.tex;
                        CharmIconList.Instance.spriteList[charmNum] = texture.missing ? texture.defaultCharmSprite : Sprite.Create(charmTex, new Rect(0, 0, charmTex.width, charmTex.height), new Vector2(0.5f, 0.5f));
                        
                        break;
                }
            }
            
            texRoutineRunning = false;
        }

        private static void Log(object message) => Modding.Logger.Log("[Sprite Loader] " + message);
    }
}
