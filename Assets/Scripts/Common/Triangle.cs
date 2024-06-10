using UnityEngine;
using System;

public class Triangle : AbstractObject
{
	public Vector3 vertex1;
	public Vector3 vertex2;
	public Vector3 vertex3;

	public Triangle(Vector3 v1, Vector3 v2, Vector3 v3)
	{
		vertex1 = v1;
		vertex2 = v2;
		vertex3 = v3;
		this.location = (vertex1 + vertex2 + vertex3) / 3.0f;
	}

	public void Draw()
	{
		Gizmos.color = new Color(1, 1, 0); ;
		Gizmos.DrawLine(vertex1, vertex2);
		Gizmos.DrawLine(vertex2, vertex3);
		Gizmos.DrawLine(vertex3, vertex1);
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
}
