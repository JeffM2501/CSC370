using UnityEngine;
using System.Collections;

public class ShowScore : MonoBehaviour
{
    public Texture2D GoImage;

    void OnGUI()
    {
        string scoreText = "You Scored " + PlayerPrefs.GetInt("LastScore").ToString();
        GUI.BeginGroup(new Rect(Screen.width / 2 - 100, Screen.height / 2 - 100, 200, 200));

        GUI.Box(new Rect(0, 0, 200, 200), GoImage);
        GUI.Label(new Rect(55, 15, 100, 30), scoreText);

        if (GUI.Button(new Rect(25, 165, 150, 30), "Back to Menu"))
            Application.LoadLevel("mainmenu");

        GUI.EndGroup();
    }
}
