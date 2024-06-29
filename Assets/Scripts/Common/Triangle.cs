using UnityEngine;
using System;
using TMPro;
using Unity.VisualScripting;
using System.Runtime.InteropServices;
using System.Linq;
using System.Collections.Generic;

public class Triangle : AbstractObject
{
	public Vector3 vertex1;
	public Vector3 vertex2;
	public Vector3 vertex3;
	public float springForce = 20f;
	public float damping = 5f;
	public float uniformScale = 0.1f;
	public float maxDeformation = 0.1f;

	public float deformationSt = 1f;
	private Mesh mesh;

	private Vector3[] verts, modifiedverts;
	private int[] ids;
	private Mesh originalMesh;


	Vector3[] newVertices;
	public Triangle(Vector3 v1, Vector3 v2, Vector3 v3, Mesh sharedMesh, int[] ids, Transform transform, Vector3[] newVertices)
	{

		this.vertex1 = v1;
		this.vertex2 = v2;
		this.vertex3 = v3;
		this.mesh = sharedMesh;
		this.verts = mesh.vertices;
		this.modifiedverts = newVertices;
		this.ids = ids;
		this.location = (vertex1 + vertex2 + vertex3) / 3.0f;
	}



	void RecalculateMesh()
	{


		this.mesh.vertices = this.modifiedverts;
		this.mesh.RecalculateNormals();
		// mesh.RecalculateBounds();
		// mesh.RecalculateTangents();

	}

	public void Update() { }

	public void Deformation(Vector3 q, float dist, Vector3 normal)
	{
		// if hit 
		for (int i = 0; i < this.ids.Length; i++)
		{
			//Debug.Log("before_modifiedverts " + i + " :" + this.modifiedverts[this.ids[i]]);
			//Debug.Log(this.ids.Length);
			// Vector3 distance = modifiedverts[this.ids[i]] - q;
			float smoothingFactor = 0.2f;
			float force = deformationSt / (0.1f + q.sqrMagnitude);
			// modifiedverts[i] = modifiedverts[i] + Vector3.down * force / smoothingFactor;

			// modifiedverts[this.ids[i]] = this.titi.InverseTransformPoint(modifiedverts[this.ids[i]]) + q * force / smoothingFactor;
			this.modifiedverts[this.ids[i]] = this.modifiedverts[this.ids[i]] + 0.0000001f * -normal;
			//Debug.Log("after_modifiedverts " + i + " :" + this.modifiedverts[this.ids[i]]);
			// modifiedverts[0] = modifiedverts[0] + new Vector3(0, 0.00001f, 0);
			// if (distance.magnitude < dist)
			// {
			// 	modifiedverts[this.ids[i]] = modifiedverts[this.ids[i]] + normal * force / smoothingFactor;
			// }

			// else
			// {
			// 	modifiedverts[i] = modifiedverts[i] + Vector3.down * force / smoothingFactor;
			// }

		}
		RecalculateMesh();
	}



	// UpdateVertex(ref vertex1, originalVertex1, ref vertexVelocities[0]);
	// UpdateVertex(ref vertex2, originalVertex2, ref vertexVelocities[1]);
	// UpdateVertex(ref vertex3, originalVertex3, ref vertexVelocities[2]);

	// this.location = (vertex1 + vertex2 + vertex3) / 3.0f;
	// UpdateMesh();


	// private void UpdateVertex(ref Vector3 vertex, Vector3 originalVertex, ref Vector3 velocity)
	// {
	// 	// Vector3 displacement = vertex - originalVertex;
	// 	// displacement *= uniformScale;
	// 	// velocity -= displacement * springForce * Time.deltaTime;
	// 	// velocity *= 1f - damping * Time.deltaTime;
	// 	// vertex += velocity * (Time.deltaTime / uniformScale);
	// 	// vertex = originalVertex + new Vector3(0.01f, 0.01f, 0.01f);
	// 	// Limit the deformation

	// 	// if (Vector3.Distance(vertex, originalVertex) > maxDeformation)
	// 	// {
	// 	// 	// vertex = originalVertex + (vertex - originalVertex).normalized * maxDeformation;
	// 	// 	vertex = originalVertex + new Vector3(0.01f, 0.01f, 0.01f);
	// 	// }
	// }
	// private void UpdateMesh()
	// {
	// 	Vector3[] vertices = mesh.vertices;
	// 	vertices[triangleIndices[0]] = vertex1;
	// 	vertices[triangleIndices[1]] = vertex2;
	// 	vertices[triangleIndices[2]] = vertex3;
	// 	mesh.vertices = vertices;
	// 	// mesh.RecalculateNormals();
	// }

	// public void ApplyDeformingForce(Vector3 point, float force)
	// {
	// 	ApplyForceToVertex(ref vertex1, point, force, ref vertexVelocities[0]);
	// 	ApplyForceToVertex(ref vertex2, point, force, ref vertexVelocities[1]);
	// 	ApplyForceToVertex(ref vertex3, point, force, ref vertexVelocities[2]);
	// }

	// private void ApplyForceToVertex(ref Vector3 vertex, Vector3 point, float force, ref Vector3 velocity)
	// {
	// 	// Vector3 pointToVertex = vertex - point;
	// 	// pointToVertex *= uniformScale;
	// 	// float attenuatedForce = force / (0.1f + pointToVertex.sqrMagnitude);
	// 	// float velocityChange = attenuatedForce * Time.deltaTime;
	// 	// velocity += pointToVertex.normalized * velocityChange;
	// }


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
