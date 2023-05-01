import json
import requests

# initial_request = requests.get("https://api.spotify.com/v1/playlists/5dTvsfgbFHpkG4rjaUZeds/tracks?market=IN&limit=1&offset=0",headers={"Authorization":"Bearer BQA6qniclw9DaQywxM91k215NS7SjKNwyK6exl7Ahyu7nF_m34dt37SBI4Qct1-kKAveb7LpRT7m4Qb8nmyIATjCYypBdKTXIfXl1Jm_ScOlA2vydPk8mv5yiq4SnCzPMTljjrO5EOCpF5GmmAXDMFdM4Q6EymH9xY5d-r61yIH3cnikdIHWITFehC4bvBPcP1HbndItXUfravDHs6AFA20labO_eOu_30hboNY9m57w9OrNLB2fCKrmlTKdEfvL_bR8LHYPfMPOKngQUIpeiugz_lupKMFlD22RLyIb8kmvHjmRO9YSvOFHzi4ScmLu4LmRwJcF_lPbBWiix5jNyusteK0jWVIexMjl-Y9ZNNyoRPs"})

# total = initial_request.json()["total"]

# final_request = requests.get(f"https://api.spotify.com/v1/playlists/5dTvsfgbFHpkG4rjaUZeds/tracks?market=IN&limit=100&offset=0",headers={"Authorization":"Bearer BQA6qniclw9DaQywxM91k215NS7SjKNwyK6exl7Ahyu7nF_m34dt37SBI4Qct1-kKAveb7LpRT7m4Qb8nmyIATjCYypBdKTXIfXl1Jm_ScOlA2vydPk8mv5yiq4SnCzPMTljjrO5EOCpF5GmmAXDMFdM4Q6EymH9xY5d-r61yIH3cnikdIHWITFehC4bvBPcP1HbndItXUfravDHs6AFA20labO_eOu_30hboNY9m57w9OrNLB2fCKrmlTKdEfvL_bR8LHYPfMPOKngQUIpeiugz_lupKMFlD22RLyIb8kmvHjmRO9YSvOFHzi4ScmLu4LmRwJcF_lPbBWiix5jNyusteK0jWVIexMjl-Y9ZNNyoRPs"})

# fh = open("data.json","w")

# json.dump(final_request.json(),fh,indent=4)

# fh.close()

fh = open("data.json","r")

final_request = json.load(fh)

for song in final_request["items"]:
    image = song["track"]["album"]["images"][0]["url"]
    raw_data = requests.get(image)
    with open(f"../Thumbs/{song['track']['name']}.jpg","wb") as f:
        f.write(raw_data.content)