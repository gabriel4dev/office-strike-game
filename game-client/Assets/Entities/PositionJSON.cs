using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Entities
{
    [Serializable]
    public class PositionJSON
    {
        public float[] position { get; set; }

        public PositionJSON(Vector3 pPosition)
        {
            this.position = new float[] { pPosition.x, pPosition.y, pPosition.z };
        }
    }
}
