using System.IO;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CustomKnight{
    public interface ISelectableSkin{
        public string GetId();
        public string GetName(); 
        public bool shouldCache();
        public bool hasSwapper();
        public string getSwapperPath();
        public bool Exists(string FileName);
        public Texture2D GetTexture(string FileName);

    }
    public class StaticSkin : ISelectableSkin{
        public string SkinDirectory = "";
        public StaticSkin(string DirectoryName) {
            SkinDirectory = DirectoryName;
        }
        public bool shouldCache() => true;
        public string GetId() => SkinDirectory;
        public string GetName() => SkinDirectory;
        public bool hasSwapper() => true;
        public string getSwapperPath() => Path.Combine(SkinManager.SKINS_FOLDER,SkinDirectory);

        public bool Exists(string FileName){
            string file = ($"{SkinManager.SKINS_FOLDER}/{SkinDirectory}/{FileName}").Replace("\\", "/");
            return File.Exists(file);
        }
        public Texture2D GetTexture(string FileName){
            Texture2D texture = null;
            try{
                string file = ($"{SkinManager.SKINS_FOLDER}/{SkinDirectory}/{FileName}").Replace("\\", "/");
                byte[] texBytes = File.ReadAllBytes(file);
                texture = new Texture2D(2, 2);
                texture.LoadImage(texBytes);
            } catch(Exception e){
                CustomKnight.Instance.Log(e.ToString());
            }
            return texture;
        } 
    }
    
}