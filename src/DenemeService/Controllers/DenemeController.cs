using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace DenemeService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DenemeController : ControllerBase
    {
        List<string> _list = new List<string>{ "A", "B", "C",};
        
        [HttpPost]
        public async Task<ActionResult> CreateDene([FromForm]string deneme)
        {
            _list.Add(deneme);
            return Ok(_list);
        }   
    }
}