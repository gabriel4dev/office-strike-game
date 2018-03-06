using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Entities
{
    class RotationJSON
    {
        public float[] rotation;

        public RotationJSON(Quaternion pQuaternion)
        {
            this.rotation = new float[] { pQuaternion.eulerAngles.x, pQuaternion.eulerAngles.y, pQuaternion.eulerAngles.z };
        }
    }
}
