﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class FollowCam : MonoBehaviour {
    static public FollowCam S;
    public bool _____________________________;
    static public GameObject POI; // The static point of interest

    [Header("Set in Inspector")]
    public float easing = 0.05f;
    public Vector2 minXY = Vector2.zero;

    [Header("Set Dynamically")]
    public float camZ; // The desired z pos of the camera

    private void Awake()
    {
        S = this;
        camZ = this.transform.position.z;
    }

    private void FixedUpdate()
    {
        Vector3 destination;
        // If there is no poi, return to P: [0,0,0]
        if (POI == null)
        {
            destination = Vector3.zero;
        }
        else
        {
            // Get the position of the poi
            destination = POI.transform.position;
            destination.z = camZ;
            transform.position = destination;
            // If poi is a Projectile, check to see if it's at rest
            if (POI.tag == "Projectile")
            {
                // if it is sleeping (that is, not moving)
                if (POI.GetComponent<Rigidbody>().IsSleeping())
                {
                    // return to default view
                    // in the next update
                    POI = null;
                    return;
                }
            }
        }


        // Limit the X & Y to Max values
        // the slingshot starts in -x, -y territory, so don't
        // start moving until the projectile gets
        // past the 0,0 point of the world

        destination.x = Mathf.Max(minXY.x, destination.x);
        destination.y = Mathf.Max(minXY.y, destination.y);

        // Interpolate from the current Camera position toward destination
        destination = Vector3.Lerp(transform.position, destination, easing);

        // Force destination.z to be camZ to keep the camera far enough away
        destination.z = camZ;

        // Set the camera to the destination
        transform.position = destination;

        // Set the orthographicSize of the Camera to keep Ground in view
        Camera.main.orthographicSize = destination.y + 10;
    }
}
