using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Satchel.GameObjectUtils;

namespace CustomKnight
{
    public class SDCrystalBurst : Skinable_Tk2ds
    {
        public static string NAME = "SDCrystalBurst";
        public SDCrystalBurst() : base(SDCrystalBurst.NAME){}

        public override List<Material> GetMaterials(){
            GameObject hc = HeroController.instance.gameObject;
            return new List<Material>{
                hc.FindGameObjectInChildren("SD Crystal Burst GL").GetComponent<ParticleSystemRenderer>().material,
                hc.FindGameObjectInChildren("SD Crystal Burst GR").GetComponent<ParticleSystemRenderer>().material,
                hc.FindGameObjectInChildren("SD Crystal Burst W").GetComponent<ParticleSystemRenderer>().material
            };
        }

    }
}