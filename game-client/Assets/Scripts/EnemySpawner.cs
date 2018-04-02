using Assets.Entities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
        //this.SpawnEnemies();
	}

    public void SpawnEnemies(EnemiesJSON enemiesJSON)
    {
        foreach (UserJSON enemyJSON in enemiesJSON.enemies)
        {
            if(enemyJSON.health <= 0)
            {
                //TODO spawn healing orb/fruir/soul whatever gabo want xD
                continue;
            }
            Vector3 pos = new Vector3(enemyJSON.position[0], enemyJSON.position[1], enemyJSON.position[2]);
            Quaternion rot = Quaternion.Euler(enemyJSON.rotation[0], enemyJSON.rotation[1], enemyJSON.rotation[2]);

            GameObject newEnemy = Instantiate(this.enemy, pos, rot) as GameObject;

            newEnemy.name = enemyJSON.name;
            PlayerController pc = newEnemy.GetComponent<PlayerController>();
            pc.isLocalPlayer = false;
            Transform t = pc.transform.Find("HealthbarCanvas");
            Transform t1 = t.transform.Find("EnemyName");
            Text vEnemyName = t1.GetComponent<Text>();
            vEnemyName.text = enemyJSON.name;

            Health h = newEnemy.GetComponent<Health>();
            h.currentHealth = enemyJSON.health;
            h.OnChangeHealth();
            h.destroyOnDeath = true;
            h.isEnemy = true;
        }
    }
}
