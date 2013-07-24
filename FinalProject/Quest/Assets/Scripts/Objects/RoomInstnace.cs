using UnityEngine;
using System.Collections;

public class RoomInstnace : MonoBehaviour
{
    public int RoomID;

    protected bool Enabled = true;

    void Alive()
    {
        GameState.Instance.RoomStartup(this);
    }

    protected void SetChildRenderingEnable(bool set)
    {
        for (int i = 0; i < transform.GetChildCount(); i++ )
        {
            GameObject child = transform.GetChild(i).gameObject;
            if (child.renderer != null)
                child.renderer.enabled = set;
        }
    }

    public void Show()
    {
        Enabled = true;
        SetChildRenderingEnable(Enabled);
      //  gameObject.renderer.enabled = true;
    }

	void Start ()
	{
        if (!Enabled)
            SetChildRenderingEnable(false);
	}

	void Update ()
	{
	
	}
}
