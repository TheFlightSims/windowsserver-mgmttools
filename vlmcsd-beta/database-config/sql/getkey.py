"""

                        TheFlightSims System Administrators
       DO NOT USE IT FOR ILLEGAL USES. ONLY FOR EDUCATIONAL AND RESEARCHES ONLY.
               https://github.com/TheFlightSims/windowsserver-mgmttools

   
   VLMCSD - a free, open-source portable Volume activation

   Support systems
    + Supported OS gens： Windows, GNU/Linux (all distros), macOS, UNIX
    + Supported CPU architectures: x86, arm, mips, PowerPC, Sparc, s390
    
    This script is used to download all keys for Windows/Office products. DO NOT use it for
    any illegal operation. 
       
    Thanks to TheFlightSims sysadmins, Hotbird64, @kkkgo, @harukaMa and other contributors for 
    all contributions to this project.

    By community, for community.

Report problems at: https://github.com/TheFlightSims/windowsserver-mgmttools/issues

"""

#Installed libs
import os, math, re, csv

try:
    #Install it from "pip install"
    import requests, re
    from bs4 import BeautifulSoup
    import pandas as pd
except:
    cs = os.system
    if os.name == 'nt':
        cs('python -m pip install requests bs4 pandas')
    else:
        cs('python3 -m pip install requests bs4 pandas')
    import requests, re
    from bs4 import BeautifulSoup
    import pandas as pd

#This header is used to report the current browser emulator. Recommend for maintenance to update the header to the lastest emulator version and OS
headers = {
    "User-Agent": "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:66.0) Gecko/20100101 Firefox/66.0",
    "Accept-Encoding": "*",
    "Connection": "keep-alive"
}

#This module will be used to download and generate keys
def gen(pag, web, ver):

    #Looping from page 1 to page "pag"
    for loop in range(1,pag+1):
        
        #In here, the application generates database from the website. Note that the db also contains chats, system messages, e.t.c that are not the PID.
        #Generate file is "gencontent.msmetadata", which is the metadata file
        
        #Print the current data sample number
        print("Current data sample: " + str(loop))
        
        # Download database from the website, and write all content into the file "gencontent.msmetadata"
        for i in (BeautifulSoup(requests.get(str(web) + str(loop), headers=headers).text, 'html.parser')):
            with open('getcontent.msmetadata', 'a', encoding="utf-8") as f:
                for j in i.text:
                    if len(i.text) == 0:
                        pass
                    else:
                        f.write(i.text)
        '''
        #Export all keys every 2 pages
        if int(loop) % int(2) == 0 and int(loop) != 0:
            
            #Filter the keys only, and export it into the file *_pre.txt
            for line in open('getcontent.msmetadata', 'r', encoding="utf-8").readlines():
                
                #Filter the PID only, and write it into the file *_pre.txt
                if (line.startswith("Key: ")):
                    open(str(ver) + str("_pre.txt"),'a',encoding="utf-8").write(line)
                
                if (line.startswith("Description: ")):
                    open(str(ver) + str("_pre.rowkey"),'a',encoding="utf-8").write(line)
                    open(str(ver) + str("_pre.rowkey"),'a',encoding="utf-8").write("\n")
            #Clear the file to prevent memory error
            os.remove("getcontent.msmetadata")
            
            #Delete keys that duplicated
        #If the selecting page isn't every 2 pages, continue the code and ignore the export
        else: 
            continue
        '''
    #Open *_pre.txt and re-write it into the new *.txt file. The "Key: " is replaced in there.
    for liner in open(str(ver) + str("_pre.txt"), 'r'):
        open(str(ver) + str(".txt"), 'a').write(liner.replace('Key: ', ' '))
    
    #Delete old *_pre.txt
    os.remove(str(ver) + str("_pre.txt"))


def win7():
    #Notificate to user that the keys is being generated
    print("Downloading Microsoft Windows 7 Keys")
    
    #Define how much page that can be generated from
    pag = int(9)
    
    #Source webpage
    web = str("https://jike.info/topic/381/windows-7-professional-enterprise-mak%E6%BF%80%E6%B4%BB%E5%AF%86%E9%92%A5/19?lang=en-US&page=")

    #Set Windows Version
    ver = str("win7")
    
    #Start downloading and generating
    gen(pag, web, ver)

def win81():
    #Notificate to user that the keys is being generated
    print("Downloading Microsoft Windows 8.1 Keys")
    
    #Define how much page that can be generated from
    pag = int(29)
    
    #Source webpage
    web = str("https://jike.info/topic/343/windows-8-1-pro%E6%BF%80%E6%B4%BB%E5%AF%86%E9%92%A5/19?lang=en-US&page=")

    #Set Windows Version
    ver = str("win81")
    
    #Start downloading and generating
    gen(pag, web, ver)

def win10():
    #Notificate to user that the keys is being generated
    print("Downloading Microsoft Windows 10/11 Keys")
    
    #Define how much page that can be generated from
    pag = int(94)
    
    #Source webpage
    web = str("https://jike.info/topic/2631/win-10-rtm-professional-retail-oem-mak?lang=en-US&page=")

    #Set Windows Version
    ver = str("win10")
    
    #Start downloading and generating
    gen(pag, web, ver)
    
def win10home():
    #Notificate to user that the keys is being generated
    print("Downloading Microsoft Windows 10/11 Keys (Core)")
    
    #Define how much page that can be generated from
    pag = int(94)
    
    #Source webpage
    web = str("https://jike.info/topic/8925/windows-10-core-home-retail?lang=en-US&page=")

    #Set Windows Version
    ver = str("win10home")
    
    #Start downloading and generating
    gen(pag, web, ver)

def server1619():
    #Notificate to user that the keys is being generated
    print("Downloading Microsoft Windows Server 2016 - 2019 Keys")
    
    #Define how much page that can be generated from
    pag = int(9)
    
    #Source webpage
    web = str("https://jike.info/topic/5050/windows-server-2016-2019-retail?lang=en-US&page=")

    #Set Windows Version
    ver = str("server1619")
    
    #Start downloading and generating
    gen(pag, web, ver)
    

def server2022():
    #Notificate to user that the keys is being generated
    print("Downloading Microsoft Windows Server 2022 Keys")
    
    #Define how much page that can be generated from
    pag = int(9)
    
    #Source webpage
    web = str("https://jike.info/topic/6165/windows-server-2022-key?lang=en-US&page=")

    #Set Windows Version
    ver = str("server2022")
    
    #Start downloading and generating
    gen(pag, web, ver)

def office2010():
    #Notificate to user that the keys is being generated
    print("Downloading Microsoft Office 2010 Keys")
    
    #Define how much page that can be generated from
    pag = int(3)
    
    #Source webpage
    web = str("https://jike.info/topic/360/office-2010-proplus-vl_mak%E6%BF%80%E6%B4%BB%E5%AF%86%E9%92%A5?lang=en-US&page=")

    #Set Windows Version
    ver = str("office2010")
    
    #Start downloading and generating
    gen(pag, web, ver)

def office2013():
    #Notificate to user that the keys is being generated
    print("Downloading Microsoft Office 2013 Keys")
    
    #Define how much page that can be generated from
    pag = int(13)
    
    #Source webpage
    web = str("https://jike.info/topic/277/office-2013-professional-plus%E6%BF%80%E6%B4%BB%E5%AF%86%E9%92%A5?lang=en-US&page=")

    #Set Windows Version
    ver = str("office2013")
    
    #Start downloading and generating
    gen(pag, web, ver)
    
def office2016():
    #Notificate to user that the keys is being generated
    print("Downloading Microsoft Office 2016 Keys")
    
    #Define how much page that can be generated from
    pag = int(52)
    
    #Source webpage
    web = str("https://jike.info/topic/2502/office-2016-proplus-retail/99?lang=zh-CN&page=")

    #Set Windows Version
    ver = str("office2016")
    
    #Start downloading and generating
    gen(pag, web, ver)

def office2019():
    #Notificate to user that the keys is being generated
    print("Downloading Microsoft Office 2019 Keys")
    
    #Define how much page that can be generated from
    pag = int(90)
    
    #Source webpage
    web = str("https://jike.info/topic/1095/office-2019-professional-plus-retail?lang=en-US&page=")

    #Set Windows Version
    ver = str("office2019")
    
    #Start downloading and generating
    gen(pag, web, ver)
    
def office2021():
    #Notificate to user that the keys is being generated
    print("Downloading Microsoft Office 2021 Keys")
    
    #Define how much page that can be generated from
    pag = int(33)
    
    #Source webpage
    web = str("https://jike.info/topic/7168/office-2021-professional-plus-retail?lang=en-US&page=")

    #Set Windows Version
    ver = str("office2021")
    
    #Start downloading and generating
    gen(pag, web, ver)

#Where we start
def start():
    ask = int(input("1. Download Keys for Microsoft Windows 7 \n2. Download Keys for Microsoft Windows 8.1 \n3. Download Keys for Microsoft Windows 10/11 \n4. Download Keys for Microsoft Windows Server 2016/2019 \n5. Download Keys for Microsoft Windows Server 2022 \n6. Download Keys for Microsoft Office 2010 \n7. Download Keys for Microsoft Office 2013 \n8. Download Keys for Microsoft Office 2016 \n9. Download Keys for Microsoft Office 2019 \n10.Download Keys for Microsoft Office 2021 \nChoose operation mode: "))
    if ask == 1:
        win7()
    elif ask == 2:
        win81()
    elif ask == 3:
        win10()
        win10home()
    elif ask == 4:
        server1619()
    elif ask == 5:
        server2022()
    elif ask == 6:
        office2010()
    elif ask == 7:
        office2013()
    elif ask == 8:
        office2016()
    elif ask == 9:
        office2019()
    elif ask == 10:
        office2021()
    else:
        print("Invaild input. Please do it again...")
        start()
#------------------------------------------------------------------------------------------------------------------

start()