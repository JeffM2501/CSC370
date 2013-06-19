using UnityEngine;
using System.Collections;

public class lodManager : MonoBehaviour
 {
    public Mesh LOD1;
    public Mesh LOD2;

    public float LODDist1;
    public float LODDist2;

    public float UpdateInterval = 1.0f;
    public int CurrentLOD = 1;
    public MeshFilter Filter;
    public Transform ThisTrasform;

    void Awake()
    {
        Filter = this.GetComponent("MeshFilter") as MeshFilter;
        ThisTrasform = transform;
        float startIn = Random.RandomRange(0, UpdateInterval);
        InvokeRepeating("UpdateLOD", startIn, UpdateInterval);
    }

    void UpdateLOD()
    {
        float distanceFromObject = Vector3.Distance(ThisTrasform.position, Camera.main.transform.position);
        if (distanceFromObject < LODDist1)
        {
            if (CurrentLOD != 1)
            {
                CurrentLOD = 1;
                Filter.mesh = LOD1;
            }
        }
        else if (distanceFromObject < LODDist2)
        {
            if (CurrentLOD != 2)
            {
                CurrentLOD = 2;
                Filter.mesh = LOD2;
            }
        }
        else
        {
            CurrentLOD = 0;
            Filter.mesh = null;
        }
    }

	void Start ()
	{
	
	}

	void Update () 
	{
	
	}
}
