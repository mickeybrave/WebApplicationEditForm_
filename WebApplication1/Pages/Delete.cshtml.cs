using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace WebApplication1.Pages
{
    public class DeleteModel : PageModel
    {
        [BindProperty]
        public Customer Customer { get; set; }


        private readonly IDataReader _dataReader;
        private readonly ILogger<IndexModel> _logger;

        public DeleteModel(ILogger<IndexModel> logger, IDataReader dataReader)
        {
            _logger = logger;
            _dataReader = dataReader;
        }
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Customer = (Customer)await _dataReader.GetTask(id.GetValueOrDefault());

            if (Customer == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            await _dataReader.DeleteCustomerTask(id.GetValueOrDefault());

            return RedirectToPage("./Index");
        }
    }
}
