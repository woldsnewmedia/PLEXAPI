using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PLEXAPI.Data;
using PLEXAPI.Models;

// NON REPO VERSION FOR NOW .....

namespace PLEXAPI.Controllers
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class TestingController : ControllerBase
    {

        private readonly ILogger<TestingController> _logger;

        private readonly APIDbContext _context;

        public TestingController(ILogger<TestingController> logger, APIDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        /*
         * Testing function
         */ 
        [HttpGet]
        [Route("/Testing")]
        public IEnumerable<TestingDataVM> Testing1()
        {

            var a = new[]
            {
                "Stork, woolly-necked","Lizard, blue-tongued","Fox, silver-backed","Agouti","European badger","Skimmer, four-spotted","North American river otter","Nyala","Ornate rock dragon","Bushbaby, large-eared"
            };
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new TestingDataVM
            {
                Date = DateTime.Now.AddDays(index),
                Age = rng.Next(15, 35),                
                Animal = a[rng.Next(a.Length)] 
            })
            .ToArray();
        }
    }

}
