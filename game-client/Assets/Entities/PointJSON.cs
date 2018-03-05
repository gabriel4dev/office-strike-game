using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Entities
{
    [Serializable]
    public class PointJSON
    {
        public float[] position { get; set; }
        public float[] rotation { get; set; }
        public PointJSON(SpawnPoint pSpawnPoint)
        {
            this.position = new float[] { pSpawnPoint.transform.position.x, pSpawnPoint.transform.position.y, pSpawnPoint.transform.position.z };
            this.rotation = new float[] { pSpawnPoint.transform.eulerAngles.x, pSpawnPoint.transform.eulerAngles.y, pSpawnPoint.transform.eulerAngles.z };
        }
    }
}
