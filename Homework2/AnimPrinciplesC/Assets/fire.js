var bulletObject: GameObject;

function Update () 
{
	if(Input.GetButtonDown("Fire1"))
	{
	   var newBullet: GameObject = Instantiate(bulletObject, this.transform.position, this.transform.rotation);
	   newBullet.rigidbody.AddForce(this.transform.forward * 500);	
	}
}
