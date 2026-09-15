using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace IMS.API.Controllers
{
   
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AuthenticatedController : ControllerBase
    {

    }
}
