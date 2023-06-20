using Microsoft.AspNetCore.Mvc;
using NipApi.Model;

namespace NipApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NipController : ControllerBase
    {

        private readonly ILogger<NipController> _logger;
        private readonly IWLApi _wlapi;
        private readonly INipRepository _nipRepository;

        public NipController(ILogger<NipController> logger, IWLApi wlapi, INipRepository nipRepository)
        {
            _logger = logger;
            _wlapi = wlapi;
            _nipRepository = nipRepository;
        }

        [HttpGet("{nip}")]
        public async Task<NipDetails> Get(long nip)
        {
            var nipDetails = await _wlapi.GetNipDetailsAsync(nip);

            if (nipDetails.Nip == nip && !await _nipRepository.NipExistsAsync(nip))
            {
                await _nipRepository.AddAsync(nipDetails);
            }
            return nipDetails;
        }
    }
}