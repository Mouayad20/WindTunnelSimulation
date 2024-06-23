using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MainOctree : MonoBehaviour
{
	Octree octree;
	Octree carOctree;
	Bounds region;
	// Pyramid pyramid;


	List<Particle> particles = new List<Particle>();

	/// new 
	public List<GameObject> worldObjects;

	List<Particle> inRegionParticels = new List<Particle>();

	private float timer = 0f;
	private float refreshRate = 0.5f;
	private Bounds boundary;
	private bool showShapes = false;

	GameObject model;

	void Start()
	{
		model = GameObject.Find("Car");
		// for malaz pc
		Parameters.carCenter = new Vector3(model.transform.position.x, model.transform.position.y + 0.5f, model.transform.position.z);

		// Parameters.carCenter = new Vector3(model.transform.position.x, model.transform.position.y, model.transform.position.z);

		boundary = new Bounds(Parameters.octreeCenter, new Vector3(Parameters.octreeWidth, Parameters.octreeHeight, Parameters.octreeDepth));
		// pyramid = new Pyramid(Vector3.zero, 0.75f, 0.75f);



		if (model != null)
		{
			MeshFilter meshFilter = model.GetComponent<MeshFilter>();

			if (meshFilter != null)
			{
				Mesh mesh = meshFilter.mesh;

				Vector3[] vertices = mesh.vertices;
				int[] triangles = mesh.triangles;

				Transform transform = model.transform;
				Bounds localBounds = mesh.bounds;
				region = TransformBounds(transform, localBounds);
				carOctree = new Octree(region, Parameters.carOctreeCapacity);

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

		// for (int i = 0; i < Parameters.numberOfParticles; i++)
		// {
		// 	particles.Add(new Particle(new Vector3(Parameters.octreeWidth / 2, UnityEngine.Random.Range(0, 2f), UnityEngine.Random.Range(-1f, 1f))));
		// }

	}

	void Update()
	{
		PrintFrameRate();

		if (Input.GetKeyDown("space"))
		{
			showShapes = !showShapes;
		}

		particles.Add(new Particle(new Vector3(Parameters.octreeWidth / 2, UnityEngine.Random.Range(0, 1f), UnityEngine.Random.Range(-0.5f, 0.5f))));
		// particles.Add(new Particle(new Vector3(Parameters.octreeWidth / 2, UnityEngine.Random.Range(0, 1f), UnityEngine.Random.Range(-0.5f, 0.5f))));


		octree = new Octree(boundary, Parameters.octreeCapacity);

		for (int i = 0; i < particles.Count; i++)
		{

			octree.Insert(particles[i]);

			if (particles[i].GetLocation().x < UnityEngine.Random.Range(-2f, 2f)  )
			{
				// particles[i].Move();
				particles[i].MoveToLastPoint(model);
			}
			else
			{
				particles[i].Move();
			}

			if (region.Contains(particles[i].GetLocation()))
			{
				List<AbstractObject> triangles = carOctree.query(particles[i].getRejoinAround());
				foreach (Triangle triangle in triangles)
				{
					if (particles[i].DetectCollision(triangle))
					{
						inRegionParticels.Add(particles[i]);
						break;
					}
				}

			}
			else if (inRegionParticels.Contains(particles[i]))
			{
				//Debug.Log("hahahahaha");
				//inRegionParticels.Remove(particle);
			}
			if (particles[i].isDead())
			{

				particles.RemoveAt(i);

			}
		}
	}

	void OnDrawGizmos()
	{
		if (Application.isPlaying)
		{
			foreach (Particle particle in inRegionParticels)
			{


				particle.color = Color.red;

			}
			foreach (Particle p in particles)
			{
				p.Draw();
			}
			if (showShapes)
			{
				Gizmos.color = new Color(1, 0, 0);
				Gizmos.DrawWireCube(region.center, region.size);
				carOctree.Draw();
				octree.Draw();
				// pyramid.Draw();
			}

		}
	}

	void PrintFrameRate()
	{
		timer += Time.deltaTime;
		if (timer >= refreshRate)
		{
			float fps = 1f / Time.deltaTime;
			// Debug.Log("Frame Rate: " + fps.ToString("F0"));
			timer = 0f;
		}
	}
	
	private Bounds TransformBounds(Transform transform, Bounds localBounds)
	{
		// Transform the center to world space
		Vector3 center = transform.TransformPoint(localBounds.center);

		// Get the 8 corners of the local bounds
		Vector3[] localCorners = new Vector3[8];
		localCorners[0] = localBounds.min;
		localCorners[1] = new Vector3(localBounds.min.x, localBounds.min.y, localBounds.max.z);
		localCorners[2] = new Vector3(localBounds.min.x, localBounds.max.y, localBounds.min.z);
		localCorners[3] = new Vector3(localBounds.max.x, localBounds.min.y, localBounds.min.z);
		localCorners[4] = new Vector3(localBounds.min.x, localBounds.max.y, localBounds.max.z);
		localCorners[5] = new Vector3(localBounds.max.x, localBounds.min.y, localBounds.max.z);
		localCorners[6] = new Vector3(localBounds.max.x, localBounds.max.y, localBounds.min.z);
		localCorners[7] = localBounds.max;

		// Transform the corners to world space
		for (int i = 0; i < 8; i++)
		{
			localCorners[i] = transform.TransformPoint(localCorners[i]);
		}

		// Create a new bounds that encapsulates these world space corners
		Bounds worldBounds = new Bounds(localCorners[0], Vector3.zero);
		for (int i = 1; i < 8; i++)
		{
			worldBounds.Encapsulate(localCorners[i]);
		}

		return worldBounds;
	}
}
