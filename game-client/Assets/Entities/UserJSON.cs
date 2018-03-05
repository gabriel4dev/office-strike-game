using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Entities
{
    [Serializable]
    public class UserJSON
    {
        public string name { get; set; }
        public float[] position { get; set; }
        public float[] rotation { get; set; }
        public int health { get; set; }
        
        public static UserJSON CreateFromJSON(string pData)
        {
            return JsonUtility.FromJson<UserJSON>(pData);
        }
    }
}
