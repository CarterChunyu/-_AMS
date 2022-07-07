using DataAccess;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Business
{
    public class WebPasswordLogManager
    {
        public WebPasswordLogDao webPasswordLogDao { get; set; }
    
        public WebPasswordLogManager()
        {
            webPasswordLogDao = new WebPasswordLogDao();
            
        }

        public WebPasswordLog FindByPk(string systemId, string userName)
        {
            try
            {
                WebPasswordLog obj = webPasswordLogDao.FindByPk(systemId, userName);
                return obj;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        public void ClearErrorCount(string systemId, string userName)
        {
            try
            {
                webPasswordLogDao.ClearErrorCount(systemId, userName);
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        public void AddErrorCount(string systemId, string userName)
        {
            try
            {
                webPasswordLogDao.AddErrorCount(systemId, userName);
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateStatus(string systemId, string userName, WebPasswordLog.PWStatusType status)
        {
            try
            {
                webPasswordLogDao.UpdateStatus(systemId, userName, status);
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        public void Insert(string systemId, string userName)
        {
            try
            {
                webPasswordLogDao.Insert(systemId, userName);
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateDate(string systemId, string userName, DateTime dateTime)
        {
            try
            {
                webPasswordLogDao.UpdateDate(systemId, userName, dateTime);
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
    }
}
