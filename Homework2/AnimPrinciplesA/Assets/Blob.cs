using UnityEngine;
using System.Collections;

public class Blob : MonoBehaviour 
{
    public GameObject Explosion;
    public float ExplosionLife = 5.0f;
    public float DetailLevel = 1.0f;

    public float Countdown = 5.0f;

    private float Timeset = 0;
    private bool ExplosionActive = false;
    private Quaternion ExplosionDirecton;
    private Vector3 ExplosionLocation = Vector3.zero;

	void Start ()
	{
	
	}
	
	void Update ()
	{
        if (Time.fixedTime >= Timeset && ExplosionActive)
        {
            print("Boom");
            Explode();
            ExplosionActive = false;
            Destroy(this.gameObject);
        }
	}

    void Explode()
    {
        GameObject exp = Instantiate(Explosion, ExplosionLocation, ExplosionDirecton) as GameObject;
        (exp.GetComponent("Detonator") as Detonator).detail = DetailLevel;
        Destroy(exp, ExplosionLife);
    }

    void OnCollisionEnter(Collision collision)
    {
        print("collision");
	    if(collision.gameObject.name != "First Person Controller")
	    {
		    rigidbody.isKinematic = true;
		    Destroy (this.collider);
		
		    ContactPoint contact = collision.contacts[0];
            Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal);

            ExplosionDirecton = rot;

            float offsetSize = (Explosion.GetComponent("Detonator") as Detonator).size / 3.0f;

            ExplosionLocation = contact.point + contact.normal * offsetSize;

		    this.transform.position = contact.point;
		    this.transform.rotation = rot;

            float x = transform.localScale.x * collision.relativeVelocity.magnitude / 5.0f;
            float y = transform.localScale.y * 2.0f;
            float z = transform.localScale.z * collision.relativeVelocity.magnitude / 5.0f;

            transform.localScale = new Vector3(x, y, z);

            Timeset = Time.fixedTime + Countdown;
            ExplosionActive = true;
	    }
    }
}
