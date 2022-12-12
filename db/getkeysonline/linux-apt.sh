#!/bin/sh
sudo apt update
sudo apt full-upgraade -y
sudo apt install gcc g++ python3 python3-pip -y

pip install requests beautifulsoup4 pandas urllib3