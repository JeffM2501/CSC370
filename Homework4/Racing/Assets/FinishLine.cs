using UnityEngine;
using System.Collections;

public class FinishLine : MonoBehaviour 
{
    private string Winner = string.Empty;

    private Vector3 PlayerLocation = Vector3.zero;
    private Vector3 NPCLocation = Vector3.zero;

	void Start ()
	{
        PlayerLocation = GameObject.Find("Car Prefab").transform.position;
        NPCLocation = GameObject.Find("Player2").transform.position;
	}

    void  OnTriggerEnter(Collider collision)
    {
        //if there is no winner yet
        if (Winner == string.Empty)
        {
            //make this one the winner
            Winner = collision.gameObject.name;
        }
    }

    void OnGUI ()
    {
        if(Winner != string.Empty)
        {
            GUI.BeginGroup(new Rect(Screen.width / 2 - 100f, Screen.height / 2 - 100, 200, 200));

            GUI.Box (new Rect (0,0,200,200), "Game Over");
            GUI.Label(new Rect (20,20,200,100), "Winner: " + Winner);
            if(GUI.Button (new Rect (50,60,100,30), "Play Again"))
            {
                //reset the game
                Winner ="";
                GameObject.Find("Car Prefab").transform.position =  PlayerLocation;
                GameObject.Find("Player2").transform.position = NPCLocation;
                (GameObject.Find("Player2").GetComponent("Waypoints") as Waypoints).Reset();
            }
            GUI.EndGroup ();
        }
    }

	void Update ()
	{
	
	}
}
