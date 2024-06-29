using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;

public class Particle : AbstractObject
{
	public float lifespan;
	public Color color;

	private List<Vector3> pathHistory = new List<Vector3>();
	// private Material particleMaterial = new Material(Shader.Find("Custom/LineShader"));
	public Particle(Vector3 location)
	{
		this.velocity = new Vector3(UnityEngine.Random.Range(-0.01f, 0.01f), 0, UnityEngine.Random.Range(-0.01f, 0.01f)) * Parameters.particlesVelocity;
		this.lifespan = 255;
		this.location = location;
		this.color = Parameters.initialParticleColor;

		/// new 
		// Shader lineShader = Shader.Find("Custom/LineShader");

		// if (lineShader == null)
		// {
		// 	Debug.LogError("Shader not found! Make sure the shader file is named correctly and placed in the project.");
		// }
		// this.particleMaterial = new Material(lineShader);

	}

	public void UpdateValues(Vector3 vel)
	{
		this.velocity += vel;
		this.location += velocity;
	}

	public void MoveToLastPoint(GameObject model)
	{
		// this.location += (new Vector3(-4.822f, model.transform.position.y + 0.5f, UnityEngine.Random.Range(-2f, 2f)) - this.location).normalized * Parameters.particlesVelocity / 20;
		this.location += (new Vector3(-1 * (Parameters.octreeWidth / 2), Parameters.octreeCenter.y / 2, Parameters.octreeCenter.z) - this.location).normalized * 0.03f;
		pathHistory.Add(this.location);
		if (pathHistory.Count > 50)
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

			if (s >= 0 && t >= 0 && (s + t) <= 1)
			{
				float distance = Vector3.Distance(this.location, q);
				if (distance <= 0.1f)
				{
					this.location += normal * Parameters.particlesVelocity / 20;

					if (Parameters.withDeformation)
					{
						triangle.Deformation(q, dist, normal);
					}
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

	public void Move()
	{
		float randomX = UnityEngine.Random.Range(-0.5f, 0f);
		float randomY = UnityEngine.Random.Range(-0.5f, 0);
		float randomZ = UnityEngine.Random.Range(-0.5f, 0.5f);

		Vector3 randomAcceleration = new Vector3(randomX, 0, 0);

		this.velocity += randomAcceleration * Time.deltaTime * Parameters.particlesVelocity;
		this.location += this.velocity * Time.deltaTime;
		this.lifespan -= 0.1f;

		pathHistory.Add(this.location);
		if (pathHistory.Count > 50)
		{
			pathHistory.RemoveAt(0);
		}
	}

	public bool isDead()
	{
		if (this.GetLocation().x <= -4.822f)
			return true;
		else
			return false;
	}

	public void Draw()
	{
		if (!Parameters.applyShader)
		{
			GL.PushMatrix();
			GL.Begin(GL.LINES);
			GL.Color(this.color);

			// Draw the sphere
			DrawSphere(this.location, Parameters.particleRadius);

			GL.End();
			GL.PopMatrix();
		}
		else
		{
			GL.PushMatrix();
			GL.Begin(GL.LINES);
			GL.Color(this.color);

			// Draw the path history
			for (int i = 0; i < pathHistory.Count - 1; i++)
			{
				GL.Vertex(pathHistory[i]);
				GL.Vertex(pathHistory[i + 1]);
			}

			GL.End();
			GL.PopMatrix();
		}
	}

	void DrawSphere(Vector3 center, float radius)
	{
		int segments = 24; // Adjust for detail
		for (int i = 0; i < segments; i++)
		{
			float theta = (i * 2.0f * Mathf.PI) / segments;
			float nextTheta = ((i + 1) * 2.0f * Mathf.PI) / segments;

			Vector3 p1 = center + new Vector3(radius * Mathf.Cos(theta), radius * Mathf.Sin(theta), 0);
			Vector3 p2 = center + new Vector3(radius * Mathf.Cos(nextTheta), radius * Mathf.Sin(nextTheta), 0);

			GL.Vertex(p1);
			GL.Vertex(p2);
		}
	}


	public Bounds getRejoinAround()
	{
		return new Bounds(this.location, new Vector3(0.5f, 0.5f, 0.5f));
	}
}
