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

        public void Reset(){
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
}