using UnityEngine;
using System.Collections;

public class Explode : MonoBehaviour 
{
    public GameObject Explosion;

    protected Vector3 ExplosionSize;

	void Start ()
	{
        Detonator det = Explosion.GetComponent("Detonator") as Detonator;
        ExplosionSize = new Vector3(det.size, det.size, det.size);
	}
	
	void Update ()
	{
	
	}

    void OnCollisionEnter(Collision col)
    {
        rigidbody.AddExplosionForce(3000, col.contacts[0].point, 3000);
        Destroy(gameObject);
        Vector3 pos = col.contacts[0].point + Vector3.Scale(col.contacts[0].normal, ExplosionSize);

        GameObject splode = Instantiate(Explosion, pos, Quaternion.FromToRotation(Vector3.up, col.contacts[0].normal)) as GameObject;

        (splode.GetComponent("Detonator") as Detonator).detail = 1;
        Destroy(splode, 5);

    }
}
