using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Satchel.GameObjectUtils;

namespace CustomKnight
{
    public class Hud : Skinable_noCache
    {
        public static string NAME = "Hud";
        public Hud() : base(Hud.NAME){}

        public override Material GetMaterial(){
            Material _hudMat = null ;
            foreach (tk2dSprite i in GameCameras.instance.hudCanvas.GetComponentsInChildren<tk2dSprite>())
            {
                if (i.name == "Health 1")
                {
                    _hudMat = i.GetCurrentSpriteDef().material;
                    break;
                }
            }
            return _hudMat;
        }

    }
}