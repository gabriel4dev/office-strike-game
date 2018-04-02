using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float speed = 6f;
    public Rigidbody playerRigidBody;
    public float timeBetweenBullets;

    public GameObject bulletPrefab;
    public Transform bulletSpawn;
    public bool isLocalPlayer = false;

    private Vector3 oldPosition;
    private Vector3 currentPosition;
    private Quaternion oldRotation;
    private Quaternion currentRotation;

    private Vector3 movement;

    private float timer;

    private int floorMask;
    private float camRayLength = 100f;

    private PlayerShooting playerShooting;

    private void Awake()
    {
        this.floorMask = LayerMask.GetMask("Floor");
        this.playerRigidBody = GetComponent<Rigidbody>();
        this.playerShooting = GetComponentInChildren<PlayerShooting>();
    }


    // Use this for initialization
    void Start () {
        this.oldPosition = this.transform.position;
        this.currentPosition = this.oldPosition;
        this.oldRotation = this.transform.rotation;
        this.currentRotation = this.oldRotation;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (!isLocalPlayer)
        {
            return;
        }
        this.timer = this.timer + Time.deltaTime;

        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        this.Move(h, v);
        this.Turn();
	}

    private void Move(float h, float v)
    {
        this.movement.Set(h, 0f, v);
        this.movement = this.movement.normalized * this.speed * Time.deltaTime;
        this.playerRigidBody.MovePosition(this.transform.position + movement);
        this.currentPosition = this.transform.position + movement;
        if (this.currentPosition != this.oldPosition)
        {
            NetworkManager.instance.GetComponent<NetworkManager>().CommandMove(this.currentPosition);
            this.oldPosition = this.currentPosition;
        }
    }

    private void Turn()
    {
        Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit floorHit;
        if(Physics.Raycast(camRay, out floorHit, camRayLength, floorMask))
        {
            Vector3 playerToMouse = floorHit.point - this.transform.position;
            playerToMouse.y = 0f;

            Quaternion newRotation = Quaternion.LookRotation(playerToMouse);
            this.playerRigidBody.MoveRotation(newRotation);
            this.currentRotation = newRotation;
            if (this.currentRotation != this.oldRotation)
            {
                NetworkManager.instance.GetComponent<NetworkManager>().CommandTurn(this.currentRotation);
                this.oldRotation = this.currentRotation;
            }
        }
    }

    public void Shoot()
    {
        if(this.playerShooting != null)
        {
            this.playerShooting.Shoot();
        }
    }
}
