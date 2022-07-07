using DataAccess;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AMS.Controllers
{
    [CustomAuthorize(AccessLevel = "System,Accounting,Sales,SalesManager,")]
    public class ICASHOPInvoiceFileController : BaseController
    {
        public ActionResult Index()
        {
            ViewBag.TARGET_MONTH = DateTime.Now.ToString("yyyyMM");
            ViewBag.DateTypeList = GetDateTypeList();
            return View();
        }

        [HttpPost]
        public ActionResult Index(FormCollection value)
        {
            ICASHOPInvoiceFileDAO icashopInvoiceFileDAO = new ICASHOPInvoiceFileDAO();
            DataTable dt = new DataTable();
            //ViewBag.MerchantList = GetMerchantList();
            ViewBag.DateTypeList = GetDateTypeList();
            string TARGET_MONTH = value["TARGET_MONTH"];
            //string MerchantName = value["MerchantName"];
            string TARGET_DateType = value["TARGET_DateType"];
            string Group_TaiYa = string.Empty;
            string Group_TaiSu = string.Empty;
            string Group_XiOu = string.Empty;
            string Group_FuMou = string.Empty;
            string Group_TongYiDuJiaCun = string.Empty;
            if(value["MerchantGroup"]!=null&& !string.IsNullOrWhiteSpace(value["MerchantGroup"].ToString()))
            {
                foreach(var item in value["MerchantGroup"].Split(','))
                {
                    switch (item)
                    {
                        case "TaiYa":
                            Group_TaiYa = "Y";
                            ViewBag.CH_TAIYA = "checked";
                            break;
                        case "TaiSu":
                            Group_TaiSu = "Y";
                            ViewBag.CH_TAISU = "checked";
                            break;
                        case "XiOu":
                            Group_XiOu = "Y";
                            ViewBag.CH_XIOU = "checked";
                            break;
                        case "FuMou":
                            Group_FuMou = "Y";
                            ViewBag.CH_FUMOU = "checked";
                            break;
                        case "TongYiDuJiaCun":
                            Group_TongYiDuJiaCun = "Y";
                            ViewBag.CH_TONGYIDUJIACUN = "checked";
                            break;
                    }
                }
            }
            
            if (!string.IsNullOrWhiteSpace(TARGET_MONTH) && !string.IsNullOrWhiteSpace(TARGET_DateType)
                && (!string.IsNullOrWhiteSpace(Group_TaiYa)||!string.IsNullOrWhiteSpace(Group_TaiSu)||!string.IsNullOrWhiteSpace(Group_XiOu)
                || !string.IsNullOrWhiteSpace(Group_FuMou)||!string.IsNullOrWhiteSpace(Group_TongYiDuJiaCun)))
            {
                ViewBag.TARGET_MONTH = TARGET_MONTH;
                //ViewBag.MerchantName = MerchantName;
                ViewBag.TARGET_DateType = TARGET_DateType;
                ViewBag.MerchantGroup = value["MerchantGroup"];

                #region search
                if (Request.Form["searchConfirm"] != null)
                {
                    ViewBag.InvoiceList = icashopInvoiceFileDAO.GetInvoiceData(TARGET_MONTH,TARGET_DateType,Group_TaiYa,Group_TaiSu,Group_XiOu,Group_FuMou,Group_TongYiDuJiaCun);
                }
                #endregion

                #region excel
                if (Request.Form["ExportExcel"] != null)
                {
                    dt = icashopInvoiceFileDAO.GetInvoiceData(TARGET_MONTH, TARGET_DateType, Group_TaiYa, Group_TaiSu, Group_XiOu, Group_FuMou, Group_TongYiDuJiaCun);
                    ViewBag.InvoiceList = dt;

                    if (dt.Rows.Count > 0)
                    {
                        ExcelPackage excel = new ExcelPackage();
                        ExcelWorksheet sheet1 = excel.Workbook.Worksheets.Add("發票檔資料");

                        int i = 0;
                        foreach (DataColumn col in dt.Columns)
                        {
                            sheet1.Cells[1, (i + 1)].Value = col.ColumnName.ToString();
                            sheet1.Cells[1, (i + 1)].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                            i++;
                        }

                        int irow = 1;
                        int icol = 0;
                        foreach (DataRow row in dt.Rows)
                        {
                            foreach (DataColumn col in dt.Columns)
                            {
                                sheet1.Cells[(irow + 1), (icol + 1)].Value = row[col.ColumnName].ToString();
                                sheet1.Cells[(irow + 1), (icol + 1)].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                                icol++;
                            }
                            irow++;
                            icol = 0;
                        }

                        sheet1.Cells[sheet1.Dimension.Address].AutoFitColumns();

                        var stream = new MemoryStream();
                        excel.SaveAs(stream);
                        stream.Close();

                        return File(stream.ToArray(), "application/vnd.ms-excel", string.Format("發票檔資料_{0}_", TARGET_MONTH) + ".xlsx");
                    }
                }
                #endregion

                #region dat
                if (Request.Form["ExportDat"] != null)
                {
                    dt = icashopInvoiceFileDAO.GetInvoiceData(TARGET_MONTH, TARGET_DateType, Group_TaiYa, Group_TaiSu, Group_XiOu, Group_FuMou, Group_TongYiDuJiaCun);
                    ViewBag.InvoiceList = dt;

                    if (dt.Rows.Count > 0)
                    {
                        MemoryStream ms = new MemoryStream();
                        StreamWriter sw = new StreamWriter(ms,System.Text.Encoding.GetEncoding("BIG5"));

                        foreach (DataRow row in dt.Rows)
                        {
                            List<string> list = new List<string>();
                            foreach (DataColumn col in dt.Columns)
                            {
                                list.Add('"' + row[col.ColumnName].ToString() + '"');
                            }
                            string str = string.Join(",", list.ToArray());
                            sw.WriteLine(str);

                        }
                        sw.Flush();
                        ms.Position = 0;
                        return File(ms.ToArray(), "application/octet-stream", string.Format("發票檔_{0}", TARGET_MONTH) + ".dat");
                    }
                }
                #endregion

            }
            else
            {
                ViewBag.TARGET_MONTH = DateTime.Now.ToString("yyyyMM");
            }
            return View();
        }

        public List<SelectListItem> GetMerchantList()
        {
            ICASHOPDAO icashopDAO = new ICASHOPDAO();
            DataTable dt = icashopDAO.GetGroupNameData();
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Text = "ALL", Value = "-" });
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow irow in dt.Rows)
                {
                    items.Add(new SelectListItem { Text = irow["GroupName"].ToString(), Value = irow["GroupName"].ToString() });
                }
            }
            return items;
        }

        public List<SelectListItem> GetDateTypeList()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Text = "機構日期", Value = "Merchant" });
            items.Add(new SelectListItem { Text = "OP日期", Value = "OP" });
            return items;
        }
    }
}