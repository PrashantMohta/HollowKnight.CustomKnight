using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Satchel;

namespace CustomKnight
{
    public class PinsScarab : Skinnable_Sprite
    {
        public static string NAME = "PinsScarab";
        public PinsScarab() : base(PinsScarab.NAME){}

        public override void SaveDefaultTexture(){
            try{
                GameMap Map = GameManager.instance.gameMap?.GetComponent<GameMap>();
                ckTex.defaultSprite = Map.mapMarkersBlue[0].GetComponent<SpriteRenderer>().sprite;
            } catch(Exception e){
                Modding.Logger.Log($"skinnable {name} : {e}");
            }
        }
        public override void ApplySprite(Sprite sprite){
            GameMap Map = GameManager.instance.gameMap?.GetComponent<GameMap>();
            Modding.Logger.Log($"count of markers {Map.mapMarkersBlue.Length}");
            foreach(var pin in Map.mapMarkersBlue){
                var anim = pin.GetComponent<Animator>();
                if(anim != null){
                    GameObject.Destroy(anim);
                }
                pin.GetComponent<SpriteRenderer>().sprite = sprite;

                pin.LogWithChildren();
            }
        }

    }
}