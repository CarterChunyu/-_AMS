using AMS.Models;
using Business;
using CardValidator;
using Domain.Entities;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace AMS.Controllers
{
    [CustomAuthorize(AccessLevel = "System,Accounting,Sales,Customer,Marketing,PM,SalesManager")]
    public class AccountingController : BaseController
    {
        public AmChoiceManager choiceManager { get; set; }
        public AmEntReceDManager amEntReceDManager { get; set; }
        public AmEntReceLogDManager amEntReceLogDManager { get; set; }
        public AmEntAccountDManager amEntAccountDManager { get; set; }
        public AmAccountRoleManager amAccountRoleManager { get; set; }
        public CrBankAdjcaseMTmpManager crBankAdjcaseMTmpManager { get; set; }
        public CrBankAdjcaseDTmpManager crBankAdjcaseDTmpManager { get; set; }
        public HwCounterDManager hwCounterDManager { get; set; }
        public GmMerchantManager gmMerchantManager { get; set; }
        public CrBankAdjcaseManager crBankAdjcaseManager { get; set; }
        public CmBankDManager cmBankDManager { get; set; }


        public AccountingController()
        {
            choiceManager = new AmChoiceManager();
            amEntReceDManager = new AmEntReceDManager();
            amEntReceLogDManager = new AmEntReceLogDManager();
            amEntAccountDManager = new AmEntAccountDManager();
            amAccountRoleManager = new AmAccountRoleManager();
            hwCounterDManager = new HwCounterDManager();
            crBankAdjcaseMTmpManager = new CrBankAdjcaseMTmpManager();
            crBankAdjcaseDTmpManager = new CrBankAdjcaseDTmpManager();
            gmMerchantManager = new GmMerchantManager();
            crBankAdjcaseManager = new CrBankAdjcaseManager();
            cmBankDManager = new CmBankDManager();
        }

        // GET: Accounting
        public ActionResult Index()
        {
            AmChoice selAll = new AmChoice();
            selAll.Value = "";
            selAll.Name = "全部";
            IList<AmChoice> checkStatusList = this.choiceManager.FindByCode("Check_Status");
            checkStatusList.Add(selAll);
            ViewBag.CheckStatusSearch = new SelectList(checkStatusList, "Value", "Name", "");
            ViewBag.ACCTCheckStatusSearch = new SelectList(checkStatusList, "Value", "Name", "");
            List<AmEntAccountD> receTypeList = this.amEntAccountDManager.FindByRoleStatus(this.amRoles.RoleId,"ONLINE");
            if (this.amRoles.RoleId == "Accounting" || this.amRoles.RoleId == "System")
                receTypeList = this.amEntAccountDManager.FindByStatus("ONLINE");
            //receTypeList.Add(selAll);
            ViewBag.ReceTypeSearch = new SelectList(receTypeList, "AcctCode", "Department");
            return View();
        }

        [HttpPost]
        public ActionResult Result()
        {
            string startDate = Request.Form["StartDate"];
            ViewBag.StartDate = startDate;
            string endDate = Request.Form["EndDate"];
            ViewBag.EndDate = endDate;
            string receType = Request.Form["ReceTypeSearch"];
            ViewBag.ReceTypeSel = receType;
            string caseNo = Request.Form["CaseNoSearch"];
            ViewBag.CaseNoSel = caseNo;
            string checkStatus = Request.Form["CheckStatusSearch"];
            ViewBag.CheckStatusSel = checkStatus;
            string acctCheckStatus = Request.Form["AcctCheckStatusSearch"];
            ViewBag.AcctCheckStatusSel = acctCheckStatus;

            AmChoice selAll = new AmChoice();
            selAll.Value = "";
            selAll.Name = "全部";
            IList<AmChoice> checkStatusList = this.choiceManager.FindByCode("Check_Status");
            checkStatusList.Add(selAll);
            ViewBag.CheckStatusSearch = new SelectList(checkStatusList, "Value", "Name", checkStatus);
            ViewBag.ACCTCheckStatusSearch = new SelectList(checkStatusList, "Value", "Name", acctCheckStatus);
            List<AmEntAccountD> receTypeList = this.amEntAccountDManager.FindByRoleStatus(this.amRoles.RoleId, "ONLINE");
            if (this.amRoles.RoleId == "Accounting" || this.amRoles.RoleId == "System")
                receTypeList = this.amEntAccountDManager.FindByStatus("ONLINE");
            ViewBag.ReceTypeSearch = new SelectList(receTypeList, "AcctCode", "Department", receType);

            List<AmEntReceD> amEntReceList = amEntReceDManager.FindBySearch(receType, startDate, endDate, caseNo, checkStatus, acctCheckStatus);

            if (Request.Form["ExportExcel"] != null) //產生標籤Excel
            {
                var workbook = new HSSFWorkbook();
                var sheet = workbook.CreateSheet("收款明細表");
                var columns = new[] { "繳款編號", "帳號", "匯款帳號", "交易日期", "存入金額", "摘要", "案件編號", "核對狀態", "會計核對狀態", "經辦人", "備註" };
                var headerRow = sheet.CreateRow(0);

                HSSFCellStyle cs = (HSSFCellStyle)workbook.CreateCellStyle();
                //啟動多行文字
                //cs.WrapText = true;
                //文字置中
                cs.VerticalAlignment = VerticalAlignment.Center;
                cs.Alignment = HorizontalAlignment.Center;
                //框線樣式及顏色
                cs.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
                cs.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
                cs.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
                cs.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
                cs.BottomBorderColor = NPOI.HSSF.Util.HSSFColor.Grey25Percent.Index;
                cs.LeftBorderColor = NPOI.HSSF.Util.HSSFColor.Grey25Percent.Index;
                cs.RightBorderColor = NPOI.HSSF.Util.HSSFColor.Grey25Percent.Index;
                cs.TopBorderColor = NPOI.HSSF.Util.HSSFColor.Grey25Percent.Index;
                //背景顏色
                cs.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.Tan.Index;
                cs.FillPattern = NPOI.SS.UserModel.FillPattern.SolidForeground;
                HSSFFont font1 = (HSSFFont)workbook.CreateFont();
                //字體顏色
                font1.Color = NPOI.HSSF.Util.HSSFColor.DarkBlue.Index;
                //字體粗體
                font1.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.Bold;
                //字體尺寸
                font1.FontHeightInPoints = 12;
                cs.SetFont(font1);

                headerRow.HeightInPoints = 20;

                //create header
                for (int i = 0; i < columns.Length; i++)
                {
                    var cell = headerRow.CreateCell(i);
                    cell.SetCellValue(columns[i]);
                    cell.CellStyle = cs;
                }

                //fill content 
                HSSFCellStyle csc = (HSSFCellStyle)workbook.CreateCellStyle();
                //啟動多行文字
                //csc.WrapText = true;
                //文字置中
                csc.VerticalAlignment = VerticalAlignment.Center;
                csc.Alignment = HorizontalAlignment.Center;
                HSSFFont fontc = (HSSFFont)workbook.CreateFont();
                //字體尺寸
                fontc.FontHeightInPoints = 11;
                csc.SetFont(fontc);

                int rowIndex = 0;
                for(int j=0;j<amEntReceList.Count;j++)
                {
                    rowIndex++;
                    AmEntReceD obj = amEntReceList[j];
                    sheet.CreateRow(rowIndex);

                    ICell cell0 = sheet.GetRow(rowIndex).CreateCell(0);
                    cell0.CellStyle = csc;
                    cell0.SetCellValue(obj.Sn);

                    ICell cell1 = sheet.GetRow(rowIndex).CreateCell(1);
                    cell1.CellStyle = csc;
                    cell1.SetCellValue(obj.BankAcctTo);
                    

                    ICell cell2 = sheet.GetRow(rowIndex).CreateCell(2);
                    cell2.CellStyle = csc;
                    cell2.SetCellValue(obj.BankAcctFrom);

                    ICell cell3 = sheet.GetRow(rowIndex).CreateCell(3);
                    cell3.CellStyle = csc;
                    cell3.SetCellValue(obj.ActDate);

                    ICell cell4 = sheet.GetRow(rowIndex).CreateCell(4);
                    cell4.CellStyle = csc;
                    cell4.SetCellValue(obj.Amt);

                    ICell cell5 = sheet.GetRow(rowIndex).CreateCell(5);
                    cell5.CellStyle = csc;
                    cell5.SetCellValue(obj.Summary);

                    ICell cell6 = sheet.GetRow(rowIndex).CreateCell(6);
                    cell6.CellStyle = csc;
                    cell6.SetCellValue(obj.CaseNo);

                    ICell cell7 = sheet.GetRow(rowIndex).CreateCell(7);
                    cell7.CellStyle = csc;
                    cell7.SetCellValue(obj.CheckStatusName);

                    ICell cell8 = sheet.GetRow(rowIndex).CreateCell(8);
                    cell8.CellStyle = csc;
                    cell8.SetCellValue(obj.AcctCheckStatusName);

                    ICell cell9 = sheet.GetRow(rowIndex).CreateCell(9);
                    cell9.CellStyle = csc;
                    cell9.SetCellValue(obj.Operator);

                    ICell cell10 = sheet.GetRow(rowIndex).CreateCell(10);
                    cell10.CellStyle = csc;
                    cell10.SetCellValue(obj.Remark);

                    sheet.SetColumnWidth(0, 15 * 256);
                    sheet.SetColumnWidth(1, 15 * 256);
                    sheet.SetColumnWidth(2, 25 * 256);
                    sheet.SetColumnWidth(3, 15 * 256);
                    sheet.SetColumnWidth(4, 15 * 256);
                    sheet.SetColumnWidth(5, 15 * 256);
                    sheet.SetColumnWidth(6, 15 * 256);
                    sheet.SetColumnWidth(7, 15 * 256);
                    sheet.SetColumnWidth(8, 15 * 256);
                    sheet.SetColumnWidth(9, 15 * 256);
                    sheet.SetColumnWidth(10, 30 * 256);

                    sheet.GetRow(rowIndex).HeightInPoints = 18;
                }

                var stream = new MemoryStream();
                workbook.Write(stream);
                stream.Close();

                return File(stream.ToArray(), "application/vnd.ms-excel", "label" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls");


            }
            else {
                AmEntReceDModel result = new AmEntReceDModel();
                result.AmEntReceDList = amEntReceList;
                return View(result);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit()
        {
            string action = Request.Form["Action"];

            string sn = Request.Form["Sn"];
            string bankAcctTo = Request.Form["BankAcctTo"];
            AmEntReceD obj = this.amEntReceDManager.FindByPk(sn, bankAcctTo);
            if (obj == null)
            {
                return HttpNotFound();
            }
            string startDate = Request.Form["StartDate"];
            ViewBag.StartDate = startDate;
            string endDate = Request.Form["EndDate"];
            ViewBag.EndDate = endDate;
            string receType = Request.Form["ReceTypeSearch"];
            ViewBag.ReceTypeSel = receType;
            string caseNo = Request.Form["CaseNoSearch"];
            ViewBag.CaseNoSel = caseNo;
            string checkStatus = Request.Form["CheckStatusSearch"];
            ViewBag.CheckStatusSel = checkStatus;
            string acctCheckStatus = Request.Form["AcctCheckStatusSearch"];
            ViewBag.AcctCheckStatusSel = acctCheckStatus;

            FormsIdentity id = (FormsIdentity)User.Identity;
            FormsAuthenticationTicket ticket = id.Ticket;
            ViewBag.Dept = this.amAccountRoleManager.IsRoleBankAcct(obj.BankAcctTo, ticket.UserData);

            if (action == "Update")
            {
                try
                {
                    obj.CaseNo = Request.Form["CaseNo"];
                    obj.CheckStatus = Request.Form["CheckStatus"];
                    obj.AcctCheckStatus = Request.Form["AcctCheckStatus"];
                    obj.Remark = Request.Form["Remark"];
                    obj.Operator = User.Identity.Name;
                    AmEntReceLogD objLog = new AmEntReceLogD();

                    objLog.LogDate = DateTime.Now.ToString("yyyyMMdd");
                    objLog.LogTime = DateTime.Now.ToString("yyyyMMddHHmmss");
                    objLog.Sn = obj.Sn;
                    objLog.BankAcctTo = obj.BankAcctTo;
                    objLog.ActDate = obj.ActDate;
                    objLog.BillNo = obj.BillNo;
                    objLog.Amt = obj.Amt;
                    objLog.Bal = obj.Bal;
                    objLog.ActStatus = obj.ActStatus;
                    objLog.Summary = obj.Summary;
                    objLog.BankAcctFrom = obj.BankAcctFrom;
                    objLog.CaseNo = obj.CaseNo;
                    objLog.CheckStatus = obj.CheckStatus;
                    objLog.AcctCheckStatus = obj.AcctCheckStatus;
                    objLog.Operator = obj.Operator;
                    objLog.Remark = obj.Remark;
                    objLog.InvoiceNo = obj.InvoiceNo;
                    objLog.InvoiceDate = obj.InvoiceDate;

                    this.amEntReceDManager.Update(obj);
                    this.amEntReceLogDManager.Insert(objLog);
                    ModelState.AddModelError("", "資料更新成功");
                }
                catch
                {
                    ModelState.AddModelError("", "資料更新錯誤");
                }
            }

            IList<AmChoice> checkStatusList = this.choiceManager.FindByCode("Check_Status");
            ViewBag.CheckStatus = new SelectList(checkStatusList, "Value", "Name", obj.CheckStatus);
            ViewBag.ACCTCheckStatus = new SelectList(checkStatusList, "Value", "Name", obj.AcctCheckStatus);
            
            AmEntReceDModel result = new AmEntReceDModel();
            result.AmEntReceD = obj;
            return View(result);
        }

        public ActionResult CreateBankAdjD(string adjCaseNo, string startDate, string endDate, string statusSearch)
        {
            CrBankAdjcaseMTmp objM = this.crBankAdjcaseMTmpManager.FindByPk(adjCaseNo);
            ViewBag.MainStatus = objM.Status; 
            ViewBag.StartDate = startDate;
            ViewBag.EndDate = endDate;
            ViewBag.StatusSel = statusSearch;
            CrBankAdjcaseDTmp obj = new CrBankAdjcaseDTmp();
            //ViewBag.BankMerchant = new SelectList(this.gmMerchantManager.FindAllBnak(), "MerchantNo", "MerchantName");
            List<CrBankAdjcaseDTmp> dlist = this.crBankAdjcaseDTmpManager.FindByAdjCaseNoDisplay(adjCaseNo);
            CrBankAdjcaseDTmpModel result = new CrBankAdjcaseDTmpModel();
            result.CrBankAdjcaseDTmpList = dlist;
            result.CrBankAdjcaseDTmp = obj;
            return View(result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateBankAdjD(FormCollection collection)
        {
            CrBankAdjcaseDTmp obj = new CrBankAdjcaseDTmp();
            obj.AdjCaseNo = Request.Form["AdjCaseNo"];
            //obj.BankMerchant = Request.Form["BankMerchant"];
            obj.AdjAmt = Request.Form["AdjAmt"];
            obj.IccNo = Request.Form["IccNo"];
            obj.CreateUser = User.Identity.Name;
            obj.UpdateUser = User.Identity.Name;
            string errorMsg = "";
            if (obj.IccNo.Length == 16)
            {
                VertifyTool2 vt2 = new VertifyTool2();
                if (!vt2.CardChkVertify(obj.IccNo))
                {
                    errorMsg = "卡號驗證不合法!!";
                }
                else
                {
                    if (this.crBankAdjcaseDTmpManager.Exist(obj.AdjCaseNo, obj.IccNo))
                    {
                        errorMsg = "錯誤：卡片重複!!";
                    }
                    else { 
                        //卡號前兩碼找銀行
                        CmBankD cmBankD = this.cmBankDManager.FindByPk(obj.IccNo.Substring(0, 2));
                        if (cmBankD == null)
                            errorMsg = "錯誤：查無銀行!!";
                        else
                            obj.BankMerchant = cmBankD.MerchantNo;
                    }
                    //cmSvcD = this.cmSvcManager.FindByPk(iccNo);
                    //if (cmSvcD == null)
                    //{
                    //    errorMsg = "錯誤：卡片不存在主檔";
                    //}
                }
            }
            else
            {
                errorMsg = "卡號錯誤!!";
            }
            if (errorMsg == "")
            {
                try
                {
                    this.crBankAdjcaseDTmpManager.Insert(obj);
                    ModelState.AddModelError("", "新增調帳明細成功!!");
                }
                catch (Exception ex)
                {
                    //log.Debug(ex.Message);
                    ModelState.AddModelError("", "新增失敗:" + ex.Message);
                }
            }
            else {
                ModelState.AddModelError("", errorMsg);
            }
            CrBankAdjcaseMTmp objM = this.crBankAdjcaseMTmpManager.FindByPk(obj.AdjCaseNo);
            ViewBag.MainStatus = objM.Status; 
            ViewBag.StartDate = Request.Form["StartDate"];
            ViewBag.EndDate = Request.Form["EndDate"];
            ViewBag.StatusSel = Request.Form["StatusSearch"];
            //ViewBag.BankMerchant = new SelectList(this.gmMerchantManager.FindAllBnak(), "MerchantNo", "MerchantName", obj.BankMerchant);
            List<CrBankAdjcaseDTmp> dlist = this.crBankAdjcaseDTmpManager.FindByAdjCaseNoDisplay(obj.AdjCaseNo);
            CrBankAdjcaseDTmpModel result = new CrBankAdjcaseDTmpModel();
            result.CrBankAdjcaseDTmpList = dlist;
            result.CrBankAdjcaseDTmp = obj;
            return View(result);
        }

        public ActionResult DeleteBankAdjD(string adjCaseNo, string iccNo, string startDate, string endDate, string statusSearch)
        {
            try
            {
                // TODO: Add delete logic here
                CrBankAdjcaseDTmp obj = this.crBankAdjcaseDTmpManager.FindByPk(adjCaseNo, iccNo);
                if (obj == null)
                {
                    return HttpNotFound();
                }

                this.crBankAdjcaseDTmpManager.Delete(adjCaseNo, iccNo);
                return RedirectToAction("CreateBankAdjD", new { adjCaseNo = adjCaseNo, startDate = startDate, endDate = endDate, statusSearch = statusSearch });
            }
            catch
            {
                return View();
            }
        }

        public ActionResult BankAdjIndex()
        {
            string isChecked = "0";
            //ViewBag.StartDate = DateTime.Today.AddDays(-1).ToString("yyyy-MM-dd");
            //ViewBag.StartDate = DateTime.Today.ToString("yyyyMMdd");
            //ViewBag.EndDate = DateTime.Today.ToString("yyyyMMdd");
            ViewBag.StartDate = "";
            ViewBag.EndDate = "";
            List<CrBankAdjcaseMTmp> list = this.crBankAdjcaseMTmpManager.FindByAdjDateDisplay(ViewBag.StartDate, ViewBag.EndDate, isChecked);
 
            if (Request.Form["SearchConfirm"] != null)
            {
                isChecked = Request.Form["StatusSearch"];
                ViewBag.StartDate = Request.Form["StartDate"];
                ViewBag.EndDate = Request.Form["EndDate"];
                list = this.crBankAdjcaseMTmpManager.FindByAdjDateDisplay(ViewBag.StartDate, ViewBag.EndDate, isChecked);
            }
            else if (Request.Form["ExportExcel"] != null) //產生標籤Excel
            {
                isChecked = Request.Form["StatusSearch"];
                ViewBag.StartDate = Request.Form["StartDate"];
                ViewBag.EndDate = Request.Form["EndDate"];
                list = this.crBankAdjcaseMTmpManager.FindByAdjDateDisplay(ViewBag.StartDate, ViewBag.EndDate, isChecked);

                var workbook = new HSSFWorkbook();
                var sheet = workbook.CreateSheet("委外客服異動");
                var columns = new[] { "項次", "案件編號", "調帳原因", "調帳說明", "調帳日期", "清分日", "預計匯款日", "調帳方向", "狀態", "更新日期", "建立者", "更新者" };
                var headerRow = sheet.CreateRow(0);

                HSSFCellStyle cs = (HSSFCellStyle)workbook.CreateCellStyle();
                //啟動多行文字
                //cs.WrapText = true;
                //文字置中
                cs.VerticalAlignment = VerticalAlignment.Center;
                cs.Alignment = HorizontalAlignment.Center;
                //框線樣式及顏色
                cs.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
                cs.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
                cs.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
                cs.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
                cs.BottomBorderColor = NPOI.HSSF.Util.HSSFColor.Grey25Percent.Index;
                cs.LeftBorderColor = NPOI.HSSF.Util.HSSFColor.Grey25Percent.Index;
                cs.RightBorderColor = NPOI.HSSF.Util.HSSFColor.Grey25Percent.Index;
                cs.TopBorderColor = NPOI.HSSF.Util.HSSFColor.Grey25Percent.Index;
                //背景顏色
                cs.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.Tan.Index;
                cs.FillPattern = NPOI.SS.UserModel.FillPattern.SolidForeground;
                HSSFFont font1 = (HSSFFont)workbook.CreateFont();
                //字體顏色
                font1.Color = NPOI.HSSF.Util.HSSFColor.DarkBlue.Index;
                //字體粗體
                font1.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.Bold;
                //字體尺寸
                font1.FontHeightInPoints = 12;
                cs.SetFont(font1);

                headerRow.HeightInPoints = 20;

                //create header
                for (int i = 0; i < columns.Length; i++)
                {
                    var cell = headerRow.CreateCell(i);
                    cell.SetCellValue(columns[i]);
                    cell.CellStyle = cs;
                }

                //fill content 
                HSSFCellStyle csc = (HSSFCellStyle)workbook.CreateCellStyle();
                //啟動多行文字
                //csc.WrapText = true;
                //文字置中
                csc.VerticalAlignment = VerticalAlignment.Center;
                csc.Alignment = HorizontalAlignment.Center;
                HSSFFont fontc = (HSSFFont)workbook.CreateFont();
                //字體尺寸
                fontc.FontHeightInPoints = 11;
                csc.SetFont(fontc);

                int rowIndex = 0;

                for (int j = 0; j < list.Count; j++)
                {
                    rowIndex++;

                    CrBankAdjcaseMTmp obj = list[j];
                    sheet.CreateRow(rowIndex);

                    ICell cell00 = sheet.GetRow(rowIndex).CreateCell(0);
                    cell00.CellStyle = csc;
                    cell00.SetCellValue(rowIndex);

                    ICell cell0 = sheet.GetRow(rowIndex).CreateCell(1);
                    cell0.CellStyle = csc;
                    cell0.SetCellValue(obj.AdjCaseNo);

                    ICell cell1 = sheet.GetRow(rowIndex).CreateCell(2);
                    cell1.CellStyle = csc;
                    cell1.SetCellValue(obj.AdjCaseInfo);


                    ICell cell2 = sheet.GetRow(rowIndex).CreateCell(3);
                    cell2.CellStyle = csc;
                    cell2.SetCellValue(obj.AdjCaseContext);

                    ICell cell3 = sheet.GetRow(rowIndex).CreateCell(4);
                    cell3.CellStyle = csc;
                    cell3.SetCellValue(obj.AdjDate);

                    ICell cell4 = sheet.GetRow(rowIndex).CreateCell(5);
                    cell4.CellStyle = csc;
                    cell4.SetCellValue(obj.CptDate);

                    ICell cell5 = sheet.GetRow(rowIndex).CreateCell(6);
                    cell5.CellStyle = csc;
                    cell5.SetCellValue(obj.RemittanceDate);

                    ICell cell6 = sheet.GetRow(rowIndex).CreateCell(7);
                    cell6.CellStyle = csc;
                    cell6.SetCellValue(obj.AdjFlag);

                    ICell cell7 = sheet.GetRow(rowIndex).CreateCell(8);
                    cell7.CellStyle = csc;
                    cell7.SetCellValue(obj.Status);

                    ICell cell8 = sheet.GetRow(rowIndex).CreateCell(9);
                    cell8.CellStyle = csc;
                    cell8.SetCellValue(obj.UptDatetime);

                    ICell cell9 = sheet.GetRow(rowIndex).CreateCell(10);
                    cell9.CellStyle = csc;
                    cell9.SetCellValue(obj.CreateUser);

                    ICell cell10 = sheet.GetRow(rowIndex).CreateCell(11);
                    cell10.CellStyle = csc;
                    cell10.SetCellValue(obj.UpdateUser);

                    sheet.SetColumnWidth(0, 10 * 256);
                    sheet.SetColumnWidth(1, 22 * 256);
                    sheet.SetColumnWidth(2, 22 * 256);
                    sheet.SetColumnWidth(3, 45 * 256);
                    sheet.SetColumnWidth(4, 15 * 256);
                    sheet.SetColumnWidth(5, 15 * 256);
                    sheet.SetColumnWidth(6, 30 * 256);
                    sheet.SetColumnWidth(7, 22 * 256);
                    sheet.SetColumnWidth(8, 22 * 256);
                    sheet.SetColumnWidth(9, 22 * 256);
                    sheet.SetColumnWidth(10, 22 * 256);
                    sheet.SetColumnWidth(11, 22 * 256);

                    sheet.GetRow(rowIndex).HeightInPoints = 18;
                }

                var stream = new MemoryStream();
                workbook.Write(stream);
                stream.Close();

                return File(stream.ToArray(), "application/vnd.ms-excel", "BankAdj" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls");


            }
            ViewBag.StatusSearch = SetDropDown(true, "Check_Status", isChecked);
            ViewBag.StatusSel = isChecked;
            CrBankAdjcaseMTmpModel result = new CrBankAdjcaseMTmpModel();
            result.CrBankAdjcaseMTmpList = list;
            return View(result);
        }

        public ActionResult CreateBankAdj()
        {
            ViewBag.Status = SetDropDown(false, "Check_Status", "0");
            ViewBag.AdjFlag = SetDropDown(false, "ADJ_FLAG", "IC");
            CrBankAdjcaseMTmp obj = new CrBankAdjcaseMTmp();
            obj.AdjDate = DateTime.Today.AddDays(2).ToString("yyyy-MM-dd");
            CrBankAdjcaseMTmpModel result = new CrBankAdjcaseMTmpModel();
            result.CrBankAdjcaseMTmp = obj;
            return View(result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateBankAdj(FormCollection collection)
        {
            CrBankAdjcaseMTmp obj = new CrBankAdjcaseMTmp();
            obj.AdjCaseInfo = Request.Form["AdjCaseInfo"];
            obj.AdjCaseContext = Request.Form["AdjCaseContext"];

            obj.AdjDate = Request.Form["AdjDate"];
            if (obj.AdjDate != "")
            {
                obj.RemittanceDate = Convert.ToDateTime(obj.AdjDate).AddDays(2).ToString("yyyyMMdd");
                obj.AdjDate = Convert.ToDateTime(obj.AdjDate).ToString("yyyyMMdd");
                obj.CptDate = obj.AdjDate;
            }
            else {
                obj.RemittanceDate = "";
                obj.CptDate = "";
            }
            obj.AdjFlag = Request.Form["AdjFlag"];            
            obj.Status = "0";
            obj.CreateUser = User.Identity.Name.ToString();

            using (TransactionScope tx = new TransactionScope())
            {
                try
                {
                    //取號
                    string y = DateTime.Today.ToString("yyyy");
                    string m = DateTime.Today.ToString("MM");
                    int counter = hwCounterDManager.GetCounter(y, m, "ADJ");
                    if (counter > 0)
                    {
                        this.hwCounterDManager.Update(y, m, "ADJ");
                    }
                    else
                    {
                        this.hwCounterDManager.Insert(y, m, "ADJ");
                    }


                    obj.AdjCaseNo = y + m + hwCounterDManager.GetCounter(y, m, "ADJ").ToString().PadLeft(4, '0');
                    
                    this.crBankAdjcaseMTmpManager.Insert(obj);
                    tx.Complete();
                    TempData["message"] = "新增調帳成功，案件編號：" + obj.AdjCaseNo;
                    return RedirectToAction("BankAdjIndex");
                }
                catch (Exception ex)
                {
                    //log.Debug(ex.Message);
                    ModelState.AddModelError("", "新增失敗:" + ex.Message);
                }
            }

            ViewBag.AdjFlag = SetDropDown(false, "ADJ_FLAG", obj.AdjFlag);
            ViewBag.Status = SetDropDown(false, "Check_Status", obj.Status);
            CrBankAdjcaseMTmpModel result = new CrBankAdjcaseMTmpModel();
            result.CrBankAdjcaseMTmp = obj;
            return View(result);
        }

        public ActionResult EditBankAdj(string adjCaseNo, string startDate, string endDate, string statusSearch)
        {
            CrBankAdjcaseMTmp obj = this.crBankAdjcaseMTmpManager.FindByPk(adjCaseNo);
            if (obj == null)
            {
                return HttpNotFound();
            }
            ViewBag.StartDate = startDate;
            ViewBag.EndDate = endDate;
            ViewBag.StatusSel = statusSearch;
            ViewBag.Status = SetDropDown(false, "Check_Status", obj.Status);
            ViewBag.AdjFlag = SetDropDown(false, "ADJ_FLAG", obj.AdjFlag);
            if (ViewBag.UserRole == "Accounting" || ViewBag.UserRole == "System")
            {
                if (obj.AdjDate == "")
                {
                    obj.AdjDate = DateTime.Today.AddDays(2).ToString("yyyy-MM-dd");
                }
                else
                {
                    obj.AdjDate = this.SetDateFormat(obj.AdjDate);
                    obj.RemittanceDate = this.SetDateFormat(obj.RemittanceDate);
                    obj.CptDate = this.SetDateFormat(obj.CptDate);
                }
            }
            CrBankAdjcaseMTmpModel result = new CrBankAdjcaseMTmpModel();
            result.CrBankAdjcaseMTmp = obj;
            return View(result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditBankAdj()
        {
            string adjCaseNo = Request.Form["AdjCaseNo"];
            CrBankAdjcaseMTmp obj = this.crBankAdjcaseMTmpManager.FindByPk(adjCaseNo);
            if (obj == null)
            {
                return HttpNotFound();
            }

            string errorMsg = "";
            if (Request.Form["AdjDate"]!=null)
            {
                string adjDate = Request.Form["AdjDate"];
                if (adjDate != "")
                {
                    DateTime adjD = Convert.ToDateTime(adjDate);
                    if ((adjD - DateTime.Today).Days < 2)
                    {
                        errorMsg = "錯誤：調帳日期需為 D+2";
                    }
                    else {
                        obj.AdjDate = adjDate;
                    }
                }               
            }
            if (errorMsg == "")
            {
                using (TransactionScope tx = new TransactionScope())
                {
                    try
                    {
                        obj.AdjCaseNo = adjCaseNo;
                        obj.AdjCaseInfo = Request.Form["AdjCaseInfo"];
                        obj.AdjCaseContext = Request.Form["AdjCaseContext"];
                        obj.AdjFlag = Request.Form["AdjFlag"];
                        obj.UpdateUser = User.Identity.Name.ToString();
                        if (obj.AdjDate != "")
                        {
                            obj.AdjDate = Request.Form["AdjDate"];
                            obj.RemittanceDate = Convert.ToDateTime(obj.AdjDate).AddDays(2).ToString("yyyyMMdd");
                            obj.AdjDate = Convert.ToDateTime(obj.AdjDate).ToString("yyyyMMdd");
                            obj.CptDate = obj.AdjDate;
                            obj.Status = Request.Form["Status"];
                        }
                        if (obj.Status == "1")
                        {
                            CrBankAdjcaseM objM = new CrBankAdjcaseM();
                            objM.AdjCaseNo = obj.AdjCaseNo;
                            objM.AdjCaseInfo = obj.AdjCaseInfo;
                            objM.AdjCaseContext = obj.AdjCaseContext;
                            objM.AdjDate = obj.AdjDate;
                            objM.AdjFlag = obj.AdjFlag;
                            objM.RemittanceDate = obj.RemittanceDate;
                            objM.CptDate = obj.CptDate;
                            objM.UptDatetime = obj.UptDatetime;
                            this.crBankAdjcaseManager.InsertM(objM);

                            List<CrBankAdjcaseDTmp> dList = this.crBankAdjcaseDTmpManager.FindByAdjCaseNo(obj.AdjCaseNo);
                            for (int i = 0; i < dList.Count; i++)
                            {
                                CrBankAdjcaseD objD = new CrBankAdjcaseD();
                                objD.AdjCaseNo = dList[i].AdjCaseNo;
                                objD.BankMerchant = dList[i].BankMerchant;
                                objD.AdjAmt = dList[i].AdjAmt;
                                objD.IccNo = dList[i].IccNo;
                                objD.UptDatetime = dList[i].UptDatetime;
                                this.crBankAdjcaseManager.InsertD(objD);
                            }

                        }
                        this.crBankAdjcaseMTmpManager.Update(obj);
                        tx.Complete();
                        ModelState.AddModelError("", "更新調帳明細成功!!");
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("", "更新失敗:" + ex.Message);
                    }
                }
            }
            else {
                ModelState.AddModelError("", errorMsg);
            }
            if (ViewBag.UserRole == "Accounting" || ViewBag.UserRole == "System")
            {
                if (obj.AdjDate == "")
                {
                    obj.AdjDate = DateTime.Today.AddDays(2).ToString("yyyy-MM-dd");
                }
                else
                {
                    obj.AdjDate = this.SetDateFormat(obj.AdjDate);
                    obj.RemittanceDate = this.SetDateFormat(obj.RemittanceDate);
                    obj.CptDate = this.SetDateFormat(obj.CptDate);
                }
            }
            ViewBag.AdjFlag = SetDropDown(false, "ADJ_FLAG", obj.AdjFlag);
            ViewBag.Status = SetDropDown(false, "Check_Status", obj.Status);
            ViewBag.StartDate = Request.Form["StartDate"];
            ViewBag.EndDate = Request.Form["EndDate"];
            ViewBag.StatusSel = Request.Form["StatusSearch"];
            CrBankAdjcaseMTmpModel result = new CrBankAdjcaseMTmpModel();
            result.CrBankAdjcaseMTmp = obj;
            return View(result);
        }
    }
}