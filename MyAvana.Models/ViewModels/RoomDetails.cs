using System;
using System.Collections.Generic;
using System.Text;

namespace MyAvana.Models.ViewModels
{
    public class RoomDetails
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public int ParticipantCount { get; set; }

        public int MaxParticipants { get; set; }
    }
}
