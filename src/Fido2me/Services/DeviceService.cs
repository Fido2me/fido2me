using Fido2me.Data;
using Fido2me.Models;
using Microsoft.EntityFrameworkCore;

namespace Fido2me.Services
{

    public interface IDeviceService
    {
        Task<List<DeviceViewModel>> GetDevicesByAccountIdAsync(Guid accountId);
        Task<DeviceViewModel> GetDeviceByCredentialIdAsync(string credentialId);
        Task<bool> UpdateDeviceAsync(DeviceViewModel device, Guid accountId);
        Task DeleteDeviceAsync(string credentialId, Guid accountId);
        Task ChangeDeviceStatusAsync(string credentialId, Guid accountId);
    }

    public class DeviceService : IDeviceService
    {
        private readonly DataContext _dataContext;
        private readonly ILogger<DeviceService> _logger;

        public DeviceService(DataContext dataContext, ILogger<DeviceService> logger)
        {
            _dataContext = dataContext;
            _logger = logger;
        }

        public async Task ChangeDeviceStatusAsync(string credentialId, Guid accountId)
        {
            var device = await _dataContext.Credentials.FirstOrDefaultAsync(c => c.CredentialId == credentialId && c.AccountId == accountId);
            if (device != null)
            {
                device.Enabled = !device.Enabled;                
                await _dataContext.SaveChangesAsync();
            }
        }

        public async Task DeleteDeviceAsync(string credentialId, Guid accountId)
        {
            var device = await _dataContext.Credentials.FirstOrDefaultAsync(c => c.CredentialId == credentialId && c.AccountId == accountId);
            if (device != null)
            {
                _dataContext.Credentials.Remove(device);
                await _dataContext.SaveChangesAsync();
            }
        }

        public async Task<DeviceViewModel> GetDeviceByCredentialIdAsync(string credentialId)
        {
            var device = await _dataContext.Credentials.Where(c => c.CredentialId == credentialId).AsNoTracking()
                .Select(c => new DeviceViewModel() 
                { 
                    Enabled = c.Enabled,
                    CredentialId = c.CredentialId,
                    DeviceDescription = c.DeviceDescription,
                    DeviceDisplayName = c.DeviceDisplayName,
                    DeviceName = c.DeviceName,
                    RegDate = c.RegDate,
                }).FirstOrDefaultAsync();

            return device;
        }

        public async Task<List<DeviceViewModel>> GetDevicesByAccountIdAsync(Guid accountId)
        {
            var asd = await _dataContext.Credentials.Where(c => c.AccountId == accountId)
                .Select(c => new DeviceViewModel 
                { 
                    CredentialId = c.CredentialId,
                    Enabled = c.Enabled,
                    DeviceDescription = c.DeviceDescription,
                    DeviceDisplayName = c.DeviceDisplayName,
                    DeviceName = c.DeviceName,
                    RegDate = c.RegDate,
                }).ToListAsync();
            return asd;
            //List<DeviceViewModel> deviceViewModels = new List<DeviceViewModel>();
        }

        public async Task<bool> UpdateDeviceAsync(DeviceViewModel device, Guid accountId)
        {
            try
            {
                var credential = await _dataContext.Credentials.FirstOrDefaultAsync(c => c.CredentialId == device.CredentialId);
                if (credential == null)
                    return false;
            
                credential.DeviceName = device.DeviceName;
                credential.DeviceDisplayName = device.DeviceDisplayName;
                credential.Enabled = device.Enabled;

                _dataContext.Update(credential);
                await _dataContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return false;
            }

            return true;
        }
    }
}
