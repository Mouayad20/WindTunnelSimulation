using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class MyGameObject : MonoBehaviour
{
    public UnityEngine.GameObject obj;

    public float radius;
    public float mass = 20;
    public UnityEngine.Vector3 veclocity; // vector
    public UnityEngine.Vector3 acceleration; // vector
    public MyGameObject(UnityEngine.GameObject objec)
    {
        obj = objec;
    }

    public void UpdateValues2(UnityEngine.Vector3 vel)
    {
        // acceleration += force / mass;
        veclocity = vel; ;
        obj.transform.position += veclocity;
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



    public bool DetectCollision(MyGameObject objects)
    {
        if (Math.Sqrt(Math.Pow(objects.obj.transform.position.x - obj.transform.position.x, 2) +
            Math.Pow(objects.obj.transform.position.y - obj.transform.position.y, 2) +
            Math.Pow(objects.obj.transform.position.z - obj.transform.position.z, 2)) <= 10)
        {
            return true;
        }
        return false;
        // return Math.Sqrt(Math.Pow(objects.obj.transform.position.x - obj.transform.position.x, 2) +
        //     Math.Pow(objects.obj.transform.position.y - obj.transform.position.y, 2) +
        //     Math.Pow(objects.obj.transform.position.z - obj.transform.position.z, 2));
    }

    
}
