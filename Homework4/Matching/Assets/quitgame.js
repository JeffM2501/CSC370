function OnGUI () 
{
	if(GUI.Button (Rect (Screen.width-55,Screen.height-35,50,30), "Quit"))
	{
		Application.LoadLevel("mainmenu");
	}
}
