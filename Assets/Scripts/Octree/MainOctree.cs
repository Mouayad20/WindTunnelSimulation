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

	void Start()
	{

		GameObject model = GameObject.Find("Car");
		region   = new Bounds(Parameters.carCenter, new Vector3(Parameters.carWidth, Parameters.carHeight, Parameters.carDepth));
		boundary = new Bounds(Parameters.octreeCenter, new Vector3(Parameters.octreeWidth, Parameters.octreeHeight, Parameters.octreeDepth));

		if (model != null)
		{
			MeshFilter meshFilter = model.GetComponent<MeshFilter>();

			if (meshFilter != null)
			{
				Mesh mesh = meshFilter.mesh;

				Vector3[] vertices = mesh.vertices;
				int[] triangles = mesh.triangles;

				carOctree = new Octree(region, Parameters.carOctreeCapacity);

				Transform transform = model.transform;

				for (int i = 0; i < triangles.Length; i += 3)
				{
					Triangle triangle = new Triangle(
						transform.TransformPoint(vertices[triangles[i + 0]]),
						transform.TransformPoint(vertices[triangles[i + 1]]),
						transform.TransformPoint(vertices[triangles[i + 2]])
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

		// for (int i = 0 ; i < Parameters.numberOfParticles ; i++){
		// 	particles.Add(new Particle(new Vector3(Parameters.octreeWidth/2,UnityEngine.Random.Range(0,2f),UnityEngine.Random.Range(-1f,1f))));
		// }

	}

	void Update()
	{
		PrintFrameRate();

		particles.Add(new Particle(new Vector3(Parameters.octreeWidth / 2, UnityEngine.Random.Range(0, 1f), UnityEngine.Random.Range(-1f, 1f))));
		particles.Add(new Particle(new Vector3(Parameters.octreeWidth / 2, UnityEngine.Random.Range(0, 1f), UnityEngine.Random.Range(-1f, 1f))));
		particles.Add(new Particle(new Vector3(Parameters.octreeWidth / 2, UnityEngine.Random.Range(0, 1f), UnityEngine.Random.Range(-1f, 1f))));
		particles.Add(new Particle(new Vector3(Parameters.octreeWidth / 2, UnityEngine.Random.Range(0, 1f), UnityEngine.Random.Range(-1f, 1f))));
		particles.Add(new Particle(new Vector3(Parameters.octreeWidth / 2, UnityEngine.Random.Range(0, 1f), UnityEngine.Random.Range(-1f, 1f))));

		octree = new Octree(boundary, Parameters.octreeCapacity);

		for (int i = 0; i < particles.Count; i++)
		{
			Particle particle = particles[i];
			octree.Insert(particle);
			particle.Move();
			if (region.Contains(particle.GetLocation()))
			{
				List<AbstractObject> triangles = carOctree.query(particle.getRejoinAround());
				print("triangles.Count : " + triangles.Count);
				foreach (Triangle triangle in triangles )
				{
					if (triangle.IsPointNear(particle.GetLocation(), 0.00001f)) ;
					{
						particle.ChangeColor(Color.black);
					}
				}
			}
			if (particle.isDead())
			{
				particles.RemoveAt(i);
			}
		}

	}

	void OnDrawGizmos()
	{
		if (Application.isPlaying)
		{
			carOctree.Draw();
			octree.Draw();
			foreach (Particle p in particles)
			{
				p.Draw();
			}
			Gizmos.color = new Color(1, 0, 0);
			Gizmos.DrawWireCube(region.center, region.size);
		}
	}

	void PrintFrameRate()
	{
		timer += Time.deltaTime;
		if (timer >= refreshRate)
		{
			float fps = 1f / Time.deltaTime;
			Debug.Log("Frame Rate: " + fps.ToString("F0"));
			timer = 0f;
		}
	}
}
