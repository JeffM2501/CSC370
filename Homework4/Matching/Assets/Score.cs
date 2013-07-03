using UnityEngine;
using System.Collections;

public class Score : MonoBehaviour 
{
    public GUIStyle scoreStyle;
    public GameObject[] ScoringObjects;

    private int MaxBalls = 20;

    void OnGUI()
    {
        int score = 0;
        int totalBalls = 0;

        foreach (GameObject obj in ScoringObjects)
        {
            CountBalls counter = (obj.GetComponent("CountBalls") as CountBalls);
            if (counter == null)
                continue;
            score += counter.Score;
            totalBalls += counter.TotalBalls;
        }

        if (totalBalls > MaxBalls)
        {
            PlayerPrefs.SetInt("LastScore", score);
            Application.LoadLevel("gameover");
        }

        GUI.BeginGroup(new Rect(Screen.width - 85, 5, 80, 80));
        GUI.Box(new Rect(0, 0, 80, 60), "Score");
        GUI.Label(new Rect(0, 20, 80, 30), string.Format("{0:0}", score), scoreStyle);
        GUI.EndGroup();
    }

}
