using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FishNet.Object;

public class PlayerDamageDetector : NetworkBehaviour
{

static public float health = 100;
public float maxHealth = 100;
public float bulletDamage = 10;

public Image healthBar;

public GameObject DethScreen;
public GameObject DeadBody;
public GameObject AlivePlayerGun;
public GameObject AlivePlayerBody;

    void OnTriggerEnter(Collider other)
    {
    //doing damage
        if (other.gameObject.tag == "Bullet" && base.IsOwner)
        {
        health = health - bulletDamage;
        print(health);
        HealthBarFiller();

        //being ded
        if(health<=0 && base.IsOwner){
            print("ded");
            DethScreen.gameObject.SetActive (true);
            DeadBody.gameObject.SetActive (true);
            AlivePlayerGun.gameObject.SetActive (false);
            AlivePlayerBody.gameObject.SetActive (false);
            DiedServer();
        }
        else{
            DethScreen.gameObject.SetActive (false);
            DeadBody.gameObject.SetActive (false);
            AlivePlayerGun.gameObject.SetActive (true);
            AlivePlayerBody.gameObject.SetActive (true);
        }

        }
    }

    void HealthBarFiller(){
        healthBar.fillAmount = health / maxHealth;
    }

    [ServerRpc(RequireOwnership = false)]
    void DiedServer(){
        DeadBody.gameObject.SetActive (true);
        AlivePlayerGun.gameObject.SetActive (false);
        AlivePlayerBody.gameObject.SetActive (false);
        DiedObserver();
    }

    [ObserversRpc]
    void DiedObserver(){
        DeadBody.gameObject.SetActive (true);
        AlivePlayerGun.gameObject.SetActive (false);
        AlivePlayerBody.gameObject.SetActive (false);
    }
}
