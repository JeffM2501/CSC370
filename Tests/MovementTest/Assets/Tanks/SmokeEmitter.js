//Simple emisor de particlas de humo si el eje es pulsado
//Simple smoke emitter particles if the axis is pressed
function Update () {
if(Input.GetAxis("Vertical")){
particleEmitter.emit = true;}
else{
if(Input.GetAxis("Horizontal")){
particleEmitter.emit = true;}
else{
particleEmitter.emit = false;}}
}