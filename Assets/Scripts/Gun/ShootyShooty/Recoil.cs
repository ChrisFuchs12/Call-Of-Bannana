using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recoil : MonoBehaviour
{

    private Vector3 currentRotation;
    private Vector3 targetRotation;

    //hipfire recoil
    [SerializeField] private float recoilX;
    [SerializeField] private float recoilY;
    [SerializeField] private float recoilZ;

    //settings
    [SerializeField] private float snappiness;
    [SerializeField] private float returnSpeed;

    public GameObject recoilHolder;

    //fire Recoil
    static public bool shouldFireRecoil = false;

    void Update()
    {
        targetRotation = Vector3.Lerp(targetRotation, Vector3.zero, returnSpeed * Time.deltaTime);
        currentRotation = Vector3.Lerp(currentRotation, targetRotation, snappiness * Time.fixedDeltaTime);
        recoilHolder.transform.localEulerAngles = targetRotation;

        if(shouldFireRecoil == true){
            RecoilFire();
            shouldFireRecoil = false;
        }

        print(currentRotation);
        print(recoilHolder.transform.localRotation);
    }

    public void RecoilFire(){
        targetRotation += new Vector3(recoilX, Random.Range(-recoilY, recoilY), Random.Range(-recoilZ, recoilZ));
    }
}

