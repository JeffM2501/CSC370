using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour
{
	void Start ()
	{
	
	}

    public static Vector3 OriginOffset = new Vector3(0, 0.5f, 0);

    public void Move(Vector3 dir)
    {
        float dist = dir.magnitude;
        dir.Normalize();
        Ray ray = new Ray(this.transform.position + OriginOffset, dir);
        RaycastHit hit;

        float rad = 0;

        CapsuleCollider collider = this.gameObject.collider as CapsuleCollider;
        if (collider != null)
        {
            rad = collider.radius;
        }

        Debug.DrawRay(ray.origin, ray.direction, Color.blue, 0.025f);
        if (Physics.Raycast(ray, out hit, dist + rad) && hit.collider.gameObject.tag != "LootDrop")
        {
            //      print("Hit Distance " + hit.distance.ToString());
            //     print("Radius " + rad.ToString());

            if (hit.distance < rad)
                return;

            dir *= hit.distance - rad;
        }
        else
            dir *= dist;

        this.transform.position = this.transform.position + dir;
    }

	void Update ()
	{
	    
	}
}
