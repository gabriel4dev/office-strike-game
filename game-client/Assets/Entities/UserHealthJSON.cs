using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Entities
{
    [Serializable]
    public class UserHealthJSON
    {
        public string name { get; set; }
        public int health { get; set; }

        public static UserHealthJSON CreateFromJSON(string pData)
        {
            return JsonUtility.FromJson<UserHealthJSON>(pData);
        }
    }
}
