using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CalculateCollision : Singleton<CalculateCollision> {
    //utility class for calculating outputs when stuff collide

    public float calculateIntensity(Vector3 collider, Vector3 collidee, Bounds collideeBounds)
    {
        //TODO use more complicated method with colliders 
        /*
        //linear
        if (Mathf.Abs(collider.x - collidee.x) < collideeBounds.extents.x 
            && Mathf.Abs(collider.y - collidee.y) < collideeBounds.extents.y
            && Mathf.Abs(collider.z - collidee.z) < collideeBounds.extents.z)
        {
            return 1 - Vector3.Distance(collider, collidee) / Mathf.Sqrt(Mathf.Pow((collideeBounds.extents.x), 2) + Mathf.Pow((collideeBounds.extents.y), 2) + Mathf.Pow((collideeBounds.extents.z), 2));
        }
        else
        {
            return 0;
        }
        */
        Debug.Log("caculating intensity: distance: " + Vector3.Distance(collider, collidee) + " extents: x: " + collideeBounds.size.x + " y: " + collideeBounds.size.y + " z: " + collideeBounds.size.z);
        float intensity = 1 - (Vector3.Distance(collider, collidee) / Mathf.Max(collideeBounds.extents.x, collideeBounds.extents.y, collideeBounds.extents.z));
        Debug.Log("calculated intensity: " + intensity);

        if (intensity < 0f)
        {
            return 0f;
        } else if (intensity < 0.7f)
        {
            return 0.7f;
        } else if(intensity > 1.0f)
        {
            return 1.0f;
        } else
        {
            return intensity;
        }
    }
}
