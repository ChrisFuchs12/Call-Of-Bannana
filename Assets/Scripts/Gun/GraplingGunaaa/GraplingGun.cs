using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;

public class GraplingGun : NetworkBehaviour
{
    private LineRenderer lr;
    private Vector3 grapplePoint;
    public LayerMask whatIsGrappleable;
    public Transform gunTip, camera, player;
    private float maxDistance = 300f;
    private SpringJoint joint;
    private bool isGrappling = false;

    void Awake()
    {
        lr = GetComponent<LineRenderer>();
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.G) && isGrappling == false)
        {
            StartGrapple();
            isGrappling = true;
        }
        else if (Input.GetKeyDown(KeyCode.G))
        {
            StopGrapple();
            isGrappling = false;
        }
    }

    //Called after Update
    void LateUpdate()
    {
        DrawRope();
    }

    void StartGrapple()
    {
        RaycastHit hit;
        if (Physics.Raycast(camera.position, camera.forward, out hit, maxDistance, whatIsGrappleable))
        {
            grapplePoint = hit.point;
            joint = player.gameObject.AddComponent<SpringJoint>();
            joint.autoConfigureConnectedAnchor = false;
            joint.connectedAnchor = grapplePoint;

            float distanceFromPoint = Vector3.Distance(player.position, grapplePoint);

            //The distance grapple will try to keep from grapple point.
            joint.maxDistance = distanceFromPoint * 0.08f;
            joint.minDistance = distanceFromPoint * 0.25f;

            //Adjust these values to fit your game.
            joint.spring = 12.5f;
            joint.damper = 7f;
            joint.massScale = 4.5f;

            lr.positionCount = 2;

        }
    }

    void DrawRope()
    {
        if (!joint) return;

        lr.SetPosition(0, gunTip.position);
        lr.SetPosition(1, grapplePoint);
    }

    void StopGrapple()
    {
        lr.positionCount = 0;
        Destroy(joint);
    }
}


