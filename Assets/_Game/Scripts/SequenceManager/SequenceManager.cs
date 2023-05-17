using System.Collections.Generic;
using _Game.Scripts.Base.AppState;
using _Game.Scripts.Base.Singleton;
using _Game.Scripts.Base.StateMachine;
using _Game.Scripts.Enums;
using _Game.Scripts.UI;
using _Game.Scripts.UserInterface;
using UnityEngine;

namespace _Game.Scripts.SequenceManager
{
    public class SequenceManager : AbstractSingleton<SequenceManager>
    {
        [SerializeField] private List<AbstractAppStateCanvas> AppStates;
        [SerializeField] private AppStateTypes startState;

        private readonly StateMachine appStateMachine = new StateMachine();

        private void Start()
        {
            InitializeAllAppStates();
            UIManager.Instance.DisableAllCanvas();
            ChangeState(startState);
        }

        public StateMachine GetAppStateMachine() => appStateMachine;

        public void ChangeState(AppStateTypes appStateType)
        {
            appStateMachine.ChangeState(GetAppState(appStateType));
        }

        public AbstractAppStateCanvas GetAppState(AppStateTypes appStateType)
        {
            foreach (var appState in AppStates)
            {
                if (appState.appStateType == appStateType) return appState;
            }
            Debug.Log("App State Type not found!");
            return null;
        }
        private void InitializeAllAppStates()
        {
            foreach (var appState in AppStates)
            {
                appState.Initialize();
            }
        }
    }
}