using _Game.Scripts.Base.StateMachine;
using _Game.Scripts.Enums;
using UnityEngine;

namespace _Game.Scripts.Base.AppState
{
    public abstract class AbstractAppStateCanvas : MonoBehaviour , IStateCanvas
    {
        public AppStateTypes appStateType;

        public virtual void Initialize()
        {
            
        }
        public virtual void Enter()
        {
            
        }

        public virtual void Exit()
        {
            
        }
    }
}
