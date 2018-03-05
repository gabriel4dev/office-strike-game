using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public GameObject bulletPrefab;
    public Transform bulletSpawn;
    public bool isLocalPlayer = true; //TODO set back to false when networking

    private Vector3 oldPosition;
    private Vector3 currentPosition;
    private Quaternion oldRotation;
    private Quaternion currentRotation;


    // Use this for initialization
    void Start () {
        this.oldPosition = this.transform.position;
        this.currentPosition = this.oldPosition;
        this.oldRotation = this.transform.rotation;
        this.currentRotation = this.oldRotation;
	}
	
	// Update is called once per frame
	void Update () {
        if (!isLocalPlayer)
        {
            return;
        }
        var x = Input.GetAxis("Horizontal") * Time.deltaTime * 150.0f;
        var z = Input.GetAxis("Vertical") * Time.deltaTime * 3.0f;

        transform.Rotate(0, x, 0);
        transform.Translate(0, 0, z);

        this.currentPosition = this.transform.position;
        this.currentRotation = this.transform.rotation;

        if(this.currentPosition != this.oldPosition)
        {
            //TODO networking stuff (transform)
            this.oldPosition = this.currentPosition;
        }
        if(this.currentRotation != this.oldRotation)
        {
            //TODO networking stuff (rotation)
            this.oldRotation = this.currentRotation;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //TODO networking stuff (shooting)
            this.GunFire();
        }
	}

    public void GunFire()
    {
        var bullet = Instantiate(this.bulletPrefab, this.bulletSpawn.position, this.bulletSpawn.rotation) as GameObject;
        Bullet b = bullet.GetComponent<Bullet>();
        b.playerFrom = this.gameObject;
        bullet.GetComponent<Rigidbody>().velocity = bullet.transform.up * 6;

        Destroy(bullet, 2.0f);

    }
}
