using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour 
{
    void OnGUI()
    {
        // Make a group on the center of the screen
        GUI.BeginGroup(new Rect(Screen.width / 2 - 50, Screen.height / 2 - 60, 110, 120));
        // All rectangles are now adjusted to the group. (0,0) is the topleft corner of the group.

        // We'll make a box so you can see where the group is on-screen.
        GUI.Box(new Rect(0, 0, 110, 120), "Main Menu");
        if (GUI.Button(new Rect(10, 40, 90, 30), "Play Level 1"))
        {
            PlayerPrefs.SetInt("Level", 1);
            Application.LoadLevel("maingame");
        }
        if (GUI.Button(new Rect(10, 80, 90, 30), "Play Level 2"))
        {
            PlayerPrefs.SetInt("Level", 2);
            Application.LoadLevel("maingame");
        }

        // End the group we started above. This is very important to remember!
        GUI.EndGroup();
    }
}
