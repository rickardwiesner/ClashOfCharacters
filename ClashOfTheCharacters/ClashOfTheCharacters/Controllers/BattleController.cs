﻿using ClashOfTheCharacters.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ClashOfTheCharacters.Controllers
{
    [Authorize]
    public class BattleController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            var challenges = db.Challenges.Where(c => c.ChallengerId == userId || c.ReceiverId == userId && c.Accepted == false).ToList();
            var battles = db.Battles.Where(b => b.Challenge.ChallengerId == userId || b.Challenge.ReceiverId == userId /*&& DateTime.Now.Subtract(b.StartTime.TimeOfDay.Minutes) > 0*/).ToList();

            ViewBag.UserId = userId;
            ViewBag.Challenges = challenges;
            ViewBag.Battles = battles;

            return View(db.Users.ToList());
        }

        public ActionResult Challenge(string opponentId)
        {
            var userId = User.Identity.GetUserId();

            if(userId != opponentId)
            {
                var user = db.Users.Find(userId);

                if (user.Stamina >= 7)
                {
                    user.Stamina -= 7;

                    var challenge = new Challenge { ChallengerId = userId, ReceiverId = opponentId };

                    db.Challenges.Add(challenge);
                    db.SaveChanges();
                }
            }
            
            return RedirectToAction("Index");
        }

        public ActionResult Accept(int challengeId)
        {
            var userId = User.Identity.GetUserId();
            var battle = new Battle { ChallengeId = challengeId, StartTime = DateTime.Now.AddMinutes(2) };

            db.Challenges.Find(challengeId).Accepted = true;
            db.Battles.Add(battle);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        public ActionResult Cancel(int challengeId)
        {
            var userId = User.Identity.GetUserId();

            var challenge = db.Challenges.Find(challengeId);
            challenge.Challenger.Stamina += 7;

            db.Challenges.Remove(challenge);
            db.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}