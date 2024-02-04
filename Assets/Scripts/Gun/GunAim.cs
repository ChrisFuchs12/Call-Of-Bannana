using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;
using FishNet.Component.Animating;

public class GunAim : NetworkBehaviour
{

    private Animator animator;
    private NetworkAnimator netAnim;

    static public bool isAiming = false;
    
    private void Awake(){
        animator = GetComponent<Animator>();
        netAnim = GetComponent<NetworkAnimator>();
    }
    
    void Update()
    {
       if(base.IsOwner){
              animator.enabled = true;
       }
       if(!base.IsOwner){
              animator.enabled = false;
              return;
       }

        if(Input.GetMouseButton(1)){
            StartCoroutine(Reload());
            isAiming = true;
        }else{
            isAiming = false;
            StartCoroutine(Reload());
        }
    }

    public IEnumerator Reload(){
       if(isAiming == true){
            animator.SetBool("IsAiming",true);
       }else{
            animator.SetBool("IsAiming",false);
            yield return new WaitForSeconds(1);
       }
    }
}
