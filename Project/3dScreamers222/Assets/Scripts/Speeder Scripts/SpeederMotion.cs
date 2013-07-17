using UnityEngine;
using System.Collections;

public class SpeederMotion : MonoBehaviour
{
    public float HoverEffectDistance = 1;
    public float HoverEffectForce = 25;

	void Start ()
	{
	
	}

	void Update ()
	{
        Ray YonRay = new Ray(transform.position,Vector3.down * HoverEffectDistance);
        RaycastHit hit = new RaycastHit();

        if (Physics.Raycast(YonRay, out hit) && hit.distance > 0)
        {
            float param = (hit.distance / HoverEffectDistance);

            gameObject.rigidbody.AddForce(Vector3.down * (HoverEffectForce * param));

            print("Hit with " + param.ToString());
        }
	}
}
