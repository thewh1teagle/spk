@echo off
netsh advfirewall firewall set rule name="Remote Desktop - User Mode (TCP-In)" new profile=private remoteip=%1
netsh advfirewall firewall set rule name="Remote Desktop - User Mode (UDP-In)" new profile=private remoteip=%1
reg add "HKLM\SYSTEM\CurrentControlSet\Control\Terminal Server" /v fDenyTSConnections /t REG_DWORD /d 0 /f
netsh advfirewall firewall set rule group="remote desktop" new enable=yes
echo "port knocking unlocked! to ip %1:55113"