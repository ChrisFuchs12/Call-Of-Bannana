using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FishNet.Object;

public class WeponMod : NetworkBehaviour
{
    public GameObject weponModUI;
    public GameObject supressor;
    public GameObject scope;
    public GameObject crossHair;

    public Animator anim;

    private bool isInspecting1 = false;
    private bool supressorEquiped = false;
    private bool scopeEquiped = false;

    public void Start(){
       anim = GetComponent<Animator>();
    }

    void Update()
    {
        if(Input.GetKeyDown("f") && base.IsOwner){
            if(isInspecting1 == false){
                weponModUI.gameObject.SetActive(true);
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                isInspecting1 = true;
                PlayerSpawnObject.isInspecting = true;
                anim.SetBool("isInspectingGun",true);
            }else if(isInspecting1 == true){
                weponModUI.gameObject.SetActive(false);
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                isInspecting1 = false;
                PlayerSpawnObject.isInspecting = false;
                anim.SetBool("isInspectingGun",false);
            }
        }
        if(Input.GetMouseButton(1) && scopeEquiped == true && base.IsOwner){
            crossHair.gameObject.SetActive(false);
        }else{
            crossHair.gameObject.SetActive(true);
        }
    }

    public void EquipSupressor(){
        if(supressorEquiped == false){
            supressor.gameObject.SetActive(true);
            supressorEquiped = true;
        }else if(supressorEquiped == true){
            supressor.gameObject.SetActive(false);
            supressorEquiped = false;
        }
    }

        public void EquipScope(){
        if(scopeEquiped == false){
            scope.gameObject.SetActive(true);
            scopeEquiped = true;
        }else if(scopeEquiped == true){
            scope.gameObject.SetActive(false);
            scopeEquiped = false;
        }
    }
}
