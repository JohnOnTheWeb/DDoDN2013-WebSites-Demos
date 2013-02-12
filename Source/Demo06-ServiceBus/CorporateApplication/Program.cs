using SharedDomain;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorporateApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            var serviceBusConnectionString = 
                ConfigurationManager.AppSettings["serviceBusConnectionString"];

            ServiceBusHelper.ServiceBus
                .Setup(serviceBusConnectionString, "sitemonitr")
                .Subscribe<SitePingResult>((result) =>
                    {
                        var context = new SitePingDataContext();
                        context.SitePingResults.Add(result);
                        context.SaveChanges();

                        var output = string.Format("Site {0} is {1} at {2} saved to database",
                            result.Url, result.Status, result.TimeStamp);
                        Console.WriteLine(output);
                    });
        }
    }
}
