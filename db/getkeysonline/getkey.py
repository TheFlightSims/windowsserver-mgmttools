"""

                        TheFlightSims System Administrators
       DO NOT USE IT FOR ILLEGAL USES. ONLY FOR EDUCATIONAL AND RESEARCHES ONLY.
            
                           VLMCSD (BETA) on public GitHub
              https://github.com/TheFlightSimulationsOfficial/vlmcsd-beta

   
   VLMCSD - a free, open-source portable Volume activation

   Support systems
    + Supported OS gens：Windows, GNU/Linux (all distros), macOS, UNIX
    + Supported CPU architectures: x86, arm, mips, PowerPC, Sparc, s390
    

    This script is used to download all keys for Windows/Office products. DO NOT use it for
    any illegal operations. 
    
    The script divives into three-large section for easier to maintain (You can Find these):
        + I. REQUIRED LIBRARY
        + II. MAIN CORE APPLICATION
        + III. CALL KEYS FUNCTION

Report problems at: https://github.com/TheFlightSimulationsOfficial/vlmcsd-beta/issues

"""

#===================================================================================================================
#===================================================================================================================
# I. REQUIRED LIBRARY
#===================================================================================================================
#===================================================================================================================

import os
import math
import requests
from bs4 import BeautifulSoup
import re

#===================================================================================================================
#===================================================================================================================
# II. MAIN CORE APPLICATION
#===================================================================================================================
#===================================================================================================================

#This header is used to report the current browser emulator. Recommend for maintenance to update the header to the lastest emulator version and OS
headers = {
    "User-Agent": "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:66.0) Gecko/20100101 Firefox/66.0",
    "Accept-Encoding": "*",
    "Connection": "keep-alive"
}

#This module will be used to download and generate keys
def gen():

    #Looping from page 1 to page "pag"
    for loop in range(1,pag+1):
        
        #In here, the application generates database from the website. Note that the db also contains chats, system messages, e.t.c that are not the PID.
        #Generate file is "gencontent.msmetadata", which is the metadata file
        
        # Download database from the website, and write all content into the file "gencontent.msmetadata"
        for i in (BeautifulSoup(requests.get(str(web) + str(loop), headers=headers).text,'html.parser')):
            with open('getcontent.msmetadata', 'a', encoding="utf-8") as f:
                for j in i.text:
                    if len(i.text) == 0:
                        pass
                    else:
                        f.write(i.text)
        
            #Export all keys to the Keys.txt, every 1-page database. Uses UTF-8, so if opening it and the file returns to the non-readable types, open it as UTF-8.
            
            #Export all keys every 4 pages
            if int(loop) % int(1) == 0:
            
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
            
            #If the selecting page isn't every 1 page, continue the code and ignore the export
            else: 
                continue
        
    #Clear the console. Recommends to prevent verbose logging, causing memory error
    os.system('cls')

#===================================================================================================================
#===================================================================================================================
# III. CALL KEYS FUNCTION
#===================================================================================================================
#===================================================================================================================

#------------------------------------------------------------------------------------------------------------------

def win7():
    #Notificate to user that the keys is being generated
    print("Downloading Microsoft Windows 7 Keys")
    
    #Define how much page that can be generated from
    pag = int(9)
    
    #Source webpage
    web = str("https://jike.info/topic/381/windows-7-professional-enterprise-mak%E6%BF%80%E6%B4%BB%E5%AF%86%E9%92%A5/19?lang=en-US&page=")
    
    #Start downloading and generating
    gen()

def win81():
    #Notificate to user that the keys is being generated
    print("Downloading Microsoft Windows 8.1 Keys")
    
    #Define how much page that can be generated from
    pag = int(29)
    
    #Source webpage
    web = str("https://jike.info/topic/343/windows-8-1-pro%E6%BF%80%E6%B4%BB%E5%AF%86%E9%92%A5/19?lang=en-US&page=")
    
    #Start downloading and generating
    gen()

def win10():
    #Notificate to user that the keys is being generated
    print("Downloading Microsoft Windows 10/11 Keys")
    
    #Define how much page that can be generated from
    pag = int(94)
    
    #Source webpage
    web = str("https://jike.info/topic/343/windows-8-1-pro%E6%BF%80%E6%B4%BB%E5%AF%86%E9%92%A5/19?lang=en-US&page=")
    
    #Start downloading and generating
    gen()

def server1619():
    #Notificate to user that the keys is being generated
    print("Downloading Microsoft Windows Server 2016 - 2019 Keys")
    
    #Define how much page that can be generated from
    pag = int(9)
    
    #Source webpage
    web = str("https://jike.info/topic/5050/windows-server-2016-2019-retail?lang=en-US&page=")
    
    #Start downloading and generating
    gen()
    

def server2022():
    #Notificate to user that the keys is being generated
    print("Downloading Microsoft Windows Server 2022 Keys")
    
    #Define how much page that can be generated from
    pag = int(9)
    
    #Source webpage
    web = str("https://jike.info/topic/6165/windows-server-2022-key?lang=en-US&page=")
    
    #Start downloading and generating
    gen()

def office2010():
    #Notificate to user that the keys is being generated
    print("Downloading Microsoft Office 2010 Keys")
    
    #Define how much page that can be generated from
    pag = int(3)
    
    #Source webpage
    web = str("https://jike.info/topic/360/office-2010-proplus-vl_mak%E6%BF%80%E6%B4%BB%E5%AF%86%E9%92%A5?lang=en-US&page=")
    
    #Start downloading and generating
    gen()

def office2013():
    #Notificate to user that the keys is being generated
    print("Downloading Microsoft Office 2013 Keys")
    
    #Define how much page that can be generated from
    pag = int(13)
    
    #Source webpage
    web = str("https://jike.info/topic/277/office-2013-professional-plus%E6%BF%80%E6%B4%BB%E5%AF%86%E9%92%A5?lang=en-US&page=")
    
    #Start downloading and generating
    gen()
    
def office2016():
    #Notificate to user that the keys is being generated
    print("Downloading Microsoft Office 2016 Keys")
    
    #Define how much page that can be generated from
    pag = int(52)
    
    #Source webpage
    web = str("https://jike.info/topic/2502/office-2016-proplus-retail/99?lang=zh-CN&page=")
    
    #Start downloading and generating
    gen()

def office2019():
    #Notificate to user that the keys is being generated
    print("Downloading Microsoft Office 2019 Keys")
    
    #Define how much page that can be generated from
    pag = int(90)
    
    #Source webpage
    web = str("https://jike.info/topic/1095/office-2019-professional-plus-retail?lang=en-US&page=")
    
    #Start downloading and generating
    gen()
    
def office2021():
    #Notificate to user that the keys is being generated
    print("Downloading Microsoft Office 2021 Keys")
    
    #Define how much page that can be generated from
    pag = int(33)
    
    #Source webpage
    web = str("https://jike.info/topic/7168/office-2021-professional-plus-retail?lang=en-US&page=")
    
    #Start downloading and generating
    gen()

#------------------------------------------------------------------------------------------------------------------

#This will reset all required varibles
def retvar():
    loop = int(1)
    i = int(1)
    j = int(1)
    pag = int(1)
    web = str("")

#This will generate client keys by calling specific target
def cli():
    win7()
    win81()
    win10()

#This will generate server keys by calling specific target
def ser():
    server1619()
    server2022()

#This will generate Office keys by calling specific target
def off():
    office2010()
    office2013()
    office2016()
    office2019()
    office2021()

#Where we start
def start():
    buff = int(input("Select Buffer (1 Buffer = 100Mib)"))
    ask = int(input("1. Download all \n2. Download Clients Keys only \n3. Download Server Keys only \n4. Download Office Keys only \nChoose operation mode: "))
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
#------------------------------------------------------------------------------------------------------------------

start()