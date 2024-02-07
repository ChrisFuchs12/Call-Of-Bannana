using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletHitGround : MonoBehaviour
{

    public GameObject bullet;
    public ParticleSystem hitGroundEffect;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Floor")
        {
        hitGroundEffect.Play();
        }
    }

}
