using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using SSO.IdentityServer.Models;

namespace SSO.IdentityServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DocumentController : Controller
    {
        private readonly IDocumentService _documentService;

        public DocumentController(IDocumentService documentService)
        {
            _documentService = documentService;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadDocument([FromForm] UploadDocumentDto dto)
        {
            if (dto == null)
                return BadRequest("DTO je null");

            if (dto.Data == null || dto.Data.Length == 0)
                return BadRequest("File je null ili prazan");

            if (string.IsNullOrEmpty(dto.UserId))
                return BadRequest("UserId nije poslan");

            try
            {
                await _documentService.UploadDocumentAsync(dto);
                return Ok(new { message = "Document uploaded successfully." });
            }
            catch (Exception ex)
            {
                // dodatno logiraj grešku
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
