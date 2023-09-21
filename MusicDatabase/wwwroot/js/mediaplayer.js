var intervalID;

// Time Progressbar
var timeProgressBar = document.getElementById("timeProgressBar");
var timeProgress = document.getElementById("timeProgress");
var timeProgressHandle = document.getElementById("timeProgresshandle");

//Volume Progressbar
//Implementation pending!
var volumeProgressBar = document.getElementById("volumeProgressBar");
var volumeProgress = document.getElementById("volumeProgress");
var volumeProgressHandle = document.getElementById("volumeProgresshandle");

//Time Elements
var currentTime = document.getElementById("currentTime")
var totalTime = document.getElementById("totalTime")


//Player Controls
var playButton = document.getElementById("play");
var seekPrev = document.getElementById("seek-prev");
var seekNext = document.getElementById("seek-next");

//MediaPlayer element
var mediaPlayer = document.getElementById("mediaPlayer");
var mid = document.getElementById("mid");

//Search box
var search = document.getElementsByName("SearchTerm")[0];

function volumeProgressChange(event)
{
    volumeProgress.style.width = (mediaPlayer.volume * (volumeProgressBar.clientWidth / 1)) + "px";
}

function volumeProgressMove(event)
{
    if(event.pageX-volumeProgressBar.offsetLeft > volumeProgressBar.clientWidth)
    {
        return;
    }
    volumeProgress.style.width = event.pageX-volumeProgressBar.offsetLeft+"px";
    mediaPlayer.volume = volumeProgress.clientWidth / (volumeProgressBar.clientWidth / 1);
    document.onmousemove = volumeProgressMove;
    setCookie("volume",mediaPlayer.volume,30);
}

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
        mediaPlayer.play();
        playButton.classList.add("pauseBG");
        playButton.classList.remove("playBG");
    }else{
        mediaPlayer.pause();
        playButton.classList.remove("pauseBG");
        playButton.classList.add("playBG");
    }
}

function updateTime()
{
    // Calculate the current time
    minutes = Math.floor(mediaPlayer.currentTime / 60).toLocaleString('en-US', {
        minimumIntegerDigits: 2,
        useGrouping: false
    });
    seconds = Math.floor(mediaPlayer.currentTime - minutes * 60).toLocaleString('en-US', {
        minimumIntegerDigits: 2,
        useGrouping: false
    });
    currentTime.innerHTML = minutes+":"+seconds;

    // Calculate the total time
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

function playNext()
{
    clearInterval();
    window.location = "/Play/Next?mid="+mid.value;
}

function playPrev()
{
    clearInterval();
    window.location = "/Play/Previous?mid="+mid.value;
}

function handleKeyboardEvents(event)
{
    if (event.key == "ArrowUp") {
        // console.log("ArrowUp")
        mediaPlayer.volume = mediaPlayer.volume + 0.02;
        setCookie("volume",mediaPlayer.volume,30)
        volumeProgressChange();
        event.preventDefault()
    }else if (event.key == "ArrowDown") {
        mediaPlayer.volume = mediaPlayer.volume - 0.02;
        // console.log("ArrowDown")
        setCookie("volume",mediaPlayer.volume,30)
        volumeProgressChange();
        event.preventDefault()
    }else if (event.key == "ArrowLeft") {
        // console.log("ArrowLeft")
        if (event.ctrlKey)
        {
            seekPrev.click();
        }else{
            mediaPlayer.currentTime = mediaPlayer.currentTime - 5;
            timeProgressChange();
            event.preventDefault()
        }
    }else if (event.key == "ArrowRight") {
        // console.log("ArrowRight")
        if (event.ctrlKey)
        {
            seekNext.click();
        }else{
            mediaPlayer.currentTime = mediaPlayer.currentTime + 5;
            timeProgressChange();
            event.preventDefault()
        }
    }else if (event.key == " ") {
        // console.log("Space")
        playButton.click();
        event.preventDefault()
    }
}
// final implementaion and activation
mediaPlayer.onplay = () => {intervalID =  setInterval(timeProgressChange, 1000) }
mediaPlayer.onpause = () => { clearInterval(intervalID); 
    playButton.classList.remove("pauseBG");
    playButton.classList.add("playBG");
}
timeProgressBar.onmousedown = timeProgressMove;
volumeProgressBar.onmousedown = volumeProgressMove;
playButton.onclick = onPlayClicked;
seekNext.onclick = playNext;
seekPrev.onclick = playPrev;
document.onmouseup  = () => {document.onmousemove = null}

mediaPlayer.onloadedmetadata = updateTime;

mediaPlayer.volume = Number.parseFloat(getCookie("volume"));

document.onkeydown = handleKeyboardEvents;
volumeProgressChange();
window.onload = () => {playButton.click();}
mediaPlayer.onended = () => {seekNext.click();}

search.onfocus = () => {document.onkeydown = null;}
search.onblur = () => {document.onkeydown = handleKeyboardEvents;}