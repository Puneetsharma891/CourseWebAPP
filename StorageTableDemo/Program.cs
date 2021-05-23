using Microsoft.Azure.Cosmos.Table;
using System;
using System.Collections.Generic;

namespace StorageTableDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            CloudStorageAccount account = CloudStorageAccount.Parse("DefaultEndpointsProtocol=https;AccountName=storageclitest2021;AccountKey=UlOoeJuTkCn7L3tiO0Glap/+zyCQ4kHA5mIl2CEsMWSW9p0OY/W39og+iIq3V9luGHaOXd2lU9ZggG4GuZmp2Q==;EndpointSuffix=core.windows.net");

            CloudTableClient tableClient = account.CreateCloudTableClient();

            CloudTable table = tableClient.GetTableReference("Course");
            table.CreateIfNotExists();
            ////Add Entity
            //AddEntity(table);

            //AddBatchEntity(table);
            ////Update entity
            TableOperation op;
            TableResult ret;
            UpdateEnityt(table, out op, out ret);



            //Delete Entity
            op = DeleteEntity(table, op, ret);
            ///fetch entiry using simple query or multi exp query


            //RetriveData(table);

        }

        private static TableOperation DeleteEntity(CloudTable table, TableOperation op, TableResult ret)
        {
            TableOperation opd = TableOperation.Retrieve<Course>("Hindi", "C01");

            TableResult retd = table.Execute(op);

            Course cod = ret.Result as Course;


            op = TableOperation.Delete(cod);

            table.Execute(op);
            return op;
        }

        private static void UpdateEnityt(CloudTable table, out TableOperation op, out TableResult ret)
        {
            op = TableOperation.Retrieve<Course>("Hindi", "C01");
            ret = table.Execute(op);
            Course co = ret.Result as Course;

            co.InstructorName = "UPDATE pUNEET";

            TableOperation opu = TableOperation.InsertOrMerge(co);

            co.InstructorName = "UPDATE pUNEET replace";
            table.Execute(opu);

            TableOperation opu2 = TableOperation.InsertOrReplace(co);

            table.Execute(opu2);
        }

        private static void RetriveData(CloudTable table)
        {
            string filter = TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, "Hindi");
            string filter2 = TableQuery.GenerateFilterCondition("InstructorName", QueryComparisons.Equal, "Puneet");
            string filters = TableQuery.CombineFilters(filter, TableOperators.And, filter2);
            TableQuery<Course> query = new TableQuery<Course>().Where(filters);

            var courses = table.ExecuteQuery(query);
            foreach (var e in courses)
            {
                Console.WriteLine("RowKey: {0}, EmployeeEmail: {1}", e.RowKey, e.Duration);
            }
        }

        private static void AddBatchEntity(CloudTable table)
        {
            ////Add list of entity
            Course _courser1 = new Course("C01", "Hindi", 12, "Puneet");
            //Course _courser2 = new Course("C02", "English", 12, "Parul");
            Course _courser3 = new Course("C03", "Hindi", 12, "Puneet");
            Course _courser4 = new Course("C04", "Hindi", 12, "Puneet");
            //Course _courser5 = new Course("C05", "Math", 12, "Puneet");
            //Course _courser6 = new Course("C06", "science", 12, "Puneet");
            List<Course> list = new List<Course>();
            list.Add(_courser1); list.Add(_courser3); list.Add(_courser4);
            TableBatchOperation op = new TableBatchOperation();
            foreach (var n in list)
            {
                op.Insert(n);
            }

            table.ExecuteBatch(op);
        }

        private static void AddEntity(CloudTable table)
        {
            Course _course = new Course("C01", "Hindi3", 12, "Puneet");
            TableOperation addOperation = TableOperation.Insert(_course);

            TableResult ret = table.Execute(addOperation);
        }
    }
}
