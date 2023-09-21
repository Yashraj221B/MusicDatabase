import json
import spotdl
import hashlib
import requests

fh = open("data.json","r")

data = json.load(fh)

def getHash(text:str):
    sha = hashlib.sha256(text.encode())
    return sha.hexdigest().upper()

for song in data["Songs"]:
    hash = getHash(song["track"]["name"])
    print(hash)
    x = requests.get("http://localhost:5085/Database/Download/"+hash)
    print(x.content)

