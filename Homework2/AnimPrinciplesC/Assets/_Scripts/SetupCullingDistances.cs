
using UnityEngine;
using System.Collections;

public class SetupCullingDistances : MonoBehaviour
{
	public float small = 13;
	public float medium = 13;
	public float far = 13;
	
	void Start ()
	{
		float[] distances = new float[32];
		distances[10] = small;
		distances[11] = medium;
		distances[12] = far;
		camera.layerCullDistances = distances;
	}
}
