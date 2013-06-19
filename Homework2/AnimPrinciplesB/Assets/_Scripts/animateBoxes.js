var animationPath : GameObject;

var theBoxes : GameObject[];

function Start () 
{	
	while (true)
	{	
		var thisPath : GameObject = Instantiate (animationPath, transform.position, Quaternion.identity);
		
		thisPath.animation["Take 001"].speed = 0.01;
		
		var thisRotation = Quaternion.identity;
		
		thisRotation.eulerAngles = Vector3(0, Random.Range (0,360), 0);
			
		var thisBox : GameObject = Instantiate (theBoxes[Random.Range (0,theBoxes.Length)], thisPath.transform.position, thisRotation);
		
		thisBox.transform.parent = thisPath.transform;	
		
		Destroy (thisPath, thisPath.animation.clip.length * 101.0);
		
		yield new WaitForSeconds (Random.Range (3,15));
	}	
}