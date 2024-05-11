using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainOctree : MonoBehaviour
{
    Octree octree;
    Bounds region;

    void Start()
    {
        Bounds boundary = new Bounds(Parameters.octreeCenter, new Vector3(Parameters.octreeWidth, Parameters.octreeHeight, Parameters.octreeDepth));
        octree   = new Octree(boundary, Parameters.octreeCapacity);
        GameObject particlePrefab = GameObject.CreatePrimitive(PrimitiveType.Sphere); 
        particlePrefab.transform.localScale = Vector3.one * 0.5f;

        for (int i = 0 ; i < Parameters.numberOfParticles ; i++){
            Vector3 randomPosition = new Vector3(
                Random.Range(boundary.min.x, boundary.max.x),
                Random.Range(boundary.min.y, boundary.max.y),
                Random.Range(boundary.min.z, boundary.max.z)
            );

            GameObject sphere = Instantiate(particlePrefab, randomPosition, Quaternion.identity);
            Particle particle = new Particle(sphere);
            octree.Insert(particle);       
        }

        region = new Bounds(boundary.center, boundary.size / 2f);

        List<Particle> foundedParticles = octree.query(region);

        foreach (Particle particle in foundedParticles){
            particle.ChangeColor(new Color(0,0,1));
        }

    }

    void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {
            octree.Draw();

            Gizmos.color = new Color(1, 0, 0);
            Gizmos.DrawWireCube(region.center, region.size);
        }
    }
}
