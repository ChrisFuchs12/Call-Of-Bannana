using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;
using System.Threading.Tasks;
 

public class GlockScript : NetworkBehaviour
{
    // spawning and moving da bullet
    public GameObject objToSpawn;
    public Transform bulletSpawnPoint;
    public float bulletSpeed = 50;

    private bool firing = false;

    //ammo
    public float ammo = 30;
    public float maxAmmo = 30;
    public static bool ableToFire;
    public float reloadTime = 1f;

    //recoil back and forwards pew pew
    private bool shouldWeRecoil = true;
    public Transform recoilPoint;
    public Transform gunNormalPoint;
    public GameObject gun;

    //firerate stuff
    public float fireRate = 15f;

    //muzzel flash
    public ParticleSystem muzzelFlash;

    public Animator anim;

    //more firerate stuff
    private float nextTimeToFire = 0f;

    static public bool isReloading = false;

    public Recoil recoilScript;

    [HideInInspector] public GameObject spawnedObject;
 
    public override void OnStartClient()
    {
        base.OnStartClient();
        if (!base.IsOwner){
            GetComponent<GlockScript>().enabled = false;
        }
        if(base.IsOwner){
            GetComponent<GlockScript>().enabled = true;
        }
    }

    void start(){
        anim = GetComponent<Animator>();
    }

 
    private void Update()
    {   

        if(base.IsOwner){
            anim.enabled = true;
        }
        if(!base.IsOwner){
            anim.enabled = false;
            return;
        }

        //ammo
        if (ammo >= 1){
            ableToFire = true;
        }else{
            ableToFire = false;
        }
        
        //reloading
        if(Input.GetKeyDown("r")){
            ableToFire = false;
            isReloading = true;
        }

        if(ableToFire == false){
           StartCoroutine(Reload());
        }


        //fire function
        if(Input.GetMouseButton(0) && Time.time >= nextTimeToFire && ableToFire == true)
        {
            firing = true;

            //gun recoil move back and forwards pew pew
            if(shouldWeRecoil == true){
                gun.transform.position = recoilPoint.position;
                shouldWeRecoil = false;
            }else{
                gun.transform.position = gunNormalPoint.position;
                shouldWeRecoil = true;
            }



            //fire rate
            nextTimeToFire = Time.time + 1f/fireRate;

            ammo = ammo - 1;

            //muzzel flash
            muzzelFlash.Play();

            //cam shake
            //CameraShaker.Invoke();

            //spawning the bullet
            SpawnObject(objToSpawn, transform, this);
            RecoilGo();
            
            
        }
        if(Input.GetMouseButtonUp(0)){
            firing = false;
        }

        if(firing == false){
            //recoil movement back and forth like da pew pew
            gun.transform.position = gunNormalPoint.position;
            shouldWeRecoil = true;
        }
    }
 
    [ServerRpc]
    public void SpawnObject(GameObject obj, Transform player, GlockScript script)
    {
        GameObject spawned = Instantiate(objToSpawn, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        spawned.GetComponent<Rigidbody>().velocity = bulletSpawnPoint.forward * bulletSpeed;
        ServerManager.Spawn(spawned);
        SetSpawnedObject(spawned, script);
    }
 
    [ObserversRpc]
    public void SetSpawnedObject(GameObject spawned, GlockScript script)
    {
        spawned.GetComponent<Rigidbody>().velocity = bulletSpawnPoint.forward * bulletSpeed;
        script.spawnedObject = spawned;
    }

    public IEnumerator Reload(){
        ableToFire = false;
        isReloading = true;
        anim.SetBool("reloading",true);
        yield return new WaitForSeconds(reloadTime);
        ammo = maxAmmo;
        anim.SetBool("reloading",false);
        ableToFire = true;
        isReloading = false;
    }

    public void RecoilGo(){
        Recoil.shouldFireRecoil = true;
    }

}

