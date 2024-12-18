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
    public Animator akAnim;
    public Animator hkAnim;
    private float equipTime = 0.5f;
    private bool knifeAnimPlayed = false;
    private bool isEquippingKnife = false;

    //inverse kinematics stuff
    public GameObject Rhandle;
    public GameObject Lhandle;

    public GameObject RhandleAR;
    public GameObject LhandleAR;

    public GameObject RhandleAK47;
    public GameObject LhandleAK47;

    public GameObject Rhandlehk416;
    public GameObject Lhandlehk416;

    public GameObject RhandleBali;
    public GameObject LhandleBali;

    public GameObject RhandleGlock;
    public GameObject LhandleGlock;

    public float HandMoveSpeed;

    //recoil
    public GameObject weaponSlot2RecoilTing;
    public GameObject weaponSlot2RecoilTingAK;
    public GameObject weaponSlot2RecoilTingHK416;
    public GameObject weaponSlot3RecoilTing;

    public override void OnStartClient()
    {
        base.OnStartClient();
        if (!base.IsOwner)
            GetComponent<WeaponSwapping>().enabled = false;
    }

    void start(){
        knifeAnim = GetComponent<Animator>();
        akAnim = GetComponent<Animator>();
        hkAnim = GetComponent<Animator>();
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
            Lhandle.transform.position = Vector3.Lerp(Lhandle.transform.position, LhandleBali.transform.position, HandMoveSpeed);
            Rhandle.transform.position = Vector3.Lerp(Rhandle.transform.position, RhandleBali.transform.position, HandMoveSpeed);
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
            UnEquipWeponSlot1();
        }


        if(weapon2Active == true && BuyWeapons.painterMarmoOwned == true){
            weaponSlot2.gameObject.SetActive(true);
            Rhandle.transform.parent = weaponSlot2RecoilTing.transform;
            Lhandle.transform.parent = weaponSlot2RecoilTing.transform;
            Lhandle.transform.position = Vector3.Lerp(Lhandle.transform.position, LhandleAR.transform.position, HandMoveSpeed);
            Rhandle.transform.position = Vector3.Lerp(Rhandle.transform.position, RhandleAR.transform.position, HandMoveSpeed);
            EquipWeponSlot2();
        }else if(weapon2Active == true && BuyWeapons.ak47Owned == true){
            weaponSlot2.gameObject.SetActive(true);
            Rhandle.transform.parent = weaponSlot2RecoilTingAK.transform;
            Lhandle.transform.parent = weaponSlot2RecoilTingAK.transform;
            Lhandle.transform.position = Vector3.Lerp(Lhandle.transform.position, LhandleAK47.transform.position, HandMoveSpeed);
            Rhandle.transform.position = Vector3.Lerp(Rhandle.transform.position, RhandleAK47.transform.position, HandMoveSpeed);
            EquipWeponSlot2();
        }else if(weapon2Active == true && BuyWeapons.HK416Owned == true){
            weaponSlot2.gameObject.SetActive(true);
            weaponSlot2.gameObject.SetActive(true);
            Rhandle.transform.parent = weaponSlot2RecoilTingHK416.transform;
            Lhandle.transform.parent = weaponSlot2RecoilTingHK416.transform;
            Lhandle.transform.position = Vector3.Lerp(Lhandle.transform.position, Lhandlehk416.transform.position, HandMoveSpeed);
            Rhandle.transform.position = Vector3.Lerp(Rhandle.transform.position, Rhandlehk416.transform.position, HandMoveSpeed);
            EquipWeponSlot2();
        }else if(weapon2Active == false){
            gunAnim.CrossFade("New State", 0f);
            gunAnim.Update(0f);
            gunAnim.Update(0f);
            akAnim.CrossFade("New State", 0f);
            akAnim.Update(0f);
            akAnim.Update(0f);
            weaponSlot2.gameObject.SetActive(false);
            UnEquipWeponSlot2();
        }


        if(weapon3Active == true){
            weaponSlot3.gameObject.SetActive(true);
            Rhandle.transform.parent = weaponSlot3RecoilTing.transform;
            Lhandle.transform.parent = weaponSlot3RecoilTing.transform;
            Lhandle.transform.position = Vector3.Lerp(Lhandle.transform.position, LhandleGlock.transform.position, HandMoveSpeed);
            Rhandle.transform.position = Vector3.Lerp(Rhandle.transform.position, RhandleGlock.transform.position, HandMoveSpeed);
            EquipWeponSlot3();
        }else if(weapon3Active == false){
            pistolAnim.CrossFade("New State", 0f);
            pistolAnim.Update(0f);
            pistolAnim.Update(0f);
            weaponSlot3.gameObject.SetActive(false);
            UnEquipWeponSlot3();
        }
    }

    //equip server
    [ServerRpc(RequireOwnership = false)]
    void EquipWeponSlot2()
    {
        weaponSlot2.gameObject.SetActive(true);
        ObserverEquipWeponSlot2();
    }


    [ServerRpc(RequireOwnership = false)]
    void EquipWeponSlot3()
    {
        weaponSlot3.gameObject.SetActive(true);
        ObserverEquipWeponSlot3();
    }

    [ServerRpc(RequireOwnership = false)]
    void EquipWeponSlot1()
    {
        weaponSlot1.gameObject.SetActive(true);
        ObserverEquipWeponSlot1();
    }

    //equip observer
    
    [ObserversRpc]
    void ObserverEquipWeponSlot2()
    {
        weaponSlot2.gameObject.SetActive(true);
    }


    [ObserversRpc]
    void ObserverEquipWeponSlot3()
    {
        weaponSlot3.gameObject.SetActive(true);
    }

    [ObserversRpc]
    void ObserverEquipWeponSlot1()
    {
        weaponSlot1.gameObject.SetActive(true);
    }

    //unequip server
    [ServerRpc(RequireOwnership = false)]
    void UnEquipWeponSlot2()
    {
        weaponSlot2.gameObject.SetActive(false);
        ObserverUnEquipWeponSlot2();
    }

    [ServerRpc(RequireOwnership = false)]
    void UnEquipWeponSlot3()
    {
        weaponSlot3.gameObject.SetActive(false);
        ObserverUnEquipWeponSlot3();
    }

    [ServerRpc(RequireOwnership = false)]
    void UnEquipWeponSlot1()
    {
        weaponSlot1.gameObject.SetActive(false);
        ObserverUnEquipWeponSlot1();
    }

    //unequip observer
    [ObserversRpc]
    void ObserverUnEquipWeponSlot2()
    {
        weaponSlot2.gameObject.SetActive(false);
    }

    [ObserversRpc]
    void ObserverUnEquipWeponSlot3()
    {
        weaponSlot3.gameObject.SetActive(false);
    }

    [ObserversRpc]
    void ObserverUnEquipWeponSlot1()
    {
        weaponSlot1.gameObject.SetActive(false);
    }


    //poo poo knife
    public IEnumerator EquipKnife()
    {
        if (isEquippingKnife) yield break; // Prevent re-entering if already equipping

        isEquippingKnife = true;
        knifeAnim.SetBool("isEquipping", true);

        // Wait until the current animation finishes
        while (knifeAnim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f ||
               knifeAnim.IsInTransition(0))
        {
            yield return null;
        }

        knifeAnim.SetBool("isEquipping", false);
        isEquippingKnife = false;
    }


}
