using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace GigHub.Models
{
    public class Notification
    {
        public int Id { get; set; }
        public NotificationType Type { get; private set; }
        public DateTime DateTime { get; private set; }
        public DateTime? OriginalDateTime { get; set; }
        public string OriginalVenue { get; set; }

        [Required]
        public Gig Gig { get; private set; }

        public Notification()
        {
            
        }

        private Notification(Gig gig,NotificationType type)
        {
            Gig = gig ?? throw new ArgumentNullException(nameof(gig));
            Type = type;
            DateTime = DateTime.Now;
        }

        public static Notification GigCreated(Gig gig)
        {
            return new Notification(gig,NotificationType.GigCreateNotification);
        }

        public static Notification GigCanceled(Gig gig)
        {
            return new Notification(gig,NotificationType.GigCancelNotification);
        }

        public static Notification GigUpdated(Gig newGig, DateTime orginalDateTime, String originalVenue)
        {
            return new Notification(newGig, NotificationType.GigUpdateNotification)
            {
                OriginalDateTime = orginalDateTime,
                OriginalVenue = originalVenue
            };

        }
    }
}