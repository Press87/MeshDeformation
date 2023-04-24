using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(MeshFilter))]
public class MeshDeformer : MonoBehaviour
{
	[Header("mesh to deform reference")]
	Mesh deformingMesh;
	[Space]
	[Header("Vertex mesh")]
	Vector3[] originalVertices, displacedVertices;
	Vector3[] vertexVelocities;
	[Space]
	[SerializeField]
    float springForce = 20f;
	[SerializeField]
    float damping = 5f;
	#region Getreference
	private void Start()
	{
		deformingMesh = GetComponent<MeshFilter>().mesh;
		originalVertices = deformingMesh.vertices;
		displacedVertices = new Vector3[originalVertices.Length];
		vertexVelocities = new Vector3[originalVertices.Length];
		for (int i = 0; i < originalVertices.Length; i++)
		{
			displacedVertices[i] = originalVertices[i];
		}
	}
	#endregion
	/// <summary>
	/// add deformation to a mesh on point nd a specific force
	/// </summary>
	/// <param name="point"></param>
	/// <param name="force"></param>
	#region adding force
	public void AddDeformingForce(Vector3 point, float force)
	{
		for (int i = 0; i < displacedVertices.Length; i++)
		{
			AddForceToVertex(i, point, force);
		}
	}

	void AddForceToVertex(int i, Vector3 point, float force)
	{
		Vector3 pointToVertex = displacedVertices[i] - point;
		float attenuatedForce = force / (1f + pointToVertex.sqrMagnitude);
		float velocity = attenuatedForce * Time.deltaTime;
		vertexVelocities[i] += pointToVertex.normalized * velocity;
	}
	#endregion
	void Update()
	{
		for (int i = 0; i < displacedVertices.Length; i++)
		{
			UpdateVertex(i);
		}
		deformingMesh.vertices = displacedVertices;
		deformingMesh.RecalculateNormals();
	}
	/// <summary>
	/// apply the deformation foreach vertex
	/// </summary>
	/// <param name="i"></param>
	#region apply the force
	void UpdateVertex(int i)
	{
		Vector3 velocity = vertexVelocities[i];
		Vector3 displacement = displacedVertices[i] - originalVertices[i];
		velocity -= displacement * springForce * Time.deltaTime;
		velocity *= 1f - damping * Time.deltaTime;
		vertexVelocities[i] = velocity;
		displacedVertices[i] += velocity * Time.deltaTime;
	}
	#endregion




}
