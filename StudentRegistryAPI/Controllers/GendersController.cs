using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentRegistry.Models;
using StudentRegistryAPI.ViewModels;

namespace StudentRegistryAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GendersController : ControllerBase
    {

        private IUnitOfWork _context;

        public GendersController(IUnitOfWork context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GenderVM>>> GetStudents()
        {

            return await Task.FromResult(
                _context.GenderRepository.Get().
                Select(g => new GenderVM
                {
                    id = g.ID,
                    name = g.GenderName
                }).ToList());
        }
    }
}