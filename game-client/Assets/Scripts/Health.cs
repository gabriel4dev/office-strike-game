using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour {
    public const int maxHealth = 100;
    public bool destroyOnDeath;

    public int currentHealth = Health.maxHealth;
    public bool isEnemy = false;

    public RectTransform healthbar;

    private bool isLocalPlayer;

    // Use this for initialization
    void Start () {
        PlayerController vControler = GetComponent<PlayerController>();
        this.isLocalPlayer = vControler.isLocalPlayer;
	}

    public void TakeDamage(GameObject playerFrom, int ammount)
    {
        this.currentHealth -= ammount;
        Debug.Log(this.currentHealth + " de daño");
        //TODO networking stuff (Take Damage)
        this.OnChangeHealth();

    }

    public void OnChangeHealth()
    {
        Debug.Log("health Changing");
        this.healthbar.sizeDelta = new Vector2(this.currentHealth, healthbar.sizeDelta.y);
        if(this.currentHealth <= 0)
        {
            if (this.destroyOnDeath)
            {
                Destroy(gameObject);
            }
            else
            {
                this.currentHealth = Health.maxHealth;
                this.healthbar.sizeDelta = new Vector2(this.currentHealth, healthbar.sizeDelta.y);
                this.Respawn();
            }
        }
    }

    private void Respawn()
    {
        if (this.isLocalPlayer)
        {
            Vector3 spawnPos = Vector3.zero;
            Quaternion spawnRot = Quaternion.Euler(0, 180, 0);
            this.transform.position = spawnPos;
            this.transform.rotation = spawnRot;
        }
    }
}
