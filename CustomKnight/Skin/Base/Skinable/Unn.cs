using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Satchel.GameObjectUtils;

namespace CustomKnight
{
    public class Unn : Skinable_Tk2d
    {
        public static string NAME = "Unn";
        public Unn() : base(Unn.NAME){}
        public override Material GetMaterial(){
            return HeroController.instance.gameObject.GetComponent<tk2dSpriteAnimator>().GetClipByName("Slug Up").frames[0].spriteCollection.spriteDefinitions[0].material;
        }

    }
}