// Time Progressbar
var timeProgressBar = document.getElementById("timeProgressBar");
var timeProgress = document.getElementById("timeProgress");
var timeProgressHandle = document.getElementById("timeProgresshandle");

//Time Elements
var currentTime = document.getElementById("currentTime")
var totalTime = document.getElementById("totalTime")

//Volume Progressbar
//Implementation pending!

//Player Controls
var playButton = document.getElementById("play");
var seekPrev = document.getElementById("seek-prev");
var seekNext = document.getElementById("seek-next");

//MediaPlayer element
var mediaPlayer = document.getElementById("mediaPlayer");

function timeProgressChange(event)
{
    timeProgress.style.width = (mediaPlayer.currentTime * (timeProgressBar.clientWidth / mediaPlayer.duration)) + "px";
    updateTime();
}

function timeProgressMove(event)
{
    if(event.pageX-timeProgressBar.offsetLeft > timeProgressBar.clientWidth)
    {
        return;
    }
    timeProgress.style.width = event.pageX-timeProgressBar.offsetLeft+"px";
    mediaPlayer.currentTime = timeProgress.clientWidth / (timeProgressBar.clientWidth / mediaPlayer.duration);
    document.onmousemove = timeProgressMove;
}

function onPlayClicked()
{
    if (mediaPlayer.paused) {
        playButton.classList.add("pauseBG");
        playButton.classList.remove("playBG");
        mediaPlayer.play();
    }else{
        playButton.classList.remove("pauseBG");
        playButton.classList.add("playBG");
        mediaPlayer.pause();
    }
}

function updateTime()
{
    minutes = Math.floor(mediaPlayer.currentTime / 60).toLocaleString('en-US', {
        minimumIntegerDigits: 2,
        useGrouping: false
    });
    seconds = Math.floor(mediaPlayer.currentTime - minutes * 60).toLocaleString('en-US', {
        minimumIntegerDigits: 2,
        useGrouping: false
    });
    currentTime.innerHTML = minutes+":"+seconds;

    minutes = Math.floor(mediaPlayer.duration / 60).toLocaleString('en-US', {
        minimumIntegerDigits: 2,
        useGrouping: false
      });
    seconds = Math.floor(mediaPlayer.duration - minutes * 60).toLocaleString('en-US', {
        minimumIntegerDigits: 2,
        useGrouping: false
      });
    totalTime.innerHTML = minutes + ":" + seconds;
}

// final implementaion and activation
mediaPlayer.onplay = () => { setInterval(timeProgressChange, 1000) }
mediaPlayer.onpause = () => { clearInterval() 
    playButton.classList.remove("pauseBG");
    playButton.classList.add("playBG");
}
timeProgressBar.onmousedown = timeProgressMove;
playButton.onclick = onPlayClicked;
document.onmouseup  = () => {document.onmousemove = null}

mediaPlayer.onloadedmetadata = updateTime;