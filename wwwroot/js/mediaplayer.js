var timeProgressBar = document.getElementById("timeProgressBar");
var timeProgress = document.getElementById("timeProgress");
var timeProgressHandle = document.getElementById("timeProgresshandle");

var playButton = document.getElementById("playButton");

function timeProgresscChange(event)
{

}
_event = null
function timeProgressMove(event)
{
    _event = event;
    if(event.pageX-timeProgressBar.offsetLeft > timeProgressBar.clientWidth)
    {
        return;
    }
    timeProgress.style.width = event.pageX-timeProgressBar.offsetLeft+"px";
    document.onmousemove = timeProgressMove;
}

timeProgressBar.onmousedown = timeProgressMove;
document.onmouseup  = () => {document.onmousemove = null}