using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Modding;
using System.Linq;
using Satchel;
using static Modding.Logger;
namespace CustomKnight {
    public static class FixSkinStructure
    {

        public static bool dirHasPng(string sourceDirectory, SearchOption op){
           return IoUtils.DirectoryHas(sourceDirectory, "*.png", op);
        }

        public static void getPngsToRoot(string currentPath, string root){
            try {
                List<string> queue = new List<string>();
                string[] dirs = Directory.GetDirectories(currentPath);
                foreach (string dir in dirs){
                    CustomKnight.Instance.Log("Looking in" + dir);
                    if(dirHasPng(dir,SearchOption.TopDirectoryOnly)){
                        IoUtils.DirectoryCopyAllFiles(dir,root);
                    } else if(dirHasPng(dir,SearchOption.AllDirectories)){
                        queue.Add(dir);
                    }
                }
                foreach(string dir in queue){
                    getPngsToRoot(dir,root);
                }
            } catch (Exception e) {
                CustomKnight.Instance.Log("The Skin could not be fixed : " + e.ToString());
            }
        }
        
        public static void FixSkins(){
            try{
                string[] skinDirectories = Directory.GetDirectories(SkinManager.SKINS_FOLDER);
                foreach (string dir in skinDirectories)
                {
                    if(!dirHasPng(dir,SearchOption.TopDirectoryOnly) && dirHasPng(dir,SearchOption.AllDirectories)){
                        CustomKnight.Instance.Log("A broken skin found! " + dir);
                        getPngsToRoot(dir,dir);
                    }
                }
            } catch (Exception e) {
                CustomKnight.Instance.Log("Failed to fix : "+ e.ToString());
            }

        }


    }
}