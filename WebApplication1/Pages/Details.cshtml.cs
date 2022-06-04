using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace WebApplication1.Pages
{
    public class DetailsModel : PageModel
    {
        private readonly IDataReader _dataReader;
        private readonly ILogger<IndexModel> _logger;
        public DetailsModel(ILogger<IndexModel> logger, IDataReader dataReader)
        {
            _logger = logger;
            _dataReader = dataReader;
        }

        [BindProperty]
        public ICustomer Customer { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Customer = await Task.Run(() => _dataReader.GetAllCustomers().FirstOrDefault(f => f.Id == id));

            if (Customer == null)
            {
                return NotFound();
            }
            return Page();
        }

     

    }
}
