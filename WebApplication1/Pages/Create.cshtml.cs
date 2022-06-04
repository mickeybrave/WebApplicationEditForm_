using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace WebApplication1.Pages
{
    public class CreateModel : PageModel
    {
        private readonly IDataReader _dataReader;
        private readonly ILogger<IndexModel> _logger;

    
        public CreateModel(ILogger<IndexModel> logger, IDataReader dataReader)
        {
            _logger = logger;
            _dataReader = dataReader;
        }
        public IActionResult OnGet()
        {
            Customer = new Customer
            {
               
            };
            return Page();
        }

        [BindProperty]
        public Customer Customer { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            Customer.Id = _dataReader.GetAllCustomers().Count() + 1;
            _dataReader.AddCustomer(Customer);

            return RedirectToPage("./Index");
        }
    }
}
