using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Application.Homeworks.Response;

public class FileUploadDto
{
    [FromForm]
    public IFormFile File { get; set; }
}
