import json
import  requests

fh = open("data.json","r")

json_data = json.load(fh)

for song in json_data["Songs"]:
    image = song["track"]["album"]["images"][0]["url"]
    raw_data = requests.get(image)
    with open(f"../Thumbs/{song['track']['name']}.jpg","wb") as f:
        f.write(raw_data.content)