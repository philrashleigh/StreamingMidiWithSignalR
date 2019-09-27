using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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