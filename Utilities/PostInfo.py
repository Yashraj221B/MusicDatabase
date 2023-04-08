import json
import requests

fh = open("data.json","r")

final_request = json.load(fh)

for song in final_request["items"]:
    artists = []
    for i in song["track"]["artists"]:
        artists.append(i["name"])
    data = {
        "SongName": song["track"]["name"],
        "SongArtist": ' • '.join(artists),
        "SongAlbum": song["track"]["album"]["name"],
        "SongReleased": song["track"]["album"]["release_date"],
        "SongThumbnail": song["track"]["album"]["images"][0]["url"],
        "SongURL": song["track"]["external_urls"]["spotify"]
    }
    response = requests.post("https://localhost:5001/Database/Add",data=data,verify=False)
    print(json.dumps(data,indent=4))
    print(response)
    print("===========================================================================")
