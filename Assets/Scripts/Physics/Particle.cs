using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;

public class Particle : AbstractObject
{
	public float radius = 0.01f;
	public float lifespan;
	public Color color;

	private static Material airShaderMaterial;
	// private Material fogMaterial;

	/// new
	private List<Vector3> pathHistory = new List<Vector3>();
	// private GameObject sphereInstance;

	public Particle(Vector3 location)
	{
		// this.obj = gameObject;
		this.acceleration = new Vector3(0, -0.05f, 0);
		this.velocity = new Vector3(UnityEngine.Random.Range(-0.05f, 0.05f), 0.05f, UnityEngine.Random.Range(-0.05f, 0.05f));
		this.lifespan = 255;
		this.location = location;
		this.color = new Color(255f, 255f, 255f).WithAlpha(0.6f);
		// this.fogMaterial = material;


		if (airShaderMaterial == null)
		{
			// Shader shader = Shader.Find("Custom/AirShader");
			Shader shader = Shader.Find("Custom/FogShader");
			if (shader != null)
			{
				airShaderMaterial = new Material(shader);
			}
		}

		// sphereInstance = GameObject.CreatePrimitive(PrimitiveType.Sphere);
		// sphereInstance.transform.position = location;
		// sphereInstance.transform.localScale = Vector3.one * radius * 2; // Adjust scale


		// sphereInstance.GetComponent<Renderer>().material = airShaderMaterial;
		// sphereInstance.GetComponent<Renderer>().material.color = this.color;



	}

	public void UpdateValues2(Vector3 vel)
	{
		// acceleration += force / mass;
		this.velocity += vel;
		this.location += velocity;
	}

	public void MoveToLastPoint(GameObject model)
	{
		// acceleration += force / mass;
		// on malaz pc
		Vector3 modelLocation = model.transform.position;
		this.location += (new Vector3(-4.822f, UnityEngine.Random.Range(modelLocation.y + 0.5f - 1.5f, modelLocation.y + 0.5f + 1.5f), UnityEngine.Random.Range(-2f, 2f)) - this.location).normalized * 0.05f;

		pathHistory.Add(this.location);
		if (pathHistory.Count > 50) // Limit the history size
		{
			pathHistory.RemoveAt(0);
		}
	}


	public override bool DetectCollision(AbstractObject anotherObject)
	{

		if (anotherObject is Triangle triangle)
		{
			Vector3 normal = Vector3.Normalize(Vector3.Cross(triangle.vertex2 - triangle.vertex1, triangle.vertex3 - triangle.vertex1));

			float d = -Vector3.Dot(normal, triangle.vertex1);


			float dist = Vector3.Dot(normal, this.location) + d;


			Vector3 q = this.location - dist * normal;

			Vector3 u = q - triangle.vertex1;
			Vector3 v = triangle.vertex2 - triangle.vertex1;
			Vector3 w = triangle.vertex3 - triangle.vertex1;

			float s = Vector3.Dot(Vector3.Cross(u, w), normal) / Vector3.Dot(Vector3.Cross(v, w), normal);
			float t = Vector3.Dot(Vector3.Cross(v, u), normal) / Vector3.Dot(Vector3.Cross(v, w), normal);

			if (s >= 0 && t >= 0 && (s + t) <= 1) // -> inside the triangle
			{
				float distance = Vector3.Distance(this.location, q);
				if (distance <= 0.1f)
				{
					this.location += normal * 0.04f;
					return true;
				}

			}
			return false;
		}


		return false;
	}

	public void ChangeColor(Color color)
	{
		this.color = color;
		// if (sphereInstance != null)
		// {
		// 	sphereInstance.GetComponent<Renderer>().material.color = color;
		// }
	}

	public void MoveRandomly()
	{
		Vector3 randomOffset = new Vector3(
			UnityEngine.Random.Range(-0.01f, 0.01f),
			UnityEngine.Random.Range(-0.01f, 0.01f),
			UnityEngine.Random.Range(-0.01f, 0.01f)
		);
		this.GetObj().transform.position += randomOffset;
	}

	public void Move()
	{
		float randomX = UnityEngine.Random.Range(-0.5f, 0f);
		float randomY = UnityEngine.Random.Range(-0.5f, 0);
		float randomZ = UnityEngine.Random.Range(-0.5f, 0.5f);

		Vector3 randomAcceleration = new Vector3(randomX, 0, randomZ);

		this.velocity += randomAcceleration * Time.deltaTime;
		this.location += this.velocity * Time.deltaTime;
		this.lifespan -= 0.1f;
		this.color = new Color(255f, 255f, 255f, this.lifespan / 255f).WithAlpha(0.4f);

		pathHistory.Add(this.location);
		if (pathHistory.Count > 50) // Limit the history size
		{
			pathHistory.RemoveAt(0);
		}
	}

	public bool isDead()
	{
		// if (this.lifespan <= 0)
		if (this.location.x <= -4.822f)
		{
			// GameObject.Destroy(sphereInstance); // Destroy sphere instance

			return true;
		}
		return false;
	}

	///////////// old draw ///////////
	public void Draw()
	{
		Gizmos.color = this.color;
		Gizmos.DrawSphere(this.location, Parameters.particleRedius);

		/// new 
		for (int i = 0; i < pathHistory.Count - 1; i++)
		{
			Gizmos.color = color;
			Gizmos.DrawLine(pathHistory[i], pathHistory[i + 1]);
		}
	}

	//////////////// draw with air shader ////////////////

	// public void Draw()
	// {
	// 	// fogMaterial.SetColor("_Color", this.color);
	// 	// fogMaterial.SetPass(0);

	// 	// Graphics.DrawMeshNow(MeshUtils.CreateSphere(Parameters.particleRedius, 16, 16), Matrix4x4.TRS(this.location, Quaternion.identity, Vector3.one));
	// }


	// public void Draw()
	// {

	// 	// sphereInstance.transform.position = location;
	// 	for (int i = 0; i < pathHistory.Count - 1; i++)
	// 	{
	// 		Gizmos.color = new Color(200f, 200f, 200f).WithAlpha(0.4f);
	// 		Gizmos.DrawLine(pathHistory[i], pathHistory[i + 1]);
	// 	}

	// 	// airShaderMaterial.SetColor("_Color", Color.blue);
	// 	// airShaderMaterial.SetPass(0);
	// 	// Mesh sphereMesh = MeshUtils.CreateSphere(radius, 16, 16); // Create sphere mesh dynamically
	// 	// Graphics.DrawMeshNow(sphereMesh, Matrix4x4.TRS(this.location, Quaternion.identity, Vector3.one));
	// }


	public Bounds getRejoinAround()
	{
		return new Bounds(this.location, new Vector3(0.5f, 0.5f, 0.5f));
	}



}
