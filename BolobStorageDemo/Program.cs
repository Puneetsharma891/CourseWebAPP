using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Sas;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Auth;
using Microsoft.Azure.Storage.Blob;
using System;

namespace BolobStorageDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            string containername = "mycontainer2021";
            string _conn = "DefaultEndpointsProtocol=https;AccountName=storageclitest2021;AccountKey=UlOoeJuTkCn7L3tiO0Glap/+zyCQ4kHA5mIl2CEsMWSW9p0OY/W39og+iIq3V9luGHaOXd2lU9ZggG4GuZmp2Q==;EndpointSuffix=core.windows.net";
            //OldBlobClient(cred);




            NewBlobClient(containername, _conn);

            Console.Read();
        }

        private static void OldBlobClient()
        {
            StorageCredentials cred = new StorageCredentials("storageclitest2021", "UlOoeJuTkCn7L3tiO0Glap / +zyCQ4kHA5mIl2CEsMWSW9p0OY / W39og + iIq3V9luGHaOXd2lU9ZggG4GuZmp2Q ==");

            CloudStorageAccount storageAccount = new CloudStorageAccount(cred, true);

            CloudBlobClient blobclient = storageAccount.CreateCloudBlobClient();

            CloudBlobContainer container = blobclient.GetContainerReference("msstorageblob2021");

            container.CreateIfNotExists();

            CloudBlockBlob blockBlob = container.GetBlockBlobReference("course.json");
            blockBlob.UploadFromFile(@"E:\course.json");


            string content = blockBlob.DownloadText();
            Console.WriteLine(content);
        }

        private static void NewBlobClient(string containername, string _conn)
        {
            string blobName = "Course1.json";
            BlobServiceClient _serviceClient = new BlobServiceClient(_conn);
            BlobContainerClient containerClient = _serviceClient.GetBlobContainerClient(containername);
            containerClient.CreateIfNotExists();
            
           // DownloadBlobUsingSasURI();

            //CreateBlobs(containerClient);
            //DownloadAllBlobFromContainer(containerClient);
        }

        private static void DownloadBlobUsingSasURI(BlobContainerClient containerClient)
        {
            string blobName = "Course1.json";
            Uri sasURL = GenerateSasURL(containerClient.Name, blobName, containerClient);
            BlobClient _clientBlob = new BlobClient(sasURL);
            _clientBlob.DownloadTo(@"E:\sasJSONDemo.json");
        }

        private static Uri GenerateSasURL(string containername, string blobName, BlobContainerClient containerClient)
        {
            BlobClient _client = containerClient.GetBlobClient(blobName);

            BlobSasBuilder _builder = new BlobSasBuilder()
            {
                BlobContainerName = containername,
                BlobName = blobName,
                Resource = "b"
            };
            _builder.SetPermissions(BlobAccountSasPermissions.Read | BlobAccountSasPermissions.List);

            _builder.ExpiresOn = DateTime.Now.AddHours(1);

            Uri sasURL = _client.GenerateSasUri(_builder);
            return sasURL;
        }

        private static void DownloadAllBlobFromContainer(BlobContainerClient containerClient)
        {
            foreach (BlobItem blobItems in containerClient.GetBlobs())
            {
                Console.WriteLine($"Blob named {blobItems.Name} is found");

                BlobClient blobClient = containerClient.GetBlobClient(blobItems.Name);

                blobClient.DownloadTo(@"E:\" + blobItems.Name);
            }
        }

        private static void CreateBlobs(BlobContainerClient containerClient)
        {
            for (int i = 0; i < 5; i++)
            {
                BlobClient blobClient = containerClient.GetBlobClient($"Course{i}.json");



                blobClient.Upload(@"E:\course.json");
            }
        }
    }
}
