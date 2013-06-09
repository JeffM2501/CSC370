var aP:Array = new Array(9);

function Start () {	for(var i:int = 0; i < 9; i++)	{		aP[i] = GameObject.CreatePrimitive(PrimitiveType.Cube);		aP[i].transform.position.y = i + 1;	}}

function Update () {
	for(var i:int = 0; i < 9; i++)
	{
		aP[i].renderer.material.color = 
										new Color(Random.Range(0.0,1.0),
										Random.Range(0.0,1.0),
										Random.Range(0.0,1.0));
	}}