using System;
using UDS.Net.Data.Dtos;

namespace UDS.Net.Web.ViewModels
{
    public class VisitParticipant
    {
        public string ParticipantPortalBaseUrl { get; set; }

        public int ParticipationId { get; set; }

        public ParticipantDto ParticipantProfile { get; set; }

        public int CurrentVisitId { get; set; }

        public string ParticipantPortalProfileUrl
        {
            get
            {
                if (ParticipantProfile != null)
                {
                    if (ParticipantPortalBaseUrl != null)
                    {
                        return ParticipantPortalBaseUrl + ParticipantProfile.Id;
                    }
                }
                return null;
            }
        }

    }
}
