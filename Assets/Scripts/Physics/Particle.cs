using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;

public class Particle : AbstractObject
{
	public float radius;
	public float lifespan;
	public Color color;

	/// new
	private List<Vector3> pathHistory = new List<Vector3>();

	/// new
	private Material particleMaterial = new Material(Shader.Find("Custom/LineShader"));

	public Particle(Vector3 location)
	{
		// this.obj = gameObject;
		this.acceleration = new Vector3(0, -0.05f, 0);
		this.velocity = new Vector3(UnityEngine.Random.Range(-0.05f, 0.05f), 0.05f, UnityEngine.Random.Range(-0.05f, 0.05f));
		this.lifespan = 255;
		this.location = location;
		this.color = new Color(0f, 0f, 0f).WithAlpha(0.4f);

		/// new 
		// Shader lineShader = Shader.Find("Custom/LineShader");

		// if (lineShader == null)
		// {
		// 	Debug.LogError("Shader not found! Make sure the shader file is named correctly and placed in the project.");
		// }
		// this.particleMaterial = new Material(lineShader);

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
		//this.location += (new Vector3(-4.822f, model.transform.position.y + 0.5f, UnityEngine.Random.Range(-2f, 2f)) - this.location).normalized * 0.03f;

		// on others
		this.location += (new Vector3(-4.822f, model.transform.position.y, UnityEngine.Random.Range(-2f, 2f)) - this.location).normalized * 0.03f;
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
				// Vector3 locationWithRadius = new Vector3(
				// 	this.location.x + Parameters.particleRedius,
				// 	this.location.y + Parameters.particleRedius,
				// 	this.location.z + Parameters.particleRedius
				// );
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
			return true;
		else
			return false;
	}

	public void Draw()
	{
		// Gizmos.color = this.color;
		// Gizmos.DrawSphere(this.location, Parameters.particleRedius);

		/// new 
		for (int i = 0; i < pathHistory.Count - 1; i++)
		{
			Gizmos.color = color;
			Gizmos.DrawLine(pathHistory[i], pathHistory[i + 1]);
		}
	}

	public Bounds getRejoinAround()
	{
		return new Bounds(this.location, new Vector3(0.5f, 0.5f, 0.5f));
	}
}
