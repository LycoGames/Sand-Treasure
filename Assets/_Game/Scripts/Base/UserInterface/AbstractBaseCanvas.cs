using System;
using _Game.Scripts.Enums;
using UnityEngine;


namespace _Game.Scripts.Base.UserInterface
{
    public abstract class AbstractBaseCanvas : MonoBehaviour , IStartable, IQuitable
    {
        public CanvasTypes canvasType;
        
        [SerializeField] private Canvas canvas;
        public delegate void OnUIButtonClickEvent();

        public delegate void OnUIToggleClickEvent(bool isOn);
        public void Start()
        {
        }

        public void Activate()
        {
            canvas.enabled = true;
            OnStart();
        }

        public void Deactivate()
        {
            canvas.enabled = false;
            OnQuit();
        }

        public virtual void OnStart()
        {
        }

        public virtual void OnQuit()
        {
        }
    }
}


