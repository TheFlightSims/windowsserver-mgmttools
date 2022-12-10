import os
import math
import requests
from bs4 import BeautifulSoup
import re
  
def win7():
    for loop in range(1,10):
        for i in (BeautifulSoup(requests.get("https://jike.info/topic/381/windows-7-professional-enterprise-mak%E6%BF%80%E6%B4%BB%E5%AF%86%E9%92%A5/19?lang=en-US&page=" + str(loop)).text,'html.parser')):
            with open('getcontent.txt', 'a') as f:
                for j in i.text:
                    f.write(i.text)
        loop = loop + 1

def win81():
    for loop in range(1,30):
        for i in (BeautifulSoup(requests.get("https://jike.info/topic/343/windows-8-1-pro%E6%BF%80%E6%B4%BB%E5%AF%86%E9%92%A5/19?lang=en-US&page=" + str(loop)).text,'html.parser')):
            with open('getcontent.txt', 'a') as f:
                for j in i.text:
                    f.write(i.text)
        loop = loop + 1

def win10():
    for loop in range(1,95):
        for i in (BeautifulSoup(requests.get("https://jike.info/topic/2631/win-10-rtm-professional-retail-oem-mak?lang=en-US&page=" + str(loop)).text,'html.parser')):
            with open('getcontent.txt', 'a') as f:
                for j in i.text:
                    f.write(i.text)
        loop = loop + 1
        
def office2010():
    for loop in range(1,4):
        for i in (BeautifulSoup(requests.get("https://jike.info/topic/360/office-2010-proplus-vl_mak%E6%BF%80%E6%B4%BB%E5%AF%86%E9%92%A5?lang=en-US&page=1" + str(loop)).text,'html.parser')):
            with open('getcontent.txt', 'a') as f:
                for j in i.text:
                    f.write(i.text)
        loop = loop + 1

def office2013():
    for loop in range(1,14):
        for i in (BeautifulSoup(requests.get("https://jike.info/topic/277/office-2013-professional-plus%E6%BF%80%E6%B4%BB%E5%AF%86%E9%92%A5?lang=en-US&page=" + str(loop)).text,'html.parser')):
            with open('getcontent.txt', 'a') as f:
                for j in i.text:
                    f.write(i.text)
        loop = loop + 1

def office2016():
    for loop in range(1,53):
        for i in (BeautifulSoup(requests.get("https://jike.info/topic/2502/office-2016-proplus-retail/99?lang=zh-CN&page=" + str(loop)).text,'html.parser')):
            with open('getcontent.txt', 'a') as f:
                for j in i.text:
                    f.write(i.text)
        loop = loop + 1
        
def office2019():
    for loop in range(1,91):
        for i in (BeautifulSoup(requests.get("https://jike.info/topic/1095/office-2019-professional-plus-retail?lang=en-US&page=" + str(loop)).text,'html.parser')):
            with open('getcontent.txt', 'a') as f:
                for j in i.text:
                    f.write(i.text)
        loop = loop + 1

def office2021():
    for loop in range(1,34):
        for i in (BeautifulSoup(requests.get("https://jike.info/topic/7168/office-2021-professional-plus-retail?lang=en-US&page=" + str(loop)).text,'html.parser')):
            with open('getcontent.txt', 'a') as f:
                for j in i.text:
                    f.write(i.text)
        loop = loop + 1

def server1619():
    for loop in range(1,10):
        for i in (BeautifulSoup(requests.get("https://jike.info/topic/5050/windows-server-2016-2019-retail?lang=en-US&page=" + str(loop)).text,'html.parser')):
            with open('getcontent.txt', 'a') as f:
                for j in i.text:
                    f.write(i.text)
        loop = loop + 1

def server2022():
    for loop in range(1,23):
        for i in (BeautifulSoup(requests.get("https://jike.info/topic/6165/windows-server-2022-key?lang=en-US&page=" + str(loop)).text,'html.parser')):
            with open('getcontent.txt', 'a') as f:
                for j in i.text:
                    f.write(i.text)
        loop = loop + 1

def shortfile():
    for line in open('getcontent.txt', 'r').readlines():
        if (line.startswith("Key: ")):
            open('Keys.txt','a').write(line)

def run():
    run = int(1)
    for run in range(1, 11):
        loop = int(1)
        if run == 1:
            loop = int(1)
            i = int(1)
            win7()
        elif run == 2:
            loop = int(1)
            i = int(1)
            win81()
        elif run == 3:
            loop = int(1)
            i = int(1)
            win10()
        if run == 4:
            loop = int(1)
            i = int(1)
            office2010()
        if run == 5:
            loop = int(1)
            i = int(1)
            office2013()
        if run == 6:
            loop = int(1)
            i = int(1)
            office2016()
        if run == 7:
            loop = int(1)
            i = int(1)
            office2019()
        if run == 8:
            loop = int(1)
            i = int(1)
            office2022()
        if run == 9:
            loop = int(1)
            i = int(1)
            server1619()
        if run == 10:
            loop = int(1)
            i = int(1)
            server2022()
        else:
            shortfile()
        
        run = run + 1

run()