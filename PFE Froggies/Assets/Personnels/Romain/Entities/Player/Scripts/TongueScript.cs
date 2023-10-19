using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UltimateAttributesPack;

namespace Romain
{
    public class TongueScript : MonoBehaviour
    {
        [SerializeField] Transform startTonguePoint;
        [SerializeField] Transform endTonguePoint;

        [SerializeField] float maxTongueLenght;

        [SerializeField] float outTongueTime;
        [SerializeField] AnimationCurve outTongueCurve;

        [SerializeField] float inTongueTime;
        [SerializeField] AnimationCurve inTongueCurve;

        float outTongueTimer, inTongueTimer;




        void Update()
        {
 
        }

        private void FixedUpdate()
        {
  
        }



    }
}
