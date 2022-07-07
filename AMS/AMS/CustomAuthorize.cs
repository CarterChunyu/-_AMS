using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace AMS
{
    public class CustomAuthorize : AuthorizeAttribute
    {
        public string AccessLevel { get; set; }
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var authroized = base.AuthorizeCore(httpContext);
            if (!authroized)
            {
                // the user is not authenticated or the forms authentication
                // cookie has expired
                return false;
            }

            // Now check the session:
            //var myvar = httpContext.Session["SystemUsers"];
            //if (myvar == null)
            //{
            //    return false;
            //}
            
            //AmUsers us = (AmUsers)myvar;

            FormsIdentity id = (FormsIdentity)httpContext.User.Identity;
            FormsAuthenticationTicket ticket = id.Ticket;

            string privilegeLevels = ticket.UserData;

            string[] separators = { "," };
            string[] AccessLevelArray = this.AccessLevel.Split(separators, StringSplitOptions.RemoveEmptyEntries);

            if (privilegeLevels != "")
            {
                if (AccessLevelArray.Contains("All") || AccessLevelArray.Contains(privilegeLevels))
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            else
            {
                return false;
            }
        }
    }
}