var mediaPlayer = document.getElementById("mediaPlayer");

var playButton = document.getElementById("PlayButton");
var previousButton = document.getElementById("previous")
var nextButton = document.getElementById("next");
var playButtonBG = playButton.children[0];
var mid = document.getElementById("mid");

var timeProgressBar = document.getElementsByClassName("progressBar")[0];
var timeProgress = document.getElementsByClassName("progressLine")[0];
var timeProgressHandle = document.getElementsByClassName("handle")[0];

var volumeProgressBar = document.getElementsByClassName("progressBar")[1];
var volumeProgress = document.getElementsByClassName("progressLine")[1];
var volumeProgressHandle = document.getElementsByClassName("handle")[1];
var volumeIcon = document.getElementById("volumeIcon").children[0]

function moveTimeSlider(event) {
    if (event.buttons == 1) {
        timeProgress.style.width = event.x - timeProgressBar.offsetLeft + "px";
        document.onmousemove = moveTimeSlider;
        mediaPlayer.ontimeupdate = null;
        mediaPlayer.currentTime = timeProgress.clientWidth / (timeProgressBar.clientWidth / mediaPlayer.duration);
        
    } else {
        document.onmousemove = null;
        mediaPlayer.ontimeupdate = ontimeupdate;
    }
}

function moveVolumeSlider(event) {
    if (event.buttons == 1) {
        volumeProgress.style.width = event.x - volumeProgressBar.offsetLeft + "px";
        document.onmousemove = moveVolumeSlider;
        mediaPlayer.volume = (volumeProgress.clientWidth / volumeProgressBar.clientWidth).toPrecision(2);
        if(mediaPlayer.volume <= 0.009){
            volumeIcon.src = "/Assets/volume_off.svg";
            mediaPlayer.volume = 0;
            volumeProgress.style.width = "0px";
        }else if (mediaPlayer.volume <= 0.5) {
            volumeIcon.src = "/Assets/volume_down.svg";
        }else{
            volumeIcon.src = "/Assets/volume_up.svg";
        }
        setCookie("VOLUME",mediaPlayer.volume,30);
    } else {
        document.onmousemove = null;
    }
}

function ontimeupdate(event)
{
    timeProgress.style.width = (mediaPlayer.currentTime * (timeProgressBar.clientWidth /mediaPlayer.duration))+"px";
}

function PlayButtonClicked(event)
{
    if(mediaPlayer.paused)
    {
        mediaPlayer.play();
        playButtonBG.src = "/Assets/pause.svg";
    }else
    {
        mediaPlayer.pause();
        playButtonBG.src = "/Assets/play.svg";
    }
}

function refresh(event)
{
    timeProgress.style.width = (mediaPlayer.currentTime * (timeProgressBar.clientWidth /mediaPlayer.duration))+"px";
    volumeProgress.style.width = (volumeProgressBar.clientWidth * mediaPlayer.volume)+"px";
}

function keyEvent(event)
{
    if (event.key == " ") {
        event.preventDefault();
        PlayButtonClicked();
    }else if (event.key == "ArrowUp") {
        event.preventDefault();
        if((mediaPlayer.volume + 0.05)>1)
        {
            mediaPlayer.volume = 1;
        }else
        {
            mediaPlayer.volume = mediaPlayer.volume + 0.05;
        }
        volumeProgress.style.width = (volumeProgressBar.clientWidth * mediaPlayer.volume)+"px";
        if(mediaPlayer.volume <= 0.009){
            volumeIcon.src = "/Assets/volume_off.svg";
            mediaPlayer.volume = 0;
            volumeProgress.style.width = "0px";
        }else if (mediaPlayer.volume <= 0.5) {
            volumeIcon.src = "/Assets/volume_down.svg";
        }else{
            volumeIcon.src = "/Assets/volume_up.svg";
        }
    }else if (event.key == "ArrowDown") {
        event.preventDefault();
        if((mediaPlayer.volume - 0.05)<=0)
        {
            mediaPlayer.volume = 0;
        }else
        {
            mediaPlayer.volume = mediaPlayer.volume - 0.05;
        }
        volumeProgress.style.width = (volumeProgressBar.clientWidth * mediaPlayer.volume)+"px";
        if(mediaPlayer.volume <= 0.009){
            volumeIcon.src = "/Assets/volume_off.svg";
            mediaPlayer.volume = 0;
            volumeProgress.style.width = "0px";
        }else if (mediaPlayer.volume <= 0.5) {
            volumeIcon.src = "/Assets/volume_down.svg";
        }else{
            volumeIcon.src = "/Assets/volume_up.svg";
        }
    }else if (event.key == "ArrowLeft") {
        if (event.altKey) {
            return;
        }else if (event.shiftKey) {
            event.preventDefault();
            //pending Code for playing next song
        }else{
            event.preventDefault();
            mediaPlayer.currentTime = mediaPlayer.currentTime - 5;
        }
    }else if (event.key == "ArrowRight") {
        if (event.altKey) {
            return;
        }else if (event.shiftKey) {
            event.preventDefault();
            //pending Code for playing next song
        }else{
            event.preventDefault();
            mediaPlayer.currentTime = mediaPlayer.currentTime + 5;
        }        
    }
}

function PlayNext(event) {
    clearInterval();
    window.location = "/Play/Next?mid="+mid.value;
}

mediaPlayer.onended = PlayNext;
mediaPlayer.onplay = () => {setInterval(ontimeupdate,1000)}
mediaPlayer.onpause = () => {clearInterval()}
timeProgressBar.onmousedown = moveTimeSlider;
volumeProgressBar.onmousedown = moveVolumeSlider;
playButton.onclick = PlayButtonClicked;

if (getCookie("VOLUME") != "") {
    mediaPlayer.volume = getCookie("VOLUME");
}

document.onresize = refresh;
document.onkeydown = keyEvent;