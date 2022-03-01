using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UDS.Net.Data;
using UDS.Net.Web.Services;
using UDS.Net.Web.ViewModels;

namespace UDS.Net.Web.Views.Shared.Components.RecentParticipationSearch
{
    public class RecentParticipationSearchViewComponent : ViewComponent
    {
        private readonly IUserPreferencesService _userPreferencesService;
        public RecentParticipationSearchViewComponent(IUserPreferencesService userPreferencesService)
        {
            _userPreferencesService = userPreferencesService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var history = await _userPreferencesService.GetParticipationSearchHistoryByUsernameAsync(User.Identity.Name);
            if (history == null)
            {
                return View();
            }
            return View(new RecentSearches
            {
                Searches = history
            });

        }
    }
}

