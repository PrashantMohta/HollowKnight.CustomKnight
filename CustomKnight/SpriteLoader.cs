using System;
using System.Collections;
using UnityEngine;

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
        public static Texture2D DefaultShriekTex { get; private set; }
        public static Texture2D DefaultVSTex { get; private set; }

        public static Texture2D KnightTex { get; private set; }
        public static Texture2D SprintTex { get; private set; }
        public static Texture2D WraithsTex { get; private set; }
        public static Texture2D ShriekTex { get; private set; }
        public static Texture2D VSTex { get; private set; }

        public static bool LoadComplete { get; private set; }

        public static void PullDefaultTextures()
        {
            if (HeroController.instance != null && (DefaultKnightTex == null || DefaultSprintTex == null || DefaultWraithsTex == null || DefaultShriekTex == null))
            {
                DefaultKnightTex = HeroController.instance.GetComponent<tk2dSprite>().GetCurrentSpriteDef().material.mainTexture as Texture2D;
                DefaultSprintTex = HeroController.instance.GetComponent<tk2dSpriteAnimator>().GetClipByName("Sprint").frames[0].spriteCollection.spriteDefinitions[0].material.mainTexture as Texture2D;

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
                                DefaultShriekTex = spellsChild.gameObject.GetComponent<tk2dSprite>().GetCurrentSpriteDef().material.mainTexture as Texture2D;
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
            }
        }

        public static void UnloadAll()
        {
            if (loader != null)
            {
                Destroy(loader);
            }

            if (HeroController.instance != null && DefaultKnightTex != null && DefaultSprintTex != null && DefaultWraithsTex != null && DefaultShriekTex != null)
            {
                HeroController.instance.GetComponent<tk2dSprite>().GetCurrentSpriteDef().material.mainTexture = DefaultKnightTex;
                HeroController.instance.GetComponent<tk2dSpriteAnimator>().GetClipByName("Sprint").frames[0].spriteCollection.spriteDefinitions[0].material.mainTexture = DefaultSprintTex;

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
                                spellsChild.gameObject.GetComponent<tk2dSprite>().GetCurrentSpriteDef().material.mainTexture = DefaultShriekTex;
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
                loader = new GameObject();
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

        public void Start()
        {
            StartCoroutine(LoadSprites());
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

            if (ShriekTex != null)
            {
                Destroy(ShriekTex);
            }

            if (VSTex != null)
            {
                Destroy(VSTex);
            }

            LoadComplete = false;
        }

        private IEnumerator LoadSprites()
        {
            Modding.Logger.Log("[CustomKnight] - Starting texture load");

            WWW knight = new WWW(("file:///" + CustomKnight.DATA_DIR + "/" + CustomKnight.KNIGHT_PNG).Replace("\\", "/"));
            WWW sprint = new WWW(("file:///" + CustomKnight.DATA_DIR + "/" + CustomKnight.SPRINT_PNG).Replace("\\", "/"));
            WWW wraiths = new WWW(("file:///" + CustomKnight.DATA_DIR + "/" + CustomKnight.WRAITHS_PNG).Replace("\\", "/"));
            WWW shriek = new WWW(("file:///" + CustomKnight.DATA_DIR + "/" + CustomKnight.SHRIEK_PNG).Replace("\\", "/"));
            WWW vs = new WWW(("file:///" + CustomKnight.DATA_DIR + "/" + CustomKnight.VS_PNG).Replace("\\", "/"));

            yield return knight;
            yield return sprint;
            yield return wraiths;
            yield return shriek;
            yield return vs;

            DestroyObjects();

            KnightTex = knight.textureNonReadable;
            SprintTex = sprint.textureNonReadable;
            WraithsTex = wraiths.textureNonReadable;
            ShriekTex = shriek.textureNonReadable;
            VSTex = vs.textureNonReadable;

            Modding.Logger.Log("[CustomKnight] - Texture load done");
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

            HeroController.instance.GetComponent<tk2dSprite>().GetCurrentSpriteDef().material.mainTexture = KnightTex;
            HeroController.instance.GetComponent<tk2dSpriteAnimator>().GetClipByName("Sprint").frames[0].spriteCollection.spriteDefinitions[0].material.mainTexture = SprintTex;

            foreach (Transform child in HeroController.instance.gameObject.transform)
            {
                if (child.name == "Spells")
                {
                    foreach (Transform spellsChild in child)
                    {
                        if (spellsChild.name == "Scr Heads" || spellsChild.name == "Scr Base")
                        {
                            spellsChild.gameObject.GetComponent<tk2dSprite>().GetCurrentSpriteDef().material.mainTexture = WraithsTex;
                        }
                        else if (spellsChild.name == "Scr Heads 2" || spellsChild.name == "Scr Base 2")
                        {
                            spellsChild.gameObject.GetComponent<tk2dSprite>().GetCurrentSpriteDef().material.mainTexture = ShriekTex;
                        }
                    }
                }
                else if (child.name == "Focus Effects")
                {
                    foreach (Transform focusChild in child)
                    {
                        if (focusChild.name == "Heal Anim")
                        {
                            focusChild.gameObject.GetComponent<tk2dSprite>().GetCurrentSpriteDef().material.mainTexture = VSTex;
                            break;
                        }
                    }
                }
            }

            texRoutineRunning = false;
        }
    }
}
