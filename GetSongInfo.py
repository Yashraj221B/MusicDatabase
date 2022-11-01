from selenium import webdriver
import bs4
import os

options = webdriver.ChromeOptions()
options.headless = False ##set False only for devlopment purpose

chrome = webdriver.Chrome(options=options)

def GetSongInfo():
    data = {}
    
    song = input("Enter Song Name: ")
    
    chrome.get("https://www.google.com/search?q="+song)
    
    elements = chrome.find_elements_by_class_name("rVusze")
    
    for e in elements:
        print(e.text)
        text = e.text.split(":")
        data[text[0].strip()] = text[1].strip()

    chrome.get("https://www.google.com/search?q="+song+" lyrics 7Clouds&tbm=vid")

    soup = bs4.BeautifulSoup(chrome.find_element_by_class_name("ct3b9e").get_attribute("innerHTML"),features="lxml")
    
    url = soup.find("a").get("href")
    
    data["URL"] = url

    print("URL:",url)

    return song,data

def CheckAndFill(elementID,key,data):
    element = chrome.find_element_by_id(elementID)
    value = data.get(key)
    if value == None:
        element.send_keys("")
    else:
        element.send_keys(value)

while True:
    try:
        song,data = GetSongInfo()
        chrome.get("https://localhost:7056/Database/Add")
        chrome.find_element_by_id("SongName").send_keys(song)
        if data.get("Artist") != None:
            CheckAndFill("SongArtist","Artist",data)
        else:
            CheckAndFill("SongArtist","Artists",data)
        CheckAndFill("SongAlbum","Album",data)
        CheckAndFill("SongReleased","Released",data)
        CheckAndFill("SongURL","URL",data)
        input("If the data is incorrect please correct and press ENTER")
        chrome.find_element_by_class_name("btn").click()
        print("\n")
    except KeyboardInterrupt:
        print("Quitting Utility...")
        chrome.close()
    except Exception as e:
        raise
        print("Error\n",e)
