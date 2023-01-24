from selenium import webdriver
from selenium.webdriver.common.by import By
import bs4
import os

options = webdriver.ChromeOptions()
options.headless = False ##set False only for devlopment purpose

server_url = input("Enter Server URL: ")

chrome = webdriver.Chrome(options=options)

def GetSongInfo():
    data = {}
    
    song = input("Enter Song Name: ")
    
    chrome.get("https://www.google.com/search?q="+song)
    
    elements = chrome.find_elements(By.CLASS_NAME,"rVusze")
    
    for e in elements:
        print(e.text)
        text = e.text.split(":")
        data[text[0].strip()] = text[1].strip()

    chrome.get("https://www.google.com/search?q="+song+" lyrics 7Clouds&tbm=vid")

    soup = bs4.BeautifulSoup(chrome.find_element(By.CLASS_NAME,"ct3b9e").get_attribute("innerHTML"),features="lxml")
    
    url = soup.find("a").get("href")
    
    data["URL"] = url

    print("URL:",url)

    return song,data

def CheckAndFill(elementID,key,data):
    element = chrome.find_element(By.ID,elementID)
    value = data.get(key)
    if value == None:
        element.send_keys("")
    else:
        element.send_keys(value)

while True:
    try:
        song,data = GetSongInfo()
        chrome.get(server_url+"Database/Add")
        chrome.find_element(By.ID,"SongName").send_keys(song)
        if data.get("Artist") != None:
            CheckAndFill("SongArtist","Artist",data)
        else:
            CheckAndFill("SongArtist","Artists",data)
        CheckAndFill("SongAlbum","Album",data)
        CheckAndFill("SongReleased","Released",data)
        CheckAndFill("SongURL","URL",data)
        input("If the data is incorrect please correct and press ENTER")
        chrome.find_element(By.CLASS_NAME,"btn").click()
        print("\n")
    except KeyboardInterrupt:
        print("Quitting Utility...")
        chrome.quit()
        break
    except Exception as e:
        print("Error\n",e)
