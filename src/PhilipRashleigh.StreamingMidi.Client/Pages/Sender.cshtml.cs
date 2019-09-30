using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace PhilipRashleigh.StreamingMidi.Client.Pages
{
    public class SenderModel : PageModel
    {
        private readonly ILogger<SenderModel> _logger;

        public SenderModel(ILogger<SenderModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
        }
    }
}