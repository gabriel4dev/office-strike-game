using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public int damagePerShot = 10;                  // The damage inflicted by each bullet.
    public float timeBetweenBullets = 0.15f;        // The time between each shot.
    public float range = 100f;                      // The distance the gun can fire.


    float timer;                                    // A timer to determine when to fire.
    Ray shootRay = new Ray();                       // A ray from the gun end forwards.
    RaycastHit shootHit;                            // A raycast hit to get information about what was hit.
    int shootableMask;                              // A layer mask so the raycast only hits things on the shootable layer.
    ParticleSystem gunParticles;                    // Reference to the particle system.
    LineRenderer gunLine;                           // Reference to the line renderer.
    AudioSource gunAudio;                           // Reference to the audio source.
    Light gunLight;                                 // Reference to the light component.
	public Light faceLight;								// Duh
    float effectsDisplayTime = 0.2f;                // The proportion of the timeBetweenBullets that the effects will display for.

    public GameObject playerFrom;

    void Awake ()
    {
        shootableMask = LayerMask.GetMask ("Shootable");

        gunParticles = GetComponent<ParticleSystem> ();
        gunLine = GetComponent <LineRenderer> ();
        gunAudio = GetComponent<AudioSource> ();
        gunLight = GetComponent<Light> ();
    }


    private void Update ()
    {
        this.timer += Time.deltaTime;
        if (this.playerFrom.GetComponent<PlayerController>().isLocalPlayer)
        {
            if (Input.GetButton("Fire1") && timer >= timeBetweenBullets && Time.timeScale != 0)
            {
                NetworkManager.instance.GetComponent<NetworkManager>().CommandShoot();
            }
        }
        if (timer >= timeBetweenBullets * effectsDisplayTime)
        {
            this.DisableEffects();
        }
    }


    public void DisableEffects ()
    {
        this.gunLine.enabled = false;
		this.faceLight.enabled = false;
        this.gunLight.enabled = false;
    }


    public void Shoot ()
    {
        Debug.Log("Shooting: " + this.playerFrom.name);
        this.timer = 0f;

        this.gunAudio.Play ();

        this.gunLight.enabled = true;
		this.faceLight.enabled = true;

        this.gunParticles.Stop ();
        this.gunParticles.Play ();

        this.gunLine.enabled = true;
        this.gunLine.SetPosition (0, this.transform.position);

        this.shootRay.origin = this.transform.position;
        this.shootRay.direction = this.transform.forward;

        if(Physics.Raycast (this.shootRay, out this.shootHit, this.range, this.shootableMask))
        {
            Health enemyHealth = shootHit.collider.GetComponent<Health>();
            if(enemyHealth != null)
            {
                enemyHealth.TakeDamage(this.playerFrom, this.damagePerShot);
            }

            this.gunLine.SetPosition (1, this.shootHit.point);
        }
        else
        {
            this.gunLine.SetPosition (1, this.shootRay.origin + this.shootRay.direction * this.range);
        }
    }
}