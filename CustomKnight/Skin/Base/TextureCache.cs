using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using HutongGames.PlayMaker.Actions;
using static Satchel.FsmUtil;
using static CustomKnight.SkinManager;
using static Satchel.GameObjectUtils;

namespace CustomKnight {
    internal class TextureCache { // used for reducing disk reads when switching skins back and forth
        internal static Dictionary<string,Dictionary<string,CustomKnightTexture>> skinTextureCache = new Dictionary<string,Dictionary<string,CustomKnightTexture>>();
        internal static List<string> recentSkins = new List<string>();
        internal static void setSkinTextureCache(string skin,string filename,CustomKnightTexture texture){
            if(!skinTextureCache.ContainsKey(skin)){
                 skinTextureCache[skin] = new Dictionary<string,CustomKnightTexture>();
            }
            skinTextureCache[skin][filename] = texture;
        }
        internal static void clearSkinTextureCache(string skin){
            foreach(var kvp in skinTextureCache[skin]){
                GameObject.Destroy(kvp.Value.tex);
            }
            skinTextureCache.Remove(skin);
        }
        internal static void trimTextureCache(){
            if(recentSkins.Count > CustomKnight.GlobalSettings.MaxSkinCache ){ 
                recentSkins = recentSkins.GetRange(recentSkins.Count - CustomKnight.GlobalSettings.MaxSkinCache,CustomKnight.GlobalSettings.MaxSkinCache);
            }
            var toClear = new List<string>();
            foreach(var kvp in skinTextureCache){
                if(!recentSkins.Contains(kvp.Key)){
                    toClear.Add(kvp.Key);
                }
            }
            foreach(var x in toClear){
                clearSkinTextureCache(x);
            }
        }
        internal static void clearAllTextureCache(){
            var toClear = new List<string>();
            foreach(var kvp in skinTextureCache){
                toClear.Add(kvp.Key);
            }
            foreach(var x in toClear){
                clearSkinTextureCache(x);
            }
        }

    }
}
