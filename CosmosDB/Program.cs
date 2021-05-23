using Microsoft.Azure.Cosmos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CosmosDB
{
    class Program
    {
        static void Main(string[] args)
        {
            string dbName = "coursedb2021";
            string containerName = "coursecontainer2021";

            string conn = "AccountEndpoint=https://cosmosdbdemo2021.documents.azure.com:443/;AccountKey=PgvdGsXWBUAyGJDpKdXy62cg23YkTPFMjv5GjA0nVqOHK7M166Y9rogV7LObIZrYJjbJWxFePNiKWgaqdUjhvA==;";
            CosmosClient _client = new CosmosClient(conn);
           // CreateDBContainer(dbName, containerName, _client);

            ////Add 1 item
            //AddItem(dbName, containerName, _client);
            //AddBulk(dbName, containerName, conn);

            Database db = _client.GetDatabase(dbName);
            Container _container = db.GetContainer(containerName);
            // FetchMultipleItems(_container);
            //Course _course = FetchSingleItem(_container);
            //UpdateItem(_container, _course);

            //DeleteItem(_container);

           string ret = _container.Scripts.ExecuteStoredProcedureAsync<string>("Demo", new PartitionKey(""), null).GetAwaiter().GetResult();
           
        }

        private static void DeleteItem(Container _container)
        {
            _container.DeleteItemAsync<Course>("2", new PartitionKey("C02"));
        }

        private static void UpdateItem(Container _container, Course _course)
        {
            _course.Duration = 5000;
            _container.ReplaceItemAsync<Course>(_course, "2", new PartitionKey("C02")).GetAwaiter().GetResult();
        }

        private static Course FetchSingleItem(Container _container)
        {
            Course course = _container.ReadItemAsync<Course>("2", new PartitionKey("C02")).GetAwaiter().GetResult();
            Console.WriteLine($"CourseID {course.CourseId}     CourseName {course.CourseName}    Duration {course.Duration}    Instructor {course.InstructorName}");
            return course;
        }

        private static void FetchMultipleItems(Container _container)
        {
            QueryDefinition query = new QueryDefinition("select * from c");

            FeedIterator<Course> coursesIterator = _container.GetItemQueryIterator<Course>(query);

            while (coursesIterator.HasMoreResults)
            {
                FeedResponse<Course> courseResponse = coursesIterator.ReadNextAsync().GetAwaiter().GetResult();
                foreach (Course course in courseResponse.Resource)
                {
                    Console.WriteLine($"CourseID {course.CourseId}     CourseName {course.CourseName}    Duration {course.Duration}    Instructor {course.InstructorName}");
                }
            }
        }

        private static void AddBulk(string dbName, string containerName, string conn)
        {
            List<Course> courses = new List<Course>();

            courses.Add(new Course("1", "C01", "Hindi", 12, "Puneet"));
            courses.Add(new Course("2", "C02", "English", 12, "Parul"));
            courses.Add(new Course("3", "C03", "Hindi", 12, "Puneet"));
            courses.Add(new Course("4", "C04", "Hindi", 12, "Puneet"));
            courses.Add(new Course("5", "C05", "Math", 12, "Puneet"));
            courses.Add(new Course("6", "C06", "science", 12, "Puneet"));

            CosmosClient _clientNew = new CosmosClient(conn, new CosmosClientOptions { AllowBulkExecution = true });
            Database db = _clientNew.GetDatabase(dbName);
            Container _container = db.GetContainer(containerName);
            List<Task> tasks = new List<Task>();
            foreach (var course in courses)
            {
                tasks.Add(_container.CreateItemAsync(course, new PartitionKey(course.CourseId)));
            }

            Task.WhenAll(tasks).GetAwaiter().GetResult();
        }

        private static void AddItem(string dbName, string containerName, CosmosClient _client)
        {
            Course _courser1 = new Course("10", "C10", "Hindi", 12, "Puneet");
            Container _container = _client.GetContainer(dbName, containerName);
            _container.CreateItemAsync<Course>(_courser1, new PartitionKey(_courser1.CourseId)).GetAwaiter().GetResult();
        }

        private static void CreateDBContainer(string dbName, string containerName, CosmosClient _client)
        {
            _client.CreateDatabaseAsync(dbName).GetAwaiter().GetResult();

            Database db = _client.GetDatabase(dbName);
            db.CreateContainerAsync(new ContainerProperties(containerName, "/CourseId")).GetAwaiter().GetResult();
        }
    }
}
