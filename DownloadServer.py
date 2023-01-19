import hashlib
import socket
import threading

INVALID_CODE = b"-1"
START_DOWNLOAD = b"102"
END_DOWNLOAD = b"108"
URL_START = b"103"
URL_END = b"104"
HASH_START = b"105"
HASH_END = b"106"
DOWNLOAD_MEDIA = b"107"
DOWNLOAD_COMPLETE = b"108"


def generateToken(hashable: bytes) -> str:
    return hashlib.sha256(hashable).hexdigest()


def handleConnection(client: socket.socket, address: tuple):
    code = client.recv(3)
    if code == START_DOWNLOAD:

        url = None
        hash = None

        while True:
            code = client.recv(3)
            if code == URL_START:
                url = client.recv(2048)

            elif code == URL_END:
                url = url.decode("utf-8")

            elif code == HASH_START:
                hash = client.recv(2048)

            elif code == HASH_END:
                hash = hash.decode("utf-8")

            elif code == DOWNLOAD_MEDIA:
                # TODO
                client.send(DOWNLOAD_COMPLETE)

            elif code == END_DOWNLOAD:
                client.close()
                break

    else:
        client.send(INVALID_CODE)
        client.close()
    print("Disconnected: ",address)


def establishConnection(client: socket.socket, address: tuple):
    try:
        server_token = generateToken(str(address).encode())
        client.send(server_token.encode("utf-8"))

        client_token = client.recv(1024)

        if client_token == generateToken(server_token.encode()).encode():
            handleConnection(client, address)
        else:
            client.close()
    except ConnectionAbortedError:
        print("ConnectionAborted:", address)
    except ConnectionError:
        print("Connection:", address)
    except Exception as e:
        print(e)
    print("Disconnected: ",address)


def main():
    while True:
        host, port = ("127.0.0.1", 221)
        server = socket.socket()
        server.bind((host, port))

        server.listen(10)

        client, address = server.accept()
        print("Connected: ",address)
        thread = threading.Thread(
            target=establishConnection, args=(client, address))
        thread.daemon = True
        thread.start()

if __name__ == "__main__":
    main()