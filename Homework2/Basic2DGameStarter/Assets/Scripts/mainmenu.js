function OnGUI () 
{
	GUI.BeginGroup (Rect (Screen.width / 2 - 50, Screen.height / 2 - 50, 100, 150));
	GUI.Box (Rect (0,0,100,120), "Space Base\nDefense");
	if(GUI.Button (Rect (10,40,80,30), "Play"))
	{
		Application.LoadLevel("basicgame");	
	}
	if(GUI.Button (Rect (10,80,80,30), "Help"))
	{
		Application.LoadLevel("helpscreen");
	}
	GUI.EndGroup ();
}
