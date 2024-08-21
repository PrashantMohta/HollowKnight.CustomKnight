import sys
import hashlib

name=''
if len(sys.argv) < 2:
    print('''
Missing Parameters
          Usage : python sha256.py <file path>
''') 
    exit(1)
else:
    name = sys.argv[1]
    

BUF_SIZE = 65536 
sha256 = hashlib.sha256()

with open(name, 'rb') as f:
    while True:
        data = f.read(BUF_SIZE)
        if not data:
            break
        sha256.update(data)

print("{0}".format(sha256.hexdigest()))