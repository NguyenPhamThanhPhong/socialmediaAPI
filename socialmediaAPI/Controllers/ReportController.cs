using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using socialmediaAPI.Models.Entities;
using socialmediaAPI.Repositories.Interface;

namespace socialmediaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly IReportRepository _reportRepository;

        public ReportController(IReportRepository reportRepository)
        {
            _reportRepository = reportRepository;
        }
        [HttpPost("/report-create")]
        public async Task<IActionResult> Create([FromBody] Report report)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            await _reportRepository.Create(report);
            return Ok(report);
        }
        [HttpPost("/report-get-from-ids")]
        public async Task<IActionResult> GetFromIds([FromBody] List<string> ids)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            var reports = await _reportRepository.GetFromIds(ids);
            return Ok(reports);
        }
        [HttpGet("/report-get-by-id/{id}")]
        public async Task<IActionResult> GetbyId(string id)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            var report = await _reportRepository.GetbyId(id);
            return Ok(report);
        }
        [HttpPost("/report-get-all")]
        public async Task<IActionResult> GetAll()
        {
            if (!ModelState.IsValid)
                return BadRequest();
            var reports = await _reportRepository.GetAll();
            return Ok(reports);
        }
        [HttpDelete("/report-delete/{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            await _reportRepository.Delete(id);
            return Ok();
        }

    }
}
