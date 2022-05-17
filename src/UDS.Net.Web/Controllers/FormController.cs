using System;
using Microsoft.AspNetCore.Mvc;
using UDS.Net.Data;
using UDS.Net.Data.Enums;
using UDS.Net.Web.Services;

namespace UDS.Net.Web.Controllers
{
    public class FormController : Controller
    {
        protected readonly UdsContext _context;
        protected readonly IParticipantsService _participantsService;

        public FormController(UdsContext context, IParticipantsService participantsService)
        {
            _context = context;
            _participantsService = participantsService;
        }

        protected bool FormCanBeEdited(VisitStatus status)
        {
            if (status == VisitStatus.Complete)
            {
                if (User.IsInRole("Admin"))
                    return true;
                else
                    return false;
            }
            return true;
        }
    }
}
