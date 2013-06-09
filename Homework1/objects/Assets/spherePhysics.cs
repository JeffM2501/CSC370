using UnityEngine;
using System.Collections;

public class spherePhysics : MonoBehaviour 
{
	void Start ()
	{
        float bouncyAmount = 0.5f;
        Color myColor = Color.red;

        gameObject.AddComponent("Rigidbody");

        PhysicMaterial material = new PhysicMaterial();
        material.bounciness = bouncyAmount;
        gameObject.collider.material = material;

        gameObject.renderer.material.color = myColor;
	}
	
	void Update ()
	{
	
	}
}
