using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Repositories.Dtos.Models;

public class DigitalSignature
{
    public IFormFile CertificateFile { get; set; }
    public IFormFile? SignatureImageFile { get; set; }
    public string Reason { get; set; } = "";
    public string Location { get; set; } = "";
    public string Password { get; set; } = "";
}
