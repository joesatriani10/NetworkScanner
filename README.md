
# NetworkScanner

`NetworkScanner` is a simple network scanning tool built in C# that pings all IP addresses within a given subnet to find active IPs. This tool is useful for network administrators or anyone interested in scanning local networks for active devices.

## Features
- Scan a subnet (e.g., 192.168.1.0/24) for active IPs.
- Concurrently ping IP addresses to speed up the scanning process.
- Sort the active IP addresses by the last octet and print them in a readable format.

## Requirements
- .NET 5.0 or higher
- C# (compatible with Visual Studio, Visual Studio Code, Rider, etc.)

## How to Use

1. Clone or download the repository.
2. Open the project in your preferred C# IDE or editor.
3. Update the `subnet` variable in the `Program.cs` file to the subnet you want to scan (e.g., `"192.168.1"`).
4. Build and run the application.

Example:
```csharp
const string subnet = "192.168.1"; // Example subnet
var scanner = new NetworkScanner();
await scanner.ScanSubnetAsync(subnet);
```

### Example Output:
```
5 Active IPs found:
192.168.1.1
192.168.1.10
192.168.1.12
192.168.1.23
192.168.1.50
```

## Code Explanation

- **NetworkScanner Class**: This is the core class that handles scanning the network. It takes a subnet (e.g., `192.168.1`) and pings all IP addresses in that range (from `.1` to `.254`).
- **IpAddressInfo Class**: Stores information about each active IP address, including the last octet, which is used to sort the active IPs.
- **Main Method**: The entry point of the application, where the subnet to be scanned is specified.

## How It Works

1. The program splits the subnet into the base IP (first three octets) and iterates over all possible IP addresses from `.1` to `.254`.
2. It sends a ping request to each IP address asynchronously.
3. If the IP responds, it is added to the list of active IPs.
4. After scanning, it sorts the active IPs by their last octet and prints them.

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.
