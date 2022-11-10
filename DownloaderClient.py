import socket
import threading
import os
from pytube import YouTube


def download(client):
    while True:
        try:
            songurl = client.recv(1024).decode()
            print("URL:",songurl)

            mid = client.recv(1024).decode()
            print("MID:",mid)

            tube = YouTube(songurl)

            print("DOWNLOADING....")
            tube.streams.filter(only_audio=True)[0].download("Songs/",mid+".mp4")

            client.send(b"DOWNLOADED")

            print("DOWNLOADED\n")
        except KeyboardInterrupt:
            print("Keyboard Interrup")
            break
        except ConnectionResetError as E:
            print("ERROR |",E)
            break
        except Exception as E:
            print("ERROR |",E)
            break

server = socket.socket()

server.bind(("localhost",8731))

server.listen()

while True:
    print("LISTENING...")
    client,addr = server.accept()
    print("CONNECTED:",addr,"\n")
    thread = threading.Thread(target=download,args=[client])
    thread.start()