private var speed = 0.03;
private var rotationSpeed = 0.5;
private var currentAnimation = "";

function Start()
{
	//loop all animations unless otherwise set
	this.animation.wrapMode = WrapMode.Loop;
	this.animation["Idle"].layer = -1;
	this.animation["Idle1"].layer = -1;
	this.animation["Idle2"].layer = -1;
	this.animation["WalkForward"].layer = -1;
	this.animation["WalkBackward"].layer = -1;
	//don't loop shooting animation
	this.animation["ShootStraight"].wrapMode = WrapMode.Clamp;
	
	//create a blended animation
	this.animation.AddClip(animation["ShootStraight"].clip, "ShootUpperBody");
	this.animation["ShootUpperBody"].AddMixingTransform(this.transform.Find("Reference/RightGun"));
	this.animation["ShootUpperBody"].AddMixingTransform(this.transform.Find("Reference/Hips/Spine"));
	this.animation["ShootUpperBody"].wrapMode = WrapMode.Clamp;
		
	this.animation.Stop();
}

function Update () 
{
	currentAnimation = "idle";
	var translation : float = Input.GetAxis ("Vertical") * speed;
	var rotation : float = Input.GetAxis ("Horizontal") * rotationSpeed;
	if(translation > 0)
	{
		 this.animation.CrossFade("WalkForward");
		 currentAnimation = "walk";
	}
	else if (translation < 0)
	{
		 this.animation.CrossFade("WalkBackward");
	}
	else
	{
		this.animation.CrossFade("Idle");
	}
	
	if(Input.GetKeyDown("space"))
	{
		if(currentAnimation == "walk")
		{
			this.animation.CrossFade("ShootUpperBody");
		}
		else
		{
			this.animation.CrossFade("ShootStraight");
		}
	}
	
	this.transform.Translate (0, 0, translation);
	this.transform.Rotate (0, rotation, 0);
}