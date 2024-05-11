using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Particle : AbstractObject
{
    public float radius;

    public Particle(GameObject gameObject)
    {
        this.obj = gameObject;
    }

    public void UpdateValues2(Vector3 vel)
    {
        // acceleration += force / mass;
        this.velocity = vel; ;
        this.obj.transform.position += velocity;
    }

    public override bool DetectCollision(AbstractObject anotherObject)
    {
        if (anotherObject is Particle anotherParticle)
        {
            if (Math.Sqrt(Math.Pow(anotherObject.GetObj().transform.position.x - this.obj.transform.position.x, 2) +
                Math.Pow(anotherObject.GetObj().transform.position.y - this.obj.transform.position.y, 2) +
                Math.Pow(anotherObject.GetObj().transform.position.z - this.obj.transform.position.z, 2)) <= 10)
            {
                return true;
            }
        }
        return false;
    }

    public void ChangeColor(Color color)
    {
        Renderer renderer = obj.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material.color = color;
        }
        else
        {
            Debug.LogWarning("Renderer component not found on the GameObject.");
        }
    }
}
