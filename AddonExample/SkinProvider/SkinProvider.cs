using Modding;
using UnityEngine;
using CustomKnight;
using Satchel;
using System;
using System.Collections;
using System.Collections.Generic;

namespace SkinProvider{
    public class EmbeddedSkin : ISelectableSkin
    {
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
            return isAlt? Alttextures.ContainsKey("A"+FileName) : textures.ContainsKey(FileName);
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

        byte[] ISelectableSkin.GetFile(string FileName) => AssemblyUtils.GetBytesFromResources(FileName);

        bool ISelectableSkin.HasCinematic(string CinematicName) => false;

        string ISelectableSkin.GetCinematicUrl(string CinematicName) => throw new NotImplementedException();
    }
    public class SkinProvider : Mod {
        new public string GetName() => "Skin Provider";
        public override string GetVersion() => "v2";

        public EmbeddedSkin Skin = new EmbeddedSkin();

        public SkinProvider() {
            /*CustomKnight.CustomKnight.OnInit += (_, e) =>
            {
                Log("CK  OnInit");
            };
            CustomKnight.CustomKnight.OnReady += (_, e) => {
                Log("CK  OnReady");
            };
            CustomKnight.CustomKnight.OnUnload += (_, e) => {
                Log("CK  OnUnload");
            };*/
            // mod load order can impact this, so should add the listener in constructor or set order priority
            CustomKnight.CustomKnight.OnInit += (_, e) => {
                SkinManager.AddSkin(Skin);
            };
        }
        public override void Initialize()
        {

            ModHooks.HeroUpdateHook += ()=>{
                if(Input.GetKeyDown(KeyCode.O)){
                    Skin.isAlt = !Skin.isAlt;
                    SkinManager.RefreshSkin(true);
                }
            };
        }
    }

}