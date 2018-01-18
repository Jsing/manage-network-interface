using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management;
using System.Net.NetworkInformation;


public static class NetworkInterfaceManangement
{

    public static void EnableStatic(string networkAdapterName, string[] ipAddresses, string[] subnetMasks, string[] gateways, string[] dnses)
    {
        string macAddress = _NameToMacAddress(networkAdapterName);

        ManagementClass netAdapterConfigClass = new ManagementClass("Win32_NetworkAdapterConfiguration");
        ManagementObjectCollection netAdapterConfigObjectCollect = netAdapterConfigClass.GetInstances();

        foreach (ManagementObject netAdapterConfigObject in netAdapterConfigObjectCollect)
        {
            if (netAdapterConfigObject["MACAddress"] != null && netAdapterConfigObject["MACAddress"].Equals(macAddress))
            {
                // IPs, Subnet Masks
                ManagementBaseObject EnableStaticFunc = netAdapterConfigObject.GetMethodParameters("EnableStatic");

                EnableStaticFunc["IPAddress"] = ipAddresses;
                EnableStaticFunc["SubnetMask"] = subnetMasks;

                netAdapterConfigObject.InvokeMethod("EnableStatic", EnableStaticFunc, null);

                // Gateways 
                ManagementBaseObject SetGatewaysFunc = netAdapterConfigObject.GetMethodParameters("SetGateways");

                SetGatewaysFunc["DefaultIPGateway"] = gateways;

                netAdapterConfigObject.InvokeMethod("SetGateways", SetGatewaysFunc, null);

                // DNS 
                ManagementBaseObject EnableDNSFunc = netAdapterConfigObject.GetMethodParameters("SetDNSServerSearchOrder");

                EnableDNSFunc["DNSServerSearchOrder"] = dnses;

                netAdapterConfigObject.InvokeMethod("SetDNSServerSearchOrder", EnableDNSFunc, null);

            }
        }
    }

    public static void EnableDynamic(string networkAdapterName)
    {
        string macAddress = _NameToMacAddress(networkAdapterName);

        ManagementClass netAdapterConfigClass = new ManagementClass("Win32_NetworkAdapterConfiguration");
        ManagementObjectCollection netAdapterConfigObjectCollect = netAdapterConfigClass.GetInstances();

        foreach (ManagementObject netAdapterConfigObject in netAdapterConfigObjectCollect)
        {
            if (netAdapterConfigObject["MACAddress"] != null && netAdapterConfigObject["MACAddress"].Equals(macAddress))
            {
                // DHCP 
                ManagementBaseObject EnableDynamicFunc = netAdapterConfigObject.GetMethodParameters("EnableDHCP");

                netAdapterConfigObject.InvokeMethod("EnableDHCP", EnableDynamicFunc, null);

                // Dynamic DNS 
                ManagementBaseObject EnableDNSFunc = netAdapterConfigObject.GetMethodParameters("SetDNSServerSearchOrder");

                EnableDNSFunc["DNSServerSearchOrder"] = null;

                netAdapterConfigObject.InvokeMethod("SetDNSServerSearchOrder", EnableDNSFunc, null);
            }

        }
    }

    private static string _NameToMacAddress(string networkAdapterName)
    {
        string macAddress = string.Empty;
        ManagementClass netAdapterClass = new ManagementClass("Win32_NetworkAdapter");
        ManagementObjectCollection netAdapterObjectSet = netAdapterClass.GetInstances();

        foreach (ManagementObject netAdapterObj in netAdapterObjectSet)
        {
            if (netAdapterObj["NetConnectionID"] != null && netAdapterObj["NetConnectionID"].Equals(networkAdapterName))
            {
                macAddress = (string)netAdapterObj["MACAddress"];
            }
        }

        if (macAddress == string.Empty)
        {
            throw new ApplicationException(string.Format("\"{0}\" Network Adapter is not available."));
        }

        return macAddress;
    }


}
