from wakeonlan import send_magic_packet # pip install wakeonlan
import argparse

parser = argparse.ArgumentParser(description='Process some integers.')
parser.add_argument('-p', type=int, help='port')
parser.add_argument('-h', type=str, help='host (ip/domain)')
parser.add_argument('-a', type=str, help='pysical address, aa:bb:cc:dd:ee:ff')

args = parser.parse_args()
send_magic_packet(args.a, ip_address=args.h, port=args.p)