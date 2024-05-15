using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Triangles
{
    public UnityEngine.Vector3 v1, v2, v3;
    public Triangles(Vector3 v1, Vector3 v2, Vector3 v3) : base()
    {
        this.v1 = v1;
        this.v2 = v2;
        this.v3 = v3;
    }



    public bool PointTriangle(Vector3 particlePosition, float radius)
    {

        Vector3 normal = Vector3.Normalize(Vector3.Cross(v2 - v1, v3 - v1));
        // Debug.Log(v3);
        // Debug.Log(v2);
        // Debug.Log(Vector3.Cross(v2 - v1, v3 - v1));
        float d = -Vector3.Dot(normal, v1);
        //////

        float dist = Vector3.Dot(normal, particlePosition) + d;

        // if (Mathf.Abs(dist) <= radius)
        //     return true;

        // Vector3 q = particlePosition - dist * normal;

        // Vector3 u = q - v1;
        // Vector3 v = v2 - v1;
        // Vector3 w = v3 - v1;

        // float s = Vector3.Dot(Vector3.Cross(u, w), normal) / Vector3.Dot(Vector3.Cross(v, w), normal);
        // float t = Vector3.Dot(Vector3.Cross(v, u), normal) / Vector3.Dot(Vector3.Cross(v, w), normal);

        // if (s >= 0 && t >= 0 && (s + t) <= 1) // -> inside the triangle
        // {
        //     float distance = Vector3.Distance(particlePosition, q);

        //     return distance <= radius; // ->collision
        // }

        return true;
    }




}
