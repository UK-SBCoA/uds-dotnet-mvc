using System;
using System.Threading.Tasks;
using UDS.Net.Data.Dtos;

namespace UDS.Net.Web.Services
{
    public interface IParticipantsService
    {
        Task<ParticipantDto> GetParticipantAsync(int participantId);
    }
}
