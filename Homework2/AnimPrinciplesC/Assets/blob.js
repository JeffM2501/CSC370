var explosion:GameObject;
var explosionLife:float = 5.0;
var detailLevel: float = 1.0;
var countdown: float = 5.0;  //seconds until explosion
private var timeSet: float;
private var explosionActivated = false;
private var explosionDirection: Quaternion;
private var explosionLocation: Vector3;

private var stuckOn: GameObject;

function OnCollisionEnter(collision : Collision)
{
	if(collision.gameObject.name != "First Person Controller")
	{
	   stuckOn = collision.gameObject;
	   rigidbody.isKinematic = true;
	   Destroy (this.collider);
	   var contact = collision.contacts[0];
	   var rot = Quaternion.FromToRotation(Vector3.up, contact.normal);
	   explosionDirection = rot;
	   var offsetSize = explosion.GetComponent("Detonator").size / 3;
	   explosionLocation = contact.point + ((Vector3.Scale(contact.normal, Vector3(offsetSize,offsetSize,offsetSize))));
		
	   //move object to exact point of contact
	   this.transform.position = contact.point;
		
	   //rotate so y axis is pointing outward from contact point
	   this.transform.rotation = rot;
	   this.transform.localScale.x*= collision.relativeVelocity.magnitude/5.0;
	   this.transform.localScale.y*= 0.2;
	   this.transform.localScale.z*= collision.relativeVelocity.magnitude/5.0;
		
	   timeSet = Time.fixedTime + countdown;
	   explosionActivated = true;
	}
}

function Explode()
{
	var exp : GameObject = Instantiate (explosion, explosionLocation, explosionDirection);
	exp.GetComponent("Detonator").detail = detailLevel;
	Destroy(exp, explosionLife); 
	
	//if we are stuck on an object named Door destroy it
	//first check the door still exists
	if(stuckOn && stuckOn.name == "Door")
	{
		Destroy(stuckOn);	
	} 

}

function Update () 
{
	if(Time.fixedTime >= timeSet && explosionActivated)
	{
		//set off explosion
		Explode();
		explosionActivated = false;
		//remove the explosive device from game environment
		Destroy(this.gameObject);	
		
	}
}
