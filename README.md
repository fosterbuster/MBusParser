# MBusParser
Somewhat fast mbus parser. Uses ~586 ticks per wmbus packet, thats including decryption and whatnot. Or on my shitty laptop, about one minute to parse one million unique wmbus packets.
## Features
 - Conforms to _EN 13757-3_ and _EN 13757-4._ **2013** (If you want support for more recent standards, feel free to buy me one.)
 - Tested on more than one million wmbus real-world packets!

## Whats working.
- Able to wmbus and mbus. Encrypted and unencryped.
- FB, FD VIFE's fully supported.

## TODO:

- Parse date and date time.
- Write more tests.
- Support more orthogonal value information extensions
- Ensure correct unit is chosen.
