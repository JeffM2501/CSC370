using UnityEngine;
using System.Collections;

public class HideSouths : MonoBehaviour 
{
    GameObject HiddenObject = null;

    public GameObject TargetObject;

    public Vector3 TargetCenterOffset = new Vector3(0, 0.5f, 0);

	void Start ()
	{

	}
	
	void Update ()
	{
        if (TargetObject == null)
            return;

        if (HiddenObject != null)
            HiddenObject.renderer.enabled = true;
        
        HiddenObject = null;

        Ray ray = new Ray(Camera.mainCamera.transform.position + TargetCenterOffset, TargetObject.transform.position - Camera.mainCamera.transform.position);
      //  Ray ray = Camera.mainCamera.ScreenPointToRay(Input.mousePosition);

   

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100) && hit.transform.gameObject.tag == "SouthWalls")
        {
            Debug.DrawRay(ray.origin,ray.direction * 100, Color.red);
          //  print("Hit object " + hit.transform.gameObject.ToString());
            HiddenObject = hit.transform.gameObject;
            HiddenObject.renderer.enabled = false;
        }
	}
}
