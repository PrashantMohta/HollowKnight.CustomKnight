using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;

namespace CustomKnight{
    public static class Utils {

        public static FsmStateAction _GetAction(PlayMakerFSM fsm, string stateName, int index)
        {
            foreach (FsmState t in fsm.FsmStates)
            {
                if (t.Name != stateName) continue;
                FsmStateAction[] actions = t.Actions;

                Array.Resize(ref actions, actions.Length + 1);

                return actions[index];
            }

            return null;
        }

        public static T _GetAction<T>(PlayMakerFSM fsm, string stateName, int index) where T : FsmStateAction
        {
            return GetAction(fsm, stateName, index) as T;
        }
        public static FsmStateAction GetAction(this PlayMakerFSM fsm, string stateName, int index) {
            return Utils._GetAction(fsm, stateName, index);
        }

        public static T GetAction<T>(this PlayMakerFSM fsm, string stateName, int index) where T : FsmStateAction {
            return Utils._GetAction<T>(fsm, stateName, index); 
        }

        public static GameObject FindGameObjectInChildren( this GameObject gameObject, string name )
        {
            if( gameObject == null )
                return null;

            foreach( var t in gameObject.GetComponentsInChildren<Transform>( true ) )
            {
                if( t.name == name )
                    return t.gameObject;
            }
            return null;
        }
    
    
        public static Texture2D duplicateTexture(Texture2D source)
        {
            RenderTexture renderTex = RenderTexture.GetTemporary(
                        source.width,
                        source.height,
                        0,
                        RenderTextureFormat.Default,
                        RenderTextureReadWrite.Linear);

            Graphics.Blit(source, renderTex);
            RenderTexture previous = RenderTexture.active;
            RenderTexture.active = renderTex;
            Texture2D readableText = new Texture2D(source.width, source.height);
            readableText.ReadPixels(new Rect(0, 0, renderTex.width, renderTex.height), 0, 0);
            readableText.Apply();
            RenderTexture.active = previous;
            RenderTexture.ReleaseTemporary(renderTex);
            return readableText;
        }
        public static void DumpTexture(Texture t,string name){
            File.WriteAllBytes(
                (SkinManager.DATA_DIR + "/" + SkinManager.SKIN_FOLDER + "/" + name +".png").Replace("\\", "/"),
                duplicateTexture((Texture2D)t).EncodeToPNG());
        }
        public static Texture2D LoadTexture(string name){
            byte[] texBytes = File.ReadAllBytes((SkinManager.DATA_DIR + "/" + SkinManager.SKIN_FOLDER + "/" + name +".png").Replace("\\", "/"));
            
            var tex = new Texture2D(2, 2);
            tex.LoadImage(texBytes);
            return tex;
        }
        public static void dumpAll(){
            return;
            foreach(KeyValuePair<string, CustomKnightTexture> entry in SkinManager.Textures){
                if(entry.Value == null || (entry.Value.defaultTex == null && entry.Value.defaultCharmSprite == null)){
                    continue;
                }
                CustomKnight.Instance.Log(entry.Key);
                if(entry.Value.defaultTex != null)
                    DumpTexture(entry.Value.defaultTex,entry.Key);
                
                if(entry.Value.defaultCharmSprite != null)
                    DumpTexture(entry.Value.defaultCharmSprite.texture,entry.Key);
            }
        }
        public static void dumpfunction(){
            return;
            var x2 = HeroController.instance.gameObject.transform.GetComponentsInChildren<SpriteRenderer>();
            foreach(var i in x2){
                CustomKnight.Instance.Log("hero controller tk2dSprite" + i.gameObject.name);
                DumpTexture(i.sprite.texture,i.gameObject.name);
            }
            
            try{
                    DumpTexture(CharmIconList.Instance.spriteList[1].texture,"charmed");
            } catch (Exception e){
                CustomKnight.Instance.Log("error");
            }
            var x = HeroController.instance.gameObject.transform.GetComponentsInChildren<ParticleSystemRenderer>();
            foreach(var i in x){
                CustomKnight.Instance.Log("hero controller particles" + i.gameObject.name);
                DumpTexture(i.material.mainTexture,i.gameObject.name);
            }
            var x1 = HeroController.instance.gameObject.transform.GetComponentsInChildren<tk2dSprite>();
            foreach(var i in x1){
                CustomKnight.Instance.Log("hero controller tk2dSprite" + i.gameObject.name);
                DumpTexture(i.GetCurrentSpriteDef().material.mainTexture,i.gameObject.name);
            }
            foreach (tk2dSprite i in GameCameras.instance.hudCanvas.GetComponentsInChildren<tk2dSprite>())
            {
                CustomKnight.Instance.Log("tk2d " + i.gameObject.name);
                DumpTexture(i.GetCurrentSpriteDef().material.mainTexture,i.gameObject.name);
                if(i.name == "Vessel 2"){
                    CustomKnight.Instance.Log("found vessel");
                    i.GetCurrentSpriteDef().material.mainTexture = LoadTexture("Vessel 2");
                }
            }
            foreach (SpriteRenderer i in GameCameras.instance.hudCanvas.GetComponentsInChildren<SpriteRenderer>(true))
            {
                CustomKnight.Instance.Log("spriteRenderer " +i.gameObject.name);
                DumpTexture(i.sprite.texture,i.gameObject.name);
            }
        }
    }
}