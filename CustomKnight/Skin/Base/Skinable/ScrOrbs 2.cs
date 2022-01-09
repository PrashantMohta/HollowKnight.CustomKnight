using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Satchel.GameObjectUtils;

namespace CustomKnight
{
    public class ScrOrbs2 : Skinable_Tk2d
    {
        public static string NAME = "ScrOrbs2";
        public ScrOrbs2() : base(ScrOrbs2.NAME){}
        public override Material GetMaterial(){
            return HeroController.instance.gameObject.FindGameObjectInChildren("Scr Orbs 2").GetComponent<ParticleSystemRenderer>().material;
        }

    }
}