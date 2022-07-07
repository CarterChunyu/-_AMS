using Business;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;

namespace AMS.Controllers
{
    public class BaseController : Controller
    {
        protected AmRoles amRoles { get; set; }
        protected AmChoiceManager amChoiceManager { get; set; }
        protected AmUsersManager amUsersManager { get; set; }
        protected WebPasswordLogManager webPasswordLogManager { get; set; }
        protected string systemId = "AM_USERS";
        public static int PasswordInterval
        {
            get
            {
                string passwordInterval = System.Configuration.ConfigurationManager.AppSettings["PasswordInterval"];
                int i = int.Parse(passwordInterval);
                return i;
            }
        }

        public BaseController()
        {
            amChoiceManager = new AmChoiceManager();
            amUsersManager = new AmUsersManager();
            webPasswordLogManager = new WebPasswordLogManager();
        }

        // 初始化DropDownList      
        List<SelectListItem> GetSelectItem(bool dvalue = true)
        {
            List<SelectListItem> items = new List<SelectListItem>();
            if (dvalue) { items.Insert(0, new SelectListItem { Text = "--請選擇--", Value = "" }); }
            return items;
        }

        // 第一層下拉選項       
        protected List<SelectListItem> SetDropDown(bool dvalue, string choiceCode, string defaultValue = "")
        {
            List<SelectListItem> items = GetSelectItem(dvalue);
            items.AddRange(new SelectList(this.amChoiceManager.FindByCode(choiceCode), "Value", "Name", defaultValue));
            return items;
        }

        protected string SetTimeFormat(string strTime)
        {
            strTime = strTime.Substring(0, 4) + "-" + strTime.Substring(4, 2) + "-" + strTime.Substring(6, 2) + " " + strTime.Substring(8, 2) + ":" + strTime.Substring(10, 2) + ":" + strTime.Substring(12, 2) + ":";
            return strTime;
        }

        protected string SetDateFormat(string strTime)
        {
            strTime = strTime.Substring(0, 4) + "-" + strTime.Substring(4, 2) + "-" + strTime.Substring(6, 2);
            return strTime;
        }
        
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string controllerName = filterContext.RouteData.Values["controller"].ToString();
            if (controllerName != "Home")
            {
                WebPasswordLog obj = this.webPasswordLogManager.FindByPk(systemId, User.Identity.Name);

                if (obj.Status == WebPasswordLog.PWStatusType.Lock)
                {
                    filterContext.Result = new RedirectResult("~/Login/NotifyAdmin");
                }
                else if (obj.Status != WebPasswordLog.PWStatusType.Normal)
                {
                    filterContext.Result = new RedirectResult("~/Home/ChangePassword");
                } 
            }
            base.OnActionExecuting(filterContext);
        }

        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);
            IList<AmMenu> mmList = null;
            
            if (requestContext.HttpContext.Request.IsAuthenticated)
            {
                FormsIdentity id = (FormsIdentity)requestContext.HttpContext.User.Identity;
                FormsAuthenticationTicket ticket = id.Ticket;

                AmRolesManager arm = new AmRolesManager();
                amRoles = arm.FindByPk(ticket.UserData);
                AmMenuManager mm = new AmMenuManager();
                mmList = mm.GetMenu(ticket.UserData, "ONLINE", 0);

                ViewBag.UserRole = ticket.UserData;
            }
            ViewBag.MenuList = mmList;
            ViewBag.Version = System.Web.Configuration.WebConfigurationManager.AppSettings["webpages:Version"];
            
        }
    }
}