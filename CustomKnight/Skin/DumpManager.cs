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

namespace CustomKnight {
    public class DumpManager{

        public bool enabled = false;
        public DumpManager(){
            ModHooks.LanguageGetHook += SaveTextDump;
            UnityEngine.SceneManagement.SceneManager.sceneLoaded += dumpAllSprites;
            On.HutongGames.PlayMaker.Actions.ActivateGameObject.DoActivateGameObject += dumpAllSprites;
        }


        public Dictionary<string,bool> isTextureDumped = new Dictionary<string,bool>();

        public void dumpSpriteForGo(GameObject go){
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
                SaveSpriteDump(name, sr.sprite);
                return;
            } 
            tk2dSprite tk2ds = go.GetComponent<tk2dSprite>();
            if(tk2ds != null){
                SaveTextureDump(name, (Texture2D) tk2ds.GetCurrentSpriteDef().material.mainTexture);
                return;
            }
        }
        public void dumpAllSprites(){
           if(!enabled) {return;} 
           var GOList = GameObjectUtils.GetAllGameObjectsInScene();
           foreach(var go in GOList){
               dumpSpriteForGo(go);
           }
        }

        public void dumpAllSprites(Scene scene,LoadSceneMode mode){
            dumpAllSprites();
        }
        
        public void dumpAllSprites(On.HutongGames.PlayMaker.Actions.ActivateGameObject.orig_DoActivateGameObject orig, HutongGames.PlayMaker.Actions.ActivateGameObject self){
            orig(self);
            if(self.gameObject.GameObject.Value != null){
                if(!enabled) {return;} 
                var allGoList = GameObjectUtils.GetAllGameObjectsInScene();
                foreach(var go in allGoList){
                    dumpSpriteForGo(go);
                }
            }
        }
        public void SaveSpriteDump(string objectName, Sprite sprite){
            if(!enabled) {return;} 
            string DUMP_DIR = Path.Combine(SkinManager.DATA_DIR,"Dump");
            Scene scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
            string scenePath = Path.Combine(DUMP_DIR,scene.name);
            if (!Directory.Exists(DUMP_DIR))
            {
                Directory.CreateDirectory(DUMP_DIR);
            }
            if(!Directory.Exists(scenePath)){
                Directory.CreateDirectory(scenePath);
            }
            
            string outpath = Path.Combine(scenePath,objectName.Replace('/',Path.DirectorySeparatorChar)+".png");
            try{
                if(!Directory.Exists(Path.GetDirectoryName(outpath))){
                    Directory.CreateDirectory(Path.GetDirectoryName(outpath));
                }
            } catch (IOException e){

            }
            if(!isTextureDumped.TryGetValue(outpath,out bool path) && !File.Exists(outpath)){
                Texture2D dupe = (Texture2D) SpriteUtils.ExtractTextureFromSprite(sprite);
                byte[] texBytes = dupe.EncodeToPNG();
        
                File.WriteAllBytes(outpath,texBytes);
                isTextureDumped[outpath] = true;
            }            
        }
        public void SaveTextureDump(string objectName, Texture2D texture){
            if(!enabled) {return;} 
            string DUMP_DIR = Path.Combine(SkinManager.DATA_DIR,"Dump");
            Scene scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
            string scenePath = Path.Combine(DUMP_DIR,scene.name);
            if (!Directory.Exists(DUMP_DIR))
            {
                Directory.CreateDirectory(DUMP_DIR);
            }
            if(!Directory.Exists(scenePath)){
                Directory.CreateDirectory(scenePath);
            }
            
            string outpath = Path.Combine(scenePath,objectName.Replace('/',Path.DirectorySeparatorChar)+".png");
            try{
                if(!Directory.Exists(Path.GetDirectoryName(outpath))){
                    Directory.CreateDirectory(Path.GetDirectoryName(outpath));
                }
            } catch (IOException e){

            }
            if(!isTextureDumped.TryGetValue(outpath,out bool path) && !File.Exists(outpath)){
                Texture2D dupe = TextureUtils.duplicateTexture(texture);
                byte[] texBytes = dupe.EncodeToPNG();
        
                File.WriteAllBytes(outpath,texBytes);
                isTextureDumped[outpath] = true;
            }            
        }
        public void SaveTextDump( string key, string value){
            if(!enabled) {return;} 
            string DUMP_DIR = Path.Combine(SkinManager.DATA_DIR,"Dump");
            Scene scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
            string scenePath = Path.Combine(DUMP_DIR,scene.name);
            if (!Directory.Exists(DUMP_DIR))
            {
                Directory.CreateDirectory(DUMP_DIR);
            }
            if(!Directory.Exists(scenePath)){
                Directory.CreateDirectory(scenePath);
            }
            try{
                if(!Directory.Exists(Path.GetDirectoryName(Path.Combine(scenePath,key)))){
                    Directory.CreateDirectory(Path.GetDirectoryName(Path.Combine(scenePath,key)));
                }
            } catch (IOException e){

            }
            if(!File.Exists(Path.Combine(scenePath,key+".txt"))){
                File.WriteAllText(Path.Combine(scenePath,key+".txt"),value);
            }
        }
        public string SaveTextDump( string key, string sheet , string value){
            SaveTextDump( key, value);
            return value;
        }

        public void Unload(){
            ModHooks.LanguageGetHook -= SaveTextDump;
            UnityEngine.SceneManagement.SceneManager.sceneLoaded -= dumpAllSprites;
            On.HutongGames.PlayMaker.Actions.ActivateGameObject.DoActivateGameObject -= dumpAllSprites;
        }    
        public void Log(string str) {
            CustomKnight.Instance.Log("[DumpManager] " +str);
        }
    }
}