using Modding;
using UnityEngine;
using CustomKnight;
using Satchel;
using System;
using System.Collections;
using System.Collections.Generic;

namespace SkinProvider{
    public class EmbeddedSkin : ISelectableSkin{
        public string SkinName = "EmbeddedSkin";
        public Dictionary<string,Texture2D> textures = new();
        public Dictionary<string,Texture2D> Alttextures = new();

        public bool isAlt = false;
        public EmbeddedSkin() {
            textures["Knight.png"] = AssemblyUtils.GetTextureFromResources("Knight.png");
            textures["Sprint.png"] = AssemblyUtils.GetTextureFromResources("Sprint.png");
            Alttextures["Knight.png"] = AssemblyUtils.GetTextureFromResources("AKnight.png");
            Alttextures["Sprint.png"] = AssemblyUtils.GetTextureFromResources("ASprint.png");
        }

        public bool shouldCache() => false;
        public string GetId() => SkinName;
        public string GetName() => SkinName;
        public bool hasSwapper() => false;
        public string getSwapperPath() => "";

        public bool Exists(string FileName){
            return isAlt? Alttextures.ContainsKey(FileName) : textures.ContainsKey(FileName);
        }
        public Texture2D GetTexture(string FileName){
            Texture2D texture = null;
            try{
                texture = isAlt? Alttextures[FileName] : textures[FileName];
            } catch(Exception e){
                Modding.Logger.Log(e.ToString());
            }
            return texture;
        } 
    }
    public class SkinProvider : Mod {
        new public string GetName() => "Skin Provider";
        public override string GetVersion() => "v1";

        public EmbeddedSkin Skin = new EmbeddedSkin();
        public override void Initialize()
        {
            CustomKnight.CustomKnight.OnReady += (_,e)=>{
                SkinManager.AddSkin(Skin);
            };
            ModHooks.HeroUpdateHook += ()=>{
                if(Input.GetKeyDown(KeyCode.O)){
                    Skin.isAlt = !Skin.isAlt;
                    SkinManager.RefreshSkin(true);
                }
            };
        }
    }

}