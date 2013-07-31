using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Patrol : MonoBehaviour
{
    public List<GameObject> Waypoints = new List<GameObject>();

    public GameObject Doorway = null;
    public GameObject Driveway = null;

    public Graph WaypointGraph = new Graph();

    public int CurrentWaypoint = -1;
    private float Speed = 8;
    private float RotationSpeed = 5;
    private float Accuracy = 1.0f;


    protected GameObject CurrentWaypointObject = null;

    void OnGUI()
    {
        GameObject obj = Waypoints[0];
        if (CurrentWaypoint >= 0)
            obj = Waypoints[CurrentWaypoint];

        GUI.Box(new Rect(10, 10, 100, 110), "Guard's Orders");
        if (GUI.Button(new Rect(20, 40, 80, 20), "Patrol"))
        {
            CurrentWaypoint = 0;
            WaypointGraph.AStar(Waypoints[0], Waypoints[Waypoints.Count - 1]);
            animation.Play("run");
        }

        if (GUI.Button(new Rect(20, 60, 80, 20), "Doorway"))
        {
            WaypointGraph.AStar(obj, Doorway);
            animation.Play("run");
        }

        if (GUI.Button(new Rect(20, 80, 80, 20), "Driveway"))
        {
            WaypointGraph.AStar(obj, Driveway);
            animation.Play("run");
        }
    }

	// Use this for initialization
	void Start ()
    {
        for (int i = 0; i < Waypoints.Count; i++)
        {
            WaypointGraph.AddNode(Waypoints[i], true, true);
            if (i > 0)
            {
                WaypointGraph.AddEdge(Waypoints[i - 1], Waypoints[i]);
                WaypointGraph.AddEdge(Waypoints[i], Waypoints[i - 1]);
            }
            if (i == Waypoints.Count - 1)
            {
                WaypointGraph.AddEdge(Waypoints[i], Waypoints[0]);
                WaypointGraph.AddEdge(Waypoints[0], Waypoints[i]);
            }
        }

        WaypointGraph.AddNode(Doorway, true, true);
        WaypointGraph.AddNode(Driveway, true, true);

        WaypointGraph.AddEdge(Doorway, Driveway);
        WaypointGraph.AddEdge(Driveway, Doorway);

        WaypointGraph.AddEdge(Waypoints[0], Driveway);
        WaypointGraph.AddEdge(Driveway, Waypoints[0]);

        WaypointGraph.AddEdge(Waypoints[1], Doorway);
        WaypointGraph.AddEdge(Doorway, Waypoints[1]);

        animation["run"].wrapMode = WrapMode.Loop;
	}

    Vector3 DeltaToWaypoint()
    {
        Vector3 vec = WaypointGraph.getPathPoint(CurrentWaypoint).transform.position -  transform.position;

        vec.Set(vec.x, 0, vec.z);
        return vec;
    }

    // Update is called once per frame
    void Update()
    {
        WaypointGraph.debugDraw();

        int pathLength = WaypointGraph.getPathLength();

        if (pathLength == 0 || CurrentWaypoint == pathLength)
        {
            animation.Play("idle");
            return;
        }

        Vector3 direction = DeltaToWaypoint();

        if (Vector3.Magnitude(direction) < Accuracy)
            CurrentWaypoint++;
        else
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), RotationSpeed * Time.deltaTime);
            transform.Translate(0, 0, Time.deltaTime * Speed);
        }
	}
}
