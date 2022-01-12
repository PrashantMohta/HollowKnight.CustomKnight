using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HutongGames.PlayMaker.Actions;
using static Satchel.GameObjectUtils;
using static Satchel.FsmUtil;

namespace CustomKnight
{
    public class Grimm : Skinable_Tk2d
    {
        public static string NAME = "Grimm";
        public Grimm() : base(Grimm.NAME){}
        public override Material GetMaterial(){
            GameObject hc = HeroController.instance.gameObject;
            GameObject charmEffects = hc.FindGameObjectInChildren("Charm Effects");
            PlayMakerFSM spawnGrimmchild = charmEffects.LocateMyFSM("Spawn Grimmchild");
            GameObject grimm = spawnGrimmchild.GetAction<SpawnObjectFromGlobalPool>("Spawn", 2).gameObject.Value;
            var _grimmMat = grimm.GetComponent<tk2dSprite>().GetCurrentSpriteDef().material;
            
            return _grimmMat;
        }

    }
}