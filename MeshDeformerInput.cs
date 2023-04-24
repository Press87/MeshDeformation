using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshDeformerInput : MonoBehaviour
{
	[Header("Force magnitude to Apply")]
	[Tooltip("this is the amount of force")]
	[SerializeField]
    float force = 10f;
   

	public float Force { get => force; set => force = value; }
	public float ForceOffset { get => forceOffset; set => forceOffset = value; }
	[Header("force offset to Apply")]
	[SerializeField]
    float forceOffset = 0.1f;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            HandleInput();
        }
    }




	#region  inputs
	/// <summary>
	/// check the input point for deformation
	/// </summary>
	void HandleInput()
    {
        Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(inputRay, out hit))
        {
            MeshDeformer deformer = hit.collider.GetComponent<MeshDeformer>();
            if (deformer)
            {
                Vector3 point = hit.point;
                point += hit.normal * ForceOffset;
                deformer.AddDeformingForce(point, Force);
            }
        }
    }
	#endregion
}
