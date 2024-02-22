using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyWeapons : MonoBehaviour
{
    //bools
    public bool Powned = false;
    public bool Gowned = true;
    public bool AKowned = true;

    //static stuff
    public static bool painterMarmoOwned = false;
    public static bool glockOwned = true;
    public static bool ak47Owned = true;

    //game objects
    public GameObject painterMarmo;
    public GameObject glock;
    public GameObject ak47;

    
    void Start()
    {
        
    }

    
    void Update()
    {
        // stuff for testing delete before releace
        painterMarmoOwned = Powned;
        glockOwned = Gowned;
        ak47Owned = AKowned;
        

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

    }
}
