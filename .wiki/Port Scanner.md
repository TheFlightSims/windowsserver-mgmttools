# Port Scanner

PortScanner is used to scan any opening ports of the target computer.

## Contributors

| Contributor | Description |
|--|--|
| [@IceMoonHSV](https://github.com/IceMoonHSV) | Original developers of Port Scanner |
| [@TheFlightSimsOfficial](https://github.com/TheFlightSimsOfficial) | Contributor |
| [@shiroinekotfs](https://github.com/shiroinekotfs) | Contributor |

## Use Port Scanner

Try to run `PortScanner.exe` and you'll get this help screen.

```bash
PortScanner is a part of Windows Server Management Tools
Currently maintained by TheFlightSims

See more at: https://github.com/TheFlightSims/windowsserver-mgmttools

PortScanner.exe {hosts} {ports} [timeout] [outfile]

DESCRIPTION:
    PortScanner is used to scan any opening ports of the target computer
    The commands are depend on how you use, but general mapping are still
    can be seen above.

GENERAL COMMANDS:
    hosts         - Target computers. Can use FQDN or IP. Seperate by comma

    ports         - Ports that needs to be scanned. Seperate by comma.
                    You can use defined ports. See below for more info.
                    Port range is not supported in this version

    timeout       - Maximum scanning time per port. Useful when scanning on
                    large number of ports

    outfile       - Export results to a file. R/W is required on the target
                    directory

DEFINED PORTS:

    Defined ports are the ports containing specific roles, providing specific
    features on computer. The list below shows all defined ports on PortScanner

    admin         - 135, 139, 445, 3389, 5985, 5986, 8000, 8080.

    web           - 21, 23, 25, 80, 443, 8080.

    top20         - 21, 22, 23, 25, 53, 80, 110, 111, 135, 139,
                    143, 443, 445, 993, 995, 1723, 3306, 3389,
                    5900, 8080.

    server-common - 7, 9, 13, 17, 19, 25, 42, 80, 88, 110, 111,
                    119, 135, 149, 389, 443, 445, 465, 563, 587,
                    636, 808, 993, 995, 1433, 1688, 1801, 3268,
                    3269, 3387, 3388, 3389, 4044, 6516, 6881,
                    8000, 8080, 8800, 8391, 8443, 8530, 8531,
                    9389

    all-ports    - All ports in range 1, 65535

SAMPLES:
    PortScanner.exe hosts=127.0.0.1,google.com ports=21,22,23 timeout=5000 outfile=C:\scans.txt
    PortScanner.exe hosts=localhost ports=admin
```
