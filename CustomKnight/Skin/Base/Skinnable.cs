using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CustomKnight
{   
    public abstract class Skinnable
    {
        public string name;

        public Skinnable(string name){
            this.name = name;
        }
        public CustomKnightTexture _ckTex;
        public CustomKnightTexture ckTex {
             get{ 
                if(_ckTex == null){
                    _ckTex = new CustomKnightTexture(name + ".png", false, null, null);
                }
                return _ckTex;
            } 
            set{
                _ckTex = value;
            }
        }

        public abstract void SaveDefaultTexture();
        public abstract void ApplyTexture(Texture2D tex);

        public void SaveTexture(){
            Modding.Logger.Log($"SaveTexture skinnable {name}");
            SaveDefaultTexture();
        }

        public void Apply(){
            Modding.Logger.Log($"Apply skinnable {name}");
            ApplyTexture(ckTex.currentTexture);
        }

        public virtual void Reset(){
            Modding.Logger.Log($"Reset skinnable {name}");
            ApplyTexture(ckTex.defaultTex);
        }
        

    }
    
    public abstract class Skinnable_Single : Skinnable
    {
        public Skinnable_Single(string name) : base(name){}
        public Material _material;
        public Material material {
            get{ 
                if(_material == null){
                    try{
                        _material = GetMaterial();
                    } catch (Exception e){
                        CustomKnight.Instance.Log(e.ToString());
                    }
                }
                return _material;
            } 
            set{
                _material = value;
            }
        }

        public abstract Material GetMaterial();

    }
    public abstract class Skinnable_Multiple : Skinnable
    {

        public Skinnable_Multiple(string name) : base(name){}
        public List<Material> _materials;
        public List<Material> materials {
            get{ 
                if(_materials == null){
                    try{
                        _materials = GetMaterials();
                    } catch (Exception e){
                        CustomKnight.Instance.Log(e.ToString());
                    }
                }
                return _materials;
            } 
            set{
                _materials = value;
            }
        }

        public abstract List<Material> GetMaterials();
    }
    public abstract class Skinnable_Sprite : Skinnable
    {
        public Skinnable_Sprite(string name) : base(name){}

        public override void ApplyTexture(Texture2D tex){
            if(!ckTex.missing){
                ApplySprite(Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f)));
            } else {
                Modding.Logger.Log($"Missing Sprite for skinnable {name}");
                Reset();
            }
        }
        public abstract void ApplySprite(Sprite sprite);
        public override void Reset(){
            Modding.Logger.Log($"Reset skinnable {name}");
            ApplySprite(ckTex.defaultSprite);
        }
    }

    public abstract class Skinnable_Tk2d : Skinnable_Single
    {
        public Skinnable_Tk2d(string name) : base(name){}

        public override void SaveDefaultTexture(){
            if(material != null && material.mainTexture != null){
                ckTex.defaultTex = material.mainTexture as Texture2D;
            } else {
                Modding.Logger.Log($"skinnable {name} : material is null");
            }
        }
        public override void ApplyTexture(Texture2D tex){
            if(ckTex.defaultTex == null){
                //incase we do not have the default texture save it.
                ckTex.defaultTex = material.mainTexture as Texture2D;
            }
            material.mainTexture = tex;
        }
    }

    public abstract class Skinnable_Tk2ds : Skinnable_Multiple
    {
        public Skinnable_Tk2ds(string name) : base(name){}

        public override void SaveDefaultTexture(){
            if(materials != null && materials[0].mainTexture != null){
                ckTex.defaultTex = materials[0].mainTexture as Texture2D;
            } else {
                Modding.Logger.Log($"skinnable {name} : material is null");
            }
        }
        public override void ApplyTexture(Texture2D tex){
            foreach(var mat in materials){
                mat.mainTexture = tex;
            }
        }
    }

}