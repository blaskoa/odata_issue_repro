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
    public class ChildrenController : ODataController
    {
        private readonly MyDbContext _context;

        public ChildrenController(MyDbContext context)
        {
            _context = context;
        }

        [EnableQuery(PageSize = 10)]
        // [EnableQuery]
        [HttpGet]
        public IQueryable<Child> Get(ODataQueryOptions<Child> queryOptions)
        {
            return _context.Children.AsQueryable();
        }
    }
}