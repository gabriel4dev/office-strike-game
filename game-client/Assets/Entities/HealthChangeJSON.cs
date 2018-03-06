using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Entities
{
    [Serializable]
    public class HealthChangeJSON
    {
        public string name;
        public int healthChange;
        public string from;
        public bool isEnemy;

        public HealthChangeJSON(string pName, int pHealth, string pFrom, bool pIsEnemy)
        {
            this.name = pName;
            this.healthChange = pHealth;
            this.from = pFrom;
            this.isEnemy = pIsEnemy;
        }
    }
}
