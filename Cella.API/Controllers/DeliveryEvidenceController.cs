using Cella.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Cella.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeliveryEvidenceController : ControllerBase
    {
        [HttpPost("upload-signature")]
        public async Task<IActionResult> UploadSignature([FromForm] UploadSignatureDto dto)
        {
            // Save the signature file to storage and associate with the order
            // For demo, just return success
            return Ok();
        }

        [HttpPost("upload-photo")]
        public async Task<IActionResult> UploadPhoto([FromForm] UploadPhotoDto dto)
        {
            // Save the photo file to storage and associate with the order
            // For demo, just return success
            return Ok();
        }
    }
}
