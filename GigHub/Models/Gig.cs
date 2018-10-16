using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace GigHub.Models
{
    public class Gig
    {
        public int Id { get; set; }

        public bool IsCanceled { get; private set; }

        public ApplicationUser Artist { get; set; }

        [Required]
        public string ArtistId { get; set; }

        public DateTime DateTime { get; set; }

        [Required]
        [StringLength(255)]
        public string Venue { get; set; }

        public Genre Genre { get; set; }

        [Required]
        public byte GenreId { get; set; }

        public ICollection<Attendance> Attendances { get; private set; }

        public Gig()
        {
            Attendances = new Collection<Attendance>();
        }

        public void Cancel()
        {
            IsCanceled = true;
            var notification = Notification.GigCanceled(this);

            foreach (var user in Attendances.Select(a => a.Attendee))
            {
                user.Notify(notification);
            }
        }

        public void Update(DateTime newDt,string newVenue, byte newGenreId)
        {
            var notification = Notification.GigUpdated(this,DateTime,Venue);

            DateTime = newDt;
            Venue = newVenue;
            GenreId = newGenreId;

            foreach (var user in Attendances.Select(a => a.Attendee))
            {
                user.Notify(notification);
            }
        }
    }
}