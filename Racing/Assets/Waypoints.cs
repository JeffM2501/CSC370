using UnityEngine;
using System.Collections;

public class Waypoints : MonoBehaviour 
{
    public int currentWP  = 0;    public GameObject[] WaypointList;    public float Speed = 5;    public float TurnSpeed = 2;    public float Accuracy = 5;

    Vector3 StartingLocation;
    Quaternion StaringOrintation;

	void Start ()
	{
        StartingLocation = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        StaringOrintation = new Quaternion(transform.rotation.x, transform.rotation.y, transform.rotation.z,transform.rotation.w);
	}

    public void Reset()
    {
        transform.position = new Vector3(StartingLocation.x, StartingLocation.y, StartingLocation.z);
        transform.rotation = new Quaternion(StaringOrintation.x, StaringOrintation.y, StaringOrintation.z, StaringOrintation.w);
    }
	
	void Update ()
	{
        if (WaypointList.Length == 0) return;
        if (Vector3.Distance(WaypointList[currentWP].transform.position, transform.position) < Accuracy)
        {
            currentWP++;
            if (currentWP >= WaypointList.Length)
            {
                currentWP = 0;
            }
        }

        //rotate towards target
        Vector3 direction = WaypointList[currentWP].transform.position - transform.position;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), TurnSpeed * Time.deltaTime);
        transform.Translate(0, 0, Time.deltaTime * Speed);
	}
}
