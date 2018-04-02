using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
    [HideInInspector]
    public GameObject playerFrom;

    public AudioSource gunAudio;
    public ParticleSystem gunParticles;

}
