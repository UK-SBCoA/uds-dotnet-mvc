using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UDS.Net.Data;
using UDS.Net.Data.Entities;

namespace COA.UDS.Web.Views.Shared.Components.FormHeader
{
    public class FormHeaderViewComponent : ViewComponent
    {
        private readonly UdsContext _context;

        public FormHeaderViewComponent(UdsContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(Visit currentVisit, FormBase currentForm, string formTitle)
        {
            ///TODO use VisitService to get previous and next form
            ViewData["Title"] = formTitle;
            return View(currentVisit);
        }
    }
}
