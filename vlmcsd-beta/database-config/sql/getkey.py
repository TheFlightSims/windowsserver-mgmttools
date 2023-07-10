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
       
    Thanks to @takinekotfs, Hotbird64, @kkkgo, @harukaMa and other contributors for 
    all contributions to this project.

    By community, for community.

Report problems at: https://github.com/TheFlightSims/windowsserver-mgmttools/issues

"""

#Installed libs
import os


# Install from PyPI if `import` failed
try:
    import requests
    from bs4 import BeautifulSoup
except:
    cs = os.system
    if os.name == 'nt':
        cs('python -m pip install requests bs4')
    else:
        cs('python3 -m pip install requests bs4')
    import requests
    from bs4 import BeautifulSoup

# Recommended for who change this header, since this's old version header
headers = {
    "User-Agent": "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:66.0) Gecko/20100101 Firefox/66.0",
    "Accept-Encoding": "*",
    "Connection": "keep-alive"
}

#This module will be used to download and generate keys
def gen(pag, web, ver):

    for loop in range(1,pag+1):

        print("Current data sample: " + str(loop))

        for i in (BeautifulSoup(requests.get(str(web) + str(loop), headers=headers).text, 'html.parser')):
            with open('getcontent.msmetadata', 'a', encoding="utf-8") as f:
                for j in i.text:
                    if len(i.text) == 0:
                        pass
                    else:
                        f.write(i.text)

        if int(loop) % int(2) == 0 and int(loop) != 0:            
            for line in open('getcontent.msmetadata', 'r', encoding="utf-8").readlines():
                
                if (line.startswith("Key: ")):
                    open(str(ver) + str("_pre.rowkey"),'a',encoding="utf-8").write(line)
                
                if (line.startswith("Description: ")):
                    open(str(ver) + str("_pre.rowkey"),'a',encoding="utf-8").write(line)
                    open(str(ver) + str("_pre.rowkey"),'a',encoding="utf-8").write("\n")
            #Clear the file to prevent memory error
            os.remove("getcontent.msmetadata")

        #If the selecting page isn't every 2 pages, continue the code and ignore the export
        else:
            continue
            
    for liner in open(str(ver) + str("_pre.rowkey"), 'r'):
        open(str(ver) + str(".txt"), 'a').write(liner.replace('Key: ', ' '))
    
    #Delete old *_pre.rowkey
    os.remove(str(ver) + str("_pre.rowkey"))

def win7():
    print("Downloading Microsoft Windows 7 Keys")
    pag = int(9)
    web = str("https://jike.info/topic/381/windows-7-professional-enterprise-mak%E6%BF%80%E6%B4%BB%E5%AF%86%E9%92%A5/19?lang=en-US&page=")
    ver = str("win7")
    gen(pag, web, ver)

def win81():
    print("Downloading Microsoft Windows 8.1 Keys")
    pag = int(29)
    web = str("https://jike.info/topic/343/windows-8-1-pro%E6%BF%80%E6%B4%BB%E5%AF%86%E9%92%A5/19?lang=en-US&page=")
    ver = str("win81")
    gen(pag, web, ver)

def win10():
    print("Downloading Microsoft Windows 10/11 Keys")
    pag = int(94)
    web = str("https://jike.info/topic/2631/win-10-rtm-professional-retail-oem-mak?lang=en-US&page=")
    ver = str("win10")
    gen(pag, web, ver)
    
def win10home():
    print("Downloading Microsoft Windows 10/11 Keys (Core)")
    pag = int(94)
    web = str("https://jike.info/topic/8925/windows-10-core-home-retail?lang=en-US&page=")
    ver = str("win10home")
    gen(pag, web, ver)

def server1619():
    print("Downloading Microsoft Windows Server 2016 - 2019 Keys")
    pag = int(9)
    web = str("https://jike.info/topic/5050/windows-server-2016-2019-retail?lang=en-US&page=")
    ver = str("server1619")
    gen(pag, web, ver)
    

def server2022():
    print("Downloading Microsoft Windows Server 2022 Keys")
    pag = int(9)
    web = str("https://jike.info/topic/6165/windows-server-2022-key?lang=en-US&page=")
    ver = str("server2022")
    gen(pag, web, ver)

def office2010():
    print("Downloading Microsoft Office 2010 Keys")
    pag = int(3)
    web = str("https://jike.info/topic/360/office-2010-proplus-vl_mak%E6%BF%80%E6%B4%BB%E5%AF%86%E9%92%A5?lang=en-GB&page=")
    ver = str("office2010")
    gen(pag, web, ver)

def office2013():
    print("Downloading Microsoft Office 2013 Keys")
    pag = int(13)
    web = str("https://jike.info/topic/277/office-2013-professional-plus%E6%BF%80%E6%B4%BB%E5%AF%86%E9%92%A5?lang=en-US&page=")
    ver = str("office2013")
    gen(pag, web, ver)
    
def office2016():
    print("Downloading Microsoft Office 2016 Keys")
    pag = int(52)
    web = str("https://jike.info/topic/2502/office-2016-proplus-retail/99?lang=zh-CN&page=")
    ver = str("office2016")
    gen(pag, web, ver)

def office2019():
    print("Downloading Microsoft Office 2019 Keys")
    pag = int(90)
    web = str("https://jike.info/topic/1095/office-2019-professional-plus-retail?lang=en-US&page=")
    ver = str("office2019")
    gen(pag, web, ver)
    
def office2021():
    print("Downloading Microsoft Office 2021 Keys")
    pag = int(33)
    web = str("https://jike.info/topic/7168/office-2021-professional-plus-retail?lang=en-US&page=")
    ver = str("office2021")
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
