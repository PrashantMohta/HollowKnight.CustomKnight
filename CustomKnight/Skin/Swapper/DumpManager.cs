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

        public bool enabled = false;
        public DumpManager(){
            if(CustomKnight.isSatchelInstalled()){
                ModHooks.LanguageGetHook += SaveTextDump;
                UnityEngine.SceneManagement.SceneManager.sceneLoaded += dumpAllSprites;
                On.HutongGames.PlayMaker.Actions.ActivateGameObject.DoActivateGameObject += dumpAllSprites;
            }
        }


        public Dictionary<string,bool> isTextureDumped = new Dictionary<string,bool>();

        public void dumpSpriteForGo(Scene scene,GameObject go){ 
            if(go == null){return;}           
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
                var spriteDef = tk2ds.GetCurrentSpriteDef();
                if(spriteDef == null || spriteDef.material == null){return;}
                SaveTextureDump(scene,name, (Texture2D) spriteDef.material.mainTexture);
                return;
            }
        }
        public void dumpAllSprites(Scene scene){
            if(!enabled) {return;} 
            var GOList = scene.GetAllGameObjects();
            foreach(var go in GOList){
                if(scene!= null && go != null){
                    dumpSpriteForGo(scene,go);
                }
            }
        }
        public void dumpAllSprites(){
           if(!enabled) {return;} 
           var scenes = SceneUtils.GetAllLoadedScenes();
           foreach(var scene in scenes){ 
                dumpAllSprites(scene);
           }
        }

        public void dumpAllSprites(Scene scene,LoadSceneMode mode){
            dumpAllSprites();
        }
        
        public void dumpAllSprites(On.HutongGames.PlayMaker.Actions.ActivateGameObject.orig_DoActivateGameObject orig, HutongGames.PlayMaker.Actions.ActivateGameObject self){
            orig(self);
            if(enabled && self.gameObject.GameObject.Value != null){
                dumpAllSprites(self.gameObject.GameObject.Value.scene);
            }
        }
        public void SaveSpriteDump(Scene scene,string objectName, Sprite sprite){
            if(!enabled) {return;} 
            string DUMP_DIR = Path.Combine(SkinManager.DATA_DIR,"Dump");
            string scenePath = Path.Combine(DUMP_DIR,scene.name);
            EnsureDirectory(DUMP_DIR);
            EnsureDirectory(scenePath);
            
            string outpath = Path.Combine(scenePath,objectName.Replace('/',Path.DirectorySeparatorChar)+".png");
            try{
                EnsureDirectory(Path.GetDirectoryName(outpath));
            } catch (IOException e){

            }
            if(!isTextureDumped.TryGetValue(outpath,out bool path) && !File.Exists(outpath)){
                Texture2D dupe = (Texture2D) SpriteUtils.ExtractTextureFromSprite(sprite);
                byte[] texBytes = dupe.EncodeToPNG();
        
                File.WriteAllBytes(outpath,texBytes);
                isTextureDumped[outpath] = true;
            }            
        }
        public void SaveTextureDump(Scene scene,string objectName, Texture2D texture){
            if(!enabled) {return;} 
            string DUMP_DIR = Path.Combine(SkinManager.DATA_DIR,"Dump");
            string scenePath = Path.Combine(DUMP_DIR,scene.name);
            EnsureDirectory(DUMP_DIR);
            EnsureDirectory(scenePath);
            
            string outpath = Path.Combine(scenePath,objectName.Replace('/',Path.DirectorySeparatorChar)+".png");
            try{
                EnsureDirectory(Path.GetDirectoryName(outpath));
            } catch (IOException e){

            }
            if(!isTextureDumped.TryGetValue(outpath,out bool path) && !File.Exists(outpath)){
                Texture2D dupe = TextureUtils.duplicateTexture(texture);
                byte[] texBytes = dupe.EncodeToPNG();
        
                File.WriteAllBytes(outpath,texBytes);
                Texture2D.DestroyImmediate(dupe);
                isTextureDumped[outpath] = true;
            }            
        }
        public void SaveTextDump( string key, string value){
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

            }
            if(!File.Exists(outpath)){
                File.WriteAllText(outpath,value);
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