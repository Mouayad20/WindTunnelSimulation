using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Physics
{
    public List<Particle> worldObjects = new();

    public Physics(List<GameObject> objs)
    {
        foreach (GameObject i in objs)
        {
            worldObjects.Add(new Particle(i.transform.position));
        }
    }

    public void ApplyGravity(float gravity)
    {
        foreach (Particle i in worldObjects)
        {
            // ApplyForce(new Vector3(0, -i.mass * gravity, 0));
            ApplyForce(new Vector3(0, -0.0098f, 0));
        }
    }

    public void ApplyForce(Vector3 force)
    {

        foreach (Particle i in worldObjects)
        {
            i.UpdateValues(force, 0.01f);
        }

    }

    public void ApplyObjectForce(Vector3 force, Particle obj)
    {
        obj.UpdateValues(force, 0.01f);
    }

    public bool AfterCollision(Particle obj1, Particle obj2)
    {

        obj1.SetMass(2);
        obj2.SetMass(2);
        Vector3 newVelocityObj1 = ((obj1.GetMass() - obj2.GetMass()) / (obj1.GetMass() + obj2.GetMass())) * (obj1.GetVelocity()) +
                          ((2 * obj2.GetMass()) / (obj1.GetMass() + obj2.GetMass())) * (obj2.GetVelocity());
        Vector3 newVelocityObj2 = ((2 * obj1.GetMass()) / (obj1.GetMass() + obj2.GetMass())) * (obj1.GetVelocity()) +
                          ((obj2.GetMass() - obj1.GetMass()) / (obj1.GetMass() + obj2.GetMass())) * (obj2.GetVelocity());

        Vector3 newMovementVector = new Vector3(obj2.GetObj().transform.position.x - obj1.GetObj().transform.position.x,
                                                obj2.GetObj().transform.position.y - obj1.GetObj().transform.position.y,
                                                obj2.GetObj().transform.position.z - obj1.GetObj().transform.position.z);

        newMovementVector.Normalize();
        
        // Debug.Log(newMovementVector * newVelocityObj2.magnitude);
        Debug.Log((newVelocityObj2));

        // obj1.EmptyValues();

        obj1.UpdateValues2(newVelocityObj1);
        // obj2.EmptyValues();
        obj2.UpdateValues2(newVelocityObj2);
        return true;
    }
}
