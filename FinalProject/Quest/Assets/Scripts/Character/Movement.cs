using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour
{
	void Start ()
	{
	
	}

    public void Move(Vector2 dir)
    {
        Ray ray = new Ray(this.transform.position,dir);
        RaycastHit hit;

        if (this.collider.Raycast(ray, out hit, dir.magnitude))
        {
            dir.Normalize();
            dir *= hit.distance;
        }

        this.transform.position = this.transform.position + new Vector3(dir.x, 0, dir.y);
    }

	void Update ()
	{
	    
	}
}
