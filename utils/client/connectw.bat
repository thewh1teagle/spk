@echo off
python wake_up.py -a aa:bb:cc:dd:ee:ff -h subdomain.duckdns.org -p 1338
timeout 30
python send.py
timeout 3
mstsc /console /V:subdomain.duckdns.org:3398