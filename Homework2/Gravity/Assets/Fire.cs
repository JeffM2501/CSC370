using UnityEngine;
using System.Collections;

public class Fire : MonoBehaviour
{
    public bool UseGravity = true;
    public float ShotForce = 2000;

    public GameObject ThingToShoot;

	// Use this for initialization
	void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            GameObject shot = null;

            if (ThingToShoot != null)
                shot = Instantiate(ThingToShoot) as GameObject;
            else
                shot =GameObject.CreatePrimitive(PrimitiveType.Sphere);
            shot.transform.position = ray.origin;
            shot.AddComponent("Rigidbody");
            shot.rigidbody.AddForce(ray.direction * ShotForce);
            shot.rigidbody.useGravity = UseGravity;
            shot.rigidbody.mass = 10;
            Destroy(shot, 10);
        }
	}
}
