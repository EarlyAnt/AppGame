using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ABLoader
{
    public class ResetList : MonoBehaviour
    {
        [SerializeField, Range(0.01f, 5f)]
        private float space;

        [ContextMenu("排列顺序")]
        private void Sort()
        {
            for (int i = 0; i < this.transform.childCount; i++)
            {
                Vector3 localPosition = this.transform.GetChild(i).localPosition;
                localPosition.x = i * this.space;
                this.transform.GetChild(i).localPosition = localPosition;
                Debug.LogFormat("<><ResetList.Sort>localPosition: {0}", localPosition);
            }
        }
    }
}