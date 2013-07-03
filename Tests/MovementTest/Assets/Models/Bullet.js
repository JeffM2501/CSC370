var colision = 0;
var bulletspeed = 400;
var bullet : Transform;
function Update () {
	if(Collision){
	if(colision == 3){
	}
	else{
	colision +=1;
	rigidbody.velocity = transform.TransformDirection(Vector3(0,0,bulletspeed));
	}
	}
	else{
	rigidbody.velocity = transform.TransformDirection(Vector3(0,0,bulletspeed));
	}
	}