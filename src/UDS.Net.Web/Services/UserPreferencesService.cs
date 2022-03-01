using System;
using System.Collections.Generic;
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
    public class UserPreferencesService : IUserPreferencesService
    {
        private readonly UserContext _userContext;
        public UserPreferencesService(UserContext userContext)
        {
            _userContext = userContext;
        }

        public async Task<UserPreference> AddAsync(string username, UserPreferenceOptions option)
        {
            UserPreference preference = new UserPreference
            {
                Username = username,
                Preference = option
            };
            await _userContext.UserPreferences.AddAsync(preference);
            await _userContext.SaveChangesAsync();
            return preference;
        }
        public async Task<UserPreference> UpdateAsync(UserPreference preference)
        {
            preference.UpdatedAt = DateTime.Now;
            _userContext.UserPreferences.Update(preference);
            await _userContext.SaveChangesAsync();
            return preference;
        }
        public async Task<UserPreference> GetByUserNameAndTypeAsync(string username, UserPreferenceOptions optionType)
        {
            UserPreference preference = null;
            if(username != null) {
                preference = await _userContext.UserPreferences.Where(x => x.Username == username && x.Preference == optionType).SingleOrDefaultAsync();
            } 
            return preference;
        }
        public async Task<int[]> AddToParticipationSearchHistoryAsync(int successfulSearch, string username)
        {
            var preference = await GetByUserNameAndTypeAsync(username, UserPreferenceOptions.ParticipationSearchHistory); // all searches are stored in an array in the value
            if (preference == null)
            {
                preference = await AddAsync(username, UserPreferenceOptions.ParticipationSearchHistory); // creates preference with an empty value
            }

            if (preference.Value == null) // if it's new
            {
                int[] searchArray = new int[] { successfulSearch };
                preference.Value = JsonConvert.SerializeObject(searchArray);
            }
            else // if it's not new
            {
                var searchCollection = JsonConvert.DeserializeObject<List<int>>(preference.Value);

                if(searchCollection.Contains(successfulSearch))
                {
                    searchCollection.Remove(successfulSearch);
                }

                searchCollection = searchCollection.Append(successfulSearch).ToList(); // put it in the end (the view reverses and displays properly)

                if (searchCollection.Count > 5)
                    searchCollection = searchCollection.Skip(1).ToList();

                preference.Value = JsonConvert.SerializeObject(searchCollection);
            }

            var updated = await UpdateAsync(preference);
            return JsonConvert.DeserializeObject<int[]>(updated.Value);

        }
        public async Task<int[]> GetParticipationSearchHistoryByUsernameAsync(string username)
        {
            var searchHistory = await _userContext.UserPreferences.Where(x => x.Username == username && x.Preference == UserPreferenceOptions.ParticipationSearchHistory).SingleOrDefaultAsync();
            int[] history;
            if (searchHistory != null)
            {
                history = JsonConvert.DeserializeObject<int[]>(searchHistory.Value);
            }
            else
            {
                history = new int[] { };
            }
            return history;
        }
    }
}

