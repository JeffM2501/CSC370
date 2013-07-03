var Shot : AudioClip;
var timer = 50;
var bulletspeed = 10;
var bullet : Transform;
	function Update () {
	if(timer == 50){
	if(Input.GetKey(KeyCode.Space)){
	audio.PlayOneShot(Shot);
	var bulletprefab : Transform;
	bulletprefab = Instantiate(bullet,
	transform.position, transform.rotation);
	timer = 0;
	}
	}
	else{
	timer += 1;
	}
	}