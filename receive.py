from rsa.pkcs1 import decrypt
from scapy.all import sniff, IP
import time
import json
import rsa

def load_privkey(path):
    with open(path, 'rb') as f:
        keydata = f.read()
    return rsa.PrivateKey.load_pkcs1(keydata)

privkey = load_privkey('private.pem')


def correct_knock(payload: bytes, privkey: rsa.PrivateKey, secret: str, threshold: float):
    decrypted = rsa.decrypt(payload, privkey)
    unserialized = json.loads(decrypted)
    payload_secret, payload_timestamp = unserialized['secret'], float( unserialized['timestamp'] )
    if payload_secret == secret and ( time.time() - payload_timestamp - threshold ) <= 0:
        return True
    else:
        return False


def packet_handler(packet):
    ip_layer = packet.getlayer(IP)
    packet_size = packet.sprintf("%IP.len%")
    port = packet.dport
    data = packet.load
    print("[!] New Packet: {src} -> {dst}".format(src=ip_layer.src, dst=ip_layer.dst))
    print('decrypting data...')
    if correct_knock(data, privkey, '123', 1.0):
        print('correct knock! unlocked...')
    else:
        print('Oh, not correct knock :(')


if __name__ == '__main__':
    sniff(filter=f"port 1189", prn=packet_handler)
    