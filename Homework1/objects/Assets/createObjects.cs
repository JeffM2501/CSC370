using UnityEngine;
using System.Collections;

public class createObjects : MonoBehaviour 
{
	void Start ()
	{
        GameObject capsule = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        capsule.transform.position.Set(1, 5, 2);
        capsule.AddComponent("Ridgidbody");

        PhysicMaterial material = new PhysicMaterial();
        material.bounciness = 1;
        capsule.collider.material = material;
	}
	
	void Update ()
	{
	
	}
}
