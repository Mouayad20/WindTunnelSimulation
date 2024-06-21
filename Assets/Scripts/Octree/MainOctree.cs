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
		// Parameters.carCenter = new Vector3(model.transform.position.x, model.transform.position.y + 0.5f, model.transform.position.z);

		Parameters.carCenter = new Vector3(model.transform.position.x, model.transform.position.y, model.transform.position.z);

		region = new Bounds(Parameters.carCenter, new Vector3(Parameters.carWidth, Parameters.carHeight, Parameters.carDepth));
		boundary = new Bounds(Parameters.octreeCenter, new Vector3(Parameters.octreeWidth, Parameters.octreeHeight, Parameters.octreeDepth));
		carOctree = new Octree(region, Parameters.carOctreeCapacity);
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
		particles.Add(new Particle(new Vector3(Parameters.octreeWidth / 2, UnityEngine.Random.Range(0, 1f), UnityEngine.Random.Range(-0.5f, 0.5f))));

		particles.Add(new Particle(new Vector3(Parameters.octreeWidth / 2, UnityEngine.Random.Range(0, 1f), UnityEngine.Random.Range(-0.5f, 0.5f))));
		particles.Add(new Particle(new Vector3(Parameters.octreeWidth / 2, UnityEngine.Random.Range(0, 1f), UnityEngine.Random.Range(-0.5f, 0.5f))));

		particles.Add(new Particle(new Vector3(Parameters.octreeWidth / 2, UnityEngine.Random.Range(0, 1f), UnityEngine.Random.Range(-0.5f, 0.5f))));
		particles.Add(new Particle(new Vector3(Parameters.octreeWidth / 2, UnityEngine.Random.Range(0, 1f), UnityEngine.Random.Range(-0.5f, 0.5f))));

		particles.Add(new Particle(new Vector3(Parameters.octreeWidth / 2, UnityEngine.Random.Range(0, 1f), UnityEngine.Random.Range(-0.5f, 0.5f))));
		particles.Add(new Particle(new Vector3(Parameters.octreeWidth / 2, UnityEngine.Random.Range(0, 1f), UnityEngine.Random.Range(-0.5f, 0.5f))));



		octree = new Octree(boundary, Parameters.octreeCapacity);

		for (int i = 0; i < particles.Count; i++)
		{

			octree.Insert(particles[i]);

			if (particles[i].color == Color.red)
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
}
