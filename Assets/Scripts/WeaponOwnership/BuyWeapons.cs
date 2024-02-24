using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyWeapons : MonoBehaviour
{
    //bools
    public bool Powned = false;
    public bool Gowned = true;
    public bool AKowned = true;
    public bool HKowned = false;

    //static stuff
    public static bool painterMarmoOwned = false;
    public static bool glockOwned = true;
    public static bool ak47Owned = true;
    public static bool HK416Owned = false;

    //game objects
    public GameObject painterMarmo;
    public GameObject glock;
    public GameObject ak47;
    public GameObject HK416;

    
    void Start()
    {
        
    }

    
    void Update()
    {
        // stuff for testing delete before releace
        painterMarmoOwned = Powned;
        glockOwned = Gowned;
        ak47Owned = AKowned;
        HK416Owned = HKowned;
        

        if(painterMarmoOwned == true){
            painterMarmo.SetActive(true);
        }else{
            painterMarmo.SetActive(false);
        }

        if(glockOwned == true){
            glock.SetActive(true);
        }else{
            glock.SetActive(false);
        }

        if(ak47Owned == true){
            ak47.SetActive(true);
        }else{
            ak47.SetActive(false);
        }

        if(HK416Owned == true){
            HK416.SetActive(true);
        }else{
            HK416.SetActive(false);
        }

    }
}
