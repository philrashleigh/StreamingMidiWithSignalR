using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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