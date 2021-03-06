﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Entities
{
    [Serializable]
    public class EnemiesJSON
    {
        public List<UserJSON> enemies;
        
        public static EnemiesJSON CreateFromJSON(string pData)
        {
            return JsonUtility.FromJson<EnemiesJSON>(pData);
        }
    }
}
