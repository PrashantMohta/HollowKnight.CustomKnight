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

        public static Texture2D KnightTex { get; private set; }
        public static Texture2D SprintTex { get; private set; }

        public static bool LoadComplete { get; private set; }

        public static void PullDefaultTextures()
        {
            if (HeroController.instance != null && (DefaultKnightTex == null || DefaultSprintTex == null))
            {
                DefaultKnightTex = HeroController.instance.GetComponent<tk2dSprite>().GetCurrentSpriteDef().material.mainTexture as Texture2D;
                DefaultSprintTex = HeroController.instance.GetComponent<tk2dSpriteAnimator>().GetClipByName("Sprint").frames[0].spriteCollection.spriteDefinitions[0].material.mainTexture as Texture2D;
            }
        }

        public static void UnloadAll()
        {
            if (loader != null)
            {
                Destroy(loader);
            }

            if (HeroController.instance != null && DefaultKnightTex != null && DefaultSprintTex != null)
            {
                HeroController.instance.GetComponent<tk2dSprite>().GetCurrentSpriteDef().material.mainTexture = DefaultKnightTex;
                HeroController.instance.GetComponent<tk2dSpriteAnimator>().GetClipByName("Sprint").frames[0].spriteCollection.spriteDefinitions[0].material.mainTexture = DefaultSprintTex;
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

            LoadComplete = false;
        }

        private IEnumerator LoadSprites()
        {
            Modding.Logger.Log("[CustomKnight] - Starting texture load");

            WWW knight = null;
            WWW sprint = null;

            knight = new WWW(("file:///" + CustomKnight.DATA_DIR + "/" + CustomKnight.KNIGHT_PNG).Replace("\\", "/"));
            sprint = new WWW(("file:///" + CustomKnight.DATA_DIR + "/" + CustomKnight.SPRINT_PNG).Replace("\\", "/"));

            yield return knight;
            yield return sprint;

            DestroyObjects();

            KnightTex = knight.texture;
            SprintTex = sprint.texture;

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

            texRoutineRunning = false;
        }
    }
}
