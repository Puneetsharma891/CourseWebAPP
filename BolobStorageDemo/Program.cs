using Azure.Identity;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Sas;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Auth;
using Microsoft.Azure.Storage.Blob;
using System;
using System.Collections.Generic;
using BlobProperties = Azure.Storage.Blobs.Models.BlobProperties;

namespace BolobStorageDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            string containername = "mycontainer2021";
            string _conn = "DefaultEndpointsProtocol=https;AccountName=storageclitest2021;AccountKey=UlOoeJuTkCn7L3tiO0Glap/+zyCQ4kHA5mIl2CEsMWSW9p0OY/W39og+iIq3V9luGHaOXd2lU9ZggG4GuZmp2Q==;EndpointSuffix=core.windows.net";
            //OldBlobClient(cred);

            BlobClient _blob = AccessBlobUsingAppObject();
            ReadSetMetaData(_blob);

            NewBlobClient(containername, _conn);

            Console.Read();
        }

        private static void ReadSetMetaData(BlobClient _blob)
        {
            BlobProperties props = _blob.GetProperties();

            IDictionary<string, string> metadata = props.Metadata;
            metadata.Add("Test", "Test");
            _blob.SetMetadata(metadata);
        }

        private static BlobClient AccessBlobUsingAppObject()
        {
            string appId = "a36d46c9-9335-4137-9e2f-d0079fc15ff5";
            string tenantId = "9a0c6406-e26c-4288-a20c-4df2a8eb78a7";
            string secretId = "pbg-ZlEbO_.4GxC7.023jthP-oV0U_8o6s";
            string blobUrl = "https://storageclitest2021.blob.core.windows.net/mycontainer2021/Course0.json";

            BlobClient _blob = new BlobClient(new Uri(blobUrl), new ClientSecretCredential(tenantId, appId, secretId));

            _blob.DownloadTo(@"d:\" + _blob.Name);
            return _blob;
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
            Azure.Storage.Blobs.Models.BlobProperties props = _clientBlob.GetProperties();

            IDictionary<string, string> metadata = props.Metadata;
            metadata.Add("test", "test");
            _clientBlob.SetMetadata(metadata);

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
