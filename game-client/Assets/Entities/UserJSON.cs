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
        public string name;
        public float[] position;
        public float[] rotation;
        public int health;
        
        public static UserJSON CreateFromJSON(string pData)
        {
            return JsonUtility.FromJson<UserJSON>(pData);
        }
    }
}
