using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Attributes;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using WebApplication.Database;
using WebApplication.Models;

namespace WebApplication.Controllers
{
    [ODataModel]
    public class ParentsController : ODataController
    {
        private readonly MyDbContext _context;

        public ParentsController(MyDbContext context)
        {
            _context = context;
        }

        [EnableQuery(PageSize = 10)]
        // [EnableQuery]
        [HttpGet]
        public IQueryable<Parent> Get(ODataQueryOptions<Parent> queryOptions)
        {
            return _context.Parents.AsQueryable();
        }
    }
}