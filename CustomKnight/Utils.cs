using System;
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
        public static FsmStateAction GetAction(this PlayMakerFSM fsm, string stateName, int index) =>
            Utils._GetAction(fsm, stateName, index);

        public static T GetAction<T>(this PlayMakerFSM fsm, string stateName, int index) where T : FsmStateAction =>
            Utils._GetAction<T>(fsm, stateName, index); 

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
    }
}