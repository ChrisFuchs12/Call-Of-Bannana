using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;
using System.Threading.Tasks;
 

public class PlayerSpawnObject : NetworkBehaviour
{
    // spawning and moving da bullet
    public GameObject objToSpawn;
    public Transform bulletSpawnPoint;
    public float bulletSpeed = 50;

    //bloom
    public float bloomIncreaceAmount = 1;
    public float maxBloom = 5;
    public float maxNegativeBloomOnY = -85;
    public float maxPositiveBloomOnY = -95;
    private float BloomY = -90;
    private float currentBloom = 0;

    //recoil up and down
    public GameObject gun;
    public float maxRecoilRotationZ = 30;
    public float recoilRotationZIncreaceAmount = 1;
    public float recoilRotationZRecoverySpeed = 1;
    public float maxRecoilRecovery = 0;

    private float currentRecoilRotationZ = 0;
    private bool firing = false;

    //recoil back and forwards pew pew
    private bool shouldWeRecoil = true;
    public Transform recoilPoint;
    public GameObject gunNormalPoint;
    [SerializeField, Range(0, 0.1f)] private float recoilSpeed = 0.01f;

    //recoil side to side movement
    public float maxNegativeRecoilOnY = -5;
    public float maxPositiveRecoilOnY = 5;

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

    [HideInInspector] public GameObject spawnedObject;
 
    public override void OnStartClient()
    {
        base.OnStartClient();
        if (!base.IsOwner){
            GetComponent<PlayerSpawnObject>().enabled = false;
        }
        if(base.IsOwner){
            GetComponent<PlayerSpawnObject>().enabled = true;
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

            //recoil
            currentRecoilRotationZ = currentRecoilRotationZ + recoilRotationZIncreaceAmount;

            if(currentRecoilRotationZ >= maxRecoilRotationZ){
                currentRecoilRotationZ = maxRecoilRotationZ;
                currentRecoilY = Random.Range(maxNegativeRecoilOnY, maxPositiveRecoilOnY);
            }

            gun.transform.localEulerAngles = new Vector3(0,currentRecoilY,-currentRecoilRotationZ);

            //bloom
            if(Input.GetMouseButton(1)){
                currentBloom = 0;
                BloomY = -90;
                currentRecoilY = 0;
                gun.transform.position = gunNormalPoint.transform.position;
            }

            currentBloom = currentBloom + bloomIncreaceAmount;

            if(currentBloom >= maxBloom){
                currentBloom = maxBloom;
                BloomY = Random.Range(maxNegativeBloomOnY, maxPositiveBloomOnY);
            }

            bulletSpawnPoint.transform.localEulerAngles = new Vector3(-currentBloom,BloomY,0);
            //print(currentBloom);
            //print(BloomY);

            //ammo
            ammo = ammo - 1;
            //print(ammo);

            //muzzel flash
            muzzelFlash.Play();

            //spawning the bullet
            SpawnObject(objToSpawn, transform, this);
            
        }
        if(Input.GetMouseButtonUp(0)){
            firing = false;
        }

        if(firing == false){

            //recoil movement back and forth like da pew pew
            gun.transform.position = gunNormalPoint.transform.position;
            shouldWeRecoil = true;

            //recoil rotation
            currentRecoilRotationZ = currentRecoilRotationZ - recoilRotationZRecoverySpeed;

            if(currentRecoilRotationZ <= maxRecoilRecovery){
                currentRecoilRotationZ = maxRecoilRecovery;
            }

            gun.transform.localEulerAngles = new Vector3(0,0,-currentRecoilRotationZ);

        }
    }
 
    [ServerRpc]
    public void SpawnObject(GameObject obj, Transform player, PlayerSpawnObject script)
    {
        
        GameObject spawned = Instantiate(objToSpawn, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        spawned.GetComponent<Rigidbody>().velocity = bulletSpawnPoint.forward * bulletSpeed;
        ServerManager.Spawn(spawned);
        SetSpawnedObject(spawned, script);
    }
 
    [ObserversRpc]
    public void SetSpawnedObject(GameObject spawned, PlayerSpawnObject script)
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
}
