using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyWeapons : MonoBehaviour
{
    //bools
    public static bool painterMarmoOwned = true;
    public static bool glockOwned = true;

    //game objects
    public GameObject painterMarmo;
    public GameObject glock;

    
    void Start()
    {
        
    }

    
    void Update()
    {
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

    }
}
