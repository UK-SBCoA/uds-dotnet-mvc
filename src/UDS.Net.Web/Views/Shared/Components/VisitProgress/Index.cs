using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UDS.Net.Data;
using UDS.Net.Data.Enums;
using UDS.Net.Web.Services;

namespace COA.UDS.Web.Views.Shared.Components.VisitProgress
{
    public class VisitProgressViewComponent : ViewComponent
    {
        private readonly IVisitService _service;

        public VisitProgressViewComponent(UdsContext context, IVisitService service)
        {
            _service = service;
        }

        public async Task<IViewComponentResult> InvokeAsync(int id, VisitType visitType, FormStatus formStatus)
        {
            var progress = await _service.GetVisitFormsInStatusAsync(id, visitType, formStatus);

            return View("Default", progress + " " + formStatus.ToString().ToLower());
        }



    }
}
