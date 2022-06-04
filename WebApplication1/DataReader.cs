using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1
{
    public interface ICustomer
    {
        [Display(Name = "Customer Identity Number")]
        int Id { get; set; }
        [Display(Name = "First Name")]
        string FirstName { get; set; }

        [Display(Name = "Last Name")]
        string LastName { get; set; }

    }


    public class Customer : ICustomer
    {
        [JsonProperty("id")]
        [Display(Name = "Customer Identity Number")]
        public int Id { get; set; }

        [JsonProperty("firstName")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [JsonProperty("lastName")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
    }

    public interface IDataReader
    {
        IEnumerable<ICustomer> GetAllCustomers();
        ICustomer Get(int id);

        Task<ICustomer> GetTask(int id);
        void AddCustomer(Customer newCustomer);
        void UpdateCustomer(Customer exisingCustomer);

        Task UpdateCustomerTask(Customer exisingCustomer);

        void DeleteCustomer(int id);

        Task DeleteCustomerTask(int id);
    }

    /// <summary>
    /// Usage of interface is to be able to use Factory pattern for flexibity. For instance if we need to use different data source like excel or remote claud db or in the case we have different customer implementation
    /// </summary>
    public class DataReader : IDataReader
    {
        private readonly string _filePath;
        private const string ConstDataPathConfig = "DataPath";
        public DataReader(IConfiguration config)
        {
            var dataSourceJsonPath = config[ConstDataPathConfig];
            if (string.IsNullOrEmpty(dataSourceJsonPath))
            {
                throw new ArgumentNullException(nameof(dataSourceJsonPath));
            }
            this._filePath = dataSourceJsonPath;
        }
        public IEnumerable<ICustomer> GetAllCustomers()
        {
            using (StreamReader r = new StreamReader(_filePath))
            {
                string json = r.ReadToEnd();
                return JsonConvert.DeserializeObject<List<Customer>>(json);
            }
        }
        public ICustomer Get(int id)
        {
            using (StreamReader r = new StreamReader(_filePath))
            {
                string json = r.ReadToEnd();
                return JsonConvert.DeserializeObject<List<Customer>>(json).FirstOrDefault(f => f.Id == id);
            }
        }

        public Task<ICustomer> GetTask(int id)
        {
            return Task.Run(() => this.Get(id));
        }


        public void AddCustomer(Customer newCustomer)
        {
            var json = File.ReadAllText(_filePath);
            var customers = JsonConvert.DeserializeObject<List<Customer>>(json);
            customers.Add(newCustomer);
            File.WriteAllText(_filePath, JsonConvert.SerializeObject(customers));
        }

        public void UpdateCustomer(Customer exisingCustomer)
        {
            var json = File.ReadAllText(_filePath);
            var customers = JsonConvert.DeserializeObject<List<Customer>>(json);
            foreach (var customer in customers)
            {
                if (customer.Id == exisingCustomer.Id)
                {
                    customer.FirstName = exisingCustomer.FirstName;
                    customer.LastName = exisingCustomer.LastName;
                }
            }
            File.WriteAllText(_filePath, JsonConvert.SerializeObject(customers));
        }

        public Task UpdateCustomerTask(Customer exisingCustomer)
        {
            return Task.Run(() => UpdateCustomer(exisingCustomer));
        }


        public void DeleteCustomer(int id)
        {
            var json = File.ReadAllText(_filePath);
            var customers = JsonConvert.DeserializeObject<List<Customer>>(json);
            var indexToRemove = customers.FindIndex(i => i.Id == id);
            customers.RemoveAt(indexToRemove);
            File.WriteAllText(_filePath, JsonConvert.SerializeObject(customers));
        }

        public Task DeleteCustomerTask(int id)
        {
            return Task.Run(() => DeleteCustomer(id));
        }

    }


}
