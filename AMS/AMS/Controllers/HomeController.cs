using AMS.Models;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Business;
using System.Web.Security;
using Common.Logging;

namespace AMS.Controllers
{
    [CustomAuthorize(AccessLevel = "All")]
    public class HomeController : BaseController
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(HomeController));

        public ActionResult Index()
        {
            return View();
        }

        public FileStreamResult CreateFile()
        {
            //todo: add some data from your database into that string:
            var string_with_your_data1 = "\"E\",\"20150108\",\"X150109001\",\"0011\",\"0001\",\"226801\",\"\",\"C\",\"848202\",\"\",\"\",\"NT$\",\"1\",\"\",\"0108卡務餘額異動\"\r\n";
            var string_with_your_data2 = "\"E\",\"20150108\",\"X150109001\",\"0011\",\"0002\",\"118802\",\"\",\"D\",\"17035102\",\"22555003\",\"\",\"NT$\",\"1\",\"711\",\"統一超商股份有限公司\"\r\n";
            var byteArray = Encoding.ASCII.GetBytes(string_with_your_data1 + string_with_your_data2);
            byte[] data = System.Text.Encoding.Default.GetBytes(string_with_your_data1 + string_with_your_data2);
            var stream = new MemoryStream(data);

            return File(stream, "text/plain", "your_file_name1.dat");
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult ChangePassword()
        {
            WebPasswordLog obj = this.webPasswordLogManager.FindByPk(systemId, User.Identity.Name);
            if (obj == null)
            {
                log.Error("ActionResut: ChangePassword-WebPasswordLog無資料");
                return HttpNotFound();
            }
            ViewBag.LoginType = obj.Status.ToString();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(PasswordModel pm)
        {
            try
            {
                #region CheckOldPassword
                AmUsers user = this.amUsersManager.FindByAuth(User.Identity.Name, pm.OldPassword);
                if (user == null)
                {
                    ModelState.AddModelError("", "原密碼輸入錯誤");
                    return View();
                }
                #endregion

                #region CheckPassowrdRepeat
                AmUsers p = amUsersManager.FindHistory(User.Identity.Name, pm.Password);
                if (p != null)
                {
                    ModelState.AddModelError("", "新密碼不得與前4次舊密碼重覆");
                    return View();
                }
                #endregion

                #region UpdatePassword
                this.amUsersManager.UpdatePassword(User.Identity.Name + "", pm.Password, User.Identity.Name + "");
                this.amUsersManager.UpdatePasswordLog(User.Identity.Name + "", pm.Password, User.Identity.Name + "");//更新密碼記錄
                #endregion

                #region UpdateWebPasswordLog
                this.webPasswordLogManager.UpdateStatus(systemId, User.Identity.Name, WebPasswordLog.PWStatusType.Normal);
                this.webPasswordLogManager.ClearErrorCount(systemId, User.Identity.Name);
                this.webPasswordLogManager.UpdateDate(systemId, User.Identity.Name, DateTime.Now);
                #endregion

                #region 登出
                Session["User"] = null;
                Session["MyMenu"] = null;
                FormsAuthentication.SignOut();
                #endregion

                #region 回傳結果
                string url = @Url.Content("~/Login/Login");
                return Content("<script>alert('密碼變更完成，謝謝您');window.location.href = '" + url + "';</script>", "text/html");
                #endregion
            }
            catch(Exception ex)
            {
                log.Error(ex.Message);
                log.Error(ex.StackTrace);
                return View();
            }
        }
    }
}