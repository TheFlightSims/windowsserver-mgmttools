"""

                        TheFlightSims System Administrators
       DO NOT USE IT FOR ILLEGAL USES. ONLY FOR EDUCATIONAL AND RESEARCHES ONLY.
            
                           VLMCSD (BETA) on public GitHub
              https://github.com/TheFlightSimulationsOfficial/vlmcsd-beta

   
   VLMCSD - a free, open-source portable Volume activation

   Support systems
    + Supported OS gens：Windows, GNU/Linux (all distros), macOS, UNIX
    + Supported CPU architectures: x86, arm, mips, PowerPC, Sparc, s390
    
====================================================================================

This file is used to download all keys for Windows/Office products. DO NOT use it for
any illegal operations. 

Report problems at: https://github.com/TheFlightSimulationsOfficial/vlmcsd-beta/issues

"""

#----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
#----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

#Import required libraries
import os
import math
import requests
from bs4 import BeautifulSoup
import re
    

#Generate Keys
#Note that every keys downloaded can only be used by one product. 

def win7():
    #Notificate to user that the keys is being generated
    print("Downloading Microsoft Office 2019 Keys")
    pag = int(9+1)
    web = str("https://jike.info/topic/381/windows-7-professional-enterprise-mak%E6%BF%80%E6%B4%BB%E5%AF%86%E9%92%A5/19?lang=en-US&page=")

def win81():
def win10():
def server1619():
def server2022():
def office2010():
def office2013():
def office2016():
def office2019():
def office2021():

#----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
#----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------


#Operation modules
#Any failure in editing in this may cause the application crash, OS crash, OS hang


#This module will be used to download and generate keys
#Download Keys for Microsoft Office 2019
def gen():

    #Looping from page 1 to page 90 (91-1)
    for loop in range(1,pag):
        
        #In here, the application generates database from the website. Note that the db also contains chats, system messages, e.t.c that are not the PID.
        #Note that the generated file is very heavy (~600Mib - 10Gb), so stay caution.
        #Generate file is "gencontent.msmetadata", which is the metadata file
        
        
        # Download database from the website, and write all content into the file "gencontent.msmetadata"
        for i in (BeautifulSoup(requests.get(str(web) + str(loop), headers=headers).text,'html.parser')):
            with open('getcontent.msmetadata', 'a', encoding="utf-8") as f:
                for j in i.text:
                    if len(i.text) == 0:
                        pass
                    else:
                        f.write(i.text)
        
            #Export all keys to the Keys.txt, every 400Mib database (where 4 pages ~ 400Mib). Uses UTF-8, so if opening it and the file returns to the non-readable types, convert them to UTF-8
            
            #Export all keys every 4 pages
            if int(loop) % int(buff) == 0:
            
                #Notificate to user that the keys is being generated
                print("Shorting Keys...")
                
                #Filter the keys only, and export it into the file keys.txt
                for line in open('getcontent.msmetadata', 'r', encoding="utf-8").readlines():
                    
                    #Filter the PID only, and write it into the file
                    if (line.startswith("Key: ")):
                        open('Keys.txt','a',encoding="utf-8").write(line)
                        open('Keys.txt','a',encoding="utf-8").write("\n")
                
                #Clear the file to prevent memory error
                os.remove("getcontent.msmetadata")
            
            #If the selecting page isn't every 4 pages, continue the code and ignore the export
            else: continue
        
        #Next 1 page
        loop = loop + 1
        
    #Clear the console. Recommends to prevent verbose logging, causing memory error
    os.system('cls')

#This header is used to report the current browser emulator. Recommend for maintenance to update the header to the lastest emulator version and OS
headers = {
    "User-Agent": "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:66.0) Gecko/20100101 Firefox/66.0",
    "Accept-Encoding": "*",
    "Connection": "keep-alive"
}

def retvar():
    loop = int(1)
    i = int(1)
    j = int(1)

def cli():
    win7()
    win81()
    win10()

def ser():
    server1619()
    server2022()

def off():
    office2010()
    office2013()
    office2016()
    office2019()
    office2021()

def start():
    buff = int(input("Select Buffer (1 Buffer = 100Mib)"))
    ask = int(input("1. Download all \n2. Download Windows Clients Keys only \n3. Download Windows Server Keys only \n4. Download Microsoft Office Keys only \nChoose operation mode: "))
    if ask == 1: 
        cli()
        retvar()
        ser()
        retvar()
        off()
    elif ask == 2:
        cli()
        retvar()
    elif ask == 3:
        ser()
        retvar()
    elif ask == 4:
        off()
        retvar()
    else:
        print("Invaild input. Please do it again...")
        os.console('cls')
        start()


start()