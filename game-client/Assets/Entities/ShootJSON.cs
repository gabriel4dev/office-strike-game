using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Entities
{
    [Serializable]
    public class ShootJSON
    {
        public string name { get; set; }

        public static ShootJSON CreateFromJSON(string pData)
        {
            return JsonUtility.FromJson<ShootJSON>(pData);
        }
    }
}
