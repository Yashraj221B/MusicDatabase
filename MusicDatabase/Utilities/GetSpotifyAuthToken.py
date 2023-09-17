import requests

url = "https://accounts.spotify.com/api/token"
headers = {
    "Content-Type": "application/x-www-form-urlencoded"
}
data = {
    "grant_type": "client_credentials",
    "client_id": "fe396dd3f2d94659a6346529ed12e047",
    "client_secret": "70f9a4dae8094c5a82f5397da4141fc0"
}

response = requests.post(url, headers=headers, data=data)
response_data = response.json()

print(response_data)
