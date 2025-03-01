using System.Collections.Concurrent;
using System.Net.NetworkInformation;

namespace NetworkScanner
{
    public class NetworkScanner
    {
        public async Task ScanSubnetAsync(string subnet)
        {
            // Split the subnet into base IP for iteration
            var baseIp = subnet.Split('.').Take(3).ToArray();
            var activeIps = new ConcurrentBag<IpAddressInfo>();

            var tasks = new List<Task>();
            // Iterate over all possible IP addresses in the subnet (1 to 254)
            for (var i = 1; i <= 254; i++)
            {
                var ipAddress = $"{baseIp[0]}.{baseIp[1]}.{baseIp[2]}.{i}";
                tasks.Add(CheckIpAsync(ipAddress, activeIps));
            }

            await Task.WhenAll(tasks);

            // Sort the list by the last octet of the IP
            var sortedIps = activeIps.OrderBy(ip => ip.LastOctet).ToList();

            // Print the sorted active IPs
            Console.WriteLine($"\n{sortedIps.Count} Active IPs found:");
            foreach (var ip in sortedIps)
            {
                Console.WriteLine(ip.Address);
            }
        }

        // Check if the IP is active and return a bool, avoiding shared list access
        private async Task<bool> CheckIpAsync(string ipAddress, ConcurrentBag<IpAddressInfo> activeIps)
        {
            using var ping = new Ping();
            try
            {
                var reply = await ping.SendPingAsync(ipAddress, 500);
                // If the reply status is successful, return true and add the IP to the list
                if (reply.Status == IPStatus.Success)
                {
                    activeIps.Add(new IpAddressInfo(ipAddress));
                    return true;
                }
                return false;
            }
            catch
            {
                // Ignore any errors and return false if unable to reach the IP
                return false;
            }
        }
    }

    // Class to store IP address information
    public record IpAddressInfo(string Address)
    {
        public int LastOctet { get; } = int.Parse(Address.Split('.').Last());
    }

    public abstract class Program
    {
        public static async Task Main(string[] args)
        {
            const string subnet = "192.168.1"; // Example subnet
            var scanner = new NetworkScanner();
            await scanner.ScanSubnetAsync(subnet);
        }
    }
}
