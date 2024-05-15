using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class MyGameObject : MonoBehaviour /// sphare
{
    public UnityEngine.GameObject obj;

    public float radius = 5;
    public float mass = 20;
    public UnityEngine.Vector3 veclocity; // vector
    public UnityEngine.Vector3 acceleration; // vector
    public MyGameObject(UnityEngine.GameObject objec)
    {
        obj = objec;
    }

    public void UpdateValues(UnityEngine.Vector3 force, float dt)
    {
        acceleration += force / mass;
        veclocity += acceleration * dt;
        obj.transform.position += veclocity * dt;
    }

    public void EmptyValues()
    {
        acceleration = new UnityEngine.Vector3(0, 0, 0);
        veclocity = new UnityEngine.Vector3(0, 0, 0);
    }
    public bool HandleCollision(MyGameObject o)
    {
        if (Math.Sqrt(Math.Pow(o.obj.transform.position.x - this.obj.transform.position.x, 2) +
            Math.Pow(o.obj.transform.position.y - this.obj.transform.position.y, 2) +
            Math.Pow(o.obj.transform.position.z - this.obj.transform.position.z, 2)) <= 2 * this.radius)
        {
            return true;
        }
        return false;
    }
}
