using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace PhilipRashleigh.StreamingMidi.Client.Pages
{
    public class ReceiverModel : PageModel
    {
        private readonly ILogger<ReceiverModel> _logger;

        public ReceiverModel(ILogger<ReceiverModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
        }
    }
}