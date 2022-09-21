using HutongGames.PlayMaker;
namespace CustomKnight
{
    public static class FSMExtension
    {
        public static T GetFirstAction<T>(this PlayMakerFSM fsm, string stateName) where T : FsmStateAction//Hollow Point Change Hatchling fsm
        {
            foreach(var act in fsm.GetState(stateName).Actions)
            {
                if(act is T)
                {
                    return act as T;
                }
            }
            return null;
        }
    }
}
