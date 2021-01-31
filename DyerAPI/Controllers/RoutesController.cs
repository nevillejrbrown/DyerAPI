using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace DyerAPI.Controllers
{
    [Route("[controller]/[action]")]
    public class RoutesController : Controller
    {
        private readonly IActionDescriptorCollectionProvider _actionDescriptorCollectionProvider;

        public RoutesController(IActionDescriptorCollectionProvider actionDescriptorCollectionProvider)
        {
            this._actionDescriptorCollectionProvider = actionDescriptorCollectionProvider;
        }

        [HttpGet]
        [HttpPut]
        public ActionResult<IEnumerable<RouteInfo>> Index()
        {
            var routes = _actionDescriptorCollectionProvider.ActionDescriptors.Items.Select(x => new RouteInfo {
                Controller = x.RouteValues["Controller"],
                Action = x.RouteValues["Action"],
                Parameters = x.Parameters.Select(param => new Parameter
                {
                    Name = param.Name,
                    Type = param.ParameterType.Name,
                }),
                Template = x.AttributeRouteInfo?.Template,
                Name = x.AttributeRouteInfo?.Name,
                HttpMethods = string.Join(", ", x.ActionConstraints?.OfType<HttpMethodActionConstraint>().First().HttpMethods ?? new List<string>()),
            }).OrderBy(x => x.Controller).ToList();
            return routes;
        }
    }

    public class Parameter
    {
        public string Name { get; set; }
        public string Type { get; set; }
    }

    public class RouteInfo
    {
        public string Controller { get; set; }
        public string Action { get; set; }

        public IEnumerable<Parameter> Parameters{ get; set; }
        public string Template { get; set; }
        public string Name { get; set; }
        public string HttpMethods { get; set; }


    }
}
