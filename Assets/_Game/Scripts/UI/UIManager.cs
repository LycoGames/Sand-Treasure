using System.Collections.Generic;
using _Game.Scripts.Base.Singleton;
using _Game.Scripts.Base.UserInterface;
using _Game.Scripts.Enums;
using UnityEngine;

namespace _Game.Scripts.UI
{
    public class UIManager : AbstractSingleton<UIManager> 
    {
        [SerializeField] private List<AbstractBaseCanvas> canvases;

        public void Initialize()
        {
            DisableAllCanvas();
        }
        public AbstractBaseCanvas GetCanvas(CanvasTypes canvasType)
        {
            foreach (var canvas in canvases)
            {
                if (canvas.canvasType == canvasType) return canvas;
            }
            Debug.Log("Canvas not found!");
            return null;
        }

        public void EnableCanvas(CanvasTypes canvasType)
        {
            AbstractBaseCanvas selectedCanvas=GetCanvas(canvasType);
            if(selectedCanvas)selectedCanvas.Activate();
        }

        public void DisableCanvas(CanvasTypes canvasType)
        {
            AbstractBaseCanvas selectedCanvas=GetCanvas(canvasType);
            if(selectedCanvas)selectedCanvas.Deactivate();
        }

        public void DisableAllCanvas()
        {
            foreach (var canvas in canvases)
            {
                canvas.Deactivate();
            }
        }
    }
}
