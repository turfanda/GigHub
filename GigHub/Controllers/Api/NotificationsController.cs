using GigHub.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using AutoMapper;
using System.Web.Http;
using GigHub.Dtos;
using Microsoft.AspNet.Identity;

namespace GigHub.Controllers.Api
{
    [Authorize]
    public class NotificationsController : ApiController
    {
        private readonly ApplicationDbContext _contex;

        public NotificationsController()
        {
            _contex = new ApplicationDbContext();
        }
        public IEnumerable<NotificationDto> GetNewNotifications()
        {
            string userId =User.Identity.GetUserId();
            var notifications = _contex.UserNotifications
                .Where(a => a.UserId == userId && !a.IsRead)
                .Select(a=>a.Notification)
                .Include(a=>a.Gig.Artist)
                .ToList();

            //Mapper.Initialize(c =>
            //{
            //    c.CreateMap<ApplicationUser, UserDto>();
            //    c.CreateMap<Gig, GigDto>();
            //    c.CreateMap<Notification, NotificationDto>();
            //});

            //return notifications.Select(Mapper.Map<Notification, NotificationDto>);

            return notifications.Select(c => new NotificationDto
            {
                DateTime = c.DateTime,
                OriginalDateTime = c.OriginalDateTime,
                OriginalVenue = c.OriginalVenue,
                Type = c.Type,
                Gig = new GigDto
                {
                    IsCanceled = c.Gig.IsCanceled,
                    DateTime = c.Gig.DateTime,
                    Id = c.Gig.Id,
                    Venue = c.Gig.Venue,
                    Genre = c.Gig.Genre,
                    Artist = new UserDto
                    {
                        Id = c.Gig.Artist.Id,
                        Name = c.Gig.Artist.Name
                    }
                }

            });
        }

        [HttpPost]
        public IHttpActionResult Read()
        {
            string userId = User.Identity.GetUserId();
            var notifications = _contex.UserNotifications
                .Where(a => a.UserId == userId && !a.IsRead);

            if (notifications.Any())
            {
                foreach (var notification in notifications)
                {
                    notification.Read();
                }

                _contex.SaveChanges();
                return Ok();
            }
            else
            {
                return NotFound();
            }            
        }
    }
}
