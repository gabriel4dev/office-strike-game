using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
    [HideInInspector]
    public GameObject playerFrom;

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("HITTTT");
        var hit = collision.gameObject;
        var health = hit.GetComponent<Health>();
        if(health != null)
        {
            Debug.Log("taking damage");
            health.TakeDamage(this.playerFrom, 10);
        }
        Destroy(gameObject);

    }
}
