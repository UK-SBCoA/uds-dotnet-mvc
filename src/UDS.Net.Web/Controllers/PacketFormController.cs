using System;
using Microsoft.AspNetCore.Mvc;
using UDS.Net.Data;
using UDS.Net.Data.Enums;
using UDS.Net.Web.Services;

namespace UDS.Net.Web.Controllers
{
    /// <summary>
    /// For forms that make up a packet that is ultimately submitted to NACC (multi-site data coordinator) use this
    /// controller as a base so that the form has access to the Z1 checklist.
    /// </summary>
    public class PacketFormController : FormController
    {
        protected readonly IChecklistService _checklistService;

        public PacketFormController(UdsContext context, IParticipantsService participantsService, IChecklistService checklistService) : base(context, participantsService)
        {
            _checklistService = checklistService;
        }
    }
}
