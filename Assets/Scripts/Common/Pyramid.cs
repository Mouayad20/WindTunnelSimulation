using System.Collections.Generic;
using UnityEngine;

public class Pyramid
{
	public List<Triangle> triangles;

	public Pyramid(Vector3 baseCenter, float baseSize, float height)
	{
		triangles = new List<Triangle>();

		Vector3 baseVertex1 = baseCenter + new Vector3(-baseSize / 2, 0, -baseSize / 2);
		Vector3 baseVertex2 = baseCenter + new Vector3(baseSize / 2, 0, -baseSize / 2);
		Vector3 baseVertex3 = baseCenter + new Vector3(baseSize / 2, 0, baseSize / 2);
		Vector3 baseVertex4 = baseCenter + new Vector3(-baseSize / 2, 0, baseSize / 2);

		Vector3 apex = new Vector3(0, height, 0);

		triangles.Add(new Triangle(baseVertex1, baseVertex2, apex));
		triangles.Add(new Triangle(baseVertex2, baseVertex3, apex));
		triangles.Add(new Triangle(baseVertex3, baseVertex4, apex));
		triangles.Add(new Triangle(baseVertex4, baseVertex1, apex));

		triangles.Add(new Triangle(baseVertex1, baseVertex2, baseVertex3));
		triangles.Add(new Triangle(baseVertex1, baseVertex3, baseVertex4));
	}

	public void Draw()
	{
		foreach (Triangle triangle in triangles)
		{
			triangle.Draw();
		}
	}
}
