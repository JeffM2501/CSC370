using UnityEngine;
using System.Collections;

public class PodMechanics : MonoBehaviour
{
    public GameObject PodModel = null;

    Vector3 TargetOrientation = Vector3.up;

	void Start ()
	{
	
	}

	void Update ()
	{
        PodModel.transform.rotation = Quaternion.Slerp(PodModel.transform.rotation, Quaternion.LookRotation(TargetOrientation), Time.deltaTime);
	}

    void OnCollisionEnter(Collision col)
    {

        if (col.collider.gameObject.tag == "DriveableSurface")
        {
            float angle = 0;
            col.transform.rotation.ToAngleAxis(out angle, out TargetOrientation);
        }
    }
}
