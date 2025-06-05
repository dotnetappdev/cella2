using Microsoft.AspNetCore.Http;
using System;

namespace Cella.Models
{
    /// <summary>
    /// DTO for uploading a signature file associated with an order.
    /// </summary>
    public class UploadSignatureDto
    {
        /// <summary>
        /// Gets or sets the ID of the order.
        /// </summary>
        public int OrderId { get; set; }

        /// <summary>
        /// Gets or sets the signature file.
        /// </summary>
        public IFormFile Signature { get; set; }
    }

    /// <summary>
    /// DTO for uploading a photo file associated with an order.
    /// </summary>
    public class UploadPhotoDto
    {
        /// <summary>
        /// Gets or sets the ID of the order.
        /// </summary>
        public int OrderId { get; set; }

        /// <summary>
        /// Gets or sets the photo file.
        /// </summary>
        public IFormFile Photo { get; set; }
    }
}
