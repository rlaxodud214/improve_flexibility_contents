using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PolyPerfect.City
{
    public class Wagon : MonoBehaviour
    {
        [HideInInspector]
        public int activepoint = 0;
        [HideInInspector]
        public int activePath = 0;
        [HideInInspector]
        public Vector3 target;
        public BoxCollider Collider;
        private void Awake()
        {
            if(Collider == null)
                Collider = GetComponent<BoxCollider>();
        }
    }
    
}
