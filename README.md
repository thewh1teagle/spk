# Spk
Single packet authorization port knocking

## The story behind this program
I wanted a secure way to connect to my office PC without leaving RDP open to the internet all the time. Instead, I implemented an additional secure 'knock' to open the 'door' for connection, closing it once accessed.

Here's the solution: I configured the Windows firewall to allow incoming connections to the RDP port only from a specific address, dynamically changing the address based on the incoming 'knock.'

The program listens for incoming raw packets on a specific port discreetly, making it undetectable by port scans. If the packet is smaller than 1kb, I attempt to decrypt it using a pre-configured RSA key. If decryption is successful, I check the timestamp against a configured threshold to prevent replay attacks. If all conditions are met, including the correct secret (string), the program executes a shell command with the source IP of the knock.

In my case, it runs a batch script to configure the Windows firewall, allowing incoming RDP connections only from the IP of the knock.

## My use case
##### Wake up PC remotely
Wake on lan over the internet  
with static ip + static arp address  
and ddns with duckdns.org using esp8266 as updater.  
##### Knocking server  
Port forwarding to knock server and RDP  
and batch script that configure windows firewall after correct knock  
the knock server configured to run with task scheduler.  

##### Knocking client
simple python script (see utils folder)  
that sends the knock using pre configured public rsa key.  

## requirements
openssl https://slproweb.com/products/Win32OpenSSL.html  
npcap https://nmap.org/download.html  
python for client python.org

in case of errors in windows: `net start npcap (windows only)`





