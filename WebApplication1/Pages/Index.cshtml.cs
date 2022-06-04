using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IDataReader _dataReader;

        public IndexModel(ILogger<IndexModel> logger, IDataReader dataReader)
        {
            _logger = logger;
            _dataReader = dataReader;
        }

        [BindProperty]
        public IList<ICustomer> Customers { get; set; }

        public string SearchFirstName { get; set; }
        public string SearchLastName { get; set; }


        public async Task OnGetAsync(string searchFirstName, string searchLastName)
        {
            SearchFirstName = searchFirstName;
            SearchLastName = searchLastName;
            Customers = await Task.Run(() => _dataReader.GetAllCustomers().ToList());

            if (!ModelState.IsValid)
            {
                return;
            }

            if (!String.IsNullOrEmpty(searchFirstName))
            {
                Customers = await Task.Run(() => Customers.Where(w => w.FirstName.ToUpper().Contains(searchFirstName.ToUpper())).ToList());
            }


            if (!String.IsNullOrEmpty(searchLastName))
            {
                Customers = await Task.Run(() => Customers.Where(w => w.LastName.ToUpper().Contains(searchLastName.ToUpper())).ToList());
            }

        }
    }
}
