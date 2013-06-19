var scrollSpeed : float = 0.1;

var conveyorBelt : GameObject;
private var mat : Material;

function Start ()
{
	mat = conveyorBelt.renderer.material;
	var anim : Animation = conveyorBelt.AddComponent(Animation);
	var clip = new AnimationClip ();
	clip.name = "test";
	var curve = AnimationCurve.Linear(0, 0, 1.0 / scrollSpeed, 1.0);
	clip.SetCurve("", Material, "_MainTex.offset.y", curve);
	anim.AddClip(clip, clip.name);
	anim.wrapMode = WrapMode.Loop;
	anim.Play(clip.name);
}
