import json
import requests

token = "BQCsxqdJLY3jpi9K8H4HpYHXzso3Mz-9lSLZppJ-ijyOx7QFlY5MKy9E2NpsVjCOmXXX-xHi1E9FXhGIMng9YD05pTqSM4ub73mIgIiWGtPjTVNslQU"

playlist_id = "4z59vHG22hFrMMHrrG9Uub"

initial_request = requests.get(f"https://api.spotify.com/v1/playlists/{playlist_id}/tracks?market=IN&limit=1&offset=0",headers={"Authorization":f"Bearer {token}"})

if initial_request.json().get("error", None):
    print("Authentication Error\n",json.dumps(initial_request.json(),indent=4))
    exit()

total = initial_request.json()["total"]

fh = open("data.json","w+")

json_data = {"Songs":[]}

for i in range(total//100+1):
    request = requests.get(f"https://api.spotify.com/v1/playlists/{playlist_id}/tracks?market=IN&limit=100&offset={i*100}",headers={"Authorization":f"Bearer {token}"}).json()
    json_data["Songs"].extend(request["items"])

print("Total Songs:",len(json_data["Songs"]))

for song in json_data["Songs"]:
    print("Song Name:",song["track"]["name"])
    artists = []
    for i in song["track"]["artists"]:
        artists.append(i["name"])
    print("Artists:",' • '.join(artists))
    print("Album Name:",song["track"]["album"]["name"])
    print("Release Date:",song["track"]["album"]["release_date"])
    print("Thumbnail:",song["track"]["album"]["images"][0]["url"])
    print("Song URL:",song["track"]["external_urls"]["spotify"])
    print("===========================================================================")

json.dump(json_data,fh)

fh.close()