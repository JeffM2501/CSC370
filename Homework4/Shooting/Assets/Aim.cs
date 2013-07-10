using UnityEngine;
using System.Collections;

public class Aim : MonoBehaviour 
{
    public GameObject Cannonball;
    public AudioClip FireSound;
    public AudioSource Source;
    public ParticleSystem Poofer;

    public float ShotForce = 5000;

	void Update ()
	{
        if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.RotateAround(transform.position, transform.right, 20.0f * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.RotateAround(transform.position, transform.right, -20.0f * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.RotateAround(transform.position, transform.forward, 20.0f * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.RotateAround(transform.position, transform.forward, -20.0f * Time.deltaTime);
        }

        ShotMeter meter = Camera.main.GetComponent("ShotMeter") as ShotMeter;

        if (Input.GetKey(KeyCode.Space))
            meter.ShotStrenght += (0.5f * Time.deltaTime);

        if (Input.GetKeyUp(KeyCode.Space))
        {
            GameObject shot = Instantiate(Cannonball, transform.position, transform.rotation) as GameObject;

            print(meter.ShotStrenght.ToString());
            float force = meter.ShotStrenght * ShotForce;

            shot.rigidbody.AddForce(transform.up * force);
            Destroy(shot, 15);
            Source.PlayOneShot(FireSound);

            ParticleSystem poof = Instantiate(Poofer, transform.position, transform.rotation) as ParticleSystem;
            Destroy(poof, 5);
          //  poof.Simulate();

            meter.ShotStrenght = 0;
        }
	}
}
