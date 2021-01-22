using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Test.AddtiveScene
{
    public static class CanvasManager
    {
        private static List<Canvas> canvasList = new List<Canvas>();

        public static void RegisterCanvas(Canvas canvas)
        {
            RemoveInvalidCanvas();

            if (!canvasList.Contains(canvas))
                canvasList.Add(canvas);

            canvasList.ForEach(t => t.sortingOrder = 0);
            canvas.sortingOrder = 1;
        }

        public static void UnregisterCanvas(Canvas canvas)
        {
            RemoveInvalidCanvas();

            if (canvasList.Contains(canvas))
                canvasList.Remove(canvas);

            if (canvasList.Count > 0)
                canvasList[canvasList.Count - 1].sortingOrder = 1;
        }

        private static void RemoveInvalidCanvas()
        {
            if (canvasList.Count > 0)
            {
                List<Canvas> destroyCanvas = new List<Canvas>();
                canvasList.ForEach(t => { if (t == null) destroyCanvas.Add(t); });
                destroyCanvas.ForEach(t => canvasList.Remove(t));
            }
        }
    }
}
