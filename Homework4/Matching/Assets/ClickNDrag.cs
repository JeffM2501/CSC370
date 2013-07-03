using UnityEngine;
using System.Collections;

public class ClickNDrag : MonoBehaviour 
{
    private GameObject FocusObj = null;
    private Vector3 LastMousePosition;
    private Vector3 DeltaMousePosition;

    public float MouseSensitivity = 40.0f;

	void Start ()
	{
	
	}
	
	void Update ()
	{
        if (FocusObj != null && FocusObj.tag != "ball")
            FocusObj = null;

        if (Input.GetButtonDown("Fire1"))
        {
            FocusObj = null;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100) && hit.transform.gameObject.tag == "ball")
            {
                FocusObj = hit.transform.gameObject;
                LastMousePosition = Input.mousePosition;
            }
        }

        if (FocusObj != null && Input.GetButton("Fire1"))
        {
            DeltaMousePosition = Input.mousePosition - LastMousePosition;

            FocusObj.transform.Translate(DeltaMousePosition.x / MouseSensitivity, 0, 0);
            LastMousePosition = Input.mousePosition;
        }

        if (FocusObj != null && Input.GetButtonUp("Fire1"))
            FocusObj = null;
	}
}
