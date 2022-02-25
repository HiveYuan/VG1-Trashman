using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Trashman
{
    public class BarrierClass : MonoBehaviour
    {
        public void Break() {
            Destroy(gameObject);
        }
    }
}
