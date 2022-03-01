using System;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Identity.Web;
using UDS.Net.Data.Dtos;
using Newtonsoft.Json;

namespace UDS.Net.Web.Services
{
    public static class ParticipantsServiceExtensions
    {
        public static void AddParticipantsService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpClient<IParticipantsService, ParticipantsService>();
        }
    }

    public class ParticipantsService : IParticipantsService
    {
        private readonly IHttpContextAccessor _contextAccessor;

        private readonly HttpClient _httpClient;

        private readonly string _ParticipantsScope = string.Empty;

        private readonly string _ParticipantsBaseAddress = string.Empty;

        private readonly ITokenAcquisition _tokenAcquisition;

        /// <summary>
        /// Example of implementation of this method.
        /// </summary>
        /// <param name="participationId">The PT ID or ADC ID</param>
        /// <returns>PII for participant to confirm identity</returns>
        //public async Task<ParticipantDto> GetParticipantAsync(int participationId)
        //{
        //    await PrepareAuthenticatedClient();

        //    var response = await _httpClient.GetAsync($"{_ParticipantsBaseAddress}/Participant/GetByStudyIdentity?study=ADC&studyIdentity={participationId}");
        //    if (response.StatusCode == System.Net.HttpStatusCode.OK)
        //    {
        //        if (response.Content != null)
        //        {
        //            var content = await response.Content.ReadAsStringAsync();
        //            if (content != null && !content.Equals("[]"))
        //            {
        //                ParticipantDto participant = JsonConvert.DeserializeObject<ParticipantDto>(content);

        //                return participant;
        //            }
        //        }
        //        return null;
        //    } else if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
        //    {
        //        return null;
        //    }

        //    throw new HttpRequestException($"Invalid status code in the HttpResponseMessage: {response.StatusCode}.");
        //}

        public async Task<ParticipantDto> GetParticipantAsync(int participationId)
        {
            return new ParticipantDto
            {
                Id = participationId,
                FirstName = "Janice",
                LastName = "Doe",
                DateOfBirth = DateTime.Now.AddYears(-88)
            };
        }

        private async Task PrepareAuthenticatedClient()
        {
            var accessToken = await _tokenAcquisition.GetAccessTokenForUserAsync(new[] { _ParticipantsScope });
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public ParticipantsService(ITokenAcquisition tokenAcquisition, HttpClient httpClient, IConfiguration configuration, IHttpContextAccessor contextAccessor)
        {
            _httpClient = httpClient;
            _tokenAcquisition = tokenAcquisition;
            _contextAccessor = contextAccessor;
            _ParticipantsScope = configuration["DownstreamApis:ParticipantsApi:Scope"];
            _ParticipantsBaseAddress = configuration["DownstreamApis:ParticipantsApi:Url"];
        }
    }
}
