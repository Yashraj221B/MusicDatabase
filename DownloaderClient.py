import socket
import os
from pytube import YouTube

server = socket.socket()

server.bind(("localhost",8731))

server.listen()

print("LISTENING...")
client,addr = server.accept()
print("CONNECTED:",addr,"\n")

while True:
    songurl = client.recv(1024).decode()
    print("URL:",songurl)

    mid = client.recv(1024).decode()
    print("MID:",mid)

    tube = YouTube(songurl)

    print("DOWNLOADING....")
    tube.streams.filter(only_audio=True)[0].download("Songs/",mid+".mp4")

    client.send(b"DOWNLOADED")

    print("DOWNLOADED\n")
