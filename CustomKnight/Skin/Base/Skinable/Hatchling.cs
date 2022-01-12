using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HutongGames.PlayMaker.Actions;
using static Satchel.GameObjectUtils;
using static Satchel.FsmUtil;

namespace CustomKnight
{
    public class Hatchling : Skinable_Tk2d
    {
        public static string NAME = "Hatchling";
        public Hatchling() : base(Hatchling.NAME){}
        public override Material GetMaterial(){
            GameObject hc = HeroController.instance.gameObject;
            GameObject charmEffects = hc.FindGameObjectInChildren("Charm Effects");

            PlayMakerFSM hatchlingSpawn = charmEffects.LocateMyFSM("Hatchling Spawn");
            GameObject hatchling = hatchlingSpawn.GetAction<SpawnObjectFromGlobalPool>("Hatch", 2).gameObject.Value;
            var _wombMat = hatchling.GetComponent<tk2dSprite>().GetCurrentSpriteDef().material;

            return _wombMat;
        }

    }
}