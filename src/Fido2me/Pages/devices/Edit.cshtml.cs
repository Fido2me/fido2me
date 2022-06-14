using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Fido2me.Data;
using Fido2me.Data.FIDO2;
using Fido2me.Services;
using Fido2me.Models;

namespace Fido2me.Pages.devices
{
    public class EditModel : PageModel
    {
        private readonly IDeviceService _deviceService;
        private readonly ILogger<EditModel> _logger;

        public EditModel(IDeviceService deviceService, ILogger<EditModel> logger)
        {
            _deviceService = deviceService;
            _logger = logger;
        }


        [BindProperty]
        public DeviceViewModel Device { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(string credentialId)
        {
            var device = await _deviceService.GetDeviceByCredentialIdAsync(credentialId);
            if (device == null)
            {
                return NotFound();
            }
            Device = device;

            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            ModelState.Remove("Device.DeviceDescription");
            ModelState.Remove("DeviceDescription");
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var accountId = new Guid(User.FindFirst(c => c.Type == "sub").Value);
            var result = await _deviceService.UpdateDeviceAsync(Device, accountId);
            

            return RedirectToPage("./Index");
        }


    }
}
