using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using HutongGames.PlayMaker.Actions;
using static Satchel.FsmUtil;
using static CustomKnight.SkinManager;
using static Satchel.GameObjectUtils;

namespace CustomKnight
{
    internal class SpriteLoader : MonoBehaviour
    {
        private static GameObject loader;
        private static bool texRoutineRunning;
        private static Coroutine setTexRoutine;
        internal static bool LoadComplete { get; private set; }
        internal static void PullDefaultTextures()
        {
            if (!SkinManager.savedDefaultTextures)
            {                
                if(SkinManager.Skinables != null){
                    foreach(KeyValuePair<string,Skinable> kvp in SkinManager.Skinables){
                        kvp.Value?.SaveTexture();
                    }
                }
            }
        
            SkinManager.savedDefaultTextures = true;
        }
        internal static void UnloadAll()
        {    
            if (loader != null)
            {
                Destroy(loader);
            }

            if (HeroController.instance != null)
            {
                foreach(KeyValuePair<string,Skinable> kvp in SkinManager.Skinables){
                    kvp.Value.Reset();
                }
            }
            
            if (texRoutineRunning && GameManager.instance != null)
            {
                GameManager.instance.StopCoroutine(setTexRoutine);
                texRoutineRunning = false;
            }

            DestroyObjects();
        }
        internal static void Load()
        {
            if (loader == null)
            {
                loader = new GameObject("Loader");
                loader.AddComponent<SpriteLoader>();
                DontDestroyOnLoad(loader);
            } else {
                SpriteLoader.LoadSprites();
            }
             
        }
        internal static void ModifyHeroTextures(SaveGameData data = null)
        {
            if (!texRoutineRunning)
            {
                setTexRoutine = GameManager.instance.StartCoroutine(SetHeroTex());
                texRoutineRunning = true;
            }
        }
        
        internal IEnumerator Start()
        {
            yield return new WaitWhile(
                () => HeroController.instance == null || GameManager.instance == null || GameManager.instance.gameMap == null
            );

            LoadSprites();
        }

        private static void DestroyObjects()
        {
            foreach (KeyValuePair<string, CustomKnightTexture> pair in SkinManager.Textures)
            {
                CustomKnightTexture texture = pair.Value;
                if (texture.tex != null)
                {
                    Destroy(texture.tex);
                }
            }
            
            LoadComplete = false;
        }

        internal static void SetSkin(Dictionary<string, CustomKnightTexture> SkinMap){
            SkinManager.Textures = SkinMap;
            ModifyHeroTextures();
        }
        internal static void LoadSprites()
        {
            foreach (KeyValuePair<string,Skinable> kvp in SkinManager.Skinables)
            {
                kvp.Value.prepare();
            }
            if (SkinManager.SKIN_FOLDER == null)
            {
                SkinManager.SKIN_FOLDER = "Default";
            }
            LoadComplete = false;

            foreach (KeyValuePair<string, CustomKnightTexture> pair in SkinManager.Textures)
            {

                CustomKnightTexture texture = pair.Value;
                if(SkinManager.Skinables.TryGetValue(pair.Key, out var skinable)){
                    texture = skinable.ckTex;
                    Modding.Logger.Log($"{pair.Key} found");
                }
                string file = (SkinManager.SKINS_FOLDER + "/" + SkinManager.SKIN_FOLDER + "/" + texture.fileName).Replace("\\", "/");
                texture.missing = !File.Exists(file);
                
                if (!texture.missing)
                {
                    byte[] texBytes = File.ReadAllBytes(file);
                    if (texture.tex != null)
                    {
                        Destroy(texture.tex);
                    }
                    
                    texture.tex = new Texture2D(2, 2);
                    texture.tex.LoadImage(texBytes);
                    
                } else {
                    if (texture.tex != null)
                    {
                        Destroy(texture.tex);
                    }
                }    
            }

            SetSkin(SkinManager.Textures);
            LoadComplete = true;
        }

        private static IEnumerator SetHeroTex()
        {
            yield return new WaitWhile(() => !LoadComplete || HeroController.instance == null || CharmIconList.Instance == null);
            /*foreach(var psr in GameObject.FindObjectsOfType<ParticleSystemRenderer>())
            {
                psr.gameObject.LogWithChildren();
                DumpManager.debugDumpTex((Texture2D)psr.material.mainTexture,psr.name);
            }*/
            
            PullDefaultTextures();
            CustomKnight.swapManager.resetAllTextures();
            CustomKnight.swapManager.Swap(Path.Combine(SkinManager.SKINS_FOLDER,SKIN_FOLDER));

            foreach(KeyValuePair<string,Skinable> kvp in SkinManager.Skinables){
                kvp.Value.Apply();
            }

            texRoutineRunning = false;
        }

    }
}
