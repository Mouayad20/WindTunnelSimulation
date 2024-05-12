using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Particle : AbstractObject
{
    public float radius;
    public float lifespan;

    public Particle(GameObject gameObject)
    {
        this.obj = gameObject;
        this.acceleration = new Vector3(0, -0.05f, 0); 
        this.velocity = new Vector3(UnityEngine.Random.Range(-0.05f, 0.05f), 0.05f, UnityEngine.Random.Range(-0.05f, 0.05f));
        this.lifespan = 255;
        this.ChangeColor(new Color(1,0,0));
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

    public void MoveRandomly(){
        Vector3 randomOffset = new Vector3(
            UnityEngine.Random.Range(-0.01f, 0.01f),
            UnityEngine.Random.Range(-0.01f, 0.01f),
            UnityEngine.Random.Range(-0.01f, 0.01f)
        );
        this.GetObj().transform.position += randomOffset;  
    }

    public void Move(){
        float randomX = UnityEngine.Random.Range(-0.9f,  0.9f); 
        float randomY = UnityEngine.Random.Range(-0.6f, -0.1f); 
        float randomZ = UnityEngine.Random.Range(-0.9f,  0.9f); 

        Vector3 randomAcceleration = new Vector3(randomX, randomY, randomZ);

        this.velocity += randomAcceleration * Time.deltaTime;
        this.obj.transform.position += this.velocity * Time.deltaTime;
        this.lifespan -= 0.5f;
        
    }

    public bool isDead(){
        if(this.lifespan <=0)
            return true;
        else
            return false;
    }
}
