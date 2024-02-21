using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletHitGround : MonoBehaviour
{

    public GameObject bullet;
    public ParticleSystem hitGroundEffect;
    public GameObject bulletHolePrefab;

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Floor")
        {
        hitGroundEffect.Play();
        GameObject obj = Instantiate(bulletHolePrefab, transform.position, transform.rotation);
        }
    }

}
