using UnityEngine;
using System.Collections;

public class Fire : MonoBehaviour 
{
    public GameObject BulletObject;

	void Start ()
	{
	
	}
	
	void Update ()
	{
        if (Input.GetButtonDown("Fire1"))
        {
            GameObject bullet = Instantiate(BulletObject, transform.position, transform.rotation) as GameObject;
            bullet.rigidbody.AddForce(transform.forward * 500);

            Destroy(bullet, 60);
        }
	}
}
