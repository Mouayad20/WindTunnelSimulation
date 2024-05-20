using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainOctree : MonoBehaviour
{	
	Octree octree;
	Octree carOctree;
	Bounds region;

	List<Particle> particles = new List<Particle>();

	private float timer = 0f;
	private float refreshRate = 0.5f;
	private Bounds boundary;
	private Pyramid pyramid;

	void Start()
	{
		
		GameObject model = GameObject.Find("Car");
		region = new Bounds(Parameters.carCenter, new Vector3(Parameters.carWidth, Parameters.carHeight, Parameters.carDepth));

		if (model != null)
		{
			MeshFilter meshFilter = model.GetComponent<MeshFilter>();

			if (meshFilter != null)
			{
				Mesh mesh = meshFilter.mesh;

				Vector3[] vertices = mesh.vertices;
				int[] triangles = mesh.triangles;
				
				carOctree  = new Octree(region, Parameters.carOctreeCapacity);
				
				// Transform the vertices to world coordinates
				Transform transform = model.transform;

				for (int i = 0; i < triangles.Length; i += 3)
				{
					print("or : " + vertices[triangles[i + 0]]) ;
					print("tr : " + transform.TransformPoint(vertices[triangles[i + 0]])) ;
					Triangle triangle = new Triangle(
						transform.TransformPoint(vertices[triangles[i + 0]])*10,
						transform.TransformPoint(vertices[triangles[i + 1]])*10,
						transform.TransformPoint(vertices[triangles[i + 2]])*10
					);
					
					carOctree.Insert(triangle);
					
				}
			}
			else
			{
				Debug.LogError("MeshFilter component not found on the model.");
			}
		}
		else
		{
			Debug.LogError("Model not found in the scene. Make sure the name is correct.");
		}
		
		
		
		
		boundary = new Bounds(Parameters.octreeCenter, new Vector3(Parameters.octreeWidth, Parameters.octreeHeight, Parameters.octreeDepth));
				
		// for (int i = 0 ; i < Parameters.numberOfParticles ; i++){
		// 	particles.Add(new Particle(new Vector3(Parameters.octreeWidth/2,UnityEngine.Random.Range(0,2f),UnityEngine.Random.Range(-1f,1f))));
		// }

		// List<Particle> foundedParticles = octree.query(region);
		// foreach (Particle particle in foundedParticles){
		// 	particle.ChangeColor(new Color(0,0,1));
		// }
		pyramid = new Pyramid(Vector3.zero, 0.75f,  0.75f);
	}

	void Update(){
		timer += Time.deltaTime;

		if (timer >= refreshRate)
		{
			float fps = 1f / Time.deltaTime;
			Debug.Log("Frame Rate: " + fps.ToString("F0"));
			timer = 0f;
		}

		// particles.Add(new Particle(new Vector3(Parameters.octreeWidth/2,UnityEngine.Random.Range(0,2f),UnityEngine.Random.Range(-1f,1f))));

		// octree   = new Octree(boundary, Parameters.octreeCapacity);

		// for (int i = 0 ; i < particles.Count; i++)
		// {
		// 	Particle particle = particles[i];
		// 	octree.Insert(particle);  
		// 	particle.Move();
			// if(region.Contains(particle.GetLocation()))
			// {
			// 	foreach (Triangle triangle in pyramid.triangles)
			// 	{
			// 		if(triangle.IsPointNear(particle.GetLocation(),0.001f));{
			// 			particle.color = new Color(1,0,0);
			// 		}
			// 	}
			// }
			// if (particle.isDead())
			// {
			// 	particles.RemoveAt(i);
			// }
		// }
		
		// List<Particle> otherParticles = octree.query(region);
		// foreach (Particle other in otherParticles){
		// 	other.color = new Color(1,0,0);
		// }
			

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
			carOctree.Draw();
			// octree.Draw();
			// foreach(Particle p in particles){
			// 	p.Draw();
			// }
			// pyramid.Draw();
			// Gizmos.color = new Color(1, 0, 0);
			// Gizmos.DrawWireCube(region.center, region.size);
		}
	}
}
