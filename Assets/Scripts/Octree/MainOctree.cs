using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class MainOctree : MonoBehaviour
{
	Octree octree;
	Octree carOctree;
	Bounds region;


	List<Particle> particles = new List<Particle>();

	public List<GameObject> worldObjects;

	List<Particle> inRegionParticels = new List<Particle>();

	private float timer = 0f;
	private float refreshRate = 0.5f;
	private Bounds boundary;
	private bool showShapes = false;
	private bool MoveToLastPointFlag = false;

	GameObject model;

	void Start()
	{
		model = GameObject.Find("Model");
		Parameters.carCenter = new Vector3(model.transform.position.x, model.transform.position.y + 0.5f, model.transform.position.z);

		boundary = new Bounds(Parameters.octreeCenter, new Vector3(Parameters.octreeWidth, Parameters.octreeHeight, Parameters.octreeDepth));

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

				// Vector3[] newVertices = new HashSet<Vector3>(mesh.vertices).ToArray();
				// Debug.Log(newVertices);
				Vector3[] newVertices = new Vector3[mesh.vertices.Length];
				for (int i = 0; i < mesh.vertices.Length; i++)
				{
					if (!newVertices.Contains(mesh.vertices[i]))
					{
						newVertices[i] = mesh.vertices[i];
					}

				}

				for (int i = 0; i < triangles.Length; i += 3)
				{
					int[] triangleIndices = { triangles[i], triangles[i + 1], triangles[i + 2] };
					Triangle triangle = new Triangle(
						transform.TransformPoint(vertices[triangleIndices[0]]),
						transform.TransformPoint(vertices[triangleIndices[1]]),
						transform.TransformPoint(vertices[triangleIndices[2]]),
						mesh,
						triangleIndices,
						transform,
						newVertices
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
		// particles.Add(new Particle(new Vector3(Parameters.octreeWidth / 2, model.transform.position.y, model.transform.position.z)));
		// }

	}

	void Update()
	{
		ApplyKeys();
		if (Parameters.printFrameRate)
		{
			PrintFrameRate();
		}
		particles.Add(new Particle(new Vector3(Parameters.octreeWidth / 2, UnityEngine.Random.Range(0, 1f), UnityEngine.Random.Range(-0.5f, 0.5f))));
		particles.Add(new Particle(new Vector3(Parameters.octreeWidth / 2, UnityEngine.Random.Range(0, 1f), UnityEngine.Random.Range(-0.5f, 0.5f))));

		octree = new Octree(boundary, Parameters.octreeCapacity);

		for (int i = 0; i < particles.Count; i++)
		{
			octree.Insert(particles[i]);
			if ((particles[i].color == Color.red || particles[i].GetLocation().x <= 0) && MoveToLastPointFlag)
			{
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
			if (particles[i].isDead())
			{
				particles.RemoveAt(i);

			}
		}
	}

	void OnRenderObject()
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
				DrawWireCube(region.center, region.size);
				carOctree.Draw();
				octree.Draw();
			}

		}
	}

	public static void DrawWireCube(Vector3 center, Vector3 size)
	{
		GL.PushMatrix();
		GL.Begin(GL.LINES);
		GL.Color(new Color(0, 1, 0));

		Vector3 halfSize = size * 0.5f;

		Vector3[] corners = new Vector3[8]
		{
		center + new Vector3(-halfSize.x, -halfSize.y, -halfSize.z),
		center + new Vector3(halfSize.x, -halfSize.y, -halfSize.z),
		center + new Vector3(halfSize.x, halfSize.y, -halfSize.z),
		center + new Vector3(-halfSize.x, halfSize.y, -halfSize.z),
		center + new Vector3(-halfSize.x, -halfSize.y, halfSize.z),
		center + new Vector3(halfSize.x, -halfSize.y, halfSize.z),
		center + new Vector3(halfSize.x, halfSize.y, halfSize.z),
		center + new Vector3(-halfSize.x, halfSize.y, halfSize.z)
		};

		int[] indices = new int[]
		{
		0, 1, 1, 2, 2, 3, 3, 0, // Bottom
		4, 5, 5, 6, 6, 7, 7, 4, // Top
		0, 4, 1, 5, 2, 6, 3, 7  // Sides
		};

		for (int i = 0; i < indices.Length; i += 2)
		{
			GL.Vertex(corners[indices[i]]);
			GL.Vertex(corners[indices[i + 1]]);
		}

		GL.End();
		GL.PopMatrix();
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

	private Bounds TransformBounds(Transform transform, Bounds localBounds)
	{
		Vector3 center = transform.TransformPoint(localBounds.center);

		Vector3[] localCorners = new Vector3[8];
		localCorners[0] = localBounds.min;
		localCorners[1] = new Vector3(localBounds.min.x, localBounds.min.y, localBounds.max.z);
		localCorners[2] = new Vector3(localBounds.min.x, localBounds.max.y, localBounds.min.z);
		localCorners[3] = new Vector3(localBounds.max.x, localBounds.min.y, localBounds.min.z);
		localCorners[4] = new Vector3(localBounds.min.x, localBounds.max.y, localBounds.max.z);
		localCorners[5] = new Vector3(localBounds.max.x, localBounds.min.y, localBounds.max.z);
		localCorners[6] = new Vector3(localBounds.max.x, localBounds.max.y, localBounds.min.z);
		localCorners[7] = localBounds.max;

		for (int i = 0; i < 8; i++)
		{
			localCorners[i] = transform.TransformPoint(localCorners[i]);
		}

		Bounds worldBounds = new Bounds(localCorners[0], Vector3.zero);
		for (int i = 1; i < 8; i++)
		{
			worldBounds.Encapsulate(localCorners[i]);
		}

		return worldBounds;
	}

	private void ApplyKeys()
	{
		if (Input.GetKeyDown("space"))
		{
			showShapes = !showShapes;
		}

		if (Input.GetKeyDown(KeyCode.D))
		{
			Parameters.withDeformation = !Parameters.withDeformation;
		}
		
		if (Input.GetKeyDown(KeyCode.A))
		{
			MoveToLastPointFlag = !MoveToLastPointFlag;
		}

		if (Input.GetKeyDown(KeyCode.S))
		{
			Parameters.applyShader = !Parameters.applyShader;
		}
		if (Input.GetKeyDown(KeyCode.F))
		{
			Parameters.printFrameRate = !Parameters.printFrameRate;
		}
	}
}
