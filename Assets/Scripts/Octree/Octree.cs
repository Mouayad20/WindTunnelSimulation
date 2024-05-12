using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Octree
{
    private Bounds boundary;
    private int    capacity;
    private List<Particle> particles;
    private bool   isDivided;

    private Octree child1;
    private Octree child2;
    private Octree child3;
    private Octree child4;
    private Octree child5;
    private Octree child6;
    private Octree child7;
    private Octree child8;

    public Octree(Bounds boundary, int capacity)
    {
        this.boundary  = boundary;
        this.capacity  = capacity;
        this.particles = new List<Particle>();
        this.isDivided = false;
    }

    public bool Insert(Particle particle){

        if (!this.boundary.Contains(particle.location)){
            return false;
        }
        
        if (this.particles.Count < this.capacity){
            this.particles.Add(particle);
            return true;
        }
        else {
            if ( !isDivided ){
                this.Divide();
            }
            if (this.child1.Insert(particle)) return true ;
            else if (this.child2.Insert(particle)) return true ;
            else if (this.child3.Insert(particle)) return true ;
            else if (this.child4.Insert(particle)) return true ;
            else if (this.child5.Insert(particle)) return true ;
            else if (this.child6.Insert(particle)) return true ;
            else if (this.child7.Insert(particle)) return true ;
            else if (this.child8.Insert(particle)) return true ;
            else return false;
        }
    }

    public void Divide()
    {
        float subWidth = boundary.size.x / 2f;
        float subHeight = boundary.size.y / 2f;
        float subDepth = boundary.size.z / 2f;

        Vector3 subSize = new Vector3(subWidth, subHeight, subDepth);
        Vector3 center = boundary.center;

        this.child1 = new Octree(new Bounds(center + new Vector3(-subWidth / 2f,  subHeight / 2f, -subDepth / 2f), subSize), this.capacity);
        this.child2 = new Octree(new Bounds(center + new Vector3( subWidth / 2f,  subHeight / 2f, -subDepth / 2f), subSize), this.capacity);
        this.child3 = new Octree(new Bounds(center + new Vector3(-subWidth / 2f,  subHeight / 2f,  subDepth / 2f), subSize), this.capacity);
        this.child4 = new Octree(new Bounds(center + new Vector3( subWidth / 2f,  subHeight / 2f,  subDepth / 2f), subSize), this.capacity);
        this.child5 = new Octree(new Bounds(center + new Vector3(-subWidth / 2f, -subHeight / 2f, -subDepth / 2f), subSize), this.capacity);
        this.child6 = new Octree(new Bounds(center + new Vector3( subWidth / 2f, -subHeight / 2f, -subDepth / 2f), subSize), this.capacity);
        this.child7 = new Octree(new Bounds(center + new Vector3(-subWidth / 2f, -subHeight / 2f,  subDepth / 2f), subSize), this.capacity);
        this.child8 = new Octree(new Bounds(center + new Vector3( subWidth / 2f, -subHeight / 2f,  subDepth / 2f), subSize), this.capacity);

        this.isDivided = true;
    }

    public List<Particle> query(Bounds region){
        List<Particle> foundedParticles = new List<Particle>();
        if (!this.boundary.Intersects(region)){
            return foundedParticles;
        }
        else {
            foreach (Particle particle in this.particles){
                if (region.Contains(particle.location)){
                    foundedParticles.Add(particle);
                }
            }

            if(this.isDivided){
                foundedParticles.AddRange(this.child1.query(region));
                foundedParticles.AddRange(this.child2.query(region));
                foundedParticles.AddRange(this.child3.query(region));
                foundedParticles.AddRange(this.child4.query(region));
                foundedParticles.AddRange(this.child5.query(region));
                foundedParticles.AddRange(this.child6.query(region));
                foundedParticles.AddRange(this.child7.query(region));
                foundedParticles.AddRange(this.child8.query(region));
            }

            return foundedParticles;
        }
    }

    public void Draw()
    {
        if(this.isDivided){
            this.child1.Draw();
            this.child2.Draw();
            this.child3.Draw();
            this.child4.Draw();
            this.child5.Draw();
            this.child6.Draw();
            this.child7.Draw();
            this.child8.Draw();
        }else{
            this.DrawBoundaries();
        }

    }

    public void DrawBoundaries()
    {
        Gizmos.color = new Color(0, 1, 0);;
        Gizmos.DrawWireCube(boundary.center, boundary.size);
    }

}
