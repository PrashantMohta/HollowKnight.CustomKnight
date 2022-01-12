using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Satchel.GameObjectUtils;

namespace CustomKnight
{
    public class Geo : Skinable_Tk2d
    {
        public static string NAME = "Geo";
        public Geo() : base(Geo.NAME){}

        public void GeoControl_Start(On.GeoControl.orig_Start orig, GeoControl self)
        {
            material = self.GetComponent<tk2dSprite>().GetCurrentSpriteDef().material;
            
            //save default texture because we dont have a copy
            if(ckTex.defaultTex == null){
                ckTex.defaultTex  = (Texture2D)material.mainTexture;
            }
            var geoTexture = ckTex.currentTexture;
            if (geoTexture != null  && material != null)
            {
               material.mainTexture = geoTexture;
            }
            On.GeoControl.Start -= GeoControl_Start;
            orig(self);
        }

        public override Material GetMaterial(){
            return material;
        }

    }
}