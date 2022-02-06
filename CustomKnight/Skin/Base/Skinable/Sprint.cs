using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Satchel.GameObjectUtils;

namespace CustomKnight
{
    public class Sprint : Skinable_Tk2d
    {
        public static string NAME = "Sprint";
        public Sprint() : base(Sprint.NAME){}
        public override Material GetMaterial(){
            return HeroController.instance.gameObject.GetComponent<tk2dSpriteAnimator>().GetClipByName("Sprint").frames[0].spriteCollection.spriteDefinitions[0].material;
        }

    }
}