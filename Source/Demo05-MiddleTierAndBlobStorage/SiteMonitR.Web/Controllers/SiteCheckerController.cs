using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Queue;
using SiteMonitR.Web.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace SiteMonitR.Web.Controllers
{
    public class SiteCheckerController : ApiController
    {
        private SiteMonitRContext _db;

        public SiteCheckerController()
        {
            _db = new SiteMonitRContext();
        }

        protected override void Dispose(bool disposing)
        {
            if (_db != null)
                _db.Dispose();

            base.Dispose(disposing);
        }

        private async Task<SiteStatusResult> CheckSite(MonitoredSite site)
        {
            var status = new SiteStatusResult { SiteId = site.Id, Url = site.Url };

            var client = new WebClient();

            var response = await client.DownloadStringTaskAsync(site.Url).ContinueWith(async (t) =>
            {
                status.Status = t.IsFaulted
                    ? SiteStatus.Down.ToString()
                    : SiteStatus.Up.ToString();

                // put the URL onto the queue if the site is online
                if (!t.IsFaulted)
                {
                    var account = CloudStorageAccount.DevelopmentStorageAccount;
                    var queue = account.CreateCloudQueueClient().GetQueueReference("incoming");
                    queue.CreateIfNotExists();
                    queue.AddMessage(new CloudQueueMessage(status.Url));
                }
            });

            return status;
        }

        public async Task<IQueryable<SiteStatusResult>> Get()
        {
            var sites = _db.MonitoredSites;
            var ret = new List<SiteStatusResult>();

            foreach(var site in sites)
            {
                ret.Add(await CheckSite(site));
            }

            return ret.AsQueryable();
        }

        public async Task<SiteStatusResult> Get(int? id)
        {
            return await CheckSite(_db.MonitoredSites.First(x => x.Id == id.Value));
        }
    }
}
