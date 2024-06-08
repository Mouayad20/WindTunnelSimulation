using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Particle : AbstractObject
{
	public float radius;
	public float lifespan;
	public Color color;

	public Particle(Vector3 location)
	{
		// this.obj = gameObject;
		this.acceleration = new Vector3(0, -0.05f, 0);
		this.velocity = new Vector3(UnityEngine.Random.Range(-0.05f, 0.05f), 0.05f, UnityEngine.Random.Range(-0.05f, 0.05f));
		this.lifespan = 255;
		this.location = location;
		this.color = new Color(0f, 0.5f, 1f);
	}

	public void UpdateValues2(Vector3 vel)
	{
		// acceleration += force / mass;
		this.velocity = vel; ;
		this.obj.transform.position += velocity;
	}

	public override bool DetectCollision(AbstractObject anotherObject)
	{
		if (anotherObject is Particle anotherParticle)
		{
			if (Math.Sqrt(Math.Pow(anotherObject.GetObj().transform.position.x - this.obj.transform.position.x, 2) +
				Math.Pow(anotherObject.GetObj().transform.position.y - this.obj.transform.position.y, 2) +
				Math.Pow(anotherObject.GetObj().transform.position.z - this.obj.transform.position.z, 2)) <= 10)
			{
				return true;
			}
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
		float randomX = UnityEngine.Random.Range(-1f, 0f);
		float randomY = UnityEngine.Random.Range(-1f, 0);
		float randomZ = UnityEngine.Random.Range(-0.5f, 0.5f);

		Vector3 randomAcceleration = new Vector3(randomX, 0, randomZ);

		this.velocity += randomAcceleration * Time.deltaTime;
		this.location += this.velocity * Time.deltaTime;
		this.lifespan -= 1f;
		this.color = new Color(0f, 1f, 1f, this.lifespan / 255f);
	}

	public bool isDead()
	{
		if (this.lifespan <= 0)
			return true;
		else
			return false;
	}

	public void Draw()
	{
		Gizmos.color = this.color;
		Gizmos.DrawSphere(this.location, 0.1f);
	}

	public Bounds getRejoinAround()
	{
		return new Bounds(this.location, new Vector3(0.5f, 0.5f, 0.5f));
	}
}
