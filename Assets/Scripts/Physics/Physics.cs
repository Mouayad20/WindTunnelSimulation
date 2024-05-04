using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Physics
{
    public List<MyGameObject> worldObjects = new();

    public Physics(List<GameObject> objs)
    {
        foreach (GameObject i in objs)
        {
            worldObjects.Add(new MyGameObject(i));
        }

    }
    public void ApplyGravity(float gravity)
    {

        foreach (MyGameObject i in worldObjects)
        {
            // ApplyForce(new Vector3(0, -i.mass * gravity, 0));
            ApplyForce(new Vector3(0, -0.0098f, 0));
        }


    }

    public void ApplyForce(UnityEngine.Vector3 force)
    {

        foreach (MyGameObject i in worldObjects)
        {
            i.UpdateValues(force, 0.01f);
        }

    }

    public void ApplyObjectForce(UnityEngine.Vector3 force, MyGameObject obj)
    {


        obj.UpdateValues(force, 0.01f);


    }

    public bool AfterCollision(MyGameObject obj1, MyGameObject obj2)
    {

        obj1.mass = 2;
        obj2.mass = 2;
        Vector3 newVelocityObj1 = ((obj1.mass - obj2.mass) / (obj1.mass + obj2.mass)) * (obj1.veclocity) +
                          ((2 * obj2.mass) / (obj1.mass + obj2.mass)) * (obj2.veclocity);
        Vector3 newVelocityObj2 = ((2 * obj1.mass) / (obj1.mass + obj2.mass)) * (obj1.veclocity) +
                          ((obj2.mass - obj1.mass) / (obj1.mass + obj2.mass)) * (obj2.veclocity);

        Vector3 newMovementVector = new Vector3(obj2.obj.transform.position.x - obj1.obj.transform.position.x,
                                                obj2.obj.transform.position.y - obj1.obj.transform.position.y,
                                                obj2.obj.transform.position.z - obj1.obj.transform.position.z);

        newMovementVector.Normalize();

        ;
        // Debug.Log(newMovementVector * newVelocityObj2.magnitude);
        Debug.Log((newVelocityObj2));

        // obj1.EmptyValues();

        obj1.UpdateValues2(newVelocityObj1);
        // obj2.EmptyValues();
        obj2.UpdateValues2(newVelocityObj2);
        return true;
    }
}
