using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class HideSouths : MonoBehaviour 
{
    GameObject HiddenObject = null;

    List<GameObject> HiddenObjects = new List<GameObject>();

    public GameObject TargetObject;

    public Vector3 TargetCenterOffset = new Vector3(0, 0.5f, 0);

	void Start ()
	{

	}

    Ray ScreenPointToTarget(Vector3 target, float x, float y, float depth)
    {
        Vector3 source = Camera.main.ScreenToWorldPoint(new Vector3(x, y, depth));
        return new Ray(source, target - source);
    }
	
	void Update ()
	{
        if (TargetObject == null)
            return;

        foreach (GameObject o in HiddenObjects)
        {
            o.collider.enabled = true;
            o.renderer.enabled = true;
        }

        HiddenObjects.Clear();

       // if (HiddenObject != null)
       //     HiddenObject.renderer.enabled = true;
        
        HiddenObject = null;

        Vector3 targetPos = TargetObject.transform.position + TargetCenterOffset;

        List<Ray> RaysToTest = new List<Ray>();

        float width = Camera.main.pixelWidth;
        float height = Camera.main.pixelHeight;

        RaysToTest.Add(new Ray(Camera.main.transform.position, targetPos - Camera.main.transform.position));

        float depth = 1;

        RaysToTest.Add(ScreenPointToTarget(targetPos, 0, 0, depth));
        RaysToTest.Add(ScreenPointToTarget(targetPos, 0, height, depth));
        RaysToTest.Add(ScreenPointToTarget(targetPos, width, height, depth));
        RaysToTest.Add(ScreenPointToTarget(targetPos, width, 0, depth));

        // Ray ray = new Ray(Camera.mainCamera.transform.position + TargetCenterOffset, TargetObject.transform.position - Camera.mainCamera.transform.position);
      //  Ray ray = Camera.mainCamera.ScreenPointToRay(Input.mousePosition);

        foreach (Ray ray in RaysToTest)
        {
            bool done = false;
            while (!done)
            {
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, 100) && hit.transform.gameObject.tag == "SouthWalls")
                {
                    Debug.DrawRay(ray.origin, ray.direction * 100, Color.red,0.01f);
                    //  print("Hit object " + hit.transform.gameObject.ToString());
                    HiddenObject = hit.transform.gameObject;

                    HiddenObjects.Add(HiddenObject);
                    HiddenObject.renderer.enabled = false;
                    HiddenObject.collider.enabled = false;
                }
                else
                    done = true;
            }
        }

        foreach (GameObject o in HiddenObjects)
            o.collider.enabled = true;
	}
}
