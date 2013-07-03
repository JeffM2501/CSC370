using UnityEngine;
using System.Collections;

public class Radar : MonoBehaviour
 {
    public Texture OrbSpot;
    public Transform PlayerPos;

    public float MapScale = 0.8f;

    private Vector2 RadarSpot = new Vector2(0, 0);
    private Vector2 RadarSize = new Vector2(100, 100);

    void OnGUI()
    {
        GUI.BeginGroup(new Rect(10, Screen.height - RadarSize.y - 10, RadarSize.x, RadarSize.y));

        GUI.Box(new Rect(0,0,RadarSize.x,RadarSize.y), "Radar");
        DrawSpotsForOrbs();
        GUI.EndGroup();
    }

    void DrawRadarBlip(GameObject obj, Texture spotTexture)
    {
        Vector3 gameObjPos = obj.transform.position;
        float dist = Vector3.Distance(PlayerPos.position, gameObjPos);

        Vector3 delta = PlayerPos.position - gameObjPos;

        float deltaY = Mathf.Atan2(delta.x, delta.z) * Mathf.Rad2Deg - 270 - PlayerPos.eulerAngles.y;

        RadarSpot = new Vector2(dist * Mathf.Cos(deltaY * Mathf.Deg2Rad) * MapScale, dist * Mathf.Sin(deltaY * Mathf.Deg2Rad) * MapScale);

        GUI.DrawTexture(new Rect(RadarSize.x / 2.0f + RadarSpot.x, RadarSize.y / 2.0f + RadarSpot.y, 2, 2), spotTexture);
    }

    void DrawSpotsForOrbs()
    {
        float distance = Mathf.Infinity;
        Vector2 position = transform.position;

        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("orb"))
            DrawRadarBlip(obj, OrbSpot);
    }

	void Start ()
	{
	
	}

	void Update () 
	{
	
	}
}
