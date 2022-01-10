using System;
using System.IO;
using System.Linq;

using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;           

using GlobalEnums;
using Modding;
using UnityEngine;
using UnityEngine.SceneManagement;
using Satchel;
using static Satchel.SpriteUtils;
using static Satchel.GameObjectUtils;
using static Satchel.IoUtils;

namespace CustomKnight {
    public class DumpManager{

        internal bool enabled = false;
        internal DumpManager(){
            if(CustomKnight.isSatchelInstalled()){
                ModHooks.LanguageGetHook += SaveTextDump;
                UnityEngine.SceneManagement.SceneManager.sceneLoaded += dumpAllSprites;
                On.HutongGames.PlayMaker.Actions.ActivateGameObject.DoActivateGameObject += dumpAllSprites;
            }
        }


        internal Dictionary<string,bool> isTextureDumped = new Dictionary<string,bool>();

        internal void dumpSpriteForGo(Scene scene,GameObject go){            
            var name = go.GetPath(true);
            Log("game object to be dumped -" + go.name);
            Log($"gameobject path {name}");
            Animator anim = go.GetComponent<Animator>();
            SpriteRenderer sr = go.GetComponent<SpriteRenderer>();
            if(anim != null && sr != null){
                var caf = go.GetAddComponent<CustomAnimationFrames>();
                caf.dumpPath = Path.Combine(SkinManager.DATA_DIR,"Dump");
                caf.dump = true;
                return;
            }
            if(sr != null && sr.sprite != null){
                SaveSpriteDump(scene,name, sr.sprite);
                return;
            } 
            tk2dSprite tk2ds = go.GetComponent<tk2dSprite>();
            if(tk2ds != null){
                SaveTextureDump(scene,name, (Texture2D) tk2ds.GetCurrentSpriteDef().material.mainTexture);
                return;
            }
        }
        internal void dumpAllSprites(){
           if(!enabled) {return;} 
           var scenes = SceneUtils.GetAllLoadedScenes();
           foreach(var scene in scenes){ 
                var GOList = scene.GetAllGameObjects();
                foreach(var go in GOList){
                    dumpSpriteForGo(scene,go);
                }
           }
        }

        internal void dumpAllSprites(Scene scene,LoadSceneMode mode){
            dumpAllSprites();
        }
        
        internal void dumpAllSprites(On.HutongGames.PlayMaker.Actions.ActivateGameObject.orig_DoActivateGameObject orig, HutongGames.PlayMaker.Actions.ActivateGameObject self){
            orig(self);
            if(self.gameObject.GameObject.Value != null){
                dumpAllSprites();
            }
        }
        internal void SaveSpriteDump(Scene scene,string objectName, Sprite sprite){
            if(!enabled) {return;} 
            string DUMP_DIR = Path.Combine(SkinManager.DATA_DIR,"Dump");
            string scenePath = Path.Combine(DUMP_DIR,scene.name);
            EnsureDirectory(DUMP_DIR);
            EnsureDirectory(scenePath);
            
            string outpath = Path.Combine(scenePath,objectName.Replace('/',Path.DirectorySeparatorChar)+".png");
            try{
                EnsureDirectory(Path.GetDirectoryName(outpath));
            } catch (IOException e){
                Log(e.ToString());
            }
            if(!isTextureDumped.TryGetValue(outpath,out bool path) && !File.Exists(outpath)){
                Texture2D dupe = (Texture2D) SpriteUtils.ExtractTextureFromSprite(sprite);
                byte[] texBytes = dupe.EncodeToPNG();
        
                File.WriteAllBytes(outpath,texBytes);
                isTextureDumped[outpath] = true;
            }            
        }
        internal void SaveTextureDump(Scene scene,string objectName, Texture2D texture){
            if(!enabled) {return;} 
            string DUMP_DIR = Path.Combine(SkinManager.DATA_DIR,"Dump");
            string scenePath = Path.Combine(DUMP_DIR,scene.name);
            EnsureDirectory(DUMP_DIR);
            EnsureDirectory(scenePath);
            
            string outpath = Path.Combine(scenePath,objectName.Replace('/',Path.DirectorySeparatorChar)+".png");
            try{
                EnsureDirectory(Path.GetDirectoryName(outpath));
            } catch (IOException e){
                Log(e.ToString());
            }
            if(!isTextureDumped.TryGetValue(outpath,out bool path) && !File.Exists(outpath)){
                Texture2D dupe = TextureUtils.duplicateTexture(texture);
                byte[] texBytes = dupe.EncodeToPNG();
        
                File.WriteAllBytes(outpath,texBytes);
                isTextureDumped[outpath] = true;
            }            
        }
        internal void SaveTextDump( string key, string value){
            if(!enabled) {return;} 
            string DUMP_DIR = Path.Combine(SkinManager.DATA_DIR,"Dump");
            Scene scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
            string scenePath = Path.Combine(DUMP_DIR,scene.name);
            EnsureDirectory(DUMP_DIR);
            EnsureDirectory(scenePath);
            var outpath = (Path.Combine(scenePath,key+".txt"));

            try{
                EnsureDirectory(Path.GetDirectoryName(outpath));
            } catch (IOException e){
                Log(e.ToString());
            }
            if(!File.Exists(outpath)){
                File.WriteAllText(outpath,value);
            }
        }
        internal string SaveTextDump( string key, string sheet , string value){
            SaveTextDump(sheet+key, value);
            return value;
        }

        internal void Unload(){
            ModHooks.LanguageGetHook -= SaveTextDump;
            UnityEngine.SceneManagement.SceneManager.sceneLoaded -= dumpAllSprites;
            On.HutongGames.PlayMaker.Actions.ActivateGameObject.DoActivateGameObject -= dumpAllSprites;
        }    
        internal void Log(string str) {
            CustomKnight.Instance.Log("[DumpManager] " +str);
        }
        internal static void debugDumpSprite(Sprite sprite){
            Texture2D dupe = (Texture2D) Satchel.SpriteUtils.ExtractTextureFromSprite(sprite);
            Satchel.TextureUtils.WriteTextureToFile(dupe,$"{Satchel.AssemblyUtils.getCurrentDirectory()}/{sprite.name}.png");
        }
        internal static void debugDumpTex(Texture2D tex,string name){
            Satchel.TextureUtils.WriteTextureToFile(tex,$"{Satchel.AssemblyUtils.getCurrentDirectory()}/{name}.png");
        }
    }
}