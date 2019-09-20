using System;
using System.Diagnostics;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Monitoring.Pn.Abstractions;
using Monitoring.Pn.Infrastructure.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microting.MonitoringBase.Infrastructure.Data;
using Microting.eFormApi.BasePn.Abstractions;
using Microting.eFormApi.BasePn.Infrastructure.Helpers.PluginDbOptions;
using Microting.eFormApi.BasePn.Infrastructure.Models.API;

namespace Monitoring.Pn.Services
{
    public class MonitoringPnSettingsService :IMonitoringPnSettingsService
    {
        private readonly ILogger<MonitoringPnSettingsService> _logger;
        private readonly IMonitoringLocalizationService _trashInspectionLocalizationService;
        private readonly MonitoringPnDbContext _dbContext;
        private readonly IEFormCoreService _coreHelper;
        private readonly IPluginDbOptions<MonitoringBaseSettings> _options;
        private readonly string _connectionString;
        private readonly IHttpContextAccessor _httpContextAccessor;
        
        public MonitoringPnSettingsService(ILogger<MonitoringPnSettingsService> logger,
            IMonitoringLocalizationService trashInspectionLocalizationService,
            MonitoringPnDbContext dbContext,
            IPluginDbOptions<MonitoringBaseSettings> options,
            IEFormCoreService coreHelper,
            IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _dbContext = dbContext;
            _coreHelper = coreHelper;
            _options = options;
            _httpContextAccessor = httpContextAccessor;
            _trashInspectionLocalizationService = trashInspectionLocalizationService;
        }
        
        public async Task<OperationDataResult<MonitoringBaseSettings>> GetSettings()
        {
            try
            {
                var option = _options.Value;

                if (option.SdkConnectionString == "...")
                {
                    var connectionString = _dbContext.Database.GetDbConnection().ConnectionString;

                    var dbNameSection = Regex.Match(connectionString, @"(Database=(...)_eform-angular-\w*-plugin;)").Groups[0].Value;
                    var dbPrefix = Regex.Match(connectionString, @"Database=(\d*)_").Groups[1].Value;
                    var sdk = $"Database={dbPrefix}_SDK;";
                    connectionString = connectionString.Replace(dbNameSection, sdk);
                    await _options.UpdateDb(settings => { settings.SdkConnectionString = connectionString;}, _dbContext, UserId);
                }

                return new OperationDataResult<MonitoringBaseSettings>(true, option);
            }
            catch(Exception e)
            {
                Trace.TraceError(e.Message);
                _logger.LogError(e.Message);
                return new OperationDataResult<MonitoringBaseSettings>(false,
                    _trashInspectionLocalizationService.GetString("ErrorWhileObtainingMonitoringSettings"));
            }
        }

        public async Task<OperationResult> UpdateSettings(MonitoringBaseSettings monitoringBaseSettings)
        {
            try
            {
                await _options.UpdateDb(settings =>
                {
                    settings.LogLevel = monitoringBaseSettings.LogLevel;
                    settings.LogLimit = monitoringBaseSettings.LogLimit;
                    settings.SdkConnectionString = monitoringBaseSettings.SdkConnectionString;
                }, _dbContext, UserId);

                return new OperationResult(true,
                    _trashInspectionLocalizationService.GetString("SettingsHaveBeenUpdatedSuccessfully"));
            }
            catch(Exception e)
            {
                Trace.TraceError(e.Message);
                _logger.LogError(e.Message);
                return new OperationResult(false, _trashInspectionLocalizationService.GetString("ErrorWhileUpdatingSettings"));
            }
        }
        
        public int UserId
        {
            get
            {
                var value = _httpContextAccessor?.HttpContext.User?.FindFirstValue(ClaimTypes.NameIdentifier);
                return value == null ? 0 : int.Parse(value);
            }
        }
    }
}