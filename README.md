install openssl (windows) https://slproweb.com/products/Win32OpenSSL.html  
npcap required for window https://nmap.org/download.html  
in case of errors in windows: `net start npcap (windows only)`  
generate keys:
`openssl genrsa -out private.pem 4096 && openssl rsa -in private.pem -outform PEM -pubout -out public.pem`
