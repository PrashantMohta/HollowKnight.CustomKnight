
using Modding;
using UnityEngine;
using CustomKnight;
using System;

namespace AsymmetricalKnight{
    public class Asymmetrical {
        public string name;
        public Texture2D leftSkin,rightSkin;

        public Asymmetrical(string name){
            this.name = name;
        }

        public Texture2D GetTexture(ISelectableSkin skin, bool isLeft){
            if(rightSkin == null){
                rightSkin = skin.Exists($"{name}.png") ? skin.GetTexture($"{name}.png") : null;
            }
            if(leftSkin == null){
                leftSkin = skin.Exists($"{name}_left.png") ? skin.GetTexture($"{name}_left.png") : rightSkin;
            }
            if(isLeft){
                return leftSkin;
            } else {
                return rightSkin;
            }
        }

 
    }
    public class AsymmetricalKnight : Mod {
        new public string GetName() => "Asymmetrical Knight";
        public override string GetVersion() => "v1";
        public void touchCustomKnightName(){ // will throw if no ck installed
                CustomKnight.CustomKnight.Instance.GetName();
        }
        public bool isCustomKnightInstalled(){
            var installed = true;
            try{
                touchCustomKnightName();
            } catch(Exception e){
                installed = false;
            }
            return installed;
        }
        public void AddCustomKnightHandlers(){
                SkinManager.OnSetSkin += (_,e) => {
                    var skin = SkinManager.GetCurrentSkin();
                    var currDirIsLeft = HeroController.instance.transform.localScale.x < 0;

                    if(lastSkin != skin.GetId()){
                        Knight = new Asymmetrical(CustomKnight.Knight.NAME);
                        Sprint = new Asymmetrical(CustomKnight.Sprint.NAME);
                        Unn = new Asymmetrical(CustomKnight.Unn.NAME);
                        Knight.GetTexture(skin,currDirIsLeft);
                        Unn.GetTexture(skin,currDirIsLeft);
                        Sprint.GetTexture(skin,currDirIsLeft);
                        lastSkin = skin.GetId();
                    }
                };
        }
        public override void Initialize()
        {
            if(isCustomKnightInstalled()){ //do not do anything with ck if ck is not installed
                ModHooks.HeroUpdateHook +=  UpdateSkin;
                AddCustomKnightHandlers();
            } else {
                Log("Error : Custom Knight not found");
            }
        }

        Asymmetrical Knight;
        Asymmetrical Sprint;
        Asymmetrical Unn;
        string lastSkin = "";
        bool lastDirWasLeft = false;
        public void UpdateSkin(){
            var skin = SkinManager.GetCurrentSkin();
            var currDirIsLeft = HeroController.instance.transform.localScale.x > 0;

            if(lastSkin != skin.GetId()){
                Knight = new Asymmetrical(CustomKnight.Knight.NAME);
                Sprint = new Asymmetrical(CustomKnight.Sprint.NAME);
                Unn = new Asymmetrical(CustomKnight.Unn.NAME);
                Knight.GetTexture(skin,currDirIsLeft);
                Unn.GetTexture(skin,currDirIsLeft);
                Sprint.GetTexture(skin,currDirIsLeft);
                lastSkin = skin.GetId();
            }

            if(currDirIsLeft != lastDirWasLeft){
                var knight = Knight.GetTexture(skin,currDirIsLeft);
                var unn = Unn.GetTexture(skin,currDirIsLeft);
                var sprint = Sprint.GetTexture(skin,currDirIsLeft);
                if(knight != null){
                    SkinManager.Skinables[CustomKnight.Knight.NAME].ApplyTexture(knight);
                }
                if(sprint != null){
                    SkinManager.Skinables[CustomKnight.Sprint.NAME].ApplyTexture(sprint);
                }
                if(unn != null){
                    SkinManager.Skinables[CustomKnight.Unn.NAME].ApplyTexture(unn);
                }
                lastDirWasLeft = currDirIsLeft;
            }
        }

    }
}
