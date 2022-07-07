using AMS.Models;
using Business;
using Domain.Entities;
using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Net.Mail;
using Common.Logging;

namespace AMS.Controllers
{
    public class LoginController : Controller
    {
        #region Field
        private static readonly ILog log = LogManager.GetLogger(typeof(LoginController));
        private string systemId = "AM_USERS";
        #endregion

        #region Property
        public AmUsersManager amUsersManager { get; set; }
        public WebPasswordLogManager webPasswordLogManager { get; set; }
        public static int PasswordInterval
        {
            get
            {
                string passwordInterval = System.Configuration.ConfigurationManager.AppSettings["PasswordInterval"];
                int i = int.Parse(passwordInterval);
                return i;
            }
        }
        #endregion

        #region Ctor
        public LoginController()
        {
            amUsersManager = new AmUsersManager();
            webPasswordLogManager = new WebPasswordLogManager();
        } 
        #endregion

        #region ActionResult
        public ActionResult Login()
        {
            ViewBag.Version = System.Web.Configuration.WebConfigurationManager.AppSettings["webpages:Version"];

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginUserModel tempUser, string returnUrl)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    AmUsers user = this.amUsersManager.FindByAuth(tempUser.Username, tempUser.Password);
                    WebPasswordLog obj = this.webPasswordLogManager.FindByPk(systemId, tempUser.Username);
                    if (user != null)
                    {
                        var now = DateTime.Now;
                        var ticket = new FormsAuthenticationTicket(
                            version: 1,
                            name: user.Username,
                            issueDate: now,
                            expiration: now.AddMinutes(30),
                            isPersistent: tempUser.RememberMe,
                            userData: user.Role,
                            cookiePath: FormsAuthentication.FormsCookiePath);

                        var encryptedTicket = FormsAuthentication.Encrypt(ticket);
                        var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
                        Response.Cookies.Add(cookie);

                        if (obj == null)
                        {
                            this.webPasswordLogManager.Insert(systemId, user.Username);
                            this.webPasswordLogManager.UpdateStatus(systemId, tempUser.Username, WebPasswordLog.PWStatusType.First);   //避免使用者進入到修改密碼後，點選別的功能
                            return RedirectToAction("ChangePassword", "Home", new { loginType = WebPasswordLog.PWStatusType.First }); //首次登入
                        }

                        switch (obj.Status)
                        {
                            case WebPasswordLog.PWStatusType.First:
                                return RedirectToAction("ChangePassword", "Home", new { loginType = WebPasswordLog.PWStatusType.First }); //此情況會發生在進入首次登入後進入密碼變更頁，但是沒有變更密碼又登出
                            case WebPasswordLog.PWStatusType.Normal:
                                if (obj.UpdateTime.AddMonths(PasswordInterval) < DateTime.Today) //檢查密碼是否過期
                                {
                                    this.webPasswordLogManager.UpdateStatus(systemId, tempUser.Username, WebPasswordLog.PWStatusType.Expired); //到期鎖定
                                    return RedirectToAction("ChangePassword", "Home", new { loginType = WebPasswordLog.PWStatusType.Expired });
                                }
                                else
                                {
                                    this.webPasswordLogManager.ClearErrorCount(systemId, user.Username);
                                }
                                break;
                            case WebPasswordLog.PWStatusType.Lock:
                                return RedirectToAction("NotifyAdmin", "Login");
                            case WebPasswordLog.PWStatusType.ReSend:
                                return RedirectToAction("ChangePassword", "Home", new { loginType = WebPasswordLog.PWStatusType.ReSend });
                            case WebPasswordLog.PWStatusType.Expired:
                                return RedirectToAction("ChangePassword", "Home", new { loginType = WebPasswordLog.PWStatusType.Expired });
                            default:
                                break;
                        }



                        this.amUsersManager.UpdateRegDate(user.Username, DateTime.Now);

                        if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                        && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                        {
                            return Redirect(returnUrl);
                        }
                        else
                        {
                            return RedirectToAction("Index", "Home");
                        }
                    }
                    else
                    {
                        if (obj == null)
                        {
                            this.webPasswordLogManager.Insert(systemId, tempUser.Username);
                            this.webPasswordLogManager.AddErrorCount(systemId, tempUser.Username);
                        }
                        else if (obj.ErrorCount + 1 >= 3)
                        {
                            this.webPasswordLogManager.AddErrorCount(systemId, tempUser.Username);
                            this.webPasswordLogManager.UpdateStatus(systemId, tempUser.Username, WebPasswordLog.PWStatusType.Lock);
                            return RedirectToAction("NotifyAdmin", "Login");
                        }
                        else
                        {
                            this.webPasswordLogManager.AddErrorCount(systemId, tempUser.Username);
                        }

                        ModelState.AddModelError("", "登入認證錯誤");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "登入失敗");
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                log.Error(ex.StackTrace);
            }
            return View();
        }

        public ActionResult ForgetPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ForgetPassword(string id, string email)
        {
            try
            {
                AmUsers user = this.amUsersManager.FindByAuth(id);

                if (user == null)
                {
                    TempData["message"] = "請確認使用者帳號正確";
                    return View();
                }

                if (user.Email != email)
                {
                    TempData["message"] = "信箱錯誤，請重新輸入";
                    return View();
                }

                string newPassWord = Helpers.RandomPassword.GetRandomPassword(8);
                string message = string.Format("您的新密碼為：{0}", newPassWord);

                SendMail(email, email, "帳務系統補發通知", message);

                //變更密碼
                this.amUsersManager.UpdatePassword(user.Username, newPassWord, user.Username);

                WebPasswordLog obj = this.webPasswordLogManager.FindByPk(systemId, user.Username);
                if (obj == null)
                {
                    this.webPasswordLogManager.Insert(systemId, user.Username);
                }

                this.webPasswordLogManager.UpdateStatus(systemId, user.Username, WebPasswordLog.PWStatusType.ReSend);
                this.webPasswordLogManager.ClearErrorCount(systemId, user.Username);

                string url = @Url.Content("~/Login/Login");
                return Content("<script>alert('密碼已寄至您的信箱');window.location.href = '" + url + "';</script>", "text/html");
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                log.Error(ex.StackTrace);
            }
            return View();
        }

        public ActionResult NotifyAdmin()
        {
            return View();
        }

        public ActionResult Logout()
        {
            //Session["SystemUsers"] = null;
            FormsAuthentication.SignOut();
            //清除所有的 session
            Session.RemoveAll();

            //建立一個同名的 Cookie 來覆蓋原本的 Cookie
            HttpCookie cookie1 = new HttpCookie(FormsAuthentication.FormsCookieName, "");
            cookie1.Expires = DateTime.Now.AddYears(-1);
            Response.Cookies.Add(cookie1);

            //建立 ASP.NET 的 Session Cookie 同樣是為了覆蓋
            HttpCookie cookie2 = new HttpCookie("ASP.NET_SessionId", "");
            cookie2.Expires = DateTime.Now.AddYears(-1);
            Response.Cookies.Add(cookie2);
            return RedirectToAction("Login", "Login");
        } 
        #endregion

        #region Private Method

        /// <summary>寄信
        /// </summary>
        /// <param name="sendMail">寄件者</param>
        /// <param name="receMail">收件者</param>
        /// <param name="subject">主題</param>
        /// <param name="message">訊息</param>
        private void SendMail(string sendMail, string receMail, string subject, string message)
        {
            try
            {
                using (SmtpClient client = new SmtpClient("MAIL"))
                {
                    using (MailMessage mailMessage = new MailMessage(sendMail, receMail))
                    {
                        mailMessage.Subject = subject;
                        mailMessage.Body = message;
                        client.UseDefaultCredentials = false;
                        mailMessage.BodyEncoding = System.Text.Encoding.UTF8;
                        client.Send(mailMessage);
                    }
                }
            }
            catch (SmtpException ex)
            {
                log.Error(ex.Message);
                log.Error(ex.StackTrace);
            }
        } 
        #endregion
    }
}