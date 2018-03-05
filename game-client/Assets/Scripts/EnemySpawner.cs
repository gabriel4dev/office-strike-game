using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {

    public GameObject enemy;
    public GameObject spawnPoint;
    public int numberOfEnemies;

    [HideInInspector]
    public List<SpawnPoint> enemySpawnPoints;

    // Use this for initialization
	void Start () {
		for(int i = 0; i<this.numberOfEnemies; i++)
        {
            var spawnPos = new Vector3(Random.Range(-8f, 8f), 0f, Random.Range(-8f, 8f));
            var spawnRot = Quaternion.Euler(0f, Random.Range(0, 180), 0f);
            SpawnPoint enemySpawn = (Instantiate(this.spawnPoint, spawnPos, spawnRot) as GameObject).GetComponent<SpawnPoint>();
            this.enemySpawnPoints.Add(enemySpawn);
        }
        this.SpawnEnemies();
	}

    public void SpawnEnemies(/*TODO networking (Spawn new enemies)*/)
    {
        int i = 0;
        foreach(SpawnPoint sp in this.enemySpawnPoints)
        {
            Vector3 pos = sp.transform.position;
            Quaternion rot = sp.transform.rotation;

            GameObject newEnemy = Instantiate(this.enemy, pos, rot) as GameObject;
            newEnemy.name = "Monster_" + i;
            PlayerController pc = newEnemy.GetComponent<PlayerController>();
            pc.isLocalPlayer = false;
            Health h = newEnemy.GetComponent<Health>();
            h.currentHealth = 100;
            h.OnChangeHealth();
            h.destroyOnDeath = true;
            h.isEnemy = true;
            i++;
        }
    }
}
