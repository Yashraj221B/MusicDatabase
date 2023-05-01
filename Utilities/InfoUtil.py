import json
import requests

initial_request = requests.get("https://api.spotify.com/v1/playlists/6iWIjXdhmyifffgmD3xbQJ/tracks?market=IN&limit=1&offset=0",headers={"Authorization":"Bearer BQCX5g8lDH_Eelm7Dgtnn9Q695Wu4xIixNuB0hj4FcnXFU5QUa0uaky52-fgOzgzuSEJN9ogflvzzqABhhSPXMgmgE_TRY3956bgav_ra2dK7MHUbFK4OCCWU0PzBTWjPUFKboz_aSLrc0g8Gmc9TVbCkuKQcZI3FvD8CIN_WpwJT_ZOljXDswmStF1s7R5RxFhYnLYkfJg0RfDlA6ZjKAfFg0Ky8mTnPOd6xL4q4_OLygrRXOQdOYRt78E_gYEsXN_R-0MSoZLjbVMpx6gi2RyjplTmkNlDptgD95ikp_K6hj1XEB5wxJQXC58FynwSGuaClb3vnbYnlu5GPTwPF5p2TeQmi18hcoNBTPe0BHb1SkY"})

total = initial_request.json()["total"]

offset = 0

data = {
    "href": "https://api.spotify.com/v1/playlists/5dTvsfgbFHpkG4rjaUZeds/tracks?offset=0&limit=100&market=IN",
    "items": [],
    "limit": 50,
    "next": "https://api.spotify.com/v1/playlists/5dTvsfgbFHpkG4rjaUZeds/tracks?offset=100&limit=100&market=IN",
    "offset": offset,
    "total": total
}


for i in range((total//50)+1):
    offset += i * 50
    print(offset)
    final_request = initial_request = requests.get(f"https://api.spotify.com/v1/playlists/6iWIjXdhmyifffgmD3xbQJ/tracks?market=IN&limit=50&offset={offset}",headers={"Authorization":"Bearer BQCX5g8lDH_Eelm7Dgtnn9Q695Wu4xIixNuB0hj4FcnXFU5QUa0uaky52-fgOzgzuSEJN9ogflvzzqABhhSPXMgmgE_TRY3956bgav_ra2dK7MHUbFK4OCCWU0PzBTWjPUFKboz_aSLrc0g8Gmc9TVbCkuKQcZI3FvD8CIN_WpwJT_ZOljXDswmStF1s7R5RxFhYnLYkfJg0RfDlA6ZjKAfFg0Ky8mTnPOd6xL4q4_OLygrRXOQdOYRt78E_gYEsXN_R-0MSoZLjbVMpx6gi2RyjplTmkNlDptgD95ikp_K6hj1XEB5wxJQXC58FynwSGuaClb3vnbYnlu5GPTwPF5p2TeQmi18hcoNBTPe0BHb1SkY"}).json()
    data["offset"] = offset
    data["items"].extend(final_request["items"])

file = open("data.json","w")
json.dump(data,file)
file.close()

for song in final_request["items"]:
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