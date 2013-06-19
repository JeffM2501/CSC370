using UnityEngine;
using System.Collections;

public class Blob : MonoBehaviour 
{
	void Start ()
	{
	
	}
	
	void Update ()
	{
	
	}

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.name != "First Person Controller")
        {
            rigidbody.isKinematic = true;
            Destroy(this.collider);
            ContactPoint contact = col.contacts[0];
            Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal);

            transform.position = contact.point;
            transform.rotation = rot;

            float x = transform.localScale.x * col.relativeVelocity.magnitude/5.0f;
            float y = transform.localScale.y * 2.0f;
            float z = transform.localScale.z * col.relativeVelocity.magnitude / 5.0f;

            transform.localScale = new Vector3(x, y, z);
        }
    }
}
