using UnityEngine;
using System.Collections;

public class hover : MonoBehaviour
{
    public float HoverDistance = 30;
    public float HoverForce = 25;

	void Start ()
	{
	
	}

	void Update ()
	{
        Vector3 pos = new Vector3(transform.position.x, transform.position.y - transform.localScale.y, transform.position.z);

        Ray downRay = new Ray(pos, Vector3.down);
        RaycastHit hit;

        Debug.DrawRay(pos, Vector3.down * HoverDistance);

        if (Physics.Raycast(downRay, out hit, HoverDistance))
        {
            if (hit.collider.tag == "Ground")
            {
                float param = 1.0f - (hit.distance / HoverDistance);
           //     if (this.rigidbody.velocity.y < 0)
                {
                    //    print("Hit " + hit.collider.tag + " " + param.ToString());

                    this.rigidbody.AddForce(Vector3.up * (HoverForce * param),ForceMode.Acceleration);
                }
            }
        }
	}

    void OnCollisionEnter(Collision col)
    {
        print("bam");
        this.rigidbody.AddForce(Vector3.up * 500);
    }
}
