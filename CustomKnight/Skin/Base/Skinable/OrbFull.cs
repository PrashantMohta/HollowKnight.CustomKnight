using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Satchel;

namespace CustomKnight
{
    public class OrbFull : Skinable_Sprite
    {
        public static string NAME = "OrbFull";
        public OrbFull() : base(OrbFull.NAME){}

        public override void SaveDefaultTexture(){
            try{
                foreach (SpriteRenderer i in GameCameras.instance.hudCanvas.GetComponentsInChildren<SpriteRenderer>(true))
                {
                    if (i.name == "Orb Full")
                    {
                        ckTex.defaultSprite = i.sprite;
                    }
                }
            } catch(Exception e){
                CustomKnight.Instance.Log($"skinable {name} : {e}");
            }
        }
        public override void ApplySprite(Sprite sprite){
            foreach (SpriteRenderer i in GameCameras.instance.hudCanvas.GetComponentsInChildren<SpriteRenderer>(true))
            {
                if (i.name == "Orb Full")
                {
                    i.sprite = sprite;
                }
                else if (i.name == "Pulse Sprite")
                {
                    if (i.gameObject != null)
                    {
                        GameObject.Destroy(i.gameObject);
                    }
                }
            }
        }

    }
}