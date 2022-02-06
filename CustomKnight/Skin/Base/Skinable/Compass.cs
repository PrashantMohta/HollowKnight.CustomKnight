using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CustomKnight
{
    public class Compass : Skinable_Single
    {
        public static string NAME = "Compass";
        public Compass() : base(Compass.NAME){}
        public override Material GetMaterial(){
            GameMap Map = GameManager.instance.gameMap?.GetComponent<GameMap>();
            return Map?.compassIcon?.GetComponent<tk2dSprite>()?.GetCurrentSpriteDef()?.material;
        }
        public override void SaveDefaultTexture(){
            if(material != null && material.mainTexture != null){
                ckTex.defaultTex = material.mainTexture as Texture2D;
            } else {
                CustomKnight.Instance.Log($"skinable {name} : material is null");
            }
        }
        public override void ApplyTexture(Texture2D tex){
            if(ckTex.defaultTex == null){
                //incase we do not have the default texture save it.
                ckTex.defaultTex = material.mainTexture as Texture2D;
            }
            material.mainTexture = tex;
        }

    }
}