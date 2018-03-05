using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Entities
{
    [Serializable]
    public class PlayerJSON
    {
        public string name { get; set; }
        public List<PointJSON> playerSpawnPoints { get; set; }
        public List<PointJSON> enemySpawnPoints { get; set; }

        public PlayerJSON(string pName, List<SpawnPoint> pPlayerSpawnPoints, List<SpawnPoint> pEnemySpawnPoints)
        {
            this.name = pName;
            this.playerSpawnPoints = new List<PointJSON>();
            this.enemySpawnPoints = new List<PointJSON>();

            foreach(SpawnPoint s in pPlayerSpawnPoints)
            {
                PointJSON p = new PointJSON(s);
                this.playerSpawnPoints.Add(p);
            }
            foreach (SpawnPoint s in pEnemySpawnPoints)
            {
                PointJSON p = new PointJSON(s);
                this.enemySpawnPoints.Add(p);
            }
        }
    }
}
