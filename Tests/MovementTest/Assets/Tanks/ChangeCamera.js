/*Las dos variables de camaras | The two camera vars*/ 
var cam1 : GameObject;
var cam2 : GameObject;
//
function Update () {
//Esconde el cursor del raton | Hidde the mouse cursor
Screen.showCursor = false;
//Si 1 es pulsado (boton), la camera sera cambiada a la primera
//If 1 is pressed (button), the camera will be changed to the first
if (Input.GetKey (KeyCode.Alpha1)){
cam1.camera.enabled = true;
cam2.camera.enabled = false;
}
//Si 2 es pulsado (boton), la camera sera cambiada a la segunda
//If 2 is pressed (button), the camera will be changed to the second
if (Input.GetKey (KeyCode.Alpha2)){
cam1.camera.enabled = false;
cam2.camera.enabled = true;
}
}