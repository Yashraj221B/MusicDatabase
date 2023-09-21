import json
import spotdl
import hashlib
import requests

client_id = input("Client Id: ")
client_secret = input("Client Secret: ")

resolver = spotdl.Spotdl(client_id=client_id,client_secret=client_secret)

fh = open("data.json","r")

final_request = json.load(fh)

def getHash(text:str):
    sha = hashlib.sha256(text.encode())
    return sha.hexdigest().upper()

for song in final_request["Songs"]:
    artists = []
    for i in song["track"]["artists"]:
        artists.append(i["name"])

    resolved_songs = resolver.search([song["track"]["external_urls"]["spotify"]])
    
    urls = resolver.get_download_urls(resolved_songs)

    if len(urls) == 0:
        urls = [song["track"]["external_urls"]["spotify"]]

    data = {
        "SongName": song["track"]["name"],
        "SongArtist": ' • '.join(artists),
        "SongAlbum": song["track"]["album"]["name"],
        "SongReleased": song["track"]["album"]["release_date"],
        "SongThumbnail": song["track"]["album"]["images"][0]["url"],
        "SongURL": urls[0]
    }
    response = requests.post("http://localhost:5085/Database/Add",data=data,verify=False)
    print(json.dumps(data,indent=4))
    print("===========================================================================")

