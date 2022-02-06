using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Satchel.GameObjectUtils;

namespace CustomKnight
{
    public class ScrOrbs : Skinable_Tk2d
    {
        public static string NAME = "ScrOrbs";
        public ScrOrbs() : base(ScrOrbs.NAME){}
        public override Material GetMaterial(){
            return HeroController.instance.gameObject.FindGameObjectInChildren("Scr Orbs").GetComponent<ParticleSystemRenderer>().material;
        }

    }
}