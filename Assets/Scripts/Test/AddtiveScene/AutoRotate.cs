using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Test.AddtiveScene
{
    public class AutoRotate : MonoBehaviour
    {
        [SerializeField]
        private Vector3 angle;
        [SerializeField, Range(0f, 100f)]
        private float speedRate;

        private void Start()
        {

        }
        private void Update()
        {
            this.transform.Rotate(this.angle * this.speedRate);
        }
    }
}
