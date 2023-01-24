import hashlib
import socket
import threading
from pytube import YouTube

INVALID_CODE = b"100"
START_DOWNLOAD = b"101"
END_DOWNLOAD = b"102"
URL_START = b"103"
URL_END = b"104"
URL_OK = b"105"
URL_ERROR = b"106"
HASH_START = b"107"
HASH_END = b"108"
HASH_OK = b"109"
HASH_ERROR = b"110"
DOWNLOAD_MEDIA = b"111"
DOWNLOAD_COMPLETE = b"112"
DOWNLOAD_ERROR = b"113"
TOKEN_SUCCESS = b"114"
TOKEN_ERROR = b"115"


def generateToken(hashable: bytes) -> str:
    return hashlib.sha256(hashable).hexdigest()


def handleConnection(client: socket.socket, address: tuple):
    code = client.recv(3)
    if code == START_DOWNLOAD:

        url = b""
        hash = b""

        while True:
            print("Listening for code..")
            code = client.recv(3)
            print("Code received: ",code)
            if code == URL_START:
                url = client.recv(2048)

            elif code == URL_END:
                url = url.decode("utf-8")
                if url != None and url != "":
                    client.send(URL_OK)
                else:
                    client.send(URL_ERROR)

            elif code == HASH_START:
                hash = client.recv(2048)

            elif code == HASH_END:
                hash = hash.decode("utf-8")
                if url != None and url != "":
                    client.send(HASH_OK)
                else:
                    client.send(HASH_ERROR)

            elif code == DOWNLOAD_MEDIA:
                if url == None or url == "":
                    client.send(URL_ERROR)
                elif hash == None or hash == "":
                    client.send(HASH_ERROR)
                else:
                    try:
                        media = YouTube(url)
                        media.streams.get_audio_only().download(output_path="./Songs/",filename=hash+".mp4")
                        client.send(DOWNLOAD_COMPLETE)
                    except:
                        client.send(DOWNLOAD_ERROR)

            elif code == END_DOWNLOAD:
                client.close()
                break
            else:
                client.send(INVALID_CODE);

    else:
        client.send(INVALID_CODE)
        client.close()


def establishConnection(client: socket.socket, address: tuple):
    try:
        server_token = generateToken(str(address).encode())
        client.send(server_token.encode("utf-8"))

        client_token = client.recv(1024)

        if client_token == generateToken(server_token.encode()).encode():
            client.send(TOKEN_SUCCESS)
            handleConnection(client, address)
        else:
            client.send(TOKEN_ERROR)
            client.shutdown(socket.SHUT_RDWR)
            client.close()
    except ConnectionAbortedError:
        print("ConnectionAborted:", address)
    except ConnectionResetError:
        print("ConnectionReset:", address)
    except ConnectionError:
        print("ConnectionError:", address)
    print("Disconnected: ",address)


def main():
    host, port = ("127.0.0.1", 8731)
    server = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
    server.bind((host, port))
    server.listen(10)
    while True:
        client, address = server.accept()
        print("Connected: ",address)
        thread = threading.Thread(
            target=establishConnection, args=(client, address))
        thread.daemon = False
        thread.start()

if __name__ == "__main__":
    main()