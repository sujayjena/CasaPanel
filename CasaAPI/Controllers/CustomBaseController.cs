using Microsoft.AspNetCore.Mvc;
using CasaAPI.CustomAttributes;

namespace CasaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [CustomAuthorize]
    public class CustomBaseController : ControllerBase
    {
    }
}
