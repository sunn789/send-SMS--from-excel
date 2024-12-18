using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace hamayes.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;

    public IndexModel(ILogger<IndexModel> logger)
    {
        _logger = logger;
    }

        [BindProperty]
    public List<Persons> Persons { get; set; } = new List<Persons>();
    public void OnGet()
    {
    }
     public IActionResult OnPostAsync()
    {
        var file = Request.Form.Files.FirstOrDefault();


        if (file != null && file.Length > 0)
        {
            using (var stream = new MemoryStream())
            {
                file.CopyTo(stream);
                var excelHelper = new ExcelHelper();
                Persons = excelHelper.ReadExcel(stream);
            }
        }

        return Page();
    }
}
