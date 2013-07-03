//Velocidad de la rotacion de la torreta | Speed of the turret rotation
var TurretRotationSpeed = 5.0;
function Update () {
//Obtiene la posicion del raton en el eje X | Get the mouse position in the X axis
var mousex = TurretRotationSpeed * Input.GetAxis("Mouse X");
//Mueve la torreta a la posicion del raton | Moves the turret to the mouse position
transform.Rotate(0,mousex,0);

}