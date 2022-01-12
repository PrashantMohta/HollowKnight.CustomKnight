using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Satchel.GameObjectUtils;

namespace CustomKnight
{
    public class Liquid : Skinable_Tk2d
    {
        public static string NAME = "Liquid";
        public Liquid() : base(Liquid.NAME){}
        public override Material GetMaterial(){
            return GameCameras.instance.hudCanvas.FindGameObjectInChildren("Liquid").GetComponent<tk2dSprite>().GetCurrentSpriteDef().material;
        }

    }
}