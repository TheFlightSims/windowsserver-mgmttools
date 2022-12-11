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

#Import required libraries
import os
import math
import requests
from bs4 import BeautifulSoup
import re
    
"""
Generate Windows Client Keys

Note that every keys downloaded can only be used by one product. 
"""

#Download Keys for Windows 7
def win7():

    #Notificate to user that the keys is being generated
    print("Downloading Windows 7 Keys")

    #Looping from page 1 to page 9 (10-1)
    for loop in range(1,10):
        
        """
        In here, the application generates database from the website. Note that the db also contains chats, system messages, e.t.c that are not the PID.
        Note that the generated file is very heavy (~600Mib - 10Gb), so stay caution.
        Generate file is "gencontent.msmetadata", which is the metadata file
        """
        
        # Download database from the website, and write all content into the file "gencontent.msmetadata"
        for i in (BeautifulSoup(requests.get("https://jike.info/topic/381/windows-7-professional-enterprise-mak%E6%BF%80%E6%B4%BB%E5%AF%86%E9%92%A5/19?lang=en-US&page=" + str(loop), headers=headers).text,'html.parser')):
            with open('getcontent.msmetadata', 'a', encoding="utf-8") as f:
                for j in i.text:
                    if len(i.text) == 0:
                        pass
                    else:
                        f.write(i.text)
        
            """
            Export all keys to the Keys.txt, every 400Mib database (where 4 pages ~ 400Mib). Uses UTF-8, so if opening it and the file returns to the non-readable types, convert them to UTF-8
            """
            
            #Export all keys every 4 pages
            if int(loop) % 4 == 0:
            
                #Notificate to user that the keys is being generated
                print("Shorting Keys...")
                
                #Filter the keys only, and export it into the file keys.txt
                for line in open('getcontent.msmetadata', 'r', encoding="utf-8").readlines():
                    
                    #Filter the PID only, and write it into the file
                    if (line.startswith("Key: ")):
                        open('Keys.txt','a',encoding="utf-8").write(line)
                        open('Keys.txt','a',encoding="utf-8").write("\n")
                
                #Clear the file to prevent memory error
                open(getcontent.msmetadata, 'r').close()
            
            #If the selecting page isn't every 4 pages, continue the code and ignore the export
            else: continue
        
        #Next 1 page
        loop = loop + 1
        
    #Clear the console. Recommends to prevent verbose logging, causing memory error
    os.system('cls')


#Download Keys for Windows 8.1 (Windows 8 is not supported)
def win81():

    #Notificate to user that the keys is being generated
    print("Downloading Windows 8.1 Keys")

    #Looping from page 1 to page 29 (30-1)
    for loop in range(1,30):
        
        """
        In here, the application generates database from the website. Note that the db also contains chats, system messages, e.t.c that are not the PID.
        Note that the generated file is very heavy (~600Mib - 10Gb), so stay caution.
        Generate file is "gencontent.msmetadata", which is the metadata file
        """
        
        # Download database from the website, and write all content into the file "gencontent.msmetadata"
        for i in (BeautifulSoup(requests.get("https://jike.info/topic/343/windows-8-1-pro%E6%BF%80%E6%B4%BB%E5%AF%86%E9%92%A5/19?lang=en-US&page=" + str(loop), headers=headers).text,'html.parser')):
            with open('getcontent.msmetadata', 'a', encoding="utf-8") as f:
                for j in i.text:
                    if len(i.text) == 0:
                        pass
                    else:
                        f.write(i.text)
        
            """
            Export all keys to the Keys.txt, every 400Mib database (where 4 pages ~ 400Mib). Uses UTF-8, so if opening it and the file returns to the non-readable types, convert them to UTF-8
            """
            
            #Export all keys every 4 pages
            if int(loop) % 4 == 0:
            
                #Notificate to user that the keys is being generated
                print("Shorting Keys...")
                
                #Filter the keys only, and export it into the file keys.txt
                for line in open('getcontent.msmetadata', 'r', encoding="utf-8").readlines():
                    
                    #Filter the PID only, and write it into the file
                    if (line.startswith("Key: ")):
                        open('Keys.txt','a',encoding="utf-8").write(line)
                        open('Keys.txt','a',encoding="utf-8").write("\n")
                
                #Clear the file to prevent memory error
                open(getcontent.msmetadata, 'r').close()
            
            #If the selecting page isn't every 4 pages, continue the code and ignore the export
            else: continue
        
        #Next 1 page
        loop = loop + 1
        
    #Clear the console. Recommends to prevent verbose logging, causing memory error
    os.system('cls')


#Download Keys for Windows 10
def win10():

    #Notificate to user that the keys is being generated
    print("Downloading Windows 10 Keys")

    #Looping from page 1 to page 94 (95-1)
    for loop in range(1,95):
        
        """
        In here, the application generates database from the website. Note that the db also contains chats, system messages, e.t.c that are not the PID.
        Note that the generated file is very heavy (~600Mib - 10Gb), so stay caution.
        Generate file is "gencontent.msmetadata", which is the metadata file
        """
        
        # Download database from the website, and write all content into the file "gencontent.msmetadata"
        for i in (BeautifulSoup(requests.get("https://jike.info/topic/343/windows-8-1-pro%E6%BF%80%E6%B4%BB%E5%AF%86%E9%92%A5/19?lang=en-US&page=" + str(loop), headers=headers).text,'html.parser')):
            with open('getcontent.msmetadata', 'a', encoding="utf-8") as f:
                for j in i.text:
                    if len(i.text) == 0:
                        pass
                    else:
                        f.write(i.text)
        
            """
            Export all keys to the Keys.txt, every 400Mib database (where 4 pages ~ 400Mib). Uses UTF-8, so if opening it and the file returns to the non-readable types, convert them to UTF-8
            """
            
            #Export all keys every 4 pages
            if int(loop) % 4 == 0:
            
                #Notificate to user that the keys is being generated
                print("Shorting Keys...")
                
                #Filter the keys only, and export it into the file keys.txt
                for line in open('getcontent.msmetadata', 'r', encoding="utf-8").readlines():
                    
                    #Filter the PID only, and write it into the file
                    if (line.startswith("Key: ")):
                        open('Keys.txt','a',encoding="utf-8").write(line)
                        open('Keys.txt','a',encoding="utf-8").write("\n")
                
                #Clear the file to prevent memory error
                open(getcontent.msmetadata, 'r').close()
            
            #If the selecting page isn't every 4 pages, continue the code and ignore the export
            else: continue
        
        #Next 1 page
        loop = loop + 1
        
    #Clear the console. Recommends to prevent verbose logging, causing memory error
    os.system('cls')
    

"""
Generate Microsoft Office 2010 - 2021 Keys

Note that every keys downloaded can only be used by one product. 
"""

#Download Keys for Microsoft Office 2010
def office2010():

    #Notificate to user that the keys is being generated
    print("Downloading Microsoft Office 2010 Keys")

    #Looping from page 1 to page 3 (4-1)
    for loop in range(1,4):
        
        """
        In here, the application generates database from the website. Note that the db also contains chats, system messages, e.t.c that are not the PID.
        Note that the generated file is very heavy (~600Mib - 10Gb), so stay caution.
        Generate file is "gencontent.msmetadata", which is the metadata file
        """
        
        # Download database from the website, and write all content into the file "gencontent.msmetadata"
        for i in (BeautifulSoup(requests.get("https://jike.info/topic/360/office-2010-proplus-vl_mak%E6%BF%80%E6%B4%BB%E5%AF%86%E9%92%A5?lang=en-US&page=" + str(loop), headers=headers).text,'html.parser')):
            with open('getcontent.msmetadata', 'a', encoding="utf-8") as f:
                for j in i.text:
                    if len(i.text) == 0:
                        pass
                    else:
                        f.write(i.text)
        
            """
            Export all keys to the Keys.txt, every 400Mib database (where 4 pages ~ 400Mib). Uses UTF-8, so if opening it and the file returns to the non-readable types, convert them to UTF-8
            """
            
            #Export all keys every 4 pages
            if int(loop) % 4 == 0:
            
                #Notificate to user that the keys is being generated
                print("Shorting Keys...")
                
                #Filter the keys only, and export it into the file keys.txt
                for line in open('getcontent.msmetadata', 'r', encoding="utf-8").readlines():
                    
                    #Filter the PID only, and write it into the file
                    if (line.startswith("Key: ")):
                        open('Keys.txt','a',encoding="utf-8").write(line)
                        open('Keys.txt','a',encoding="utf-8").write("\n")
                
                #Clear the file to prevent memory error
                open(getcontent.msmetadata, 'r').close()
            
            #If the selecting page isn't every 4 pages, continue the code and ignore the export
            else: continue
        
        #Next 1 page
        loop = loop + 1
        
    #Clear the console. Recommends to prevent verbose logging, causing memory error
    os.system('cls')


#Download Keys for Microsoft Office 2013
def office2013():

    #Notificate to user that the keys is being generated
    print("Downloading Microsoft Office 2013 Keys")

    #Looping from page 1 to page 13 (14-1)
    for loop in range(1,14):
        
        """
        In here, the application generates database from the website. Note that the db also contains chats, system messages, e.t.c that are not the PID.
        Note that the generated file is very heavy (~600Mib - 10Gb), so stay caution.
        Generate file is "gencontent.msmetadata", which is the metadata file
        """
        
        # Download database from the website, and write all content into the file "gencontent.msmetadata"
        for i in (BeautifulSoup(requests.get("https://jike.info/topic/277/office-2013-professional-plus%E6%BF%80%E6%B4%BB%E5%AF%86%E9%92%A5?lang=en-US&page=" + str(loop), headers=headers).text,'html.parser')):
            with open('getcontent.msmetadata', 'a', encoding="utf-8") as f:
                for j in i.text:
                    if len(i.text) == 0:
                        pass
                    else:
                        f.write(i.text)
        
            """
            Export all keys to the Keys.txt, every 400Mib database (where 4 pages ~ 400Mib). Uses UTF-8, so if opening it and the file returns to the non-readable types, convert them to UTF-8
            """
            
            #Export all keys every 4 pages
            if int(loop) % 4 == 0:
            
                #Notificate to user that the keys is being generated
                print("Shorting Keys...")
                
                #Filter the keys only, and export it into the file keys.txt
                for line in open('getcontent.msmetadata', 'r', encoding="utf-8").readlines():
                    
                    #Filter the PID only, and write it into the file
                    if (line.startswith("Key: ")):
                        open('Keys.txt','a',encoding="utf-8").write(line)
                        open('Keys.txt','a',encoding="utf-8").write("\n")
                
                #Clear the file to prevent memory error
                open(getcontent.msmetadata, 'r').close()
            
            #If the selecting page isn't every 4 pages, continue the code and ignore the export
            else: continue
        
        #Next 1 page
        loop = loop + 1
        
    #Clear the console. Recommends to prevent verbose logging, causing memory error
    os.system('cls')
    

#Download Keys for Microsoft Office 2016
def office2016():

    #Notificate to user that the keys is being generated
    print("Downloading Microsoft Office 2016 Keys")

    #Looping from page 1 to page 52 (53-1)
    for loop in range(1,53):
        
        """
        In here, the application generates database from the website. Note that the db also contains chats, system messages, e.t.c that are not the PID.
        Note that the generated file is very heavy (~600Mib - 10Gb), so stay caution.
        Generate file is "gencontent.msmetadata", which is the metadata file
        """
        
        # Download database from the website, and write all content into the file "gencontent.msmetadata"
        for i in (BeautifulSoup(requests.get("https://jike.info/topic/2502/office-2016-proplus-retail/99?lang=zh-CN&page=" + str(loop), headers=headers).text,'html.parser')):
            with open('getcontent.msmetadata', 'a', encoding="utf-8") as f:
                for j in i.text:
                    if len(i.text) == 0:
                        pass
                    else:
                        f.write(i.text)
        
            """
            Export all keys to the Keys.txt, every 400Mib database (where 4 pages ~ 400Mib). Uses UTF-8, so if opening it and the file returns to the non-readable types, convert them to UTF-8
            """
            
            #Export all keys every 4 pages
            if int(loop) % 4 == 0:
            
                #Notificate to user that the keys is being generated
                print("Shorting Keys...")
                
                #Filter the keys only, and export it into the file keys.txt
                for line in open('getcontent.msmetadata', 'r', encoding="utf-8").readlines():
                    
                    #Filter the PID only, and write it into the file
                    if (line.startswith("Key: ")):
                        open('Keys.txt','a',encoding="utf-8").write(line)
                        open('Keys.txt','a',encoding="utf-8").write("\n")
                
                #Clear the file to prevent memory error
                open(getcontent.msmetadata, 'r').close()
            
            #If the selecting page isn't every 4 pages, continue the code and ignore the export
            else: continue
        
        #Next 1 page
        loop = loop + 1
        
    #Clear the console. Recommends to prevent verbose logging, causing memory error
    os.system('cls')
    
    
    
#Download Keys for Microsoft Office 2019
def office2019():

    #Notificate to user that the keys is being generated
    print("Downloading Microsoft Office 2019 Keys")

    #Looping from page 1 to page 90 (91-1)
    for loop in range(1,91):
        
        """
        In here, the application generates database from the website. Note that the db also contains chats, system messages, e.t.c that are not the PID.
        Note that the generated file is very heavy (~600Mib - 10Gb), so stay caution.
        Generate file is "gencontent.msmetadata", which is the metadata file
        """
        
        # Download database from the website, and write all content into the file "gencontent.msmetadata"
        for i in (BeautifulSoup(requests.get("https://jike.info/topic/1095/office-2019-professional-plus-retail?lang=en-US&page=" + str(loop), headers=headers).text,'html.parser')):
            with open('getcontent.msmetadata', 'a', encoding="utf-8") as f:
                for j in i.text:
                    if len(i.text) == 0:
                        pass
                    else:
                        f.write(i.text)
        
            """
            Export all keys to the Keys.txt, every 400Mib database (where 4 pages ~ 400Mib). Uses UTF-8, so if opening it and the file returns to the non-readable types, convert them to UTF-8
            """
            
            #Export all keys every 4 pages
            if int(loop) % 4 == 0:
            
                #Notificate to user that the keys is being generated
                print("Shorting Keys...")
                
                #Filter the keys only, and export it into the file keys.txt
                for line in open('getcontent.msmetadata', 'r', encoding="utf-8").readlines():
                    
                    #Filter the PID only, and write it into the file
                    if (line.startswith("Key: ")):
                        open('Keys.txt','a',encoding="utf-8").write(line)
                        open('Keys.txt','a',encoding="utf-8").write("\n")
                
                #Clear the file to prevent memory error
                open(getcontent.msmetadata, 'r').close()
            
            #If the selecting page isn't every 4 pages, continue the code and ignore the export
            else: continue
        
        #Next 1 page
        loop = loop + 1
        
    #Clear the console. Recommends to prevent verbose logging, causing memory error
    os.system('cls')
    

#Download Keys for Microsoft Office 2021
def office2021():

    #Notificate to user that the keys is being generated
    print("Downloading Microsoft Office 2021 Keys")

    #Looping from page 1 to page 33 (34-1)
    for loop in range(1,34):
        
        """
        In here, the application generates database from the website. Note that the db also contains chats, system messages, e.t.c that are not the PID.
        Note that the generated file is very heavy (~600Mib - 10Gb), so stay caution.
        Generate file is "gencontent.msmetadata", which is the metadata file
        """
        
        # Download database from the website, and write all content into the file "gencontent.msmetadata"
        for i in (BeautifulSoup(requests.get("https://jike.info/topic/7168/office-2021-professional-plus-retail?lang=en-US&page=" + str(loop), headers=headers).text,'html.parser')):
            with open('getcontent.msmetadata', 'a', encoding="utf-8") as f:
                for j in i.text:
                    if len(i.text) == 0:
                        pass
                    else:
                        f.write(i.text)
        
            """
            Export all keys to the Keys.txt, every 400Mib database (where 4 pages ~ 400Mib). Uses UTF-8, so if opening it and the file returns to the non-readable types, convert them to UTF-8
            """
            
            #Export all keys every 4 pages
            if int(loop) % 4 == 0:
            
                #Notificate to user that the keys is being generated
                print("Shorting Keys...")
                
                #Filter the keys only, and export it into the file keys.txt
                for line in open('getcontent.msmetadata', 'r', encoding="utf-8").readlines():
                    
                    #Filter the PID only, and write it into the file
                    if (line.startswith("Key: ")):
                        open('Keys.txt','a',encoding="utf-8").write(line)
                        open('Keys.txt','a',encoding="utf-8").write("\n")
                
                #Clear the file to prevent memory error
                open(getcontent.msmetadata, 'r').close()
            
            #If the selecting page isn't every 4 pages, continue the code and ignore the export
            else: continue
        
        #Next 1 page
        loop = loop + 1
        
    #Clear the console. Recommends to prevent verbose logging, causing memory error
    os.system('cls')


"""
Generate Windows Server Keys

Note that every keys downloaded can only be used by one product. 
"""

#Download Keys for Windows 7
def server1619():

    #Notificate to user that the keys is being generated
    print("Downloading Windows Server 2016 - 2019 Keys")

    #Looping from page 1 to page 9 (10-1)
    for loop in range(1,10):
        
        """
        In here, the application generates database from the website. Note that the db also contains chats, system messages, e.t.c that are not the PID.
        Note that the generated file is very heavy (~600Mib - 10Gb), so stay caution.
        Generate file is "gencontent.msmetadata", which is the metadata file
        """
        
        # Download database from the website, and write all content into the file "gencontent.msmetadata"
        for i in (BeautifulSoup(requests.get("https://jike.info/topic/5050/windows-server-2016-2019-retail?lang=en-US&page=" + str(loop), headers=headers).text,'html.parser')):
            with open('getcontent.msmetadata', 'a', encoding="utf-8") as f:
                for j in i.text:
                    if len(i.text) == 0:
                        pass
                    else:
                        f.write(i.text)
        
            """
            Export all keys to the Keys.txt, every 400Mib database (where 4 pages ~ 400Mib). Uses UTF-8, so if opening it and the file returns to the non-readable types, convert them to UTF-8
            """
            
            #Export all keys every 4 pages
            if int(loop) % 4 == 0:
            
                #Notificate to user that the keys is being generated
                print("Shorting Keys...")
                
                #Filter the keys only, and export it into the file keys.txt
                for line in open('getcontent.msmetadata', 'r', encoding="utf-8").readlines():
                    
                    #Filter the PID only, and write it into the file
                    if (line.startswith("Key: ")):
                        open('Keys.txt','a',encoding="utf-8").write(line)
                        open('Keys.txt','a',encoding="utf-8").write("\n")
                
                #Clear the file to prevent memory error
                open(getcontent.msmetadata, 'r').close()
            
            #If the selecting page isn't every 4 pages, continue the code and ignore the export
            else: continue
        
        #Next 1 page
        loop = loop + 1
        
    #Clear the console. Recommends to prevent verbose logging, causing memory error
    os.system('cls')


#Download Keys for Windows 8.1 (Windows 8 is not supported)
def server2022():

    #Notificate to user that the keys is being generated
    print("Downloading Windows Server 2022 Keys")

    #Looping from page 1 to page 22 (23-1)
    for loop in range(1,23):
        
        """
        In here, the application generates database from the website. Note that the db also contains chats, system messages, e.t.c that are not the PID.
        Note that the generated file is very heavy (~600Mib - 10Gb), so stay caution.
        Generate file is "gencontent.msmetadata", which is the metadata file
        """
        
        # Download database from the website, and write all content into the file "gencontent.msmetadata"
        for i in (BeautifulSoup(requests.get("https://jike.info/topic/6165/windows-server-2022-key?lang=en-US&page=" + str(loop), headers=headers).text,'html.parser')):
            with open('getcontent.msmetadata', 'a', encoding="utf-8") as f:
                for j in i.text:
                    if len(i.text) == 0:
                        pass
                    else:
                        f.write(i.text)
        
            """
            Export all keys to the Keys.txt, every 400Mib database (where 4 pages ~ 400Mib). Uses UTF-8, so if opening it and the file returns to the non-readable types, convert them to UTF-8
            """
            
            #Export all keys every 4 pages
            if int(loop) % 4 == 0:
            
                #Notificate to user that the keys is being generated
                print("Shorting Keys...")
                
                #Filter the keys only, and export it into the file keys.txt
                for line in open('getcontent.msmetadata', 'r', encoding="utf-8").readlines():
                    
                    #Filter the PID only, and write it into the file
                    if (line.startswith("Key: ")):
                        open('Keys.txt','a',encoding="utf-8").write(line)
                        open('Keys.txt','a',encoding="utf-8").write("\n")
                
                #Clear the file to prevent memory error
                open(getcontent.msmetadata, 'r').close()
            
            #If the selecting page isn't every 4 pages, continue the code and ignore the export
            else: continue
        
        #Next 1 page
        loop = loop + 1
        
    #Clear the console. Recommends to prevent verbose logging, causing memory error
    os.system('cls')
    
    
"""
Operation modules

Any failure in editing in this may cause the application crash, OS crash, OS hang

Stay caution.
"""

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
    ask = int(input("1. Download all \n2. Download Windows Clients Keys only \n3. Download Windows Server Keys only \n4. Download Microsoft Office Keys only \nChoose operation mode: "))
    if ask == 1: 
        cli()
        retvar()
        server()
        retvar()
        off()
    elif ask == 2:
        cli()
        retvar()
    elif ask == 3:
        server()
        retvar()
    elif ask == 4:
        off()
        retvar()
    else:
        print("Invaild input. Please do it again...")
        os.console('cls')
        start()


start()