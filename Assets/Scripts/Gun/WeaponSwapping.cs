using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;

public class WeaponSwapping : NetworkBehaviour
{
    //slot 1 is knifes
    public GameObject weaponSlot1;
    // 2 is primary
    public GameObject weaponSlot2;
    //3 is secondary
    public GameObject weaponSlot3;

    //stuff for what is active and what isnt
    private bool weapon1Active = true;
    private bool weapon2Active = false;
    private bool weapon3Active = false;

    //animator stuff
    public Animator knifeAnim;
    public Animator gunAnim;
    public Animator pistolAnim;
    private float equipTime = 0.5f;
    private bool knifeAnimPlayed = false;
    private bool isEquippingKnife = false;

    //inverse kinematics stuff
    public GameObject Rhandle;
    public GameObject Lhandle;

    public GameObject RhandleAR;
    public GameObject LhandleAR;

    public float HandMoveSpeed;

    public GameObject RhandleGlock;
    public GameObject LhandleGlock;

    //recoil
    public GameObject weaponSlot2RecoilTing;
    public GameObject weaponSlot3RecoilTing;

    public override void OnStartClient()
    {
        base.OnStartClient();
        if (!base.IsOwner)
            GetComponent<WeaponSwapping>().enabled = false;
    }

    void start(){
        knifeAnim = GetComponent<Animator>();
        gunAnim = GetComponent<Animator>();
        pistolAnim = GetComponent<Animator>();
        gameObject.transform.parent = Rhandle.transform;
    }

    void Update()
    {
        //key inputs
        if(Input.GetKeyDown("1") && base.IsOwner && GlockScript.isReloading == false && GunAim.isAiming == false){
            weapon1Active = true;
            weapon2Active = false;
            weapon3Active = false;
        }


        if(Input.GetKeyDown("2") && base.IsOwner && GlockScript.isReloading == false && GunAim.isAiming == false){
            weapon1Active = false;
            weapon2Active = true;
            weapon3Active = false;
        }


        if(Input.GetKeyDown("3") && base.IsOwner && GlockScript.isReloading == false && GunAim.isAiming == false){
            weapon1Active = false;
            weapon2Active = false;
            weapon3Active = true;
        }

        //responses to key inputs based on true or false values
        if(weapon1Active == true){
            weaponSlot1.gameObject.SetActive(true);
            Rhandle.transform.parent = weaponSlot1.transform;
            Lhandle.transform.parent = weaponSlot1.transform;
            EquipWeponSlot1();
            if(knifeAnimPlayed == false){
                StartCoroutine(EquipKnife());
                knifeAnimPlayed = true;
            }
        }else if(weapon1Active == false){
            knifeAnim.CrossFade("New State", 0f);
            knifeAnim.Update(0f);
            knifeAnim.Update(0f);
            weaponSlot1.gameObject.SetActive(false);
            knifeAnimPlayed = false;
        }


        if(weapon2Active == true){
            weaponSlot2.gameObject.SetActive(true);
            Rhandle.transform.parent = weaponSlot2RecoilTing.transform;
            Lhandle.transform.parent = weaponSlot2RecoilTing.transform;
            Lhandle.transform.position = Vector3.MoveTowards(Lhandle.transform.position, LhandleAR.transform.position, HandMoveSpeed);
            Rhandle.transform.position = Vector3.MoveTowards(Rhandle.transform.position, RhandleAR.transform.position, HandMoveSpeed);
            EquipWeponSlot2();
            //handle.parent = weaponSlot2;
        }else if(weapon2Active == false){
            gunAnim.CrossFade("New State", 0f);
            gunAnim.Update(0f);
            gunAnim.Update(0f);
            weaponSlot2.gameObject.SetActive(false);
        }


        if(weapon3Active == true){
            weaponSlot3.gameObject.SetActive(true);
            Rhandle.transform.parent = weaponSlot3RecoilTing.transform;
            Lhandle.transform.parent = weaponSlot3RecoilTing.transform;
            Lhandle.transform.position = Vector3.MoveTowards(Lhandle.transform.position, LhandleGlock.transform.position, HandMoveSpeed);
            Rhandle.transform.position = Vector3.MoveTowards(Rhandle.transform.position, RhandleGlock.transform.position, HandMoveSpeed);
            EquipWeponSlot3();
        }else if(weapon3Active == false){
            pistolAnim.CrossFade("New State", 0f);
            pistolAnim.Update(0f);
            pistolAnim.Update(0f);
            weaponSlot3.gameObject.SetActive(false);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    void EquipWeponSlot2()
    {
        weaponSlot2.gameObject.SetActive(true);
    }

    [ServerRpc(RequireOwnership = false)]
    void EquipWeponSlot3()
    {
        weaponSlot3.gameObject.SetActive(true);
    }

    [ServerRpc(RequireOwnership = false)]
    void EquipWeponSlot1()
    {
        weaponSlot1.gameObject.SetActive(true);
    }

    public IEnumerator EquipKnife(){
        isEquippingKnife = true;
        knifeAnim.SetBool("isEquipping",true);
        yield return new WaitForSeconds(equipTime);
        knifeAnim.SetBool("isEquipping",false);
        isEquippingKnife = false;
    }

}
