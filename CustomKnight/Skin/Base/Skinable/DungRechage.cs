using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Satchel.GameObjectUtils;
using static Satchel.FsmUtil;
using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;

namespace CustomKnight
{
    public class DungRecharge : Skinable_Multiple
    {
        public static string NAME = "DungRecharge";
        public DungRecharge() : base(DungRecharge.NAME){}
        public override List<Material> GetMaterials(){
            var HC = HeroController.instance.gameObject;
            var Dung = HC.FindGameObjectInChildren("Dung");
            return new List<Material>{
                Dung.FindGameObjectInChildren("Particle 1").GetComponent<ParticleSystemRenderer>().material,
                HC.FindGameObjectInChildren("Dung Recharge").GetComponent<ParticleSystemRenderer>().material
            };
        }
        public override void SaveDefaultTexture(){
            if(materials != null && materials[0].mainTexture != null){
                ckTex.defaultTex = materials[0].mainTexture as Texture2D;
            } else {
                Modding.Logger.Log($"skinable {name} : material is null");
            }
        }

        public override void ApplyTexture(Texture2D tex){
            foreach(var mat in materials){
                mat.mainTexture = tex;
            }
            var HC = HeroController.instance.gameObject;
            // basic dung trail
            var action = HC.FindGameObjectInChildren("Dung").LocateMyFSM("Control").GetAction<SpawnObjectFromGlobalPoolOverTime>("Equipped",0);
            var prefab = GameObject.Instantiate(action.gameObject.Value);
            UnityEngine.Object.DontDestroyOnLoad(prefab);
            prefab.SetActive(false);
            prefab.FindGameObjectInChildren("Pt Normal").GetComponent<ParticleSystemRenderer>().material.mainTexture = tex;
            action.gameObject.Value = prefab;
            
            // dung cloud for spore shroom
            var action2 = HC.LocateMyFSM("Spell Control").GetAction<SpawnObjectFromGlobalPool>("Dung Cloud",0);
            var prefab2 = GameObject.Instantiate(action2.gameObject.Value);
            UnityEngine.Object.DontDestroyOnLoad(prefab2);
            prefab2.SetActive(false);
            prefab2.FindGameObjectInChildren("Pt Deep").GetComponent<ParticleSystemRenderer>().material.mainTexture = tex;
            prefab2.FindGameObjectInChildren("Pt Normal").GetComponent<ParticleSystemRenderer>().material.mainTexture = tex;
            action2.gameObject.Value = prefab2;
        }
    }
}