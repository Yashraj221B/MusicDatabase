import json
import requests

token = "BQDPL7OFfSq6gossLnDqOU1QW2wHDA4GdUnaYsz9C6m7hnrwOfYAxDTvXq13IiRB7vR0MIoP6-rXDTTc3t_-XlvKrV2FC_E5W7Td-hDgj09tHCk5KV4"

initial_request = requests.get(f"https://api.spotify.com/v1/playlists/5dTvsfgbFHpkG4rjaUZeds/tracks?market=IN&limit=1&offset=0",headers={"Authorization":f"Bearer {token}"})
total = initial_request.json()["total"]

fh = open("data.json","w+")

json_data = {"Songs":[]}

for i in range(total//100+1):
    request = requests.get(f"https://api.spotify.com/v1/playlists/5dTvsfgbFHpkG4rjaUZeds/tracks?market=IN&limit=100&offset={i*100}",headers={"Authorization":f"Bearer {token}"}).json()
    json_data["Songs"].extend(request["items"])

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