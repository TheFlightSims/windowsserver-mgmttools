import os
import math
import requests
from bs4 import BeautifulSoup
import re
  
def win7():
    print("Downloading Windows 7 Keys")
    for loop in range(1,10):
        for i in (BeautifulSoup(requests.get("https://jike.info/topic/381/windows-7-professional-enterprise-mak%E6%BF%80%E6%B4%BB%E5%AF%86%E9%92%A5/19?lang=en-US&page=" + str(loop), headers=headers).text,'html.parser')):
            with open('getcontent.msmetadata', 'a', encoding="utf-8") as f:
                for j in i.text:
                    if len(i.text) == 0:
                        pass
                    else:
                        f.write(i.text)
        loop = loop + 1

def win81():
    print("Downloading Windows 8.1 Keys")
    for loop in range(1,30):
        for i in (BeautifulSoup(requests.get("https://jike.info/topic/343/windows-8-1-pro%E6%BF%80%E6%B4%BB%E5%AF%86%E9%92%A5/19?lang=en-US&page=" + str(loop), headers=headers).text,'html.parser')):
            with open('getcontent.msmetadata', 'a', encoding="utf-8") as f:
                for j in i.text:
                    if len(i.text) == 0:
                        pass
                    else:
                        f.write(i.text)
        loop = loop + 1

def win10():
    print("Downloading Windows 10/11 Keys")
    for loop in range(1,95):
        for i in (BeautifulSoup(requests.get("https://jike.info/topic/2631/win-10-rtm-professional-retail-oem-mak?lang=en-US&page=" + str(loop), headers=headers).text,'html.parser')):
            with open('getcontent.msmetadata', 'a', encoding="utf-8") as f:
                for j in i.text:
                    if len(i.text) == 0:
                        pass
                    else:
                        f.write(i.text)
        loop = loop + 1
        
def office2010():
    print("Downloading Office 2010 Keys")
    for loop in range(1,4):
        for i in (BeautifulSoup(requests.get("https://jike.info/topic/360/office-2010-proplus-vl_mak%E6%BF%80%E6%B4%BB%E5%AF%86%E9%92%A5?lang=en-US&page=" + str(loop), headers=headers).text,'html.parser')):
            with open('getcontent.msmetadata', 'a', encoding="utf-8") as f:
                for j in i.text:
                    if len(i.text) == 0:
                        pass
                    else:
                        f.write(i.text)
        loop = loop + 1

def office2013():
    print("Downloading Office 2013 Keys")
    for loop in range(1,14):
        for i in (BeautifulSoup(requests.get("https://jike.info/topic/277/office-2013-professional-plus%E6%BF%80%E6%B4%BB%E5%AF%86%E9%92%A5?lang=en-US&page=" + str(loop), headers=headers).text,'html.parser')):
            with open('getcontent.msmetadata', 'a', encoding="utf-8") as f:
                for j in i.text:
                    if len(i.text) == 0:
                        pass
                    else:
                        f.write(i.text)
        loop = loop + 1

def office2016():
    print("Downloading Office 2016 Keys")
    for loop in range(1,53):
        for i in (BeautifulSoup(requests.get("https://jike.info/topic/2502/office-2016-proplus-retail/99?lang=zh-CN&page=" + str(loop), headers=headers).text,'html.parser')):
            with open('getcontent.msmetadata', 'a', encoding="utf-8") as f:
                for j in i.text:
                    if len(i.text) == 0:
                        pass
                    else:
                        f.write(i.text)
        loop = loop + 1
        
def office2019():
    print("Downloading Office 2019 Keys")
    for loop in range(1,91):
        for i in (BeautifulSoup(requests.get("https://jike.info/topic/1095/office-2019-professional-plus-retail?lang=en-US&page=" + str(loop), headers=headers).text,'html.parser')):
            with open('getcontent.msmetadata', 'a', encoding="utf-8") as f:
                for j in i.text:
                    if len(i.text) == 0:
                        pass
                    else:
                        f.write(i.text)
        loop = loop + 1

def office2021():
    print("Downloading Office 2021 Keys")
    for loop in range(1,34):
        for i in (BeautifulSoup(requests.get("https://jike.info/topic/7168/office-2021-professional-plus-retail?lang=en-US&page=" + str(loop), headers=headers).text,'html.parser')):
            with open('getcontent.msmetadata', 'a', encoding="utf-8") as f:
                for j in i.text:
                    if len(i.text) == 0:
                        pass
                    else:
                        f.write(i.text)
        loop = loop + 1

def server1619():
    print("Downloading Windows Server 2016 - 2019 Keys")
    for loop in range(1,10):
        for i in (BeautifulSoup(requests.get("https://jike.info/topic/5050/windows-server-2016-2019-retail?lang=en-US&page=" + str(loop), headers=headers).text,'html.parser')):
            with open('getcontent.msmetadata', 'a', encoding="utf-8") as f:
                for j in i.text:
                    if len(i.text) == 0:
                        pass
                    else:
                        f.write(i.text)
        loop = loop + 1

def server2022():
    print("Downloading Windows Server 2022 Keys")
    for loop in range(1,23):
        for i in (BeautifulSoup(requests.get("https://jike.info/topic/6165/windows-server-2022-key?lang=en-US&page=" + str(loop), headers=headers).text,'html.parser')):
            with open('getcontent.msmetadata', 'a', encoding="utf-8") as f:
                for j in i.text:
                    if len(i.text) == 0:
                        pass
                    else:
                        f.write(i.text)
        loop = loop + 1

def shortfile():
    print("Shorting Keys...")
    for line in open('getcontent.msmetadata', 'r', encoding="utf-8").readlines():
        if (line.startswith("Key: ")):
            open('Keys.txt','a',encoding="utf-8").write(line)
            open('Keys.txt','a',encoding="utf-8").write("\n")

headers = {
    "User-Agent": "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:66.0) Gecko/20100101 Firefox/66.0",
    "Accept-Encoding": "*",
    "Connection": "keep-alive"
}

def run():
    loop = int(1)
    i = int(1)
    win7()
    loop = int(1)
    i = int(1)
    win81()
    loop = int(1)
    i = int(1)
    win10()
    loop = int(1)
    i = int(1)
    office2010()
    loop = int(1)
    i = int(1)
    office2013()
    loop = int(1)
    i = int(1)
    office2016()
    loop = int(1)
    i = int(1)
    office2019()
    loop = int(1)
    i = int(1)
    office2021()
    loop = int(1)
    i = int(1)
    server1619()
    loop = int(1)
    i = int(1)
    server2022()
    loop = int(1)
    i = int(1)

run()
