/*Tank Control V 1.0 By MoT.
Este Script te permitira controlar un tanque en tu juego
This Script allows you control a tank in your game

Eres libre de copiar, modificar y usar este script a tu antojo
si lo usas, por favor, mencioname en los creditos
You are free of copy, modify and use this script in your game, 
if you use it, please, advert me in the credits
*/
//Ruedas que vamos a usar | Wheels we're gonna use
var wheel1 : WheelCollider;
var wheel2 : WheelCollider;
var wheel3 : WheelCollider;
var wheel4 : WheelCollider;
var wheelmid1 : WheelCollider;
var wheelmid2 : WheelCollider;
var smokeL : GameObject;
var smokeR : GameObject;
// Variables para el sonido | Sound variables
private var seconds:float;
var runningSound : AudioClip;
private var playing = "false";
//Variables de velocidad |Speed variables
private var speed = 10.0;
private var speed2 = 0.0;
private var rotate = 50.0;

//FUNCION DE MOVIMIENTO | MOVEMENT FUNCTION ->
function Update ()
 {
    //Si una rueda toca el suelo, el tanque puede moverse
    //If one wheel touch's the ground, the tank can move
    if (wheel1.isGrounded || wheel2.isGrounded || wheel3.isGrounded || wheel4.isGrounded || wheelmid1.isGrounded || wheelmid2.isGrounded)
    {
        seconds = 0;
        var translation = Input.GetAxis("Vertical") * speed;
        transform.Translate (0,0,translation * Time.deltaTime);
        var rotar = Input.GetAxis("Horizontal") * rotate;
        transform.Rotate (0,rotar * Time.deltaTime,0);
        //Si se pulsa el eje vertical... | If the Vertical axis is pressed...
        if(Input.GetAxis("Vertical"))
        {
            if(playing == "false")
            {
                audio.Play();
                playing = "true";
            }
            smokeL.particleEmitter.emit = true;smokeR.particleEmitter.emit = true;
            speed2=translation;}
else{
speed2=0;
//Si se pulsa el eje horizontal... | If the Horizontal axis is pressed...
if(Input.GetAxis("Horizontal")){
if(playing == "false"){
audio.Play();
playing = "true";
}
smokeL.particleEmitter.emit = true;smokeR.particleEmitter.emit = true;}
//Si las ruedas del tanque no estan en el suelo... | If the tank wheels are not grounded...
else{
audio.Stop();
playing = "false";
smokeL.particleEmitter.emit = false;smokeR.particleEmitter.emit = false;}}
}else{smokeL.particleEmitter.emit = false;smokeR.particleEmitter.emit = false;
if (seconds < 1000){
transform.Translate (0,0,speed2 * Time.deltaTime);
seconds++;
}
audio.Stop();
playing = "false";
}}



