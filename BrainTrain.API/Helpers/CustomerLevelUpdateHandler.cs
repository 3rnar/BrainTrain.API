using BrainTrain.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BrainTrain.API.Helpers
{
    public class CustomerLevelUpdateHandler
    {
        BrainTrainContext db;

        public CustomerLevelUpdateHandler(BrainTrainContext _db)
        {
            db = _db;
        }

        public void UpdateLevel(double xp, string userId)
        {
            var userRating = db.UserRatings.FirstOrDefault(r => r.UserId == userId);
            var user = db.ApplicationUsers.FirstOrDefault(u => u.Id == userId);
            if (userRating == null)
            {
                db.UserRatings.Add(new UserRatings { Rating = xp, UserId = userId });
            }
            else
            {
                userRating.Rating += xp;
            }

            var level = db.Levels.FirstOrDefault(l => userRating.Rating >= l.FromRating && userRating.Rating <= l.ToRating);

            if (user.LevelId != level.Id)
            {
                user.LevelId = level.Id;

                db.Events.Add(new Event
                {
                    DateCreated = DateTime.Now,
                    UserId = userId,
                    TypeId = 1,
                    Url = "",
                    Text = $"Поздравляем! Ты достиг нового уровня {level.Text}"
                });
            }

            db.SaveChanges();
        }
    }
}
