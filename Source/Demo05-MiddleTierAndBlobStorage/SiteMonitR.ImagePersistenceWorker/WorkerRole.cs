using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Diagnostics;
using Microsoft.WindowsAzure.ServiceRuntime;
using System.IO;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace SiteMonitR.ImagePersistenceWorker
{
    public class WorkerRole : RoleEntryPoint
    {
        public override void Run()
        {
            // blob storage account
            var account = 
                CloudStorageAccount.Parse(
                    RoleEnvironment
                        .GetConfigurationSettingValue("DataConnectionString"));

            // where we'll save the images
            var container = 
                account.CreateCloudBlobClient().GetContainerReference("output");
            container.CreateIfNotExists();
            container.SetPermissions(new BlobContainerPermissions
            {
                PublicAccess = BlobContainerPublicAccessType.Blob
            });

            // queue where the URL string is sent
            var queue = account.CreateCloudQueueClient()
                .GetQueueReference("incoming");
            queue.CreateIfNotExists();

            // local place where we'll store the file before we upload it
            var outputPath = Path.Combine(RoleEnvironment.GetLocalResource("LocalOutput").RootPath, "output.png");

            while (true)
            {
                // get the message off of the queue
                var msg = queue.GetMessage();
                if (msg != null)
                {
                    if (msg.DequeueCount < 3)
                    {
                        // start cutycapt.exe to save the image of the site
                        var args = string.Format(@" --url=""{0}"" --out=""{1}"" --max-wait=10000",
                                        msg.AsString,
                                        outputPath);

                        var proc = new Process()
                        {
                            StartInfo = new ProcessStartInfo(Environment.GetEnvironmentVariable("RoleRoot")
                                + @"\\approot\CutyCapt.exe", args)
                            {
                                UseShellExecute = false,
                                CreateNoWindow = true
                            }
                        };

                        proc.Start();
                        proc.WaitForExit();

                        if (File.Exists(outputPath))
                        {
                            var url = msg.AsString.Replace("http://", "").Replace("https://", "");
                            var filename = string.Format("{0}.png", url);

                            // delete the blob for this site so we can re-create it
                            container.GetBlockBlobReference(filename).DeleteIfExists();

                            // now save the screen shot of the site
                            var blob = container.GetBlockBlobReference(filename);
                            blob.Properties.ContentType = "image/png";
                            using (var fs = (File.Open(outputPath, FileMode.Open)))
                            {
                                blob.UploadFromStream(fs);
                                fs.Close();
                            }

                            File.Delete(outputPath);
                        }
                    }
                    queue.DeleteMessage(msg);
                }
                else
                {
                    Thread.Sleep(TimeSpan.FromSeconds(1));
                }
            }
        }

        public override bool OnStart()
        {
            return base.OnStart();
        }
    }
}
