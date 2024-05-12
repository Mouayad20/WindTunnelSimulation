using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainOctree : MonoBehaviour
{
    Octree octree;
    Bounds region;

    List<Particle> particles = new List<Particle>();

    private float timer = 0f;
    private float refreshRate = 0.5f;
    private Bounds boundary;

    void Start()
    {
        boundary = new Bounds(Parameters.octreeCenter, new Vector3(Parameters.octreeWidth, Parameters.octreeHeight, Parameters.octreeDepth));
                
        // for (int i = 0 ; i < Parameters.numberOfParticles ; i++){
        //      particles.Add(new Particle(new Vector3(Parameters.octreeCenter.x,Parameters.octreeHeight/2,Parameters.octreeCenter.z)));
        // }

        // region = new Bounds(boundary.center, boundary.size / 2f);
        // List<Particle> foundedParticles = octree.query(region);
        // foreach (Particle particle in foundedParticles){
        //     particle.ChangeColor(new Color(0,0,1));
        // }

    }

    void Update(){
        timer += Time.deltaTime;

        if (timer >= refreshRate)
        {
            float fps = 1f / Time.deltaTime;
            Debug.Log("Frame Rate: " + fps.ToString("F0"));
            timer = 0f;
        }

        particles.Add(new Particle(new Vector3(Parameters.octreeCenter.x,Parameters.octreeHeight/2,Parameters.octreeCenter.z)));

        octree   = new Octree(boundary, Parameters.octreeCapacity);

        for (int i = 0 ; i < particles.Count; i++)
        {
            Particle particle = particles[i];
            octree.Insert(particle);  
            particle.Move();
            if (particle.isDead())
            {
                particles.RemoveAt(i);
            }
        }

        // without Octree -> frameRate = 3 
        // foreach (Particle particle in particles){
        //     foreach (Particle other in particles){
        //         if(   particle.GetObj().transform.position != other.GetObj().transform.position 
        //             && (Vector3.Distance(particle.GetObj().transform.position, other.GetObj().transform.position) < 1)
        //            )
        //             particle.ChangeColor(new Color(0,0,1));
        //     }
        // }

        // without Octree -> frameRate = 17  
        // foreach (Particle particle in particles){
        //     List<Particle> otherParticles = octree.query(new Bounds(particle.GetObj().transform.position, Vector3.one * (2 * 0.5f)));
        //     foreach (Particle other in otherParticles){
        //         if(   particle.GetObj().transform.position != other.GetObj().transform.position 
        //             && (Vector3.Distance(particle.GetObj().transform.position, other.GetObj().transform.position) < 1)
        //            )
        //             particle.ChangeColor(new Color(0,0,1));
        //     }
        // }
    }

    void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {
            octree.Draw();
            foreach(Particle p in particles){
                p.Draw();
            }
            // Gizmos.color = new Color(1, 0, 0);
            // Gizmos.DrawWireCube(region.center, region.size);
        }
    }
}
