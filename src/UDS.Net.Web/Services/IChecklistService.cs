using System;
using System.Threading.Tasks;
using UDS.Net.Data.Entities;

namespace UDS.Net.Web.Services
{
    public interface IChecklistService
    {
        Task ValidateAndUpdateChecklistStatus(Visit visit, Type formType);

        string GetRequiredFormsDisplay(Visit visit);
    }
}
