import socket
import rsa
import time
import json

def load_pubkey(path: str) -> rsa.PublicKey:
    with open(path, 'rb') as f:
            keydata = f.read()
    return rsa.PublicKey.load_pkcs1_openssl_pem(keydata)

def generate_spa(secret: str, pubkey: rsa.PublicKey) -> bytes:
    data = json.dumps( { 'secret': secret, 'timestamp': time.time() } ).encode()
    data = rsa.encrypt(data, pubkey)
    
    return data

def send_knock(spa_data: bytes, host: str, port: int) -> None:
    sock = socket.socket(socket.AF_INET, socket.SOCK_DGRAM) # UDP
    # sock.setsockopt(socket.SOL_SOCKET, 25, str("wlp4s0" + '\0').encode('utf-8')) # Linux only, for loop back
    sock.sendto(spa_data, (host, port))


if __name__ == '__main__':
    pubkey = load_pubkey('public.pem')
    data = generate_spa('123', pubkey)
    send_knock(data, '1.1.1.1', 1189)
    time.sleep(3)
    send_knock(data, '1.1.1.1', 1189)
