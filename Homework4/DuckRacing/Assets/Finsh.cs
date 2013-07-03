using UnityEngine;
using System.Collections;

public class Finsh : MonoBehaviour
 {
    public int DuckCount = 0;
    public GUIStyle ScoreStyle;

    public float LevelTimeout = 120;
    private bool GameOver = false;

    void Update()
    {
        if (Time.timeSinceLevelLoad > LevelTimeout)
            GameOver = true;
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "duck")
        {
            DuckCount++;
            Destroy(collision.gameObject);
        }
    }

    void OnGUI()
    {
        GUI.BeginGroup(new Rect(Screen.width / 2 - 100, 10, 250, 70));

        if (GameOver)
        {
            GUI.Box(new Rect(0, 0, 250, 70), "GameOver");
            if (GUI.Button(new Rect(75, 30, 100, 30), "Play Again"))
                Application.LoadLevel("racing");
        }
        else
        {
            GUI.Box(new Rect(0, 0, 250, 70), "Duck Herding");
            GUI.Label(new Rect(10, 10, 250, 70), "Score:: " + DuckCount.ToString() + "Time: " + Time.fixedTime.ToString("F2"), ScoreStyle);
        }
        GUI.EndGroup();
    }
}
