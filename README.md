# MBusParser
Somewhat fast mbus parser. Uses ~1374 ticks per wmbus packet, thats including decryption and whatnot. On my laptop thats 13.7seconds to parse 100000 wmbus packets.

## Features
 - Conforms to _EN 13757-3_ and _EN 13757-4._ **2013** (If you want support for more recent standards, feel free to buy me one.)
 - Tested on more than one million wmbus real-world packets!

## Whats working.
- Able to wmbus and mbus. Encrypted and unencryped.
- FB, FD VIFE's fully supported.

## TODO:

- Parse other things than integers
- Write tests.
- Support more orthogonal value information extensions
- Ensure correct unit is chosen.
