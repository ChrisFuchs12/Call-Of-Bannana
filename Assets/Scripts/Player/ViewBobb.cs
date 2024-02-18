using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewBobb : MonoBehaviour
{
    [SerializeField, Range(0, 0.1f)] private float amplitude = 0.015f;
    [SerializeField, Range(0, 30)] private float freaquency = 10.0f;

    public Transform camera;
    public Transform camHolder;

    private float toggleSpeed = 3f;
    private Vector3 startPos;
    private CharacterController controller;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        startPos = camera.localPosition;
    }

    
    void Update()
    {
        CheckMotion();
        ResetPosition();
        camera.LookAt(FocusTarget());
    }

    private void CheckMotion(){
        float speed = new Vector3(controller.velocity.x, 0, controller.velocity.z).magnitude;

        if(speed < toggleSpeed) return;
        if(!controller.isGrounded) return;

        PlayMotion(FootStepMotion());
    }

    private Vector3 FootStepMotion(){
        Vector3 pos = Vector3.zero;
        pos.y += Mathf.Sin(Time.time * freaquency) * amplitude;
        pos.x += Mathf.Sin(Time.time * freaquency / 2) * amplitude * 2;
        return pos;
    }

    private void ResetPosition(){
        if (camera.localPosition == startPos) return;
        camera.localPosition = Vector3.Lerp(camera.localPosition, startPos, 1 * Time.deltaTime);
    }

    private void PlayMotion(Vector3 motion){
        camera.localPosition += motion; 
    }

    private Vector3 FocusTarget(){
        Vector3 pos = new Vector3(transform.position.x, transform.position.y + camHolder.localPosition.y, transform.position.z);
        pos += camHolder.forward * 15.0f;
        return pos;
    }
}
