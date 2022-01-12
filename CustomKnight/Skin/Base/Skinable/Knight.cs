using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Satchel.GameObjectUtils;

namespace CustomKnight
{
    public class Knight : Skinable_Tk2d
    {
        public static string NAME = "Knight";
        public Knight() : base(Knight.NAME){}
        public override Material GetMaterial(){
            return HeroController.instance.gameObject.GetComponent<tk2dSpriteAnimator>().GetClipByName("Idle").frames[0].spriteCollection.spriteDefinitions[0].material;
        }

    }
}