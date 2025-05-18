using System.Linq;
using System.Security.Policy;
using HutongGames.PlayMaker;
using Satchel;
using Satchel.Futils;
using UnityEngine;

namespace CustomKnight
{
    internal class Charm : Skinable_Sprite
    {
        public int charmNum;
        public string charmName;
        private Sprite currentSprite;
        public Charm(string charmName, int charmNum) : base("Charms/" + charmName)
        {
            this.charmName = charmName;
            this.charmNum = charmNum;
            CustomKnight.OnInit += CustomKnight_OnInit;
            CustomKnight.OnUnload += CustomKnight_OnUnload;
        }

        private void CustomKnight_OnUnload(object sender, EventArgs e)
        {
            CustomKnight.Instance.LogDebug($"UnHook {name}");
            On.CharmDisplay.Start -= CharmDisplay_Start;
            ModHooks.LanguageGetHook -= ModHooks_LanguageGetHook;
        }


        private void CustomKnight_OnInit(object sender, EventArgs e)
        {
            CustomKnight.Instance.LogDebug($"Hook {name}");
            On.CharmDisplay.Start += CharmDisplay_Start; 
            ModHooks.LanguageGetHook += ModHooks_LanguageGetHook;

        }

        private string ModHooks_LanguageGetHook(string key, string sheetTitle, string orig)
        {
            if (sheetTitle == "UI" && key == "CHARM_NAME_36")
            {
                return "Silksong Unlocker";
            }
            if (sheetTitle == "UI" && key == "CHARM_DESC_36")
            {
                return "Custom Knight Says Hi!";
            }
            return orig;
        }
        private void CharmDisplay_Start(On.CharmDisplay.orig_Start orig, CharmDisplay charmDisplay)
        {
            if (currentSprite != null)
            {
                switch (charmName)
                {
                    case "Charm_23_Broken":
                        charmDisplay.brokenGlassHP = currentSprite;
                        break;
                    case "Charm_24_Broken":
                        charmDisplay.brokenGlassGeo = currentSprite;
                        break;
                    case "Charm_25_Broken":
                        charmDisplay.brokenGlassAttack = currentSprite;
                        break;
                    case "Charm_36_Full":
                        charmDisplay.whiteCharm = currentSprite;
                        break;
                    case "Charm_36_Black":
                        charmDisplay.blackCharm = currentSprite;
                        break;
                }
            }
            orig(charmDisplay);
        }

        public override void SaveDefaultTexture()
        {
            try
            {
                PlayMakerFSM charmShowIfCollected;
                switch (charmName)
                {

                    case "Charm_23_Broken":
                        charmShowIfCollected = GameCameras.instance.hudCamera.gameObject.FindGameObjectInChildren(charmNum.ToString()).LocateMyFSM("charm_show_if_collected");
                        ckTex.defaultSprite = charmShowIfCollected.GetAction<SetSpriteRendererSprite>("Glass HP", 2).sprite.Value as Sprite;
                        break;

                    case "Charm_24_Broken":
                        charmShowIfCollected = GameCameras.instance.hudCamera.gameObject.FindGameObjectInChildren(charmNum.ToString()).LocateMyFSM("charm_show_if_collected");
                        ckTex.defaultSprite = charmShowIfCollected.GetAction<SetSpriteRendererSprite>("Glass Geo", 2).sprite.Value as Sprite;
                        break;

                    case "Charm_25_Broken":
                        charmShowIfCollected = GameCameras.instance.hudCamera.gameObject.FindGameObjectInChildren(charmNum.ToString()).LocateMyFSM("charm_show_if_collected");
                        ckTex.defaultSprite = charmShowIfCollected.GetAction<SetSpriteRendererSprite>("Glass Attack", 2).sprite.Value as Sprite;
                        break;

                    case "Charm_23_Fragile":
                        ckTex.defaultSprite = CharmIconList.Instance.spriteList[charmNum];
                        break;

                    case "Charm_23_Unbreakable":
                        ckTex.defaultSprite = CharmIconList.Instance.unbreakableHeart;
                        break;

                    case "Charm_24_Fragile":
                        ckTex.defaultSprite = CharmIconList.Instance.spriteList[charmNum];
                        break;

                    case "Charm_24_Unbreakable":
                        ckTex.defaultSprite = CharmIconList.Instance.unbreakableGreed;
                        break;

                    case "Charm_25_Fragile":
                        ckTex.defaultSprite = CharmIconList.Instance.spriteList[charmNum];
                        break;

                    case "Charm_25_Unbreakable":
                        ckTex.defaultSprite = CharmIconList.Instance.unbreakableStrength;
                        break;

                    case "Charm_36_Left":
                        charmShowIfCollected = GameCameras.instance.hudCamera.gameObject.FindGameObjectInChildren(charmNum.ToString()).LocateMyFSM("charm_show_if_collected");
                        ckTex.defaultSprite = charmShowIfCollected.GetAction<SetSpriteRendererSprite>("R Queen", 0).sprite.Value as Sprite;
                        break;
                    case "Charm_36_Right":
                        charmShowIfCollected = GameCameras.instance.hudCamera.gameObject.FindGameObjectInChildren(charmNum.ToString()).LocateMyFSM("charm_show_if_collected");
                        ckTex.defaultSprite = charmShowIfCollected.GetAction<SetSpriteRendererSprite>("R King", 0).sprite.Value as Sprite;
                        break;
                    case "Charm_36_Full":
                        charmShowIfCollected = GameCameras.instance.hudCamera.gameObject.FindGameObjectInChildren(charmNum.ToString()).LocateMyFSM("charm_show_if_collected");
                        ckTex.defaultSprite = charmShowIfCollected.GetAction<SetSpriteRendererSprite>("R Final", 0).sprite.Value as Sprite;
                        break;
                    case "Charm_36_Black":
                        charmShowIfCollected = GameCameras.instance.hudCamera.gameObject.FindGameObjectInChildren(charmNum.ToString()).LocateMyFSM("charm_show_if_collected");
                        ckTex.defaultSprite = charmShowIfCollected.GetAction<SetSpriteRendererSprite>("R Shade", 0).sprite.Value as Sprite;
                        break;
                    case "Charm_40_1":
                        ckTex.defaultSprite = CharmIconList.Instance.grimmchildLevel1;
                        break;
                    case "Charm_40_2":
                        ckTex.defaultSprite = CharmIconList.Instance.grimmchildLevel2;
                        break;
                    case "Charm_40_3":
                        ckTex.defaultSprite = CharmIconList.Instance.grimmchildLevel3;
                        break;
                    case "Charm_40_4":
                        ckTex.defaultSprite = CharmIconList.Instance.grimmchildLevel4;
                        break;
                    case "Charm_40_5":
                        ckTex.defaultSprite = CharmIconList.Instance.nymmCharm;
                        break;
                    default:
                        ckTex.defaultSprite = CharmIconList.Instance.spriteList[charmNum];
                        break;
                }
            }
            catch (Exception e)
            {
                CustomKnight.Instance.Log($"skinable {name} : {e}");
            }
        }
        public override void ApplySprite(Sprite sprite)
        {
            PlayMakerFSM charmShowIfCollected;
            PlayMakerFSM updateSprite  = GameCameras.instance.hudCamera.gameObject.FindGameObjectInChildren("Detail Sprite").LocateMyFSM("Update Sprite");
            currentSprite = sprite;
            switch (charmName)
            {
                case "Charm_23_Fragile":
                    CharmIconList.Instance.spriteList[charmNum] = sprite;
                    break;

                case "Charm_23_Broken":
                    charmShowIfCollected = GameCameras.instance.hudCamera.gameObject.FindGameObjectInChildren(charmNum.ToString()).LocateMyFSM("charm_show_if_collected");
                    charmShowIfCollected.GetAction<SetSpriteRendererSprite>("Glass HP", 2).sprite.Value = sprite;
                    updateSprite.GetAction<SetSpriteRendererSprite>("Glass HP", 2).sprite.Value = sprite;
                    break;

                case "Charm_23_Unbreakable":
                    CharmIconList.Instance.unbreakableHeart = sprite;
                    break;
                case "Charm_24_Fragile":
                    CharmIconList.Instance.spriteList[charmNum] = sprite;
                    break;

                case "Charm_24_Broken":
                    charmShowIfCollected = GameCameras.instance.hudCamera.gameObject.FindGameObjectInChildren(charmNum.ToString()).LocateMyFSM("charm_show_if_collected");
                    charmShowIfCollected.GetAction<SetSpriteRendererSprite>("Glass Geo", 2).sprite.Value = sprite;
                    updateSprite.GetAction<SetSpriteRendererSprite>("Glass Geo", 2).sprite.Value = sprite;
                    break;

                case "Charm_24_Unbreakable":
                    CharmIconList.Instance.unbreakableGreed = sprite;
                    break;

                case "Charm_25_Fragile":
                    CharmIconList.Instance.spriteList[charmNum] = sprite;
                    break;

                case "Charm_25_Broken":
                    charmShowIfCollected = GameCameras.instance.hudCamera.gameObject.FindGameObjectInChildren(charmNum.ToString()).LocateMyFSM("charm_show_if_collected");
                    charmShowIfCollected.GetAction<SetSpriteRendererSprite>("Glass Attack", 2).sprite.Value = sprite;
                    updateSprite.GetAction<SetSpriteRendererSprite>("Glass Attack", 2).sprite.Value = sprite;
                    break;

                case "Charm_25_Unbreakable":
                    CharmIconList.Instance.unbreakableStrength = sprite;
                    break;

                case "Charm_36_Left":
                    charmShowIfCollected = GameCameras.instance.hudCamera.gameObject.FindGameObjectInChildren(charmNum.ToString()).LocateMyFSM("charm_show_if_collected");
                    charmShowIfCollected.GetAction<SetSpriteRendererSprite>("R Queen", 0).sprite.Value = sprite;
                    updateSprite.GetAction<SetSpriteRendererSprite>("R Queen", 0).sprite.Value = sprite;
                    break;

                case "Charm_36_Right":
                    charmShowIfCollected = GameCameras.instance.hudCamera.gameObject.FindGameObjectInChildren(charmNum.ToString()).LocateMyFSM("charm_show_if_collected");
                    charmShowIfCollected.GetAction<SetSpriteRendererSprite>("R King", 0).sprite.Value = sprite;
                    updateSprite.GetAction<SetSpriteRendererSprite>("R King", 0).sprite.Value = sprite;
                    break;

                case "Charm_36_Full":
                    charmShowIfCollected = GameCameras.instance.hudCamera.gameObject.FindGameObjectInChildren(charmNum.ToString()).LocateMyFSM("charm_show_if_collected");
                    charmShowIfCollected.GetAction<SetSpriteRendererSprite>("R Final", 0).sprite.Value = sprite;
                    updateSprite.GetAction<SetSpriteRendererSprite>("R Final", 0).sprite.Value = sprite;
                    break;

                case "Charm_36_Black":
                    charmShowIfCollected = GameCameras.instance.hudCamera.gameObject.FindGameObjectInChildren(charmNum.ToString()).LocateMyFSM("charm_show_if_collected");
                    charmShowIfCollected.GetAction<SetSpriteRendererSprite>("R Shade", 0).sprite.Value = sprite;
                    updateSprite.GetAction<SetSpriteRendererSprite>("R Shade", 0).sprite.Value = sprite;
                    break;

                case "Charm_40_1":
                    CharmIconList.Instance.grimmchildLevel1 = sprite;
                    break;

                case "Charm_40_2":
                    CharmIconList.Instance.grimmchildLevel2 = sprite;
                    break;

                case "Charm_40_3":
                    CharmIconList.Instance.grimmchildLevel3 = sprite;
                    break;

                case "Charm_40_4":
                    CharmIconList.Instance.grimmchildLevel4 = sprite;
                    break;

                case "Charm_40_5":
                    CharmIconList.Instance.nymmCharm = sprite;
                    break;
                case "Charm_0":
                    charmShowIfCollected = GameCameras.instance.hudCamera.gameObject.FindGameObjectInChildren("36").LocateMyFSM("charm_show_if_collected");
                    var clone = new FsmObject();
                    clone.Value = sprite;
                    charmShowIfCollected.GetAction<SetSpriteRendererSprite>("Check", 7).sprite = clone;
                    updateSprite.GetAction<SetSpriteRendererSprite>("Update", 3).sprite = clone;

                    CharmIconList.Instance.spriteList[charmNum] = sprite;
                    break; 
                default:
                    CharmIconList.Instance.spriteList[charmNum] = sprite;
                    break;
            }
        }

    }
}