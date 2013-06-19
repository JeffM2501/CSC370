function OnGUI () 
{
	GUI.BeginGroup (Rect (Screen.width / 2 - 50, Screen.height / 2 - 50, 100, 150));
	GUI.Box (Rect (0,0,100,120), "You Lose");
	if(GUI.Button (Rect (10,40,80,30), "Back"))
	{
		Application.LoadLevel("mainmenu");	
	}
	GUI.EndGroup ();
}