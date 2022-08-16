using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Fido2me.Data;
using Fido2me.Data.FIDO2;
using System.Security.Claims;
using Fido2me.Services;
using Fido2me.Models;

namespace Fido2me.Pages.devices
{
    public class IndexModel : BasePageModel
    {
        private readonly IDeviceService _deviceService;
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(IDeviceService deviceService, ILogger<IndexModel> logger)
        {
            _deviceService = deviceService;
            _logger = logger;
        }

        public IList<DeviceViewModel> Devices { get;set; } = default!;

        public async Task OnGetAsync()
        {
            Devices = await _deviceService.GetDevicesByAccountIdAsync(AccountId);
        }

        public async Task<IActionResult> OnPostAddAsync()
        {
            
            return RedirectToPage("./Index");
        }

        public async Task<IActionResult> OnPostEnableAsync(string credentialId)
        {
            await _deviceService.ChangeDeviceStatusAsync(credentialId, AccountId);
            return RedirectToPage("./Index");
        }

        public async Task<IActionResult> OnPostDeleteAsync(string credentialId)
        {
            await _deviceService.DeleteDeviceAsync(credentialId, AccountId);
            return RedirectToPage("./Index");
        }
    }
}
