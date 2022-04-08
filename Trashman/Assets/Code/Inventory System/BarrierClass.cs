using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Trashman
{
    public class BarrierClass : MonoBehaviour
    {
        public enum BarrierType
        {
            security,
            wood
        }

        public void Break() {
            Destroy(gameObject);
        }
    }
}
