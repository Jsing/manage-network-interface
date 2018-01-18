using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


class Program
{
    static void Main(string[] args)
    {
        string networkAdapterName = "이더넷";
        string[] ipList = new string[] { "192.168.1.3"};
        string[] netMaskList = new string[] { "255.255.255.0" };
        string[] gatewayList = new string[] { "192.168.2.1"}; 
        string[] dnsList = new string[] {"192.168.1.111"};

        NetworkInterfaceManangement.EnableStatic(networkAdapterName, ipList, netMaskList, gatewayList, dnsList);
        
        //NetworkInterfaceManangement.EnableDynamic(nicName);

        Console.WriteLine("Network Interface configuration complete!");
    }
}
