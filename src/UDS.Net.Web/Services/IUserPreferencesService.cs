using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UDS.Net.Data;
using UDS.Net.Data.Entities;
using UDS.Net.Data.Enums;

namespace UDS.Net.Web.Services
{
    public interface IUserPreferencesService
    {
        Task<UserPreference> AddAsync(string username, UserPreferenceOptions option);
        Task<UserPreference> UpdateAsync(UserPreference preference);
        Task<UserPreference> GetByUserNameAndTypeAsync(string username, UserPreferenceOptions optionType);
        Task<int[]> AddToParticipationSearchHistoryAsync(int successfulSearch, string username);
        Task<int[]> GetParticipationSearchHistoryByUsernameAsync(string username);
    }
}