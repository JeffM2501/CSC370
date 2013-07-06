using UnityEngine;
using System.Collections;

public class LayerTest : MonoBehaviour
 {
    Mesh CharacterMesh;

	void Start ()
	{
        CharacterMesh = (GetComponent(typeof(MeshFilter)) as MeshFilter).mesh;

        gameObject.renderer.materials[1].color = Color.blue;
        gameObject.renderer.materials[3].color = Color.green;
       // CharacterMesh.u
	}

	void Update () 
	{
	
	}
}
