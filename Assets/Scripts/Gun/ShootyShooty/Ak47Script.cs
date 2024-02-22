using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;
using System.Threading.Tasks;
 

public class Ak47Script : NetworkBehaviour
{
    // spawning and moving da bullet
    public GameObject objToSpawn;
    public Transform bulletSpawnPoint;
    public float bulletSpeed = 50;

    //gun
    public GameObject gun;

    private bool firing = false;

    //recoil back and forwards pew pew
    private bool shouldWeRecoil = true;
    public Transform recoilPoint;
    public GameObject gunNormalPoint;

    private float currentRecoilY = 0;

    //ammo
    public float ammo = 30;
    public float maxAmmo = 30;
    private bool ableToFire;
    public float reloadTime = 1f;

    //firerate stuff
    public float fireRate = 15f;

    //muzzel flash
    public ParticleSystem muzzelFlash;

    public Animator animator;

    //more firerate stuff
    private float nextTimeToFire = 0f;

    //inspect and wepon attachments maybe
    static public bool isInspecting = false;

    //aim In
    public GameObject crosshair;

    // revamped recoil
    public Recoil recoilScript;

    [HideInInspector] public GameObject spawnedObject;
 
    public override void OnStartClient()
    {
        base.OnStartClient();
        if (!base.IsOwner){
            GetComponent<Ak47Script>().enabled = false;
        }
        if(base.IsOwner){
            GetComponent<Ak47Script>().enabled = true;
        }
    }

    void start(){
    animator = GetComponent<Animator>();
    }

 
    private void Update()
    {   

        //ammo
        if (ammo >= 1){
            ableToFire = true;
        }else{
            ableToFire = false;
        }
        
        //reloading
        if(Input.GetKeyDown("r")){
            StartCoroutine(Reload());
        }

        if(ableToFire == false){
           StartCoroutine(Reload());
        }

        if(Input.GetMouseButton(1)){
            gun.transform.position = gunNormalPoint.transform.position;
            crosshair.SetActive(false);
            shouldWeRecoil = true;
        }else{
            crosshair.SetActive(true);
        }

        //fire function
        if(Input.GetMouseButton(0) && Time.time >= nextTimeToFire && ableToFire == true && isInspecting == false)
        {
            firing = true;

            //gun recoil move back and forwards pew pew
            if(shouldWeRecoil == true){
                gun.transform.position = recoilPoint.position;
                shouldWeRecoil = false;
            }else{
                gun.transform.position = gunNormalPoint.transform.position;
                shouldWeRecoil = true;
            }

            //fire rate
            nextTimeToFire = Time.time + 1f/fireRate;

            

            //ammo
            ammo = ammo - 1;


            //muzzel flash
            muzzelFlash.Play();

            //spawning the bullet
            SpawnObject(objToSpawn, transform, this);
            RecoilGo();   
        }
        if(Input.GetMouseButtonUp(0)){
            firing = false;
        }

        if(firing == false){

            //recoil movement back and forth like da pew pew
            gun.transform.position = gunNormalPoint.transform.position;
            shouldWeRecoil = true;

        }
    }
 
    [ServerRpc]
    public void SpawnObject(GameObject obj, Transform player, Ak47Script script)
    {
        
        GameObject spawned = Instantiate(objToSpawn, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        spawned.GetComponent<Rigidbody>().velocity = bulletSpawnPoint.forward * bulletSpeed;
        ServerManager.Spawn(spawned);
        SetSpawnedObject(spawned, script);
    }
 
    [ObserversRpc]
    public void SetSpawnedObject(GameObject spawned, Ak47Script script)
    {
        spawned.GetComponent<Rigidbody>().velocity = bulletSpawnPoint.forward * bulletSpeed;
        script.spawnedObject = spawned;
    }

    public IEnumerator Reload(){
        ableToFire = false;
        animator.SetBool("reloading",true);
        yield return new WaitForSeconds(reloadTime);
        ammo = maxAmmo;
        animator.SetBool("reloading",false);
        ableToFire = true;
    }

    public void RecoilGo(){
        Recoil.shouldFireRecoil = true;
    }
}
