﻿using ClashOfTheCharacters.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ClashOfTheCharacters.Controllers
{
    public class LadderController : Controller
    {
            ApplicationDbContext context = new ApplicationDbContext();
        // GET: Ladder
        public ActionResult Index()
        {

            var listWithUsers = context.Users.OrderBy(x=> x.Rank);
            return View(listWithUsers.ToList());
        }
        //[HttpPost]
        //public ActionResult SortByName()
        //{
        //    var userList = context.Users.ToList();
        //    var orderedByName = userList.OrderBy(x => x.UserName);
        //    return View(orderedByName.ToList());
        //}
        [HttpPost]
        public ActionResult SearchForUser()
        {
            var userList = context.Users.ToList();
            List<ApplicationUser> searchList = new List<ApplicationUser>();
            string userSearch = Request["searchField"];

            foreach (var user in context.Users.ToList())
            {
                if (user.UserName.ToLower().Contains(userSearch.ToLower()))
                {
                    searchList.Add(user);
                }
            }
            return View(searchList);
        }
    }
}