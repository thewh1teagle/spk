# Spk
Single packet authorization port knocking

#### The story behind this program
I wanted to be able to connect
securly to my PC in my Office. 
But, Instead of keeping RDP open to the internet always, I wanted to use additional secure 'knock' that will open the 'door' and let me connect, and of course, after I got in, the door should closed.

The solution - 
I configured windows firewall to allow incoming connection to RDP port only from certain address, that adress changed to the adress that the knock come from.

The program listen for incoming raw packets on specific port, 'quietly' - port scan will not detect it.
if the packet is smaller than 1kb, 
I try to decrypt it with pre configured rsa key. 
if the decrypt worked, I check if the timestamp in the packet in the configured thresold (to prevent replay attacks), 
if it does, it checks if the secret (string) is correct.
if all above conditions accepted, 
it executes shell command with one argument - the source ip of the knock.
In my case, it executes batch script that configure windows firewall to allow incoming connection to rdp only from the ip the knock come from.

#### My use case
###### Wake up PC remotely
Wake on lan over the internet 
with static ip + static arp address
and ddns with duckdns.org using esp8266 as updater.
###### Knocking server
Port forwarding to knock server and RDP
and batch script that configure windows firewall after correct knock
the knock server configured to run with task scheduler.

###### Knocking client
simple python script (see utils folder)
that sends the knock using pre configured public rsa key.

#### requirements
openssl https://slproweb.com/products/Win32OpenSSL.html  
npcap https://nmap.org/download.html  
python for client python.org

in case of errors in windows: `net start npcap (windows only)`





