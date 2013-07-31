var rightNav : GUITexture;

function Start()
{
	if(Screen.width>480)
	{
		rightNav.pixelInset = new Rect(925,18,64,64);
	}
}