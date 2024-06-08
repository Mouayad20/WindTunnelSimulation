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
		Gizmos.color = new Color(1, 1, 0);
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

	// Method to check if a point is near the triangle
	public bool IsPointNear(Vector3 point, float threshold)
	{
		// Calculate the normal of the triangle
		Vector3 edge1 = vertex2 - vertex1;
		Vector3 edge2 = vertex3 - vertex1;
		Vector3 normal = Vector3.Cross(edge1, edge2).normalized;

		// Calculate the distance from the point to the plane of the triangle
		float distanceToPlane = Mathf.Abs(Vector3.Dot(normal, point - vertex1));

		// If the distance to the plane is greater than the threshold, the point is not near the triangle
		if (distanceToPlane > threshold)
		{
			return false;
		}

		// Project the point onto the plane of the triangle
		Vector3 projectedPoint = point - distanceToPlane * normal;

		// Check if the projected point is within the triangle using barycentric coordinates
		Vector3 v0 = vertex2 - vertex1;
		Vector3 v1 = vertex3 - vertex1;
		Vector3 v2 = projectedPoint - vertex1;

		float dot00 = Vector3.Dot(v0, v0);
		float dot01 = Vector3.Dot(v0, v1);
		float dot02 = Vector3.Dot(v0, v2);
		float dot11 = Vector3.Dot(v1, v1);
		float dot12 = Vector3.Dot(v1, v2);

		float invDenom = 1.0f / (dot00 * dot11 - dot01 * dot01);
		float u = (dot11 * dot02 - dot01 * dot12) * invDenom;
		float v = (dot00 * dot12 - dot01 * dot02) * invDenom;

		// Check if the point is inside the triangle
		return (u >= 0) && (v >= 0) && (u + v < 1);
	}
}
