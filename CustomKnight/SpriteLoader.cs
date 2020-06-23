using System;
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
                
                CustomKnight.Textures["Inventory"].defaultTex = CharmIconList.Instance.nymmCharm.texture;
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

            CustomKnight.CustomKnightTexture invTexture = CustomKnight.Textures["Inventory"];
            Texture2D invTex = invTexture.missing ? invTexture.defaultTex : invTexture.tex;
            PlayMakerFSM updateSprite = GameManager.instance.inventoryFSM.gameObject.FindGameObjectInChildren("Detail Sprite").LocateMyFSM("Update Sprite");

            for (int charmNum = 1; charmNum <= 40; charmNum++)
            {
                IEnumerable<Vector2> charmCoords;
                float charmX, charmY;
                Sprite charmSprite;
                Sprite newCharmSprite;
                
                float offsetX = 0.0f;
                float offsetY = 0.0f;
                float shrinkX = 2.0f;
                float shrinkY = 2.0f;

                PlayMakerFSM charmShowIfCollected;

                switch (charmNum)
                {
                    case 23:
                        // Fixed Fragile Heart
                        offsetX = 0;
                        offsetY = 0;
                        shrinkX = 0;
                        shrinkY = 0;
                        charmSprite = CharmIconList.Instance.spriteList[charmNum];
                        charmCoords = charmSprite.uv.Select(uv => ConvertUVToPixelCoordinates(uv, charmSprite.texture.width, charmSprite.texture.height));
                        charmX = charmCoords.Min(uv => uv.x);
                        charmY = charmCoords.Min(uv => uv.y);
                        newCharmSprite = Sprite.Create(invTex, new Rect(charmX + offsetX, charmY + offsetY, charmSprite.rect.width - shrinkX, charmSprite.rect.height - shrinkY), new Vector2(0.5f, 0.5f));
                        CharmIconList.Instance.spriteList[charmNum] = newCharmSprite;

                        // Broken Fragile Heart
                        offsetX = 0;
                        offsetY = 0;
                        shrinkX = 0;
                        shrinkY = 0;
                        charmShowIfCollected = GameCameras.instance.hudCamera.gameObject.FindGameObjectInChildren(charmNum.ToString()).LocateMyFSM("charm_show_if_collected");
                        charmSprite = charmShowIfCollected.GetAction<SetSpriteRendererSprite>("Glass HP", 2).sprite.Value as Sprite;
                        charmCoords = charmSprite.uv.Select(uv => ConvertUVToPixelCoordinates(uv, charmSprite.texture.width, charmSprite.texture.height));
                        charmX = charmCoords.Min(uv => uv.x);
                        charmY = charmCoords.Min(uv => uv.y);
                        newCharmSprite = Sprite.Create(invTex, new Rect(charmX + offsetX, charmY + offsetY, charmSprite.rect.width - shrinkX, charmSprite.rect.height - shrinkY), new Vector2(0.5f, 0.5f));
                        charmShowIfCollected.GetAction<SetSpriteRendererSprite>("Glass HP", 2).sprite.Value = newCharmSprite;
                        updateSprite.GetAction<SetSpriteRendererSprite>("Glass HP", 2).sprite.Value = newCharmSprite;

                        // Unbreakable Heart
                        offsetX = 0;
                        offsetY = 0;
                        shrinkX = 0;
                        shrinkY = 0;
                        charmSprite = CharmIconList.Instance.unbreakableHeart;
                        charmCoords = charmSprite.uv.Select(uv => ConvertUVToPixelCoordinates(uv, charmSprite.texture.width, charmSprite.texture.height));
                        charmX = charmCoords.Min(uv => uv.x);
                        charmY = charmCoords.Min(uv => uv.y);
                        newCharmSprite = Sprite.Create(invTex, new Rect(charmX + offsetX, charmY + offsetY, charmSprite.rect.width - shrinkX, charmSprite.rect.height - shrinkY), new Vector2(0.5f, 0.5f));
                        CharmIconList.Instance.unbreakableHeart = newCharmSprite;

                        break;
                    case 24:
                        // Fixed Fragile Greed
                        offsetX = 0;
                        offsetY = 0;
                        shrinkX = 0;
                        shrinkY = 0;
                        charmSprite = CharmIconList.Instance.spriteList[charmNum];
                        charmCoords = charmSprite.uv.Select(uv => ConvertUVToPixelCoordinates(uv, charmSprite.texture.width, charmSprite.texture.height));
                        charmX = charmCoords.Min(uv => uv.x);
                        charmY = charmCoords.Min(uv => uv.y);
                        newCharmSprite = Sprite.Create(invTex, new Rect(charmX + offsetX, charmY + offsetY, charmSprite.rect.width - shrinkX, charmSprite.rect.height - shrinkY), new Vector2(0.5f, 0.5f));
                        CharmIconList.Instance.spriteList[charmNum] = newCharmSprite;
                        
                        // Broken Fragile Greed
                        offsetX = 0;
                        offsetY = 0;
                        shrinkX = 0;
                        shrinkY = 0;
                        charmShowIfCollected = GameCameras.instance.hudCamera.gameObject.FindGameObjectInChildren(charmNum.ToString()).LocateMyFSM("charm_show_if_collected");
                        charmSprite = charmShowIfCollected.GetAction<SetSpriteRendererSprite>("Glass Geo", 2).sprite.Value as Sprite;
                        charmCoords = charmSprite.uv.Select(uv => ConvertUVToPixelCoordinates(uv, charmSprite.texture.width, charmSprite.texture.height));
                        charmX = charmCoords.Min(uv => uv.x);
                        charmY = charmCoords.Min(uv => uv.y);
                        newCharmSprite = Sprite.Create(invTex, new Rect(charmX + offsetX, charmY + offsetY, charmSprite.rect.width - shrinkX, charmSprite.rect.height - shrinkY), new Vector2(0.5f, 0.5f));
                        charmShowIfCollected.GetAction<SetSpriteRendererSprite>("Glass Geo", 2).sprite.Value = newCharmSprite;
                        updateSprite.GetAction<SetSpriteRendererSprite>("Glass Geo", 2).sprite.Value = newCharmSprite;

                        // Unbreakable Greed
                        offsetX = 0;
                        offsetY = 0;
                        shrinkX = 0;
                        shrinkY = 0;
                        charmSprite = CharmIconList.Instance.unbreakableGreed;
                        charmCoords = charmSprite.uv.Select(uv => ConvertUVToPixelCoordinates(uv, charmSprite.texture.width, charmSprite.texture.height));
                        charmX = charmCoords.Min(uv => uv.x);
                        charmY = charmCoords.Min(uv => uv.y);
                        newCharmSprite = Sprite.Create(invTex, new Rect(charmX + offsetX, charmY + offsetY, charmSprite.rect.width - shrinkX, charmSprite.rect.height - shrinkY), new Vector2(0.5f, 0.5f));
                        CharmIconList.Instance.unbreakableGreed = newCharmSprite;
                        
                        break;
                    case 25:
                        // Fixed Fragile Strength
                        offsetX = 0;
                        offsetY = 0;
                        shrinkX = 0;
                        shrinkY = 0;
                        charmSprite = CharmIconList.Instance.spriteList[charmNum];
                        charmCoords = charmSprite.uv.Select(uv => ConvertUVToPixelCoordinates(uv, charmSprite.texture.width, charmSprite.texture.height));
                        charmX = charmCoords.Min(uv => uv.x);
                        charmY = charmCoords.Min(uv => uv.y);
                        newCharmSprite = Sprite.Create(invTex, new Rect(charmX + offsetX, charmY + offsetY, charmSprite.rect.width - shrinkX, charmSprite.rect.height - shrinkY), new Vector2(0.5f, 0.5f));
                        CharmIconList.Instance.spriteList[charmNum] = newCharmSprite;
                        
                        // Broken Fragile Strength
                        offsetX = 0;
                        offsetY = 0;
                        shrinkX = 0;
                        shrinkY = 0;
                        charmShowIfCollected = GameCameras.instance.hudCamera.gameObject.FindGameObjectInChildren(charmNum.ToString()).LocateMyFSM("charm_show_if_collected");
                        charmSprite = charmShowIfCollected.GetAction<SetSpriteRendererSprite>("Glass Attack", 2).sprite.Value as Sprite;
                        charmCoords = charmSprite.uv.Select(uv => ConvertUVToPixelCoordinates(uv, charmSprite.texture.width, charmSprite.texture.height));
                        charmX = charmCoords.Min(uv => uv.x);
                        charmY = charmCoords.Min(uv => uv.y);
                        newCharmSprite = Sprite.Create(invTex, new Rect(charmX + offsetX, charmY + offsetY, charmSprite.rect.width - shrinkX, charmSprite.rect.height - shrinkY), new Vector2(0.5f, 0.5f));
                        charmShowIfCollected.GetAction<SetSpriteRendererSprite>("Glass Attack", 2).sprite.Value = newCharmSprite;
                        updateSprite.GetAction<SetSpriteRendererSprite>("Glass Attack", 2).sprite.Value = newCharmSprite;

                        // Unbreakable Strength
                        offsetX = 0;
                        offsetY = 0;
                        shrinkX = 0;
                        shrinkY = 0;
                        charmSprite = CharmIconList.Instance.unbreakableStrength;
                        charmCoords = charmSprite.uv.Select(uv => ConvertUVToPixelCoordinates(uv, charmSprite.texture.width, charmSprite.texture.height));
                        charmX = charmCoords.Min(uv => uv.x);
                        charmY = charmCoords.Min(uv => uv.y);
                        newCharmSprite = Sprite.Create(invTex, new Rect(charmX + offsetX, charmY + offsetY, charmSprite.rect.width - shrinkX, charmSprite.rect.height - shrinkY), new Vector2(0.5f, 0.5f));
                        CharmIconList.Instance.unbreakableStrength = newCharmSprite;

                        break;
                    case 36:
                        charmShowIfCollected = GameCameras.instance.hudCamera.gameObject.FindGameObjectInChildren(charmNum.ToString()).LocateMyFSM("charm_show_if_collected");
                        updateSprite = GameManager.instance.inventoryFSM.gameObject.FindGameObjectInChildren("Detail Sprite").LocateMyFSM("Update Sprite");

                        offsetX = 7;
                        offsetY = 0;
                        shrinkX = 54;
                        shrinkY = 0;
                        charmSprite = charmShowIfCollected.GetAction<SetSpriteRendererSprite>("R Queen", 0).sprite.Value as Sprite;
                        charmCoords = charmSprite.uv.Select(uv => ConvertUVToPixelCoordinates(uv, charmSprite.texture.width, charmSprite.texture.height));
                        charmX = charmCoords.Min(uv => uv.x);
                        charmY = charmCoords.Min(uv => uv.y);
                        newCharmSprite = Sprite.Create(invTex, new Rect(charmX + offsetX, charmY + offsetY, charmSprite.rect.width - shrinkX, charmSprite.rect.height - shrinkY), new Vector2(0.5f, 0.5f));
                        charmShowIfCollected.GetAction<SetSpriteRendererSprite>("R Queen", 0).sprite.Value = newCharmSprite;
                        updateSprite.GetAction<SetSpriteRendererSprite>("R Queen", 0).sprite.Value = newCharmSprite;
                        
                        offsetX = 0;
                        offsetY = 0;
                        shrinkX = 0;
                        shrinkY = 0;
                        charmShowIfCollected = GameCameras.instance.hudCamera.gameObject.FindGameObjectInChildren(charmNum.ToString()).LocateMyFSM("charm_show_if_collected");
                        charmSprite = charmShowIfCollected.GetAction<SetSpriteRendererSprite>("R King", 0).sprite.Value as Sprite;
                        charmCoords = charmSprite.uv.Select(uv => ConvertUVToPixelCoordinates(uv, charmSprite.texture.width, charmSprite.texture.height));
                        charmX = charmCoords.Min(uv => uv.x);
                        charmY = charmCoords.Min(uv => uv.y);
                        newCharmSprite = Sprite.Create(invTex, new Rect(charmX + offsetX, charmY + offsetY, charmSprite.rect.width - shrinkX, charmSprite.rect.height - shrinkY), new Vector2(0.5f, 0.5f));
                        charmShowIfCollected.GetAction<SetSpriteRendererSprite>("R King", 0).sprite.Value = newCharmSprite;
                        updateSprite.GetAction<SetSpriteRendererSprite>("R King", 0).sprite.Value = newCharmSprite;
                        
                        offsetX = 0;
                        offsetY = -3;
                        shrinkX = 0;
                        shrinkY = 8;
                        charmSprite = charmShowIfCollected.GetAction<SetSpriteRendererSprite>("R Final", 0).sprite.Value as Sprite;
                        charmCoords = charmSprite.uv.Select(uv => ConvertUVToPixelCoordinates(uv, charmSprite.texture.width, charmSprite.texture.height));
                        charmX = charmCoords.Min(uv => uv.x);
                        charmY = charmCoords.Min(uv => uv.y);
                        newCharmSprite = Sprite.Create(invTex, new Rect(charmX + offsetX, charmY + offsetY, charmSprite.rect.width - shrinkX, charmSprite.rect.height - shrinkY), new Vector2(0.5f, 0.5f));
                        charmShowIfCollected.GetAction<SetSpriteRendererSprite>("R Final", 0).sprite.Value = newCharmSprite;
                        updateSprite.GetAction<SetSpriteRendererSprite>("R Final", 0).sprite.Value = newCharmSprite;
                        
                        offsetX = 0;
                        offsetY = -5;
                        shrinkX = 0;
                        shrinkY = 12;
                        charmSprite = charmShowIfCollected.GetAction<SetSpriteRendererSprite>("R Shade", 0).sprite.Value as Sprite;
                        charmCoords = charmSprite.uv.Select(uv => ConvertUVToPixelCoordinates(uv, charmSprite.texture.width, charmSprite.texture.height));
                        charmX = charmCoords.Min(uv => uv.x);
                        charmY = charmCoords.Min(uv => uv.y);
                        newCharmSprite = Sprite.Create(invTex, new Rect(charmX + offsetX, charmY + offsetY, charmSprite.rect.width - shrinkX, charmSprite.rect.height - shrinkY), new Vector2(0.5f, 0.5f));
                        charmShowIfCollected.GetAction<SetSpriteRendererSprite>("R Shade", 0).sprite.Value = newCharmSprite;
                        updateSprite.GetAction<SetSpriteRendererSprite>("R Shade", 0).sprite.Value = newCharmSprite;
                        
                        break;
                    case 40:
                        offsetX = -12;
                        offsetY = 0;
                        shrinkX = 20;
                        shrinkY = 10;
                        charmSprite = CharmIconList.Instance.grimmchildLevel1;
                        charmCoords = charmSprite.uv.Select(uv => ConvertUVToPixelCoordinates(uv, charmSprite.texture.width, charmSprite.texture.height));
                        charmX = charmCoords.Min(uv => uv.x);
                        charmY = charmCoords.Min(uv => uv.y);
                        newCharmSprite = Sprite.Create(invTex, new Rect(charmX + offsetX, charmY + offsetY, charmSprite.rect.width - shrinkX, charmSprite.rect.height - shrinkY), new Vector2(0.5f, 0.5f));
                        CharmIconList.Instance.grimmchildLevel1 = newCharmSprite;
                        
                        offsetX = -6;
                        offsetY = 4;
                        shrinkX = 28;
                        shrinkY = 16;
                        charmSprite = CharmIconList.Instance.grimmchildLevel2;
                        charmCoords = charmSprite.uv.Select(uv => ConvertUVToPixelCoordinates(uv, charmSprite.texture.width, charmSprite.texture.height));
                        charmX = charmCoords.Min(uv => uv.x);
                        charmY = charmCoords.Min(uv => uv.y);
                        newCharmSprite = Sprite.Create(invTex, new Rect(charmX + offsetX, charmY + offsetY, charmSprite.rect.width - shrinkX, charmSprite.rect.height - shrinkY), new Vector2(0.5f, 0.5f));
                        CharmIconList.Instance.grimmchildLevel2 = newCharmSprite;
                        
                        offsetX = -14;
                        offsetY = 0;
                        shrinkX = 0;
                        shrinkY = 0;
                        charmSprite = CharmIconList.Instance.grimmchildLevel3;
                        charmCoords = charmSprite.uv.Select(uv => ConvertUVToPixelCoordinates(uv, charmSprite.texture.width, charmSprite.texture.height));
                        charmX = charmCoords.Min(uv => uv.x);
                        charmY = charmCoords.Min(uv => uv.y);
                        newCharmSprite = Sprite.Create(invTex, new Rect(charmX + offsetX, charmY + offsetY, charmSprite.rect.width - shrinkX, charmSprite.rect.height - shrinkY), new Vector2(0.5f, 0.5f));
                        CharmIconList.Instance.grimmchildLevel3 = newCharmSprite;
                        
                        offsetX = -18;
                        offsetY = 0;
                        shrinkX = 0;
                        shrinkY = 0;
                        charmSprite = CharmIconList.Instance.grimmchildLevel4;
                        charmCoords = charmSprite.uv.Select(uv => ConvertUVToPixelCoordinates(uv, charmSprite.texture.width, charmSprite.texture.height));
                        charmX = charmCoords.Min(uv => uv.x);
                        charmY = charmCoords.Min(uv => uv.y);
                        newCharmSprite = Sprite.Create(invTex, new Rect(charmX + offsetX, charmY + offsetY, charmSprite.rect.width - shrinkX, charmSprite.rect.height - shrinkY), new Vector2(0.5f, 0.5f));
                        CharmIconList.Instance.grimmchildLevel4 = newCharmSprite;
                        
                        offsetX = 0;
                        offsetY = 0;
                        shrinkX = 0;
                        shrinkY = 0;
                        charmSprite = CharmIconList.Instance.nymmCharm;
                        charmCoords = charmSprite.uv.Select(uv => ConvertUVToPixelCoordinates(uv, charmSprite.texture.width, charmSprite.texture.height));
                        charmX = charmCoords.Min(uv => uv.x);
                        charmY = charmCoords.Min(uv => uv.y);
                        newCharmSprite = Sprite.Create(invTex, new Rect(charmX + offsetX, charmY + offsetY, charmSprite.rect.width - shrinkX, charmSprite.rect.height - shrinkY), new Vector2(0.5f, 0.5f));
                        CharmIconList.Instance.nymmCharm = newCharmSprite;

                        break;
                    default:
                        // Fringe cases where charm rect positions and sizes are incorrect
                        switch (charmNum)
                        {
                            // Fury of the Fallen
                            case 6:
                                offsetY = -5;
                                shrinkY = 10;
                                break;
                            // Defender's Crest
                            case 10:
                                offsetX = -2;
                                offsetY = 0;
                                shrinkX = 20;
                                shrinkY = 10;
                                break;
                            // Shape of Unn
                            case 28:
                                offsetX = -8;
                                offsetY = -4;
                                shrinkX = 40;
                                shrinkY = 10;
                                break;
                            // Dashmaster
                            case 31:
                                offsetX = 0;
                                offsetY = 0;
                                shrinkX = 10;
                                shrinkY = 6;
                                break;
                            // Quick Slash
                            case 32:
                                offsetX = 2;
                                offsetY = 2;
                                shrinkY = 2;
                                break;
                            // Deep Focus
                            case 34:
                                shrinkY = 6;
                                break;
                        }

                        charmSprite = CharmIconList.Instance.spriteList[charmNum];
                        charmCoords = charmSprite.uv.Select(uv => ConvertUVToPixelCoordinates(uv, charmSprite.texture.width, charmSprite.texture.height));
                        charmX = charmCoords.Min(uv => uv.x);
                        charmY = charmCoords.Min(uv => uv.y);
                        newCharmSprite = Sprite.Create(invTex,
                            new Rect(charmX + offsetX, charmY + offsetY, charmSprite.rect.width - shrinkX,
                                charmSprite.rect.height - shrinkY), new Vector2(0.5f, 0.5f));
                        CharmIconList.Instance.spriteList[charmNum] = newCharmSprite;

                        break;
                }
            }
            
            texRoutineRunning = false;
        }

        private static Vector2 ConvertUVToPixelCoordinates(Vector2 uv, int width, int height)
        {
            return new Vector2(uv.x * width, uv.y * height);   
        }
        
        private static void Log(object message) => Modding.Logger.Log("[Sprite Loader] " + message);
    }
}
