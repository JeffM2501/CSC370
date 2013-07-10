using UnityEngine;
using System.Collections;

public class ShotMeter : MonoBehaviour 
{
    public float ShotStrenght = 0;

    void OnGUI()
    {
        GUI.Box (new Rect (0,0,220,40), "Force");
        ShotStrenght = Slider(new Rect(10, 20, 200, 30), ShotStrenght);
    }

    float Slider (Rect screenRect, float strength ) 
    {
        strength = GUI.HorizontalSlider(screenRect, strength, 0.0f, 1.0f);
        return strength;
    }

	void Start ()
	{
	
	}
	
	void Update ()
	{
	
	}
}
