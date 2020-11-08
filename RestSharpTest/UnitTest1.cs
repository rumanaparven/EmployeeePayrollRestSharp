using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System.Collections.Generic;
using System.Net;

namespace RestSharpTest
{
    public class Employee
    {
        public int id { get; set; }
        public string name { get; set; }
        public string salary { get; set; }
    }
    [TestClass]
    public class RestSharpTestCase
    {
        RestClient client;
        [TestInitialize]
        public void SetUp()
        {
            client = new RestClient("http://localhost:4000");
        }
        private IRestResponse getEmployeeList()
        {
            RestRequest request = new RestRequest("/employee", Method.GET);
            IRestResponse response = client.Execute(request);
            return response;
        }
        [TestMethod]
        public void OnCallingList_ReturnEmployeeList()
        {
            IRestResponse response = getEmployeeList();

            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
            List<Employee> dataResponse = JsonConvert.DeserializeObject<List<Employee>>(response.Content);
            Assert.AreEqual(5, dataResponse.Count);

            foreach (Employee e in dataResponse)
            {
                System.Console.WriteLine("ID : " + e.id + " Name : " + e.name + " Salary : " + e.salary);
            }
        }
        [TestMethod]
        public void givenEmployee_OnPost_ReturnAddedEmployee()
        {
            RestRequest request = new RestRequest("/employee", Method.POST);
            JObject jObject = new JObject();
            jObject.Add("name", "Maxx");
            jObject.Add("salary", "25000");

            request.AddParameter("application/json", jObject, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            Assert.AreEqual(response.StatusCode, HttpStatusCode.Created);
            Employee dataResponse = JsonConvert.DeserializeObject<Employee>(response.Content);
            Assert.AreEqual("Maxx", dataResponse.name);
            Assert.AreEqual("25000", dataResponse.salary);
            System.Console.WriteLine(response.Content);

        }

        [TestMethod]
        public void givenmULTIPLEEmployee_OnPost_ReturnAllEmployee()
        {

            List<Employee> employeeList = new List<Employee>();
            employeeList.Add(new Employee { name = "Sagar", salary = "45000" });
            employeeList.Add(new Employee { name = "Mady", salary = "75000" });
            foreach (Employee e in employeeList)
            {
                RestRequest request = new RestRequest("/employee", Method.POST);
                JObject jObject = new JObject();
                jObject.Add("name", e.name);
                jObject.Add("salary", e.salary);

                request.AddParameter("application/json", jObject, ParameterType.RequestBody);
                IRestResponse response1 = client.Execute(request);
                Assert.AreEqual(response1.StatusCode, HttpStatusCode.Created);
            }

            IRestResponse response = getEmployeeList();

            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
            List<Employee> dataResponse = JsonConvert.DeserializeObject<List<Employee>>(response.Content);
            Assert.AreEqual(8, dataResponse.Count);

            foreach (Employee e in dataResponse)
            {
                System.Console.WriteLine("ID : " + e.id + " Name : " + e.name + " Salary : " + e.salary);
            }
        }
        [TestMethod]
        public void GivenEmployee_OnUpdate_ShouldReturnUpdatedEmployee()
        {
            RestRequest request = new RestRequest("/employee/4", Method.PUT);
            JObject jObject = new JObject();
            jObject.Add("name", "Manny");
            jObject.Add("salary", "92000");

            request.AddParameter("application/json", jObject, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
            Employee dataResponse = JsonConvert.DeserializeObject<Employee>(response.Content);
            Assert.AreEqual("Manny", dataResponse.name);
            Assert.AreEqual("92000", dataResponse.salary);
            System.Console.WriteLine(response.Content);
        }
        [TestMethod]
        public void GivenEmployeeID_OnDelete_ShouldReturnSuccessStatus()
        {
            RestRequest request = new RestRequest("/employee/5", Method.DELETE);
            IRestResponse response = client.Execute(request);
            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
            System.Console.WriteLine(response.Content);
        }
    }
}
