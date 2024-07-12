using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;
using System.Threading.Tasks;

public class PlayerActivator : NetworkBehaviour
{
    public GameObject playerEverything;
    
    public void ActivatePlayer(){
       playerEverything.SetActive(true);
       Debug.Log("Player Activated");
    }
}
