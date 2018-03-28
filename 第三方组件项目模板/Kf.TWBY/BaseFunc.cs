using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Kf.Core;
using Kf.Core.Qualifiers;
using Microsoft.Win32;
using System.Data;
using System.Xml;

namespace Kf.TWBY
{
    /// <summary>
    /// 公用函数定义
    /// </summary>
    public class BaseFunc
    {
        public const int CountPrecision = 4;
        public static int iAmountPrecision = 2;
        public BaseFunc()
        {

        }
        private static int _IsSupportCurrency = 0;
        private static int _IsExistPublicMeter = 0;
        private static int _IsSupportTaxInvocie = 0;

        static public object IsNull(object o, object DefualtValue)
        {
            if (o == null || o == DBNull.Value)
            {
                return DefualtValue;
            }
            else
            {
                return o;
            }
        }


        public static string GetOrgListByNumber(string OrgNumber)
        {

            string[] split = OrgNumber.Split(',');
            string   _InOrganizationNumber = " and kfOrganization.Number in (";
            foreach (string s in split)
            {
                if(s.Trim().Length>0)
                   _InOrganizationNumber += "'" + s + "',";
            }
            _InOrganizationNumber = _InOrganizationNumber.Substring(0, _InOrganizationNumber.Length - 1) + ") ";
            if (_InOrganizationNumber.IndexOf(',') < 0)
            {
                _InOrganizationNumber = "";
            }
            return _InOrganizationNumber;
        }


        public static string GetOrgListByNumber(ObjectContext objContext, string OrgNumber)
        {
         
            string[] split = OrgNumber.Split(',');
            ArrayList arr = new ArrayList();
            string _InOrganizationNumber = "  (";
            foreach (string s in split)
            {
                if (s.Trim().Length > 0)
                    arr.Add(s);
            }

            if (arr.Count > 0)
            {
                Kf.Core.Qualifiers.InValuesQualifier iq = new InValuesQualifier("Number", arr);
                EntityObjectList eol = EntityObjectFactory.GetInstance(objContext, Kf.Util.EntityIDEnum.Organization).Find(iq);
                foreach (EntityObject eo in eol)
                {
                    _InOrganizationNumber +=   eo.PrimaryKeyValue.ToString() + ",";
                }
                _InOrganizationNumber = _InOrganizationNumber.Substring(0, _InOrganizationNumber.Length - 1) + ") ";

            }
            else
                return "";
            return _InOrganizationNumber;
            
        }


        //private static int _isCustomerHouseDif = 0;
        //public static bool IsCustomerHouseDif
        //{
        //    get
        //    {
        //        if (_isCustomerHouseDif == 0)
        //        {
        //            object o = Kf.BllObj.SystemProfile.GetSystemProfileValueByID(Kf.Login.Environment.objContext.Clone(),362);
        //            if (o == null || o.ToString()=="0")
        //            {
        //                _isCustomerHouseDif = 2;

        //            }
        //            else
        //                _isCustomerHouseDif = 1;

        //        }
        //        if (_isCustomerHouseDif == 1)
        //            return true;
        //        else
        //            return false;
        //    }
        //}


        //private static int _isPayCurrent = 0;
        //public static bool isPayCurrent
        //{
        //    get
        //    {
        //        if (_isPayCurrent == 0)
        //        {


        //            object o = Kf.BllObj.SystemProfile.GetSystemProfileValueByID(Kf.Login.Environment.objContext.Clone(),386);
        //            if (o == null || o.ToString() == "1")
        //            {
        //                _isPayCurrent = 1;

        //            }
        //            else
        //                _isPayCurrent = 2;

        //        }
        //        if (_isPayCurrent == 1)
        //            return true;
        //        else
        //            return false;
        //    }
        //}



        private static int _isMaterialSelectUI;
        public static bool IsMaterialSelectUI
        {
            get
            {
                if (_isMaterialSelectUI == 0)
                {
                    Kf.Entity.Base.KfUIMainContainer ui = new Kf.Entity.Base.KfUIMainContainerFactory(Kf.Login.Environment.objContext.Clone()).FindObject(Kf.Util.UIIDEnum.WHBillList);
                        if (ui == null)
                        {
                            _isMaterialSelectUI = 2;

                        }
                        else
                            _isMaterialSelectUI = 1;
                 
                }
                if (_isMaterialSelectUI == 1)
                    return true;
                else
                    return false;
            }
        }

        //public static bool IsChamberArAmount(ObjectContext _obj,object OrgID)
        //{
        //    object o = Kf.BllObj.SystemProfile.GetSystemProfileValueByID(_obj, 355,(int)OrgID);
        //    if(o.ToString()=="1")
        //    {
        //        return true;
        //    }
        //    else 
        //        return false;
        //}

        //private static int _isExsistHouseMaintain = 0;
        //static public bool IsExsistHouseMaintain
        //{
        //    get
        //    {
        //        if (_isExsistHouseMaintain==0)
        //        {
        //            Kf.MetaData.DataEntity de = new Kf.MetaData.DataEntityFactory(Kf.Login.Environment.objContext.Clone()).FindObject(2270855);
        //            if (de != null)
        //                _isExsistHouseMaintain = 1;
        //            else
        //                _isExsistHouseMaintain = 2;

        //        }

        //        if (_isExsistHouseMaintain == 1)
        //            return true;
        //        else
        //            return false;

        //    }
        //}
        //private static int _isExsistChamber = 0;
        //static public bool IsExsistChamber
        //{
        //    get
        //    {
        //        if (_isExsistChamber == 0)
        //        {
        //            Kf.MetaData.DataEntity de = new Kf.MetaData.DataEntityFactory(Kf.Login.Environment.objContext.Clone()).FindObject(Kf.Util.EntityIDEnum.ChamberRoomOrder);
        //            if (de != null)
        //                _isExsistChamber = 1;
        //            else
        //                _isExsistChamber = 2;

        //        }

        //        if (_isExsistChamber == 1)
        //            return true;
        //        else
        //            return false;

        //    }
        //}
        /*
        private static int _isContractSort = 0;
        public static bool CheckInvoiceInput(EntityObject eo, ref string strMessage, ObjectContext obj)
        {


            if (!IsSupportInvoice(obj))
                return true;

            string strNoteNumber = "";
            string strInvoiceNumber = "";
            obj.SaveChanges();
            if (eo.EntityMap.EntityID == Kf.Util.EntityIDEnum.Foregift)
            {
                strInvoiceNumber = "";
            }
            else if (eo.EntityMap.EntityID == Kf.Util.EntityIDEnum.OtherCashJnl)
                strInvoiceNumber = eo.GetProperty("CheckNo").ToString();           
            else
                strInvoiceNumber = eo.GetProperty("InvoiceNumber").ToString();
            strNoteNumber = eo.GetProperty("ReceiptNO").ToString();
            EntityObjectFactory eof = EntityObjectFactory.GetInstance(obj, 2271196);
            EntityObjectFactory eofBill = EntityObjectFactory.GetInstance(obj, 2271227);
            string[] strNote = strNoteNumber.Split(',');
            string[] strInvoice = strInvoiceNumber.Split(',');
            decimal amount = 0m;
            Hashtable htItem = new Hashtable();
            Hashtable htToll = new Hashtable();
            decimal JnlAmount = 0m;


            if (eo.EntityMap.EntityID == Kf.Util.EntityIDEnum.CashJnl || eo.EntityMap.EntityID == Kf.Util.EntityIDEnum.BankJnl
                   || eo.EntityMap.EntityID == Kf.Util.EntityIDEnum.PreJnl || eo.EntityMap.EntityID == Kf.Util.EntityIDEnum.OtherCashJnl)
                JnlAmount = (decimal)eo.GetProperty("Amount");
            else if (eo.EntityMap.EntityID == Kf.Util.EntityIDEnum.Foregift)
            {
                JnlAmount = (decimal)eo.GetProperty("AmountReceived");
            }

            if (strNoteNumber.Trim().Length > 0)
            {
                foreach (string str in strNote)
                {
                    if (str.Trim() == "")
                        continue;
                    bool b = SaveInvoiceBusiness(1, eo, obj, ref strMessage, str, ref amount, htItem, htToll);
                    if (!b)
                        return false;
                }




                if (amount > JnlAmount)
                {
                    strMessage = string.Format("收据的出票金额{0}大于单据的收款金额{1}", amount, JnlAmount);
                    return false;
                }
                foreach (DictionaryEntry de in htItem)
                {
                    if (de.Key.ToString().StartsWith("0_"))
                        continue;
                    decimal BilledAmount = Convert.ToDecimal(de.Value);
                    if (!htToll.ContainsKey(de.Key))
                    {
                        strMessage = string.Format("收据开票明细没有包括在收款单据的项目中");
                        return false;
                    }
                    if (BilledAmount > Convert.ToDecimal(htToll[de.Key]))
                    {
                        strMessage = string.Format("收据开票明细的金额大于收款单据项目的金额");
                        return false;
                    }
                }

            }

            amount = 0m;



            if (strInvoiceNumber.Trim().Length > 0)
            {
                foreach (string str in strInvoice)
                {
                    if (str.Trim() == "")
                        continue;
                    bool b = SaveInvoiceBusiness(0, eo, obj, ref strMessage, str, ref amount, htItem, htToll);
                    if (!b)
                        return false;
                }

                if (amount > JnlAmount)
                {
                    strMessage = string.Format("发票的出票金额{0}大于单据的收款金额{1}", amount, JnlAmount);
                    return false;
                }
                foreach (DictionaryEntry de in htItem)
                {
                    if (de.Key.ToString().StartsWith("1_"))
                        continue;
                    decimal BilledAmount = Convert.ToDecimal(de.Value);
                    if (!htToll.ContainsKey(de.Key))
                    {
                        strMessage = string.Format("发票开票明细没有包括在收款单据的项目中");
                        return false;
                    }
                    if (BilledAmount > Convert.ToDecimal(htToll[de.Key]))
                    {
                        strMessage = string.Format("发票开票明细的金额大于收款单据项目的金额");
                        return false;
                    }
                }
            }
            return true;

        }
        private static bool SaveInvoiceBusiness(int type, EntityObject eo, ObjectContext obj, ref string strMessage, string str, ref decimal amount, Hashtable htItem, Hashtable htToll)
        {
            string strLabel = "发票";
            if (type == 1)
                strLabel = "收据";
            EntityObject eoi = EntityObjectFactory.GetInstance(obj, 2271196).FindFirst("Number={0} and InvoiceType={1}", str, type);
            if (eoi == null)
            {
                strMessage = string.Format("输入的{1}号{0}无效", str, strLabel);
                return false;
            }
            if (!(bool)eoi.GetProperty("isBilled"))
            {
                strMessage = string.Format("输入的{1}号{0}还没有出票", str, strLabel);
                return false;
            }
            if ((bool)eoi.GetProperty("isBankOut"))
            {
                strMessage = string.Format("输入的{1}号{0}已经作废", str, strLabel);
                return false;
            }
            string strInvoicePayer = "";
            EntityObject eoBill = EntityObjectFactory.GetInstance(obj, 2271227).FindFirst("InvoiceNoteID={0}", eoi.PrimaryKeyValue);
            if (eoBill != null)
            {
                strInvoicePayer = eoBill.GetRelatedObject("CustomerID").GetProperty("Name").ToString();
            }
            else
                strInvoicePayer = eoi.GetProperty("Payer").ToString();

            string strPayer = "";
            if (eo.EntityMap.EntityID == Kf.Util.EntityIDEnum.OtherCashJnl)
                strPayer = eo.GetProperty("Customer").ToString();
            else
                strPayer = eo.GetRelatedObject("CustomerID").GetProperty("Name").ToString();
            if (strInvoicePayer.Trim() != strPayer.ToString().Trim())
            {
                strMessage = string.Format("输入的{1}号{0}不是为客户{2}出具的票据", str, strLabel, strPayer);
                return false;
            }
            bool isEx = false;



            if ((bool)eoi.GetProperty("isAmount"))
            {
                EntityObjectList eol = EntityObjectFactory.GetInstance(obj, 2271214).Find("InvoiceNoteID={0} and EntityID={1} and RefID={2} and isBankOut=0", eoi.PrimaryKeyValue, eo.EntityMap.EntityID, eo.PrimaryKeyValue);
                EntityObject eopv = EntityObjectFactory.GetInstance(obj, 2271227).FindFirst("InvoiceNoteID={0} and isForbidden=0", eoi.PrimaryKeyValue);
                EntityObjectList eolpv = EntityObjectFactory.GetInstance(obj, 2271214).Find("InvoiceNoteID={0} and isBankOut=0", eoi.PrimaryKeyValue);
                if (eopv != null && eol.Count != eolpv.Count)
                {
                    strMessage = string.Format("输入的{1}号{0}是预开票据且被其他单据关联", str, strLabel);
                    return false;
                }

                foreach (EntityObject eob in eol)
                {
                    //isEx = true;
                    foreach (EntityObject eod in eob.GetChildEntityObjects(2271233))
                    {

                        isEx = true;
                        amount += (decimal)eod.GetProperty("Amount");

                        if (htItem.ContainsKey(type.ToString() + "_" + eod.GetProperty("EntityID").ToString() + "_" + eod.GetProperty("RefID").ToString()))
                        {
                            htItem[type.ToString() + "_" + eod.GetProperty("EntityID").ToString() + "_" + eod.GetProperty("RefID").ToString()] =
                                Convert.ToDecimal(htItem[type.ToString() + "_" + eod.GetProperty("EntityID").ToString() + "_" + eod.GetProperty("RefID").ToString()]) +
                                (decimal)eod.GetProperty("Amount");
                        }
                        else
                            htItem[type.ToString() + "_" + eod.GetProperty("EntityID").ToString() + "_" + eod.GetProperty("RefID").ToString()] = (decimal)eod.GetProperty("Amount");
                    }
                }
            }
            //else
            //amount += (decimal)eoi.GetProperty("Amount");
            if (!isEx && (bool)eoi.GetProperty("isAmount"))
            {
                strMessage = string.Format("输入的{1}号{0}与收款单据没有关联", str, strLabel);
                return false;
            }

            if (!(bool)eoi.GetProperty("isAmount"))
            {


                if (eo.EntityMap.EntityID == Kf.Util.EntityIDEnum.CashJnl || eo.EntityMap.EntityID == Kf.Util.EntityIDEnum.BankJnl || eo.EntityMap.EntityID == Kf.Util.EntityIDEnum.OtherCashJnl)
                {
                    EntityObject eoPreBill = EntityObjectFactory.GetInstance(obj, 2271227).FindFirst("InvoiceNoteID={0} and isForbidden=0", eoi.PrimaryKeyValue);
                    if (eoPreBill == null)
                    {
                        strMessage = string.Format("输入的{0}号{1}没有预开票记录", strLabel, str);
                        return false;
                    }
                    if ((bool)eoPreBill.GetProperty("isNotToll") && eo.EntityMap.EntityID == Kf.Util.EntityIDEnum.OtherCashJnl)
                    {
                        EntityObject eoToll = eoPreBill.GetChildEntityObjects(2271228)[0];


                        EntityObjectFactory eofB = EntityObjectFactory.GetInstance(obj, 2271214);
                        EntityObject eob = eofB.CreateObject();
                        EntityObjectFactory eofBDetail = EntityObjectFactory.GetInstance(obj, 2271233);
                        decimal IAmount = 0;
                        eob.SetProperty("Amount", eoi.GetProperty("Amount"));
                        eob.SetProperty("EntityID", eo.EntityMap.EntityID);
                        eob.SetProperty("RefID", eo.PrimaryKeyValue);
                        eob.SetProperty("InvoiceNoteID", eoi.PrimaryKeyValue);


                        if (!htToll.ContainsKey(string.Format("{0}_{1}_{2}", type, eo.EntityMap.EntityID, eo.PrimaryKeyValue)))
                            htToll[string.Format("{0}_{1}_{2}", type, eo.EntityMap.EntityID, eo.PrimaryKeyValue)] = (decimal)eoi.GetProperty("Amount");
                        else
                            htToll[string.Format("{0}_{1}_{2}", type, eo.EntityMap.EntityID, eo.PrimaryKeyValue)] = (decimal)htToll[string.Format("{0}_{1}_{2}", type, eo.EntityMap.EntityID, eo.PrimaryKeyValue)] + (decimal)eoi.GetProperty("Amount");

                        amount += (decimal)eoi.GetProperty("Amount");
                        eoi.SetProperty("isAmount", true);
                        eoi.SetProperty("AmountDate", DateTime.Today);
                        eoi.SetProperty("Payer", strPayer);
                        eoi.SetProperty("Receiver", eoi.GetProperty("Biller"));
                        eoi.SetProperty("InvoiceStatusID", 7);

                        EntityObject eobd = eofBDetail.CreateObject();
                        eobd.SetProperty("NotesBusinessID", eob.PrimaryKeyValue);
                        eobd.SetProperty("EntityID", eo.EntityMap.EntityID);
                        eobd.SetProperty("RefID", eo.PrimaryKeyValue);
                        eobd.SetProperty("Amount", eoToll.GetProperty("Amount"));
                        eobd.SetProperty("Description", eoToll.GetProperty("Description"));
                        eobd.SetProperty("Item", eo.GetRelatedObject("TollItemID").GetProperty("Name").ToString());
                        eobd.SetProperty("CalcStartDate", Kf.Core.Toolkit.GetDateTimeDefaultValue());
                        eobd.SetProperty("CalcEndDate", Kf.Core.Toolkit.GetDateTimeDefaultValue());
                        eobd.SetProperty("TollBill", eo.ToString());
                        eobd.SetProperty("TollAmount", eo.GetProperty("Amount"));
                        eoi.SetProperty("PreInvoiceStatusID", eoi.GetProperty("InvoiceStatusID"));
                        eoi.SetProperty("InvoiceStatusID", 4);
                        eoi.SetProperty("isChecked", true);
                        eoi.SetProperty("Checker", eoi.GetProperty("Biller"));
                        eoi.SetProperty("CheckDate", DateTime.Today);

                    }
                    else if (!(bool)eoPreBill.GetProperty("isNotToll"))
                    {

                        if ((decimal)eoPreBill.GetProperty("Amount") > (decimal)eo.GetProperty("Amount"))
                        {
                            strMessage = string.Format("票据{0}的开票金额大于收款单上的收款金额", eoi.ToString());
                            return false;
                        }

                        EntityObjectList eolToll = eoPreBill.GetChildEntityObjects(2271228);

                        EntityObjectFactory eofB = EntityObjectFactory.GetInstance(obj, 2271214);
                        EntityObject eob = eofB.CreateObject();
                        EntityObjectFactory eofBDetail = EntityObjectFactory.GetInstance(obj, 2271233);
                        decimal IAmount = 0;
                        eob.SetProperty("Amount", eoi.GetProperty("Amount"));
                        eob.SetProperty("EntityID", eo.EntityMap.EntityID);
                        eob.SetProperty("RefID", eo.PrimaryKeyValue);
                        eob.SetProperty("InvoiceNoteID", eoi.PrimaryKeyValue);
                        eoi.SetProperty("isAmount", true);
                        eoi.SetProperty("AmountDate", DateTime.Today);
                        eoi.SetProperty("Payer", strPayer);
                        eoi.SetProperty("Receiver", eoi.GetProperty("Biller"));
                        eoi.SetProperty("InvoiceStatusID", 7);



                        eoi.SetProperty("PreInvoiceStatusID", eoi.GetProperty("InvoiceStatusID"));
                        eoi.SetProperty("InvoiceStatusID", 4);
                        eoi.SetProperty("isChecked", true);
                        eoi.SetProperty("Checker", eoi.GetProperty("Biller"));
                        eoi.SetProperty("CheckDate", DateTime.Today);


                        int TollEntityID = 0;
                        if (eo.EntityMap.EntityID == Kf.Util.EntityIDEnum.CashJnl)
                        {
                            TollEntityID = Kf.Util.EntityIDEnum.CashJnlGeneralTollItem;
                        }
                        else
                        {
                            TollEntityID = Kf.Util.EntityIDEnum.BankJnlGeneralTollItem;
                        }
                        EntityObjectList eolgenral = eo.GetChildEntityObjects(TollEntityID);
                        foreach (EntityObject eog in eolgenral)
                        {
                            EntityObject eopre = eolToll.FindFirst("EntityID={0} and RefID={1}", Kf.Util.EntityIDEnum.GeneralToll, eog.GetProperty("GeneralTollID"));
                            if (eopre != null)
                            {
                                if ((decimal)eopre.GetProperty("Amount") > (decimal)eog.GetProperty("Amount"))
                                {
                                    strMessage = string.Format("预开票明细中的费用单{0}的开票金额大于收款单上的该费用单的收款金额", eog.GetRelatedObject("GeneralTollID").ToString());
                                    return false;
                                }

                                EntityObject eotoll = eog.GetRelatedObject("GeneralTollID");

                                if (!htToll.ContainsKey(string.Format("{0}_{1}_{2}", type, eotoll.EntityMap.EntityID, eotoll.PrimaryKeyValue)))
                                    htToll[string.Format("{0}_{1}_{2}", type, eotoll.EntityMap.EntityID, eotoll.PrimaryKeyValue)] = eog.GetProperty("Amount");


                                EntityObject eobd = eofBDetail.CreateObject();
                                eobd.SetProperty("NotesBusinessID", eob.PrimaryKeyValue);
                                eobd.SetProperty("EntityID", eotoll.EntityMap.EntityID);
                                eobd.SetProperty("RefID", eotoll.PrimaryKeyValue);
                                eobd.SetProperty("Amount", eopre.GetProperty("Amount"));
                                eobd.SetProperty("Description", eopre.GetProperty("Description"));
                                eobd.SetProperty("Item", eotoll.GetRelatedObject("TollItemID").GetProperty("Name").ToString());
                                eobd.SetProperty("CalcStartDate", eotoll.GetProperty("CalcStartDate"));
                                eobd.SetProperty("CalcEndDate", eotoll.GetProperty("CalcEndDate"));
                                eobd.SetProperty("TollBill", eotoll.ToString());
                                eobd.SetProperty("TollAmount", eotoll.GetProperty("Amount"));

                                amount += Convert.ToDecimal(BaseFunc.IsNull(eopre.GetProperty("Amount"), 0));
                                IAmount += Convert.ToDecimal(BaseFunc.IsNull(eopre.GetProperty("Amount"), 0));
                                if (htItem.ContainsKey(type.ToString() + "_" + eotoll.EntityMap.EntityID.ToString() + "_" + eotoll.PrimaryKeyValue.ToString()))
                                {
                                    decimal Amounted = Convert.ToDecimal(htItem[type.ToString() + "_" + eotoll.EntityMap.EntityID.ToString() + "_" + eotoll.PrimaryKeyValue.ToString()]);
                                    if (Amounted + (decimal)eopre.GetProperty("Amount") > (decimal)eog.GetProperty("Amount"))
                                    {
                                        strMessage = string.Format("费用单{0}的{1}开票金额大于收款单上的该费用单的收款金额", eotoll.ToString(), strLabel);
                                        return false;
                                    }
                                    htItem[type.ToString() + "_" + eotoll.EntityMap.EntityID.ToString() + "_" + eotoll.PrimaryKeyValue.ToString()] =
                                      Amounted +
                                        (decimal)eopre.GetProperty("Amount");
                                }
                                else
                                    htItem[type.ToString() + "_" + eotoll.EntityMap.EntityID.ToString() + "_" + eotoll.PrimaryKeyValue.ToString()] = (decimal)eopre.GetProperty("Amount");


                            }

                        }




                        if (eo.EntityMap.EntityID == Kf.Util.EntityIDEnum.CashJnl)
                        {
                            TollEntityID = Kf.Util.EntityIDEnum.CashJnlMeterTollItem;
                        }
                        else
                        {
                            TollEntityID = Kf.Util.EntityIDEnum.BankJnlMeterTollItem;
                        }
                        eolgenral = eo.GetChildEntityObjects(TollEntityID);
                        foreach (EntityObject eog in eolgenral)
                        {
                            EntityObject eopre = eolToll.FindFirst("EntityID={0} and RefID={1}", Kf.Util.EntityIDEnum.MeterToll, eog.GetProperty("MeterTollID"));
                            if (eopre != null)
                            {
                                if ((decimal)eopre.GetProperty("Amount") > (decimal)eog.GetProperty("Amount"))
                                {
                                    strMessage = string.Format("预开票明细中的费用单{0}的开票金额大于收款单上的该费用单的收款金额", eog.GetRelatedObject("MeterTollID").ToString());
                                    return false;
                                }

                                EntityObject eotoll = eog.GetRelatedObject("MeterTollID");

                                if (!htToll.ContainsKey(string.Format("{0}_{1}_{2}", type, eotoll.EntityMap.EntityID, eotoll.PrimaryKeyValue)))
                                    htToll[string.Format("{0}_{1}_{2}", type, eotoll.EntityMap.EntityID, eotoll.PrimaryKeyValue)] = eog.GetProperty("Amount");


                                EntityObject eobd = eofBDetail.CreateObject();
                                eobd.SetProperty("NotesBusinessID", eob.PrimaryKeyValue);
                                eobd.SetProperty("EntityID", eotoll.EntityMap.EntityID);
                                eobd.SetProperty("RefID", eotoll.PrimaryKeyValue);
                                eobd.SetProperty("Amount", eopre.GetProperty("Amount"));
                                eobd.SetProperty("Description", eopre.GetProperty("Description"));
                                eobd.SetProperty("Item", eotoll.GetRelatedObject("TollItemID").GetProperty("Name").ToString());
                                eobd.SetProperty("CalcStartDate", eotoll.GetProperty("CalcStartDate"));
                                eobd.SetProperty("CalcEndDate", eotoll.GetProperty("CalcEndDate"));
                                eobd.SetProperty("TollBill", eotoll.ToString());
                                eobd.SetProperty("TollAmount", eotoll.GetProperty("Amount"));
                                amount += Convert.ToDecimal(BaseFunc.IsNull(eopre.GetProperty("Amount"), 0));
                                IAmount += Convert.ToDecimal(BaseFunc.IsNull(eopre.GetProperty("Amount"), 0));
                                if (htItem.ContainsKey(type.ToString() + "_" + eotoll.EntityMap.EntityID.ToString() + "_" + eotoll.PrimaryKeyValue.ToString()))
                                {
                                    decimal Amounted = Convert.ToDecimal(htItem[type.ToString() + "_" + eotoll.EntityMap.EntityID.ToString() + "_" + eotoll.PrimaryKeyValue.ToString()]);
                                    if (Amounted + (decimal)eopre.GetProperty("Amount") > (decimal)eog.GetProperty("Amount"))
                                    {
                                        strMessage = string.Format("费用单{0}的{1}开票金额大于收款单上的该费用单的收款金额", eotoll.ToString(), strLabel);
                                        return false;
                                    }
                                    htItem[type.ToString() + "_" + eotoll.EntityMap.EntityID.ToString() + "_" + eotoll.PrimaryKeyValue.ToString()] =
                                      Amounted +
                                        (decimal)eopre.GetProperty("Amount");
                                }
                                else
                                    htItem[type.ToString() + "_" + eotoll.EntityMap.EntityID.ToString() + "_" + eotoll.PrimaryKeyValue.ToString()] = (decimal)eopre.GetProperty("Amount");


                            }

                        }




                        if (eo.EntityMap.EntityID == Kf.Util.EntityIDEnum.CashJnl)
                        {
                            TollEntityID = Kf.Util.EntityIDEnum.CashJnlTempTollItem;
                        }
                        else
                        {
                            TollEntityID = Kf.Util.EntityIDEnum.BankJnlTempTollItem;
                        }
                        eolgenral = eo.GetChildEntityObjects(TollEntityID);
                        foreach (EntityObject eog in eolgenral)
                        {
                            EntityObject eopre = eolToll.FindFirst("EntityID={0} and RefID={1}", Kf.Util.EntityIDEnum.TempToll, eog.GetProperty("TempTollID"));
                            if (eopre != null)
                            {
                                if ((decimal)eopre.GetProperty("Amount") > (decimal)eog.GetProperty("Amount"))
                                {
                                    strMessage = string.Format("预开票明细中的费用单{0}的开票金额大于收款单上的该费用单的收款金额", eog.GetRelatedObject("TempTollID").ToString());
                                    return false;
                                }
                                EntityObject eotoll = eog.GetRelatedObject("TempTollID");

                                if (!htToll.ContainsKey(string.Format("{0}_{1}_{2}", type, eotoll.EntityMap.EntityID, eotoll.PrimaryKeyValue)))
                                    htToll[string.Format("{0}_{1}_{2}", type, eotoll.EntityMap.EntityID, eotoll.PrimaryKeyValue)] = eog.GetProperty("Amount");


                                EntityObject eobd = eofBDetail.CreateObject();
                                eobd.SetProperty("NotesBusinessID", eob.PrimaryKeyValue);
                                eobd.SetProperty("EntityID", eotoll.EntityMap.EntityID);
                                eobd.SetProperty("RefID", eotoll.PrimaryKeyValue);
                                eobd.SetProperty("Amount", eopre.GetProperty("Amount"));
                                eobd.SetProperty("Description", eopre.GetProperty("Description"));
                                eobd.SetProperty("Item", eotoll.GetRelatedObject("TollItemID").GetProperty("Name").ToString());
                                eobd.SetProperty("CalcStartDate", eotoll.GetProperty("CalcStartDate"));
                                eobd.SetProperty("CalcEndDate", eotoll.GetProperty("CalcEndDate"));
                                eobd.SetProperty("TollBill", eotoll.ToString());
                                eobd.SetProperty("TollAmount", eotoll.GetProperty("Amount"));
                                amount += Convert.ToDecimal(BaseFunc.IsNull(eopre.GetProperty("Amount"), 0));
                                IAmount += Convert.ToDecimal(BaseFunc.IsNull(eopre.GetProperty("Amount"), 0));
                                if (htItem.ContainsKey(type.ToString() + "_" + eotoll.EntityMap.EntityID.ToString() + "_" + eotoll.PrimaryKeyValue.ToString()))
                                {
                                    decimal Amounted = Convert.ToDecimal(htItem[type.ToString() + "_" + eotoll.EntityMap.EntityID.ToString() + "_" + eotoll.PrimaryKeyValue.ToString()]);
                                    if (Amounted + (decimal)eopre.GetProperty("Amount") > (decimal)eog.GetProperty("Amount"))
                                    {
                                        strMessage = string.Format("费用单{0}的{1}开票金额大于收款单上的该费用单的收款金额", eotoll.ToString(), strLabel);
                                        return false;
                                    }
                                    htItem[type.ToString() + "_" + eotoll.EntityMap.EntityID.ToString() + "_" + eotoll.PrimaryKeyValue.ToString()] =
                                      Amounted +
                                        (decimal)eopre.GetProperty("Amount");
                                }
                                else
                                    htItem[type.ToString() + "_" + eotoll.EntityMap.EntityID.ToString() + "_" + eotoll.PrimaryKeyValue.ToString()] = (decimal)eopre.GetProperty("Amount");


                            }

                        }
                        if (IAmount != (decimal)eoi.GetProperty("Amount"))
                        {
                            strMessage = string.Format("票据{0}的预开票费用明细与收款单的费用明细不符合", eoi.ToString());
                            return false;
                        }
                    }
                    else
                    {
                        strMessage = string.Format("票据{0}是无欠费预见开票，只能由其他收款单使用", eoi.ToString());
                        return false;
                    }

                }
                else
                {
                    strMessage = string.Format("{0}是预开票据，只有现金收款单与银行托收单或其他收款单才能使用预开票的票据", eoi.ToString());
                    return false;
                }
            }
            if (eo.EntityMap.EntityID == Kf.Util.EntityIDEnum.CashJnl || eo.EntityMap.EntityID == Kf.Util.EntityIDEnum.BankJnl)
            {
                int TollEntityID = 0;
                if (eo.EntityMap.EntityID == Kf.Util.EntityIDEnum.CashJnl)
                {
                    TollEntityID = Kf.Util.EntityIDEnum.CashJnlGeneralTollItem;
                }
                else
                {
                    TollEntityID = Kf.Util.EntityIDEnum.BankJnlGeneralTollItem;
                }
                EntityObjectList eolgenral = eo.GetChildEntityObjects(TollEntityID);
                foreach (EntityObject eog in eolgenral)
                {

                    EntityObject eotoll = eog.GetRelatedObject("GeneralTollID");

                    if (!htToll.ContainsKey(string.Format("{0}_{1}_{2}", type, eotoll.EntityMap.EntityID, eotoll.PrimaryKeyValue)))
                        htToll[string.Format("{0}_{1}_{2}", type, eotoll.EntityMap.EntityID, eotoll.PrimaryKeyValue)] = eog.GetProperty("Amount");
                }

                if (eo.EntityMap.EntityID == Kf.Util.EntityIDEnum.CashJnl)
                {
                    TollEntityID = Kf.Util.EntityIDEnum.CashJnlMeterTollItem;
                }
                else
                {
                    TollEntityID = Kf.Util.EntityIDEnum.BankJnlMeterTollItem;
                }
                eolgenral = eo.GetChildEntityObjects(TollEntityID);
                foreach (EntityObject eog in eolgenral)
                {
                    EntityObject eotoll = eog.GetRelatedObject("MeterTollID");

                    if (!htToll.ContainsKey(string.Format("{0}_{1}_{2}", type, eotoll.EntityMap.EntityID, eotoll.PrimaryKeyValue)))
                        htToll[string.Format("{0}_{1}_{2}", type, eotoll.EntityMap.EntityID, eotoll.PrimaryKeyValue)] = eog.GetProperty("Amount");

                }

                if (eo.EntityMap.EntityID == Kf.Util.EntityIDEnum.CashJnl)
                {
                    TollEntityID = Kf.Util.EntityIDEnum.CashJnlTempTollItem;
                }
                else
                {
                    TollEntityID = Kf.Util.EntityIDEnum.BankJnlTempTollItem;
                }
                eolgenral = eo.GetChildEntityObjects(TollEntityID);
                foreach (EntityObject eog in eolgenral)
                {
                    EntityObject eotoll = eog.GetRelatedObject("TempTollID");

                    if (!htToll.ContainsKey(string.Format("{0}_{1}_{2}", type, eotoll.EntityMap.EntityID, eotoll.PrimaryKeyValue)))
                        htToll[string.Format("{0}_{1}_{2}", type, eotoll.EntityMap.EntityID, eotoll.PrimaryKeyValue)] = eog.GetProperty("Amount");

                }
            }
            if (eo.EntityMap.EntityID == Kf.Util.EntityIDEnum.PreJnl)
            {
                if ((bool)eo.GetProperty("isAfterAmount"))
                {
                    EntityObjectList eolit = eo.GetChildEntityObjects(922);
                    foreach (EntityObject eoit in eolit)
                    {
                        if (!htToll.ContainsKey(string.Format("{0}_{1}_{2}", type, eoit.EntityMap.EntityID, eoit.PrimaryKeyValue)))
                            htToll[string.Format("{0}_{1}_{2}", type, eoit.EntityMap.EntityID, eoit.PrimaryKeyValue)] = eoit.GetProperty("ActualAmount");

                    }
                }
                else
                {
                    if (!htToll.ContainsKey(string.Format("{0}_{1}_{2}", type, eo.EntityMap.EntityID, eo.PrimaryKeyValue)))
                        htToll[string.Format("{0}_{1}_{2}", type, eo.EntityMap.EntityID, eo.PrimaryKeyValue)] = eo.GetProperty("ActualAmount");

                }
            }
            else if (eo.EntityMap.EntityID == Kf.Util.EntityIDEnum.Foregift)
            {
                EntityObjectList eolit = eo.GetChildEntityObjects(963);
                foreach (EntityObject eoit in eolit)
                {
                    if (!htToll.ContainsKey(string.Format("{0}_{1}_{2}", type, eoit.EntityMap.EntityID, eoit.PrimaryKeyValue)))
                        htToll[string.Format("{0}_{1}_{2}", type, eoit.EntityMap.EntityID, eoit.PrimaryKeyValue)] = eoit.GetProperty("Amount");

                }
            }
            else
            {
                if (!htToll.ContainsKey(string.Format("{0}_{1}_{2}", type, eo.EntityMap.EntityID, eo.PrimaryKeyValue)))
                    htToll[string.Format("{0}_{1}_{2}", type, eo.EntityMap.EntityID, eo.PrimaryKeyValue)] = eo.GetProperty("Amount");

            }

            obj.SaveChanges();
            return true;
        }
        
        public static bool IsContractSort(ObjectContext obj)
        {
            if (_isContractSort == 0)
            {
                object o = Kf.BllObj.SystemProfile.GetSystemProfileValueByID(obj,315);
                if (o != null)
                {
                    if (o.ToString() == "1")
                    {
                        _isContractSort = 1;
                    }
                    else
                        _isContractSort = 2;

                }
                else
                    _isContractSort = 2;
            }
            if (_isContractSort == 1)
                return true;
            else
                return false;
        }



        public static void ImportContractData(Kf.Control.KfGrids.KfGrid gridDetial)
        {

            if (gridDetial.ReadOnly)
            {
                return;
            }
            Kf.Equipment.Tenancy.ContractDataInput frmImport = new Kf.Equipment.Tenancy.ContractDataInput();
            frmImport.ShowDialog();
            if (frmImport.ImportOK)
            {


                ArrayList htColumns = new ArrayList();
                foreach (Kf.Control.KfGrids.Column column in gridDetial.Columns)
                {
                    if (column.EditMode == Infragistics.Win.UltraWinGrid.Activation.AllowEdit)
                    {
                        htColumns.Add(new DictionaryEntry(column.Caption, column));
                    }
                }
                gridDetial.DeleteAllRow();
                gridDetial.ReadOnly = false;
                int curRow = 0;


                foreach (DataRow dr in frmImport.OuterData.Rows)
                {

                    gridDetial.InsertRow(gridDetial.Rows.Count);
                    foreach (DictionaryEntry de in htColumns)
                    {
                        if (frmImport.OuterData.Columns.Contains(((Kf.Control.KfGrids.Column)de.Value).Caption))
                        {
                            if (((Kf.Control.KfGrids.Column)de.Value).ColDataType == Kf.Control.KfGrids.ColumnDataType.Boolean)
                                gridDetial.Rows[gridDetial.Rows.Count - 1].Cells[((Kf.Control.KfGrids.Column)de.Value).Name].Value = Convert.ToBoolean(BaseFunc.IsNull(dr[((Kf.Control.KfGrids.Column)de.Value).Caption], 0));
                            else if (((Kf.Control.KfGrids.Column)de.Value).ColDataType == Kf.Control.KfGrids.ColumnDataType.Decimal || ((Kf.Control.KfGrids.Column)de.Value).ColDataType == Kf.Control.KfGrids.ColumnDataType.Money)
                                gridDetial.Rows[gridDetial.Rows.Count - 1].Cells[((Kf.Control.KfGrids.Column)de.Value).Name].Value = Convert.ToDecimal(BaseFunc.IsNull(dr[((Kf.Control.KfGrids.Column)de.Value).Caption], 0));
                            else if (((Kf.Control.KfGrids.Column)de.Value).ColDataType == Kf.Control.KfGrids.ColumnDataType.Date)
                                gridDetial.Rows[gridDetial.Rows.Count - 1].Cells[((Kf.Control.KfGrids.Column)de.Value).Name].Value = dr[((Kf.Control.KfGrids.Column)de.Value).Caption];
                            else
                                gridDetial.Rows[gridDetial.Rows.Count - 1].Cells[((Kf.Control.KfGrids.Column)de.Value).Name].Value = BaseFunc.IsNull(dr[((Kf.Control.KfGrids.Column)de.Value).Caption], "");

                        }
                    }


                }



            }


        }
        */
        static public int AmountPrecision(ObjectContext objContext)
        {
            object eosys = Kf.BllObj.SystemProfile.GetSystemProfileValueByID(objContext,159);
            if (eosys == null)
            {
                iAmountPrecision = 2;
            }
            else
            {
                iAmountPrecision = Convert.ToInt32(eosys);
            }

            return iAmountPrecision;
        }
        /*
        static public EntityObject GetCustomer(ObjectContext objContext, int HouseID)
        {
            EntityObjectFactory factory = EntityObjectFactory.GetInstance(objContext, Kf.Util.EntityIDEnum.HouseCustomer);
            EntityObject eo = factory.FindFirst("HouseID={0}", HouseID);
            if (eo == null)
            {
                return null;
            }

            EntityObject customer = eo.GetRelatedObject("ResidentID");
            if (customer != null)
            {
                return customer;
            }

            customer = eo.GetRelatedObject("LesseeID");
            if (customer != null)
            {
                return customer;
            }

            customer = eo.GetRelatedObject("OwnerID");
            if (customer != null)
            {
                return customer;
            }

            return null;
        }


        static public EntityObject GetOwerCustomer(ObjectContext objContext, int HouseID)
        {
            EntityObjectFactory factory = EntityObjectFactory.GetInstance(objContext, Kf.Util.EntityIDEnum.HouseCustomer);
            EntityObject eo = factory.FindFirst("HouseID={0}", HouseID);
            if (eo == null)
            {
                return null;
            }


            EntityObject customer = eo.GetRelatedObject("OwnerID");
            if (customer != null)
            {
                return customer;
            }

            return null;
        }

        static public bool isUseSMS(ObjectContext objContext)
        {
            object eosys = Kf.BllObj.SystemProfile.GetSystemProfileValueByID(objContext, 142);
            if (eosys == null)
                return false;
            else
                return eosys.ToString() == "1" ? true : false;
        }

        static public bool isDisplyCustCard(ObjectContext objContext)
        {
            object eosys = Kf.BllObj.SystemProfile.GetSystemProfileValueByID(objContext, 236);
            if (eosys == null)
                return false;
            else
                return eosys.ToString() == "1" ? true : false;
        }

        static public bool isSczuling(ObjectContext objContext)
        {
            if (new Kf.Entity.Base.JeezUIMainContainerFactory(objContext).FindObject(2270953) != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        static public string GetOrganizationTable(ObjectContext objContext)
        {
            return "(select jzOrganization.* from jzOrganization " +
                (new EntityMap(Kf.Util.EntityIDEnum.Organization, objContext)).DataEntity.GetTableName(Kf.Login.Environment.UserID)
                + ")";
        }

        static public string GetMaterialTable(ObjectContext objContext)
        {
            return "(select jzMaterial.* from jzMaterial " +
                (new EntityMap(Kf.Util.EntityIDEnum.Material, objContext)).DataEntity.GetTableName(Kf.Login.Environment.UserID)
                + ")";
        }
        static public string GetWareHouseTable(ObjectContext objContext)
        {
            return "(select jzWarehouse.* from jzWarehouse " +
                (new EntityMap(Kf.Util.EntityIDEnum.Warehouse, objContext)).DataEntity.GetTableName(Kf.Login.Environment.UserID)
                + ")";
        }
        static public string GetMaterialUseTypeTable(ObjectContext objContext)
        {
            return "(select jzMaterialUseType.* from jzMaterialUseType " +
                (new EntityMap(Kf.Util.EntityIDEnum.MaterialUseType, objContext)).DataEntity.GetTableName(Kf.Login.Environment.UserID)
                + ")";
        }
        static public string GetArApCostItemGroupTable(ObjectContext objContext)
        {
            return "(select jzArApCostItemGroup.* from jzArApCostItemGroup " +
                (new EntityMap(Kf.Util.EntityIDEnum.ArApCostItemGroup, objContext)).DataEntity.GetTableName(Kf.Login.Environment.UserID)
                + ")";
        }
        */
        static public bool ShowNewPrice
        {
            get
            {
                ObjectContext objContext = Kf.Login.Environment.objContext.Clone();
                object eosys = Kf.BllObj.SystemProfile.GetSystemProfileValueByID(objContext,Kf.Util.SysParamEnum.SYS171);
                if (eosys != null && eosys.ToString() == "1")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        
        public static bool IsMustInputMaterialUsed
        {
            get
            {
                //ObjectContext objContext = Kf.Login.Environment.objContext.Clone();
                //object eosys = Kf.BllObj.SystemProfile.GetSystemProfileValueByID(objContext,Kf.Util.SysParamEnum.SYS232);
                //if (eosys != null && eosys.ToString() == "1")
                //{
                //    return true;
                //}
                //else
                //{
                    return false;
                //}
            }
        }
        /*
        public static bool IsExistPublicMeter(Kf.Core.ObjectContext objContext)
        {

            if (_IsExistPublicMeter == 0)
            {
                EntityObject eo = EntityObjectFactory.GetInstance(objContext, Kf.Util.EntityIDEnum.PublicMeter).FindFirst("ID>{0}", 0);
                if (eo != null)
                {
                    _IsExistPublicMeter = 1;
                }
                else
                {
                    _IsExistPublicMeter = 2;
                }
            }
            if (_IsExistPublicMeter == 1)
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        */
        public static bool isSupportAirConditionToll(ObjectContext objContext)
        {

            EntityObject eo = EntityObjectFactory.GetInstance(objContext, Kf.Util.EntityIDEnum.DataEntity).FindObject(2270564);
            if (eo != null)
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        public static Decimal GetAmountByDays(DateTime dtBeginDate, DateTime dtEndDate, decimal TollAmount)
        {
            if (dtEndDate < dtBeginDate) return 0m;

            int BeginDays = DateTime.DaysInMonth(dtBeginDate.Year, dtBeginDate.Month);
            int EndDays = DateTime.DaysInMonth(dtEndDate.Year, dtEndDate.Month);
            Kf.Core.ObjectContext objCtx = Kf.Login.Environment.objContext.Clone();

            return System.Math.Round(System.Math.Round(TollAmount, AmountPrecision(objCtx), MidpointRounding.AwayFromZero) *
               ((dtEndDate.Year * 12 + dtEndDate.Month) - (dtBeginDate.Year * 12 + dtBeginDate.Month) + 1), AmountPrecision(objCtx), MidpointRounding.AwayFromZero)
               - System.Math.Round(TollAmount * (dtBeginDate.Day - 1) / BeginDays, AmountPrecision(objCtx), MidpointRounding.AwayFromZero)
               - System.Math.Round(TollAmount * (EndDays - dtEndDate.Day) / EndDays, AmountPrecision(objCtx), MidpointRounding.AwayFromZero);



        }



        public static int MeterPrecision(Kf.Core.ObjectContext objContext)
        {

            object o = Kf.BllObj.SystemProfile.GetSystemProfileValueByID(objContext, 377);
            if (o == null)
                return 4;
            
                if (Convert.ToInt32(o) < 0 || Convert.ToInt32(o)>10)
                {
                    return 4;
                }
                return Convert.ToInt32(o);


        }



        public static bool IsSupportCurrency(Kf.Core.ObjectContext objContext)
        {
            if (_IsSupportCurrency == 0)
            {
                EntityObject eo = EntityObjectFactory.GetInstance(objContext, Kf.Util.EntityIDEnum.DataEntityCol).FindObject(12769);
                if (eo != null && eo.GetProperty("FieldName").ToString() == "CurrencyID")
                {
                    _IsSupportCurrency = 1;
                }
                else
                {
                    _IsSupportCurrency = 2;
                }
            }
            if (_IsSupportCurrency == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private static int _isSupportInvoice = 0;
        public static bool IsSupportInvoice(Kf.Core.ObjectContext objContext)
        {
            if (_isSupportInvoice == 0)
            {
               object o= Kf.BllObj.SystemProfile.GetSystemProfileValueByID(objContext, 303);
                if (o != null && o.ToString()=="1")
                {
                    
                    Kf.MetaData.DataEntity de = new Kf.MetaData.DataEntityFactory(objContext).FindObject(2271196);
                    if (de != null)
                    {
                        _isSupportInvoice = 1;
                    }
                    else
                    {
                        _isSupportInvoice = 2;
                    }
                }
                else
                {
                    _isSupportInvoice = 2;
                }
            }
            if (_isSupportInvoice == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 获取临时表的名称
        /// </summary>
        /// <returns></returns>
        static public string GetTempTableName()
        {
            return "#kf" + System.Guid.NewGuid().ToString().Replace("-", "");
        }


        /*
        public static int GetResident(ObjectContext objContext, object HouseID)
        {
            EntityObjectFactory factory = EntityObjectFactory.GetInstance(objContext, Kf.Util.EntityIDEnum.HouseCustomer);
            EntityObject houseCustomer = factory.FindFirst("HouseID={0}", HouseID);
            int CustomerID = 0;
            if (houseCustomer != null)
            {
                if ((int)houseCustomer.GetProperty("ResidentID") > 0)
                {
                    CustomerID = (int)houseCustomer.GetProperty("ResidentID");
                }
            }
            return CustomerID;
        }
        */

        //根据房间指定刷新客户
        //public static void RefreshCustomerByHouse(Kf.Core.ObjectContext objContext, Kf.Control.KfTextBoxs.KfTextBox jeeztext, string HouseNumber)
        //{
        //    jeeztext.PropertyPage.Enabled = true;
        //    if (HouseNumber.Length > 0)
        //    {
        //        EntityObjectFactory eof = EntityObjectFactory.GetInstance(objContext, Kf.Util.EntityIDEnum.House);
        //        Kf.Core.EntityObject dataEntityObjecths = eof.FindFirst("Number={0}", HouseNumber);
        //        if (dataEntityObjecths != null)
        //        {
        //            int HouseID = Convert.ToInt32(dataEntityObjecths.PrimaryKeyValue);
        //            EntityObjectFactory factory = EntityObjectFactory.GetInstance(objContext, Kf.Util.EntityIDEnum.HouseCustomer);
        //            EntityObject houseCustomer = factory.FindFirst("HouseID={0}", HouseID);
        //            int CustomerID = 0;
        //            if (houseCustomer != null)
        //            {
        //                if ((int)houseCustomer.GetProperty("ResidentID") > 0)
        //                {
        //                    CustomerID = (int)houseCustomer.GetProperty("ResidentID");
        //                }
        //                else if ((int)houseCustomer.GetProperty("LesseeID") > 0)
        //                {
        //                    CustomerID = (int)houseCustomer.GetProperty("LesseeID");
        //                }
        //                else if ((int)houseCustomer.GetProperty("OwnerID") > 0)
        //                {
        //                    CustomerID = (int)houseCustomer.GetProperty("OwnerID");
        //                }
        //            }

        //            if (CustomerID > 0)
        //            {
        //                eof = EntityObjectFactory.GetInstance(objContext, Kf.Util.EntityIDEnum.Customer);
        //                EntityObject dataEntityObject = eof.FindObject(CustomerID);
        //                jeeztext.AllowResponseValueChanged = false;
        //                jeeztext.Tag = dataEntityObject.PrimaryKeyValue;
        //                jeeztext.PropertyPage.Text = dataEntityObject.ToString();
        //                jeeztext.AllowResponseValueChanged = true;
        //                jeeztext.PropertyPage.Enabled = false;
        //            }
        //        }
        //    }

        //}
        static public void SetCurrentPeriod(ObjectContext objContext)
        {
            int CurrYear = 5000;
            int CurrPeriod = 1;
            objContext.Clear();
            EntityObjectFactory factory = EntityObjectFactory.GetInstance(objContext, Kf.Util.EntityIDEnum.CurrentPeriod);
            EntityObjectList periodlist = factory.Find("SystemName = {0}", "PMS");
            foreach (EntityObject eo in periodlist)
            {
                if (((int)eo.GetProperty("Year") * 100 + (int)eo.GetProperty("Period")) < CurrYear * 100 + CurrPeriod)
                {
                    CurrYear = (int)eo.GetProperty("Year");
                    CurrPeriod = (int)eo.GetProperty("Period");
                }
            }

            if (CurrYear == 5000)
            {
                Kf.Core.INativeQuery query = Kf.Login.RemoteCall.GetNativeQuery();
                Kf.Core.NativeQueryCommand cmd = new NativeQueryCommand();
                cmd.AppendCommandTextLine(" Select Top 1 * From jzCurrentPeriod order by [Year],[Period]");
                System.Data.DataTable dt = query.GetDataTable(objContext.ConnectionString, cmd);
                if (dt.Rows.Count > 0)
                {
                    CurrYear = (int)dt.Rows[0]["Year"];
                    CurrPeriod = (int)dt.Rows[0]["Period"];
                }
                else
                {
                    CurrYear = Kf.Login.Environment.StartYear;
                    CurrPeriod = Kf.Login.Environment.StartPeriod;
                }
            }

            Kf.Login.Environment.PMSCurrentYearPeriod.Year = CurrYear;
            Kf.Login.Environment.PMSCurrentYearPeriod.Period = CurrPeriod;
            Kf.General.UI.MainStatusBar.CurrentYearPeriod = CurrYear.ToString() + "年第" + CurrPeriod.ToString() + "期";
        }
        static public System.Collections.ArrayList GetAllGridRow(Kf.Control.KfGrids.KfGrid grid)
        {
            System.Collections.ArrayList arySelRow = new System.Collections.ArrayList();

            if (grid != null)
            {

                foreach (Infragistics.Win.UltraWinGrid.UltraGridRow r in grid.Rows)
                {
                    arySelRow.Add(r.ListIndex);
                }
            }

            return arySelRow;
        }

        static private System.Collections.Hashtable htHighLevelOrgCollection;
        static public System.Collections.ArrayList GetHighLevelOrg(ObjectContext objContext, EntityObject org)
        {
            if (org == null)
            {
                System.Collections.ArrayList arOrg = new System.Collections.ArrayList();
                arOrg.Add(0);
                return arOrg;
            }

            if (htHighLevelOrgCollection == null)
            {
                htHighLevelOrgCollection = new System.Collections.Hashtable();
            }

            if (!htHighLevelOrgCollection.Contains(org.PrimaryKeyValue))
            {
                System.Collections.ArrayList arOrg = new System.Collections.ArrayList();
                arOrg.Add(org.PrimaryKeyValue);

                NativeQueryCommand cmd = new NativeQueryCommand();
                cmd.AppendCommandTextLine("Select ParentID From jzOrganization"+Kf.Util.Constants.Hiberarchy_TableSuffix+@" Where ChildID={0}");
                cmd.AddParameter(org.PrimaryKeyValue);
                INativeQuery query = Kf.Login.RemoteCall.GetNativeQuery();

                foreach (System.Data.DataRow dr in query.GetDataTable(objContext.ConnectionString, cmd).Rows)
                {
                    arOrg.Add(dr[0]);
                }

                htHighLevelOrgCollection.Add(org.PrimaryKeyValue, arOrg);
            }
            return (System.Collections.ArrayList)htHighLevelOrgCollection[org.PrimaryKeyValue];
        }

        static private System.Collections.Hashtable htHighLevelDepartMentCollection;
        static public System.Collections.ArrayList GetHighLevelDepartMent(ObjectContext objContext, EntityObject DepartMent)
        {
            if (DepartMent == null)
            {
                System.Collections.ArrayList arDepartMent = new System.Collections.ArrayList();
                arDepartMent.Add(0);
                return arDepartMent;
            }

            if (htHighLevelDepartMentCollection == null)
            {
                htHighLevelDepartMentCollection = new System.Collections.Hashtable();
            }

            if (!htHighLevelDepartMentCollection.Contains(DepartMent.PrimaryKeyValue))
            {
                System.Collections.ArrayList arDepartMent = new System.Collections.ArrayList();
                arDepartMent.Add(DepartMent.PrimaryKeyValue);

                NativeQueryCommand cmd = new NativeQueryCommand();
                cmd.AppendCommandTextLine("Select ParentID From jzDepartMent"+Kf.Util.Constants.Hiberarchy_TableSuffix+@" Where ChildID={0}");
                cmd.AddParameter(DepartMent.PrimaryKeyValue);
                INativeQuery query = Kf.Login.RemoteCall.GetNativeQuery();

                foreach (System.Data.DataRow dr in query.GetDataTable(objContext.ConnectionString, cmd).Rows)
                {
                    arDepartMent.Add(dr[0]);
                }

                htHighLevelDepartMentCollection.Add(DepartMent.PrimaryKeyValue, arDepartMent);
            }
            return (System.Collections.ArrayList)htHighLevelDepartMentCollection[DepartMent.PrimaryKeyValue];
        }
        static private System.Collections.Hashtable htHighLevelEquipMentTypeCollection;
        static public System.Collections.ArrayList GetHighLevelEquipMentType(ObjectContext objContext, EntityObject EquipMentType)
        {
            if (EquipMentType == null)
            {
                System.Collections.ArrayList arEquipMentType = new System.Collections.ArrayList();
                arEquipMentType.Add(0);
                return arEquipMentType;
            }

            if (htHighLevelEquipMentTypeCollection == null)
            {
                htHighLevelEquipMentTypeCollection = new System.Collections.Hashtable();
            }

            if (!htHighLevelEquipMentTypeCollection.Contains(EquipMentType.PrimaryKeyValue))
            {
                System.Collections.ArrayList arEquipMentType = new System.Collections.ArrayList();
                arEquipMentType.Add(EquipMentType.PrimaryKeyValue);

                NativeQueryCommand cmd = new NativeQueryCommand();
                cmd.AppendCommandTextLine("Select ParentID From jzEquipmentType"+Kf.Util.Constants.Hiberarchy_TableSuffix+@" Where ChildID={0}");
                cmd.AddParameter(EquipMentType.PrimaryKeyValue);
                INativeQuery query = Kf.Login.RemoteCall.GetNativeQuery();

                foreach (System.Data.DataRow dr in query.GetDataTable(objContext.ConnectionString, cmd).Rows)
                {
                    arEquipMentType.Add(dr[0]);
                }

                htHighLevelEquipMentTypeCollection.Add(EquipMentType.PrimaryKeyValue, arEquipMentType);
            }
            return (System.Collections.ArrayList)htHighLevelEquipMentTypeCollection[EquipMentType.PrimaryKeyValue];
        }
        /// <summary>
        /// 添加键映射
        /// </summary>
        public static void AddKeyMapping(Kf.Control.KfGrids.KfGrid grid)
        {
            System.Windows.Forms.Keys keys;
            Infragistics.Win.UltraWinGrid.UltraGridAction action;
            Infragistics.Win.UltraWinGrid.UltraGridState disallowedState;
            Infragistics.Win.UltraWinGrid.UltraGridState requiredState;
            //添加enter键KeyMapping
            keys = System.Windows.Forms.Keys.Enter;

            action = Infragistics.Win.UltraWinGrid.UltraGridAction.BelowCell;
            requiredState = Infragistics.Win.UltraWinGrid.UltraGridState.Cell;
            disallowedState = Infragistics.Win.UltraWinGrid.UltraGridState.IsCheckbox;


            Infragistics.Win.UltraWinGrid.GridKeyActionMapping keyMapping =
                new Infragistics.Win.UltraWinGrid.GridKeyActionMapping(keys,
                action,
                disallowedState,
                requiredState, 0, 0);
            grid.DisbleEnterResponseTab = true;
            grid.KeyActionMappings.Add(keyMapping);

            keys = System.Windows.Forms.Keys.Enter;
            action = Infragistics.Win.UltraWinGrid.UltraGridAction.EnterEditMode;
            requiredState = Infragistics.Win.UltraWinGrid.UltraGridState.Cell;
            disallowedState = Infragistics.Win.UltraWinGrid.UltraGridState.IsCheckbox;

            keyMapping =
                new Infragistics.Win.UltraWinGrid.GridKeyActionMapping(keys,
                action,
                disallowedState,
                requiredState, 0, 0);
            grid.KeyActionMappings.Add(keyMapping);


            keys = System.Windows.Forms.Keys.Space;
            action = Infragistics.Win.UltraWinGrid.UltraGridAction.EnterEditMode;
            requiredState = Infragistics.Win.UltraWinGrid.UltraGridState.Cell;
            disallowedState = Infragistics.Win.UltraWinGrid.UltraGridState.IsCheckbox;

            keyMapping =
                new Infragistics.Win.UltraWinGrid.GridKeyActionMapping(keys,
                action,
                disallowedState,
                requiredState, 0, 0);
            grid.KeyActionMappings.Add(keyMapping);

        }
        //public static void SetCboCurrency(ObjectContext objctx, Kf.Control.KfComboEditors.KfComboEditor cboCurrency)
        //{
        //    if (cboCurrency != null && cboCurrency.Items.Count > 0)
        //    {
        //        EntityObject comm = EntityObjectFactory.GetInstance(objctx, Kf.Util.EntityIDEnum.Community).FindFirst("ID>{0}", 0);
        //        if (comm != null && (int)comm.GetProperty("CurrencyID") > 0)
        //        {
        //            cboCurrency.Value = comm.GetProperty("CurrencyID");
        //        }
        //        else
        //        {
        //            EntityObjectFactory eof = EntityObjectFactory.GetInstance(objctx, Kf.Util.EntityIDEnum.Currency);
        //            foreach (Infragistics.Win.ValueListItem vl in cboCurrency.Items)
        //            {
        //                if ((bool)eof.FindObject(vl.DataValue).GetProperty("IsStandardCurrency"))
        //                {
        //                    cboCurrency.Value = vl.DataValue;
        //                    break;
        //                }
        //            }
        //        }
        //    }
        //}

        public static void SetCurrencySymbol(ObjectContext objctx, int CurrencyID, System.Collections.ArrayList ControlList, Kf.ControlManager.EnumControl.SelectControl ctType)
        {
            //if (ControlList != null)
            //{
            //    EntityObject eo = null;
            //    if (CurrencyID != 0)
            //    {
            //        eo = EntityObjectFactory.GetInstance(objctx, Kf.Util.EntityIDEnum.Currency).FindObject(CurrencyID);
            //    }
            //    else
            //    {
            //        eo = EntityObjectFactory.GetInstance(objctx, Kf.Util.EntityIDEnum.Currency).FindFirst("IsStandardCurrency={0}", true);
            //    }
            //    if (eo != null)
            //    {
            //        foreach (Kf.ControlManager.ControlHelper cth in ControlList)
            //        {
            //            if (cth.ControlType == ctType)
            //            {
            //                if (cth.ControlType == Kf.ControlManager.EnumControl.SelectControl.JeezCurrencyTextBox)
            //                {
            //                    ((Kf.Control.KfCurrencyTextBoxs.KfCurrencyTextBox)cth.Control).CurrencyChar = eo.GetProperty("Sign").ToString();
            //                }
            //                else if (cth.ControlType == Kf.ControlManager.EnumControl.SelectControl.KfGrids)
            //                {
            //                    ((Kf.Control.KfGrids.KfGrid)cth.Control).CurrencySymbol = eo.GetProperty("Sign").ToString();
            //                }
            //            }
            //        }
            //    }
            //}
        }
        public static void SetCurrencySymbol(ObjectContext objctx, int CurrencyID, System.Collections.ArrayList ControlList)
        {
            //if (ControlList != null)
            //{
            //    EntityObject eo = null;
            //    if (CurrencyID != 0)
            //    {
            //        eo = EntityObjectFactory.GetInstance(objctx, Kf.Util.EntityIDEnum.Currency).FindObject(CurrencyID);
            //    }
            //    else
            //    {
            //        eo = EntityObjectFactory.GetInstance(objctx, Kf.Util.EntityIDEnum.Currency).FindFirst("IsStandardCurrency={0}", true);
            //    }
            //    if (eo != null)
            //    {
            //        foreach (Kf.ControlManager.ControlHelper cth in ControlList)
            //        {
            //            if (cth.ControlType == Kf.ControlManager.EnumControl.SelectControl.JeezCurrencyTextBox)
            //            {
            //                ((Kf.Control.KfCurrencyTextBoxs.KfCurrencyTextBox)cth.Control).CurrencyChar = eo.GetProperty("Sign").ToString();
            //            }
            //            else if (cth.ControlType == Kf.ControlManager.EnumControl.SelectControl.KfGrids)
            //            {
            //                ((Kf.Control.KfGrids.KfGrid)cth.Control).CurrencySymbol = eo.GetProperty("Sign").ToString();
            //            }
            //        }
            //    }
            //}
        }
        /*
        public static DataDynamics.ActiveReports.ARControl GetARControl(DataDynamics.ActiveReports.Section section, string ControlName)
        {
            foreach (DataDynamics.ActiveReports.ARControl CTL in section.Controls)
            {
                if (CTL.Name.ToLower() == ControlName.ToLower())
                {
                    return CTL;
                }
            }
            return null;
        }
        */




        /// <summary>
        /// 自动填充指定对象集合的下拉列表.(完全指定记录集合)
        /// </summary>
        /// <param name="objContext">对象空间</param>
        /// <param name="ce">下拉列表控件</param>
        /// <param name="EntityID">填充指定数据表ID</param>
        internal static void FillUltraComboEditor(ObjectContext objContext, Kf.Control.KfComboEditors.KfComboEditor ce, int EntityID,bool isNull,bool isLevel)
        {
            Infragistics.Win.ValueListItem oldDataValue = ce.SelectedItem;

            ce.Items.Clear();
            ce.Text = "";
            if(isNull)
            ce.Items.Add(0, " ");

            EntityObjectList eol = EntityObjectFactory.GetInstance(objContext, EntityID).FindAllObjects();

            if (eol != null)
            {
                foreach (EntityObject eo in eol)
                {
                    if (isLevel)
                       // ce.Items.Add(eo.PrimaryKeyValue, Microsoft.VisualBasic.Strings.Space(eo.ObjectHiberarchy.Level) + eo.ToString());
                        ce.Items.Add(eo.PrimaryKeyValue, eo.ToString());
                    else
                      ce.Items.Add(eo.PrimaryKeyValue, eo.ToString());
                }
            }
            if (oldDataValue == null)
            {
                ce.SelectedItem = null;
            }
            else
            {
                foreach (Infragistics.Win.ValueListItem item in ce.Items)
                {
                    if (Convert.ToInt32(item.DataValue) == Convert.ToInt32(oldDataValue.DataValue))
                    {
                        ce.SelectedItem = item;
                        break;
                    }
                }
            }
        }



        /// <summary>
        /// 自动填充指定对象集合的下拉列表.(完全指定记录集合)
        /// </summary>
        /// <param name="objContext">对象空间</param>
        /// <param name="ce">下拉列表控件</param>
        /// <param name="EntityID">填充指定数据表ID</param>
        internal static void FillUltraComboEditor(ObjectContext objContext, Kf.Control.KfComboEditors.KfComboEditor ce, int EntityID)
        {
            Infragistics.Win.ValueListItem oldDataValue = ce.SelectedItem;

            ce.Items.Clear();
            ce.Text = "";

            EntityObjectList eol = EntityObjectFactory.GetInstance(objContext, EntityID).FindAllObjects();

            if (eol != null)
            {
                foreach (EntityObject eo in eol)
                {
                    ce.Items.Add(eo.PrimaryKeyValue, eo.ToString());
                }
            }
            if (oldDataValue == null)
            {
                ce.SelectedItem = null;
            }
            else
            {
                foreach (Infragistics.Win.ValueListItem item in ce.Items)
                {
                    if (Convert.ToInt32(item.DataValue) == Convert.ToInt32(oldDataValue.DataValue))
                    {
                        ce.SelectedItem = item;
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// 自动填充指定对象集合的下拉列表.(完全指定记录集合)
        /// </summary>
        /// <param name="objContext">对象空间</param>
        /// <param name="ce">下拉列表控件</param>
        /// <param name="eol">填充指定记录集合</param>
        internal static void FillUltraComboEditor(ObjectContext objContext, Kf.Control.KfComboEditors.KfComboEditor ce, EntityObjectList eol)
        {
            Infragistics.Win.ValueListItem oldDataValue = ce.SelectedItem;

            ce.Items.Clear();
            ce.Text = "";


            if (eol != null)
            {
                foreach (EntityObject eo in eol)
                {
                    ce.Items.Add(eo.PrimaryKeyValue, eo.ToString());
                }
            }
            if (oldDataValue == null)
            {
                ce.SelectedItem = null;
            }
            else
            {
                foreach (Infragistics.Win.ValueListItem item in ce.Items)
                {
                    if (Convert.ToInt32(item.DataValue) == Convert.ToInt32(oldDataValue.DataValue))
                    {
                        ce.SelectedItem = item;
                        break;
                    }
                }
            }
        }

        #region 加载房产树
        /*

        public static void FillListReportTreeData(ObjectContext objContext, TreeType treetype, Kf.Control.KfTrees.KfTree tree, int OrgID)
        {
            EntityObjectFactory eof;
            EntityObjectList eol = new EntityObjectList();



            eof = EntityObjectFactory.GetInstance(objContext, Kf.Util.EntityIDEnum.Organization);
            EntityObject eo = eof.FindObject(OrgID);
            if (eo == null)
                return;
            eol.Add(eo);

            tree.FillTreeFromObjectList(eol, "Name", true);




            foreach (Infragistics.Win.UltraWinTree.UltraTreeNode nd in tree.Nodes)
            {
                if (nd.Tag != null)
                {
                    SetOtherNodes(objContext, treetype, tree, nd);
                }
            }

        }


        public static void FillListReportTreeData(ObjectContext objContext, TreeType treetype, Kf.Control.KfTrees.KfTree  tree, bool NeedRootNode)
        {
            EntityObjectFactory eof;
            EntityObjectList eol;
            bool bAddRootNode;

     
            Kf.Core.GlobalCache.AddDirtyEntity(EntityObjectFactory.GetInstance(objContext, Kf.Util.EntityIDEnum.HouseCustomer).EntityMap, objContext);
            Kf.Core.GlobalCache.AddDirtyEntity(EntityObjectFactory.GetInstance(objContext, Kf.Util.EntityIDEnum.House).EntityMap, objContext);
            Kf.Core.GlobalCache.AddDirtyEntity(EntityObjectFactory.GetInstance(objContext, Kf.Util.EntityIDEnum.Customer).EntityMap, objContext);
            eof = EntityObjectFactory.GetInstance(objContext, Kf.Util.EntityIDEnum.Organization);
            eol = eof.Find("Forbidden={0}", false);
            if (eol.Count == 1)
            {
                tree.FillTreeFromObjectList(eol, "Name", true);
                bAddRootNode = false;
            }
            else
            {
                tree.FillTreeFromObjectList(eol, "Name", true, NeedRootNode);
                if (tree.Nodes[0].Nodes.Count == 1)
                {
                    Infragistics.Win.UltraWinTree.TreeNodesCollection nodeC = tree.Nodes[0].Nodes;
                    if (nodeC.Tag is EntityObject)
                    {
                    }
                    else
                    {
                        tree.Nodes.Remove(tree.Nodes[0]);

                        foreach (Infragistics.Win.UltraWinTree.UltraTreeNode node in nodeC)
                        {
                            tree.Nodes.Add(node);
                        }
                    }
                    bAddRootNode = false;
                }
                else
                {
                    bAddRootNode = true;
                }
            }

            if (bAddRootNode)
            {
                foreach (Infragistics.Win.UltraWinTree.UltraTreeNode nd in tree.Nodes[0].Nodes)
                {
                    if (nd.Tag != null)
                    {
                        SetOtherNodes(objContext, treetype, tree, nd);
                    }
                }
            }
            else
            {
                foreach (Infragistics.Win.UltraWinTree.UltraTreeNode nd in tree.Nodes)
                {
                    if (nd.Tag != null)
                    {
                        SetOtherNodes(objContext, treetype, tree, nd);
                    }
                }
            }
        }
        

        public static void FillListReportTreeData(ObjectContext objContext, TreeType treetype, Kf.ControlManager.ControlHelper CTL)
        {
            FillListReportTreeData(objContext, treetype, CTL, true);
        }
        public static void FillListReportTreeData(ObjectContext objContext, TreeType treetype, Kf.ControlManager.ControlHelper CTL,bool NeedRootNode)
        {
            EntityObjectFactory eof;
            EntityObjectList eol;
            bool bAddRootNode;

            Kf.Control.KfTrees.KfTree tree = CTL.Control as Kf.Control.KfTrees.KfTree;
            Kf.Core.GlobalCache.AddDirtyEntity(EntityObjectFactory.GetInstance(objContext, Kf.Util.EntityIDEnum.HouseCustomer).EntityMap, objContext);
            Kf.Core.GlobalCache.AddDirtyEntity(EntityObjectFactory.GetInstance(objContext, Kf.Util.EntityIDEnum.House).EntityMap, objContext);
            Kf.Core.GlobalCache.AddDirtyEntity(EntityObjectFactory.GetInstance(objContext, Kf.Util.EntityIDEnum.Customer).EntityMap, objContext);
            eof = EntityObjectFactory.GetInstance(objContext, Kf.Util.EntityIDEnum.Organization);
            eol = eof.Find("Forbidden={0}", false);
            if (eol.Count == 1)
            {
                tree.FillTreeFromObjectList(eol, "Name", true);
                bAddRootNode = false;
            }
            else
            {
                tree.FillTreeFromObjectList(eol, "Name", true, NeedRootNode);
                if (tree.Nodes[0].Nodes.Count == 1)
                {
                    Infragistics.Win.UltraWinTree.TreeNodesCollection nodeC = tree.Nodes[0].Nodes;
                    if (nodeC.Tag is EntityObject)
                    {
                    }
                    else
                    {
                        tree.Nodes.Remove(tree.Nodes[0]);

                        foreach (Infragistics.Win.UltraWinTree.UltraTreeNode node in nodeC)
                        {
                            tree.Nodes.Add(node);
                        }
                    }
                    bAddRootNode = false;
                }
                else
                {
                    bAddRootNode = true;
                }
            }

            if (bAddRootNode)
            {
                foreach (Infragistics.Win.UltraWinTree.UltraTreeNode nd in tree.Nodes[0].Nodes)
                {
                    if (nd.Tag != null)
                    {
                        SetOtherNodes(objContext, treetype, tree, nd);
                    }
                }
            }
            else
            {
                foreach (Infragistics.Win.UltraWinTree.UltraTreeNode nd in tree.Nodes)
                {
                    if (nd.Tag != null)
                    {
                        SetOtherNodes(objContext, treetype, tree, nd);
                    }
                }
            }
        }
        
        private static void SetOtherNodes(ObjectContext objContext, TreeType treetype, Kf.Control.KfTrees.KfTree Tree, Infragistics.Win.UltraWinTree.UltraTreeNode Node)
        {
            if (treetype == TreeType.Organization) return;

            EntityObjectFactory eof;
            EntityObjectList eol;
            EntityObject eo;
            Infragistics.Win.UltraWinTree.UltraTreeNode node;

            foreach (Infragistics.Win.UltraWinTree.UltraTreeNode nd in Node.Nodes)
            {
                SetOtherNodes(objContext, treetype, Tree, nd);
            }
            eo = Node.Tag as EntityObject;
            switch (treetype)
            {
                case TreeType.DepartMent:
                    eof = EntityObjectFactory.GetInstance(objContext, Kf.Util.EntityIDEnum.DepartMent);
                    eol = eof.Find("OrganizationID={0} and Forbidden={1}", eo.PrimaryKeyValue, false);
                    break;
                case TreeType.EquipMentType:
                    eof = EntityObjectFactory.GetInstance(objContext, Kf.Util.EntityIDEnum.EquipmentType);
                    eol = eof.Find("OrganizationID={0} and Forbidden={1}", eo.PrimaryKeyValue, false);
                    break;
                default:
                    eof = EntityObjectFactory.GetInstance(objContext, Kf.Util.EntityIDEnum.Community);
                    eol = eof.Find("OrganizationID={0} and Forbidden={1}", eo.PrimaryKeyValue, false);
                    break;

            }

            switch (treetype)
            {
                case TreeType.DepartMent:
                    Tree.FillTreeFromObjectList(Node, eol, true, "Department");
                    break;
                case TreeType.EquipMentType:
                    Tree.FillTreeFromObjectList(Node, eol, true, "EquipMentType");
                    break;
                default:
                    foreach (EntityObject o in eol)
                    {
                        node = new Infragistics.Win.UltraWinTree.UltraTreeNode(o.PrimaryKeyValue.ToString() + "Community", o.GetProperty("Name").ToString());
                        node.Tag = o;
                        if (treetype == TreeType.Build || treetype == TreeType.House || treetype == TreeType.Park || treetype == TreeType.ParkingSpace || treetype == TreeType.MeterType || treetype == TreeType.Meter)
                        {
                            node.Nodes.Add(System.Guid.NewGuid().ToString(), "");
                        }
                        node = Tree.AddNode(Node, node);
                    }
                    break;
            }

        }
        
        public static void SetSubNode(ObjectContext objContext, TreeType treetype, Kf.Control.KfTrees.KfTree Tree, Infragistics.Win.UltraWinTree.UltraTreeNode Node)
        {
            if (Node.Nodes.Count != 1 || Node.Nodes[0].Tag != null) return;
            Node.Nodes[0].Remove();

            EntityObject eoNode = Node.Tag as EntityObject;
            int EntityID = 0;
            switch (eoNode.EntityMap.DataEntity.EntityID)
            {
                case Kf.Util.EntityIDEnum.Community:
                    if (treetype == TreeType.House || treetype == TreeType.Build || treetype == TreeType.Floor)
                    {
                        EntityID = Kf.Util.EntityIDEnum.Build;
                    }
                    else if (treetype == TreeType.MeterType)
                    {
                        EntityID = Kf.Util.EntityIDEnum.MeterType;
                    }
                    else
                    {
                        EntityID = Kf.Util.EntityIDEnum.Park;
                    }
                    break;
                case Kf.Util.EntityIDEnum.Park:
                    EntityID = Kf.Util.EntityIDEnum.ParkingSpace;
                    break;
                case Kf.Util.EntityIDEnum.Build:
                    if (treetype == TreeType.Floor)
                    {
                        EntityID = Kf.Util.EntityIDEnum.Floor;
                    }
                    else
                    {
                        EntityID = Kf.Util.EntityIDEnum.House;
                    }
                    break;
                case Kf.Util.EntityIDEnum.MeterType:
                    EntityID = Kf.Util.EntityIDEnum.PublicMeter;
                    break;


            }
            EntityObjectFactory eof = EntityObjectFactory.GetInstance(objContext, EntityID);
            EntityObjectList eol = null;
            PropertyListComparer comparer;
            EntityObject eoNode2;
            string key = "";
            System.Collections.Hashtable htHouseCustomer = null;
            switch (EntityID)
            {
                case Kf.Util.EntityIDEnum.Park:
                case Kf.Util.EntityIDEnum.Build:
                    eol = eof.Find("CommunityID = {0} and Forbidden={1}", eoNode.PrimaryKeyValue, false);
                    key = "Build";
                    break;
                case Kf.Util.EntityIDEnum.Floor:
                    eol = eof.Find("BuildID = {0}", eoNode.PrimaryKeyValue, false);
                    key = "Floor";
                    eol.Sort("Number", SortDirection.Ascending);
                    break;
                case Kf.Util.EntityIDEnum.House:
                    eol = eof.Find("BuildID = {0} and Forbidden={1}", eoNode.PrimaryKeyValue, false);
                    eol.Sort("Number", SortDirection.Ascending);
                    key = "House";

                    System.Collections.ArrayList arHouse = new System.Collections.ArrayList();
                    System.Collections.ArrayList arCustomer = new System.Collections.ArrayList();
                    EntityObjectFactory factory = EntityObjectFactory.GetInstance(objContext, Kf.Util.EntityIDEnum.HouseCustomer);
                    foreach (EntityObject houseCustomer in factory.Find("HouseObject.BuildID={0}", eoNode.PrimaryKeyValue))
                    {
                        if ((int)houseCustomer.GetProperty("ResidentID") > 0)
                        {
                            arHouse.Add(houseCustomer.GetProperty("HouseID"));
                            arCustomer.Add(houseCustomer.GetProperty("ResidentID"));
                        }
                        else if ((int)houseCustomer.GetProperty("LesseeID") > 0)
                        {
                            arHouse.Add(houseCustomer.GetProperty("HouseID"));
                            arCustomer.Add(houseCustomer.GetProperty("LesseeID"));
                        }
                        else if ((int)houseCustomer.GetProperty("OwnerID") > 0)
                        {
                            arHouse.Add(houseCustomer.GetProperty("HouseID"));
                            arCustomer.Add(houseCustomer.GetProperty("OwnerID"));
                        }
                    }

                    htHouseCustomer = new System.Collections.Hashtable();
                    factory = EntityObjectFactory.GetInstance(objContext, Kf.Util.EntityIDEnum.Customer);
                    if (arHouse.Count > 0)
                    {
                        if (!Kf.Core.GlobalCache.IsWholeFetch(factory.EntityMap, objContext))
                        {
                            FetchSpecification fs = new FetchSpecification(factory.EntityMap);
                            fs.Qualifier = new Kf.Core.Qualifiers.InValuesQualifier("ID", arCustomer);
                            EntityObjectList cusList = factory.Find(fs);
                            for (int i = 0; i < arHouse.Count; i++)
                            {
                                EntityObject eoCustomer = cusList.FindFirst("ID={0}", arCustomer[i]);
                                if (eoCustomer != null)
                                {
                                    htHouseCustomer[arHouse[i]] = eoCustomer;
                                }
                            }
                        }
                        else
                        {
                            for (int i = 0; i < arHouse.Count; i++)
                            {
                                EntityObject eoCustomer = factory.FindFirst("ID={0}", arCustomer[i]);
                                if (eoCustomer != null)
                                {
                                    htHouseCustomer[arHouse[i]] = eoCustomer;
                                }
                            }
                        }
                    }
                    break;
                case Kf.Util.EntityIDEnum.MeterType:
                    eol = eof.Find(" Forbidden={0}", false);
                    eoNode2 = Node.Tag as EntityObject;//取得管理区的主键
                    comparer = new PropertyListComparer();
                    comparer.AddPropertyComparer("Number");
                    eol.Sort(comparer);
                    key = eoNode2.PrimaryKeyValue.ToString() + "MeterType";
                    break;
                case Kf.Util.EntityIDEnum.PublicMeter:
                    eoNode2 = Node.Tag as EntityObject;//取得管理区的主键
                    eol = eof.Find("CommunityID = {0} and PublicMeterTypeID ={1} and MeterTypeID ={2} and Forbidden={3}", eoNode2.PrimaryKeyValue, 1, eoNode.PrimaryKeyValue, false);
                    comparer = new PropertyListComparer();
                    comparer.AddPropertyComparer("Number");
                    eol.Sort(comparer);
                    key = eoNode2.PrimaryKeyValue.ToString() + "PublicMeter";
                    break;
                case Kf.Util.EntityIDEnum.ParkingSpace:
                    eol = eof.Find("ParkID = {0} and Forbidden={1}", eoNode.PrimaryKeyValue, false);
                    key = "House";
                    break;

            }

            if (eol != null)
            {
                if (EntityID == Kf.Util.EntityIDEnum.PublicMeter)
                {
                    Tree.FillTreeFromObjectList(Node, eol, true, key);
                }
                else
                {
                    foreach (EntityObject eo in eol)
                    {
                        string txtName = eo.ToString();
                        if (eo.EntityMap.DataEntity.GetDataEntityCol("Name") != null)
                        {
                            txtName = eo.GetProperty("Name").ToString();
                        }
                        else if (eo.EntityMap.EntityID == Kf.Util.EntityIDEnum.House && htHouseCustomer.Contains(eo.PrimaryKeyValue))
                        {
                            txtName += "[" + ((EntityObject)htHouseCustomer[eo.PrimaryKeyValue]).GetProperty("Name").ToString() + "]";
                        }
                        Infragistics.Win.UltraWinTree.UltraTreeNode node = new Infragistics.Win.UltraWinTree.UltraTreeNode(eo.PrimaryKeyValue.ToString() + key, txtName);

                        node.Tag = eo;
                        if ((treetype == TreeType.ParkingSpace && EntityID == Kf.Util.EntityIDEnum.Park) ||
                            (treetype == TreeType.House && EntityID == Kf.Util.EntityIDEnum.Build)
                            || (treetype == TreeType.Floor && EntityID == Kf.Util.EntityIDEnum.Build)
                            || (treetype == TreeType.MeterType && EntityID == Kf.Util.EntityIDEnum.MeterType))
                        {

                            node.Nodes.Add(System.Guid.NewGuid().ToString(), "");
                        }
                        Tree.AddNode(Node, node);
                    }
                }
            }

        

        }*/
        #endregion

        static public EntityObjectList GetLevelEntity(int EntityID, int ParentID, bool bAddParent)
        {
            EntityObjectList list = new EntityObjectList();
            if (bAddParent)
            {
                EntityObjectFactory factory = EntityObjectFactory.GetInstance(Kf.Login.Environment.objContext, EntityID);
                list.Add(factory.FindObject(ParentID));
            }
            FindChildEntity(EntityID, ParentID, ref list);
            return list;
        }

        static private void FindChildEntity(int EntityID, int ParentID, ref EntityObjectList eol)
        {
            EntityObjectFactory factory = EntityObjectFactory.GetInstance(Kf.Login.Environment.objContext, EntityID);
            FetchSpecification fs = new FetchSpecification();
            fs.EntityMap = factory.EntityMap;
            fs.AddSortProperty("Level");
            fs.AddSortProperty("Number");
            fs.Qualifier = new ColumnQualifier("ParentID", new EqualsPredicate(ParentID));
            EntityObjectList list = factory.Find(fs);
            foreach (EntityObject entity in list)
            {
                if (entity.Children.Count > 0)
                {
                    eol.Add(entity);
                    FindChildEntity(EntityID, (int)entity.PrimaryKeyValue, ref eol);
                }
                else
                {
                    eol.Add(entity);
                }
            }
        }

        static public decimal GetQuotePrice(ObjectContext objcontext, int HouseID, DateTime date)
        {
            EntityObjectFactory eof = EntityObjectFactory.GetInstance(objcontext, Kf.Util.EntityIDEnum.HousePrice);
            EntityObjectList eol = eof.Find("HouseID={0}", HouseID);
            eol.Sort("PriceDate", SortDirection.Descending);
            foreach (EntityObject eo in eol)
            {
                if (((DateTime)eo.GetProperty("DateFrom")).Year == 1799 && date <= (DateTime)eo.GetProperty("DateTo"))
                {
                    return (decimal)eo.GetProperty("QuotePrice");
                }
                if (date >= (DateTime)eo.GetProperty("DateFrom") && ((DateTime)eo.GetProperty("DateTo")).Year == 1799)
                {
                    return (decimal)eo.GetProperty("QuotePrice");
                }
                if (date >= (DateTime)eo.GetProperty("DateFrom") && date <= (DateTime)eo.GetProperty("DateTo"))
                {
                    return (decimal)eo.GetProperty("QuotePrice");
                }
                if (((DateTime)eo.GetProperty("DateFrom")).Year == 1799 && ((DateTime)eo.GetProperty("DateTo")).Year == 1799)
                {
                    return (decimal)eo.GetProperty("QuotePrice");
                }


            }

            return 0;
        }

        /*
        public static bool isToll(ObjectContext objContext, int HouseID)
        {
            EntityObject eoGeneral = EntityObjectFactory.GetInstance(objContext, Kf.Util.EntityIDEnum.GeneralToll).FindFirst("HouseID={0} and Received={1} ", HouseID, 0);
            if (eoGeneral != null)
                return true;
            EntityObject eoMeter = EntityObjectFactory.GetInstance(objContext, Kf.Util.EntityIDEnum.MeterToll).FindFirst("HouseID={0} and Received={1} ", HouseID, 0);
            if (eoMeter != null)
                return true;
            EntityObject eoTemp = EntityObjectFactory.GetInstance(objContext, Kf.Util.EntityIDEnum.TempToll).FindFirst("HouseID={0} and Received={1} ", HouseID, 0);
            if (eoTemp != null)
                return true;

            return false;
        }
        
        public static bool isTollByCustomer(ObjectContext objContext, int HouseID,int customerID)
        {
            EntityObject eoGeneral = EntityObjectFactory.GetInstance(objContext, Kf.Util.EntityIDEnum.GeneralToll).FindFirst("HouseID={0} and Received={1} and CustomerID={2} ", HouseID, 0,customerID);
            if (eoGeneral != null)
                return true;
            EntityObject eoMeter = EntityObjectFactory.GetInstance(objContext, Kf.Util.EntityIDEnum.MeterToll).FindFirst("HouseID={0} and Received={1} and CustomerID={2} ", HouseID, 0, customerID);
            if (eoMeter != null)
                return true;
            EntityObject eoTemp = EntityObjectFactory.GetInstance(objContext, Kf.Util.EntityIDEnum.TempToll).FindFirst("HouseID={0} and Received={1} and CustomerID={2} ", HouseID, 0, customerID);
            if (eoTemp != null)
                return true;

            return false;
        }
*/

        protected static int _CardDecut = 0;

        public static bool isSuppierCardDeduct(ObjectContext objContext)
        {
            if (_CardDecut == 0)
            {
                object o = Kf.BllObj.SystemProfile.GetSystemProfileValueByID(objContext, 312);
                if (o != null)
                {
                    if (o.ToString() == "1")
                        _CardDecut = 1;
                    else
                        _CardDecut = -1;
                }
            }
            if (_CardDecut == 1)
                return true;
            else
                return false;
    
        }

        private static int _isContractComfirm = 0;
        public static bool isContractComfirm(ObjectContext objContext)
        {
            if (_isContractComfirm == 0)
            {
                object o = Kf.BllObj.SystemProfile.GetSystemProfileValueByID(objContext, 361);
                if (o != null)
                {
                    if (o.ToString() == "1")
                        _isContractComfirm=1;
                    else
                        _isContractComfirm = 2;
                }
            }
            if (_isContractComfirm == 1)
            {
                return true;
            }
            else
                return false;
        
           
        }

        private static int _isPixPrice = 0;
        public static bool isPixPrice(ObjectContext objContext)
        {
            if (_isPixPrice == 0)
            {
                object o = Kf.BllObj.SystemProfile.GetSystemProfileValueByID(objContext, 322);
                if (o != null)
                {
                    if (o.ToString() == "1")
                        _isPixPrice=1;
                    else
                        _isPixPrice = 2;
                }
            }
            if (_isPixPrice == 1)
            {
                return true;
            }
            else
                return false;
        }


        static public decimal GetQoutePriceAmount(ObjectContext objcontext, int HouseID, DateTime date, decimal dArea)
        {
       
            EntityObjectFactory eof = EntityObjectFactory.GetInstance(objcontext, Kf.Util.EntityIDEnum.HousePrice);
            EntityObjectList eol = eof.Find("HouseID={0}", HouseID);
            eol.Sort("PriceDate", SortDirection.Descending);
            foreach (EntityObject eo in eol)
            {
                if (date >= (DateTime)eo.GetProperty("DateFrom") && date <= (DateTime)eo.GetProperty("DateTo"))
                {
              
                        if ((decimal)eo.GetProperty("FloorX") > 0 && (decimal)eo.GetProperty("SpecX") > 0)
                        {
                            decimal d = (decimal)eo.GetProperty("QuotePrice") * (decimal)eo.GetProperty("FloorX") * (decimal)eo.GetProperty("SpecX") * dArea;
                            decimal d1 = System.Math.Round((decimal)eo.GetProperty("QuotePrice") * (decimal)eo.GetProperty("FloorX") * (decimal)eo.GetProperty("SpecX") * dArea, 0);
                            if (d - d1 > 0m)
                            {
                                d1 = d1 + 1m;
                            }
                            return d1;
                        }
                        else
                            return (decimal)eo.GetProperty("QuotePrice") * dArea;
                  
                }
                if (((DateTime)eo.GetProperty("DateFrom")).Year == 1799 && date <= (DateTime)eo.GetProperty("DateTo"))
                {

                        if ((decimal)eo.GetProperty("FloorX") > 0 && (decimal)eo.GetProperty("SpecX") > 0)
                        {
                            decimal d = (decimal)eo.GetProperty("QuotePrice") * (decimal)eo.GetProperty("FloorX") * (decimal)eo.GetProperty("SpecX") * dArea;
                            decimal d1 = System.Math.Round((decimal)eo.GetProperty("QuotePrice") * (decimal)eo.GetProperty("FloorX") * (decimal)eo.GetProperty("SpecX") * dArea, 0);
                            if (d - d1 > 0m)
                            {
                                d1 = d1 + 1m;
                            }
                            return d1;
                        }
                        else
                            return (decimal)eo.GetProperty("QuotePrice") * dArea;
                   
                }
                if (date >= (DateTime)eo.GetProperty("DateFrom") && ((DateTime)eo.GetProperty("DateTo")).Year == 1799)
                {
                        if ((decimal)eo.GetProperty("FloorX") > 0 && (decimal)eo.GetProperty("SpecX") > 0)
                        {
                            decimal d = (decimal)eo.GetProperty("QuotePrice") * (decimal)eo.GetProperty("FloorX") * (decimal)eo.GetProperty("SpecX") * dArea;
                            decimal d1 = System.Math.Round((decimal)eo.GetProperty("QuotePrice") * (decimal)eo.GetProperty("FloorX") * (decimal)eo.GetProperty("SpecX") * dArea, 0);
                            if (d - d1 > 0m)
                            {
                                d1 = d1 + 1m;
                            }
                            return d1;
                        }
                        else
                            return (decimal)eo.GetProperty("QuotePrice") * dArea;

                }
                if (((DateTime)eo.GetProperty("DateFrom")).Year == 1799 && ((DateTime)eo.GetProperty("DateTo")).Year == 1799)
                {
                        if ((decimal)eo.GetProperty("FloorX") > 0 && (decimal)eo.GetProperty("SpecX") > 0)
                        {
                            decimal d = (decimal)eo.GetProperty("QuotePrice") * (decimal)eo.GetProperty("FloorX") * (decimal)eo.GetProperty("SpecX") * dArea;
                            decimal d1 = System.Math.Round((decimal)eo.GetProperty("QuotePrice") * (decimal)eo.GetProperty("FloorX") * (decimal)eo.GetProperty("SpecX") * dArea, 0);
                            if (d - d1 >= 0.1m)
                            {
                                d1 = d1 + 1m;
                            }
                            return d1;
                        }
                        else
                            return (decimal)eo.GetProperty("QuotePrice") * dArea;
                   
                }


            }

            return 0;
        }
        static public Hashtable GetRentItem(ObjectContext objcontext, int HouseID, DateTime date, decimal dArea)
        {
            bool _PixPrice = isPixPrice(objcontext);
            EntityObjectFactory eof = EntityObjectFactory.GetInstance(objcontext, Kf.Util.EntityIDEnum.HousePrice);
            EntityObjectList eol = eof.Find("HouseID={0}", HouseID);
            eol.Sort("PriceDate", SortDirection.Descending);
            EntityObjectList eolTemp = new EntityObjectList();
            foreach (EntityObject eo in eol)
            {
                EntityObject eoItem = eo.GetRelatedObject("TollItemID");
                if (eoItem != null && (int)eoItem.GetProperty("TollItemTypeID") == 5 && eolTemp.FindFirst("TollItemID={0}", eoItem.PrimaryKeyValue) == null && date >= (DateTime)eo.GetProperty("DateFrom") && date <= (DateTime)eo.GetProperty("DateTo"))
                {
                    eolTemp.Add(eo);
                }
                if (eoItem != null && (int)eoItem.GetProperty("TollItemTypeID") == 5 && eolTemp.FindFirst("TollItemID={0}", eoItem.PrimaryKeyValue) == null && ((DateTime)eo.GetProperty("DateFrom")).Year == 1799 && date <= (DateTime)eo.GetProperty("DateTo"))
                {
                    eolTemp.Add(eo);
                }
                if (eoItem != null && (int)eoItem.GetProperty("TollItemTypeID") == 5 && eolTemp.FindFirst("TollItemID={0}", eoItem.PrimaryKeyValue) == null && date >= (DateTime)eo.GetProperty("DateFrom") && ((DateTime)eo.GetProperty("DateTo")).Year == 1799)
                {
                    eolTemp.Add(eo);
                }
                if (eoItem != null && (int)eoItem.GetProperty("TollItemTypeID") == 5 && eolTemp.FindFirst("TollItemID={0}", eoItem.PrimaryKeyValue) == null && ((DateTime)eo.GetProperty("DateFrom")).Year == 1799 && ((DateTime)eo.GetProperty("DateTo")).Year == 1799)
                {
                    eolTemp.Add(eo);
                }
            }
            Hashtable ht = new Hashtable();
            foreach (EntityObject eo in eolTemp)
            {
                if (!_PixPrice)
                {
                    if ((decimal)eo.GetProperty("FloorX") > 0 && (decimal)eo.GetProperty("SpecX") > 0)
                    {
                        decimal d = (decimal)eo.GetProperty("BasePrice") * (decimal)eo.GetProperty("FloorX") * (decimal)eo.GetProperty("SpecX") * dArea;
                        decimal d1 = System.Math.Round((decimal)eo.GetProperty("BasePrice") * (decimal)eo.GetProperty("FloorX") * (decimal)eo.GetProperty("SpecX") * dArea, 0);
                        if (d - d1 > 0m)
                        {
                            d1 = d1 + 1m;
                        }
                        ht[eo.GetProperty("TollItemID")] = d1;
                    }
                    else
                        ht[eo.GetProperty("TollItemID")] = (decimal)eo.GetProperty("BasePrice") * dArea;
                }
                else
                    ht[eo.GetProperty("TollItemID")] = (decimal)eo.GetProperty("BasePrice");
            }
            return ht;
        }
        static public Hashtable GetTempItem(ObjectContext objcontext, int HouseID, DateTime date, decimal dArea)
        {
            bool _PixPrice = isPixPrice(objcontext);
            EntityObjectFactory eof = EntityObjectFactory.GetInstance(objcontext, Kf.Util.EntityIDEnum.HousePrice);
            EntityObjectList eol = eof.Find("HouseID={0}", HouseID);
            eol.Sort("PriceDate", SortDirection.Descending);
            EntityObjectList eolTemp = new EntityObjectList();
            foreach (EntityObject eo in eol)
            {
                EntityObject eoItem = eo.GetRelatedObject("TollItemID");
                if (eoItem != null && (int)eoItem.GetProperty("TollItemTypeID") == 3 && eolTemp.FindFirst("TollItemID={0}", eoItem.PrimaryKeyValue) == null && date >= (DateTime)eo.GetProperty("DateFrom") && date <= (DateTime)eo.GetProperty("DateTo"))
                {
                    eolTemp.Add(eo);
                }
                if (eoItem != null && (int)eoItem.GetProperty("TollItemTypeID") == 3 && eolTemp.FindFirst("TollItemID={0}", eoItem.PrimaryKeyValue) == null && ((DateTime)eo.GetProperty("DateFrom")).Year == 1799 && date <= (DateTime)eo.GetProperty("DateTo"))
                {
                    eolTemp.Add(eo);
                }
                if (eoItem != null && (int)eoItem.GetProperty("TollItemTypeID") == 3 && eolTemp.FindFirst("TollItemID={0}", eoItem.PrimaryKeyValue) == null && date >= (DateTime)eo.GetProperty("DateFrom") && ((DateTime)eo.GetProperty("DateTo")).Year == 1799)
                {
                    eolTemp.Add(eo);
                }
                if (eoItem != null && (int)eoItem.GetProperty("TollItemTypeID") == 3 && eolTemp.FindFirst("TollItemID={0}", eoItem.PrimaryKeyValue) == null && ((DateTime)eo.GetProperty("DateFrom")).Year == 1799 && ((DateTime)eo.GetProperty("DateTo")).Year == 1799)
                {
                    eolTemp.Add(eo);
                }
            }
            Hashtable ht = new Hashtable(); 
            foreach (EntityObject eo in eolTemp)
            {
                if (!_PixPrice)
                {
                    if ((decimal)eo.GetProperty("FloorX") > 0 && (decimal)eo.GetProperty("SpecX") > 0)
                    {
                        decimal d = (decimal)eo.GetProperty("BasePrice") * (decimal)eo.GetProperty("FloorX") * (decimal)eo.GetProperty("SpecX") * dArea;
                        decimal d1 = System.Math.Round((decimal)eo.GetProperty("BasePrice") * (decimal)eo.GetProperty("FloorX") * (decimal)eo.GetProperty("SpecX") * dArea, 0);
                        if (d - d1 > 0m)
                        {
                            d1 = d1 + 1m;
                        }
                        ht[eo.GetProperty("TollItemID")] = d1;
                    }
                    else
                        ht[eo.GetProperty("TollItemID")] = (decimal)eo.GetProperty("BasePrice") * dArea;
                }
                else
                    ht[eo.GetProperty("TollItemID")] = (decimal)eo.GetProperty("BasePrice");
            }
            return ht;
        }
        static public decimal GetBasePrice(ObjectContext objcontext, int HouseID, DateTime date, decimal dArea, int TollItemID)
        {
            bool _PixPrice = isPixPrice(objcontext);
            EntityObjectFactory eof = EntityObjectFactory.GetInstance(objcontext, Kf.Util.EntityIDEnum.HousePrice);
            EntityObjectList eol = eof.Find("HouseID={0}", HouseID);
            eol.Sort("PriceDate", SortDirection.Descending);
            if (TollItemID > 0 && eof.EntityMap.DataEntity.GetDataEntityCol("TollItemID") != null)
            {
                EntityObjectList eol2 = eol.Find("TollItemID={0}", TollItemID);
                if (eol2.Count > 0)
                {                    
                    eol = eol2;
                }
            }
            foreach (EntityObject eo in eol)
            {
                if (date >= (DateTime)eo.GetProperty("DateFrom") && date <= (DateTime)eo.GetProperty("DateTo"))
                {
                    if (!_PixPrice)
                    {
                        if ((decimal)eo.GetProperty("FloorX") > 0 && (decimal)eo.GetProperty("SpecX") > 0)
                        {
                            decimal d = (decimal)eo.GetProperty("BasePrice") * (decimal)eo.GetProperty("FloorX") * (decimal)eo.GetProperty("SpecX") * dArea;
                            decimal d1 = System.Math.Round((decimal)eo.GetProperty("BasePrice") * (decimal)eo.GetProperty("FloorX") * (decimal)eo.GetProperty("SpecX") * dArea, 0);
                            if (d - d1 > 0m)
                            {
                                d1 = d1 + 1m;
                            }
                            return d1;
                        }
                        else
                            return (decimal)eo.GetProperty("BasePrice") * dArea;
                    }
                    else
                        return (decimal)eo.GetProperty("BasePrice");
                }
                if (((DateTime)eo.GetProperty("DateFrom")).Year == 1799 && date <= (DateTime)eo.GetProperty("DateTo"))
                {
                    if (!_PixPrice)
                    {
                        if ((decimal)eo.GetProperty("FloorX") > 0 && (decimal)eo.GetProperty("SpecX") > 0)
                        {
                            decimal d = (decimal)eo.GetProperty("BasePrice") * (decimal)eo.GetProperty("FloorX") * (decimal)eo.GetProperty("SpecX") * dArea;
                            decimal d1 = System.Math.Round((decimal)eo.GetProperty("BasePrice") * (decimal)eo.GetProperty("FloorX") * (decimal)eo.GetProperty("SpecX") * dArea, 0);
                            if (d - d1 > 0m)
                            {
                                d1 = d1 + 1m;
                            }
                            return d1;
                        }
                        else
                            return (decimal)eo.GetProperty("BasePrice") * dArea;
                    }
                    else
                        return (decimal)eo.GetProperty("BasePrice");
                }
                if (date >= (DateTime)eo.GetProperty("DateFrom") && ((DateTime)eo.GetProperty("DateTo")).Year == 1799)
                {
                    if (!_PixPrice)
                    {
                        if ((decimal)eo.GetProperty("FloorX") > 0 && (decimal)eo.GetProperty("SpecX") > 0)
                        {
                            decimal d = (decimal)eo.GetProperty("BasePrice") * (decimal)eo.GetProperty("FloorX") * (decimal)eo.GetProperty("SpecX") * dArea;
                            decimal d1 = System.Math.Round((decimal)eo.GetProperty("BasePrice") * (decimal)eo.GetProperty("FloorX") * (decimal)eo.GetProperty("SpecX") * dArea, 0);
                            if (d - d1 > 0m)
                            {
                                d1 = d1 + 1m;
                            }
                            return d1;
                        }
                        else
                            return (decimal)eo.GetProperty("BasePrice") * dArea;
                    }
                    else
                    {
                        return (decimal)eo.GetProperty("BasePrice");
                    }
                }
                if (((DateTime)eo.GetProperty("DateFrom")).Year == 1799 && ((DateTime)eo.GetProperty("DateTo")).Year == 1799)
                {
                    if (!_PixPrice)
                    {
                        if ((decimal)eo.GetProperty("FloorX") > 0 && (decimal)eo.GetProperty("SpecX") > 0)
                        {
                            decimal d = (decimal)eo.GetProperty("BasePrice") * (decimal)eo.GetProperty("FloorX") * (decimal)eo.GetProperty("SpecX") * dArea;
                            decimal d1 = System.Math.Round((decimal)eo.GetProperty("BasePrice") * (decimal)eo.GetProperty("FloorX") * (decimal)eo.GetProperty("SpecX") * dArea, 0);
                            if (d - d1 >= 0.1m)
                            {
                                d1 = d1 + 1m;
                            }
                            return d1;
                        }
                        else
                            return (decimal)eo.GetProperty("BasePrice") * dArea;
                    }
                    else
                        return (decimal)eo.GetProperty("BasePrice");
                }


            }
            return 0;
        }
        static public string GetChildOrgStr(ObjectContext objContext, object OrgID)
        {
            string strOrg = "(";
            EntityObjectList eol = GetLevelEntity(Kf.Util.EntityIDEnum.Organization, (int)OrgID, true);
            int i = 0;
            foreach (EntityObject eo in eol)
            {
                if (i > 0)
                    strOrg += ",";

                i++;
                strOrg += eo.PrimaryKeyValue.ToString();
            }
            strOrg += ")";
            return strOrg;




            return strOrg;

        }

        static public ArrayList GetChildOrgList(ObjectContext objContext, object OrgID)
        {
            ArrayList arr = new ArrayList();
            EntityObjectList eol = GetLevelEntity(Kf.Util.EntityIDEnum.Organization, (int)OrgID, true);
          
            foreach (EntityObject eo in eol)
            {
                arr.Add(eo.PrimaryKeyValue);
            }
            return arr;

            
        }

        public static List<int> GetAllComInterface()
        {
            List<int> list = new List<int>();
            RegistryKey pregkey = Registry.LocalMachine.OpenSubKey("HARDWARE\\DEVICEMAP\\SERIALCOMM\\");
            string[] nameList = pregkey.GetValueNames();
            string value;
            Regex regex = new Regex(@"(\d+)");
            foreach (string name in nameList)
            {
                value = Convert.ToString(pregkey.GetValue(name, ""));
                if (!string.IsNullOrEmpty(value))
                {
                    Match match = regex.Match(value);
                    if (match.Success)
                    {
                        list.Add(Convert.ToInt32(match.Groups[1].Value));
                    }
                }
            }
            pregkey.Close();

            return list;
        }

        public static void WriteTaxLog(string msg)
        {
            //return; 
            System.IO.StreamWriter sw = new System.IO.StreamWriter(AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "\\Tax.log", true);
            try
            {
                sw.WriteLine(msg);
                sw.Flush();
            }
            catch { }
            finally
            {
                sw.Close();
            }
        }
        /*
        public static bool GetTaxControlMachineInfo(Form frm, ref string sMachineCode)
        {
            BaseFunc.WriteTaxLog("GetTaxControlMachineInfo");
            switch (GetTaxControlMachineType(frm))
            {
                case TaxMachineType.FISCAL:
                    return GetFTaxControlMachineInfo(frm, ref sMachineCode);
                case TaxMachineType.ZHONGDING:
                    return GetZTaxControlMachineInfo(ref sMachineCode);
                case TaxMachineType.None:
                    Kf.Util.UI.MsgBox.Show("没找到税控设备.");
                    break;
                default:
                    break;
            }
            return false;
        }

        private static bool GetZTaxControlMachineInfo(ref string sMachineCode)
        {
            string str = Kf.Util.Constants.GetConfigData("ZMachineNumber");
            sMachineCode = str;
            return true;
        }

        public static System.Collections.ArrayList GetInvoiceTaxData(Kf.Core.INativeQuery query, string strSQL, Kf.Core.ObjectContext objContext, int EntityID, int iType)
        {
            Kf.Core.NativeQueryCommand cmd = new NativeQueryCommand();
            cmd.CommandText = strSQL;
            System.Data.DataTable dt = query.GetDataTable(objContext.ConnectionString, cmd);
            System.Collections.ArrayList arData = new System.Collections.ArrayList();
            System.Collections.Hashtable ht = new System.Collections.Hashtable();

            int oldID = 0;
            int oldCustomerID = 0;
            int oldHouseID = 0;
            decimal Poundage = 0m;
            DateTime oldMinDate = new DateTime(1799, 1, 2);
            DateTime oldMaxDate = new DateTime(1799, 1, 2);
            int i = 0;
            decimal sumAmount = 0m;
            System.Collections.ArrayList arDetailData = new System.Collections.ArrayList();
            foreach (DataRow row in dt.Rows)
            {
                if ((iType == 1 && oldID != 0 && oldID != (int)row["ID"]) || (iType == 0 &&  ((oldID != 0 && oldID != (int)row["ID"]) || (oldHouseID != 0 && oldHouseID != (int)row["HouseID"]))) || (iType == 2 && ((oldID != 0 && oldID != (int)row["ID"]) || ( oldMaxDate.Year != 1799 && oldMaxDate.Year * 100 + oldMaxDate.Month != ((DateTime)row["EndDate"]).Year * 100 + ((DateTime)row["EndDate"]).Month))))
                {
                    ht["Amount"] = sumAmount;
                    Kf.Core.EntityObject eo = Kf.Core.EntityObjectFactory.GetInstance(objContext, Kf.Util.EntityIDEnum.Customer).FindObject(oldCustomerID);
                    ht["Name"] = "";
                    if (eo != null)
                    {
                        ht["Name"] = eo.GetProperty("Name");
                        if (oldHouseID != 0)
                        {
                            Kf.Core.EntityObject Houseeo = Kf.Core.EntityObjectFactory.GetInstance(objContext, Kf.Util.EntityIDEnum.House).FindObject(oldHouseID);
                            if (Houseeo != null)
                            {
                                ht["Name"] += "(" + Houseeo.GetRelatedObject("BuildID").GetRelatedObject("CommunityID").GetProperty("Name").ToString() + " " + Houseeo.GetRelatedObject("BuildID").GetProperty("Name").ToString() + "-" + Houseeo.GetProperty("Number").ToString() + ",";
                                if (oldMinDate.Year == oldMaxDate.Year && oldMaxDate.Month == oldMinDate.Month)
                                {
                                    ht["Name"] += oldMinDate.Year.ToString() + "." + oldMinDate.Month.ToString() + ")";
                                }
                                else
                                {
                                    ht["Name"] += oldMinDate.Year.ToString() + "." + oldMinDate.Month.ToString() + "-" + oldMaxDate.Year.ToString() + "." + oldMaxDate.Month.ToString() + ")";
                                }
                            }
                        }
                    }
                    ht["ID"] = oldID;
                    ht["EntityID"] = EntityID;
                    ht["CustomerID"] = oldCustomerID;
                    ht["HouseID"] = oldHouseID;
                    if (iType == 2)
                    {
                        ht["Year"] = oldMaxDate.Year;
                        ht["Month"] = oldMaxDate.Month;
                    }
                    else
                    {
                        ht["Year"] = 0;
                        ht["Month"] = 0;
                    }
                    ht["Poundage"] = Poundage;
                    ht["Detail"] = CombineDetailData(arDetailData);
                    arData.Add(ht);
                    ht = new System.Collections.Hashtable();
                    arDetailData = new System.Collections.ArrayList();
                    i = 0;
                    oldHouseID = 0;
                    sumAmount = 0m;
                    Poundage = 0m;
                }
                sumAmount += (decimal)row["Amount"];
                System.Collections.Hashtable ht1 = new System.Collections.Hashtable();
                ht1["jysm"] = row["ItemName"].ToString();
                ht1["sfsm"] = row["Name"].ToString();
                ht1["Money"] = (decimal)row["Amount"];
                ht1["TaxIndex"] = (string)row["TaxIndex"];
                ht1["TaxNumber"] = (string)row["TaxNumber"];
                arDetailData.Add(ht1);
                oldID = (int)row["ID"];
                if ((int)row["HouseID"] > 0)
                {
                    oldCustomerID = (int)row["CustomerID"];
                    oldHouseID = (int)row["HouseID"];
                    oldMinDate = (DateTime)row["BeginDate"];
                    oldMaxDate = (DateTime)row["EndDate"];
                }
                if ((int)row["IsPoundage"] > 0)
                {
                    Poundage = (decimal)row["Amount"];
                }
                i++;
            }
            if (sumAmount > 0)
            {
                ht["Amount"] = sumAmount;
                Kf.Core.EntityObject eo = Kf.Core.EntityObjectFactory.GetInstance(objContext, Kf.Util.EntityIDEnum.Customer).FindObject(oldCustomerID);
                ht["Name"] = "";
                if (eo != null)
                {
                    ht["Name"] = eo.GetProperty("Name");
                    if (oldHouseID != 0)
                    {
                        Kf.Core.EntityObject Houseeo = Kf.Core.EntityObjectFactory.GetInstance(objContext, Kf.Util.EntityIDEnum.House).FindObject(oldHouseID);
                        if (Houseeo != null)
                        {
                            ht["Name"] += "(" + Houseeo.GetRelatedObject("BuildID").GetRelatedObject("CommunityID").GetProperty("Name").ToString() + " " + Houseeo.GetRelatedObject("BuildID").GetProperty("Name").ToString() + "-" + Houseeo.GetProperty("Number").ToString() + ",";
                            if (oldMinDate.Year == oldMaxDate.Year && oldMaxDate.Month == oldMinDate.Month)
                            {
                                ht["Name"] += oldMinDate.Year.ToString() + "." + oldMinDate.Month.ToString() + ")";
                            }
                            else
                            {
                                ht["Name"] += oldMinDate.Year.ToString() + "." + oldMinDate.Month.ToString() + "-" + oldMaxDate.Year.ToString() + "." + oldMaxDate.Month.ToString() + ")";
                            }
                        }
                    }
                }
                ht["ID"] = oldID;
                ht["EntityID"] = EntityID;
                ht["CustomerID"] = oldCustomerID;
                ht["HouseID"] = oldHouseID;
                if (iType == 2)
                {
                    ht["Year"] = oldMaxDate.Year;
                    ht["Month"] = oldMaxDate.Month;
                }
                else
                {
                    ht["Year"] = 0;
                    ht["Month"] = 0;
                }
                ht["Poundage"] = Poundage;
                ht["Detail"] = CombineDetailData(arDetailData);
                arData.Add(ht);
            }
            return arData;
        }

        private static ArrayList CombineDetailData(ArrayList detailData)
        {
            //return detailData;
            string taxIndex = String.Empty;
            Decimal amount = 0m;
            ArrayList newDetails = new ArrayList();
            System.Collections.Hashtable item;
            System.Collections.Hashtable preItem;
            System.Collections.Hashtable temp;
            string exp = string.Empty;
            string sfsm = string.Empty;
            for (int i = 0; i < detailData.Count; i++)
            {
                item = detailData[i] as Hashtable;
                if (taxIndex != String.Empty && taxIndex != item["TaxIndex"].ToString())
                {
                    preItem = detailData[i - 1] as Hashtable;
                    temp = new System.Collections.Hashtable();
                    temp["jysm"] = preItem["jysm"];
                    temp["sfsm"] = exp;
                    temp["Money"] = amount;
                    temp["TaxIndex"] = preItem["TaxIndex"];
                    temp["TaxNumber"] = preItem["TaxNumber"];

                    newDetails.Add(temp);
                    amount = 0m;
                    exp = string.Empty;
                    sfsm = string.Empty;
                }
                if (sfsm == string.Empty)
                {
                    sfsm = item["sfsm"].ToString();
                }
                else if (sfsm.IndexOf(item["sfsm"].ToString()) < 0)
                {
                    sfsm += "/" + item["sfsm"].ToString();
                }
                exp += item["sfsm"] + ":" + System.Math.Round((decimal)item["Money"], 2).ToString() + " ";
                amount += Convert.ToDecimal(item["Money"]);
                taxIndex = item["TaxIndex"].ToString();
            }

            if (amount > 0m)
            {
                preItem = detailData[detailData.Count - 1] as Hashtable;
                temp = new System.Collections.Hashtable();
                temp["jysm"] = preItem["jysm"];
                temp["sfsm"] = exp;
                temp["Money"] = amount;
                temp["TaxIndex"] = preItem["TaxIndex"];
                temp["TaxNumber"] = preItem["TaxNumber"];

                newDetails.Add(temp);
            }

            return newDetails;
        }


        /// <summary>
        /// 查询数据集合，并构建发票
        /// </summary>
        /// <param name="query">插叙对象</param>
        /// <param name="strSQL">SQL语句</param>
        /// <param name="objContext">数据对象</param>
        /// <param name="EntityID">实体ID</param>
        /// <param name="iType">iType只取1</param>
        /// <returns></returns>
        /// written by linewei 20110504
        public static List<ArrayList> GetInvoiceTaxDataByOut(Kf.Core.INativeQuery query, string strSQL, Kf.Core.ObjectContext objContext, int EntityID, int iType)
        {
            Kf.Core.NativeQueryCommand cmd = new NativeQueryCommand();
            cmd.CommandText = strSQL;
            System.Data.DataTable dt = query.GetDataTable(objContext.ConnectionString, cmd);
            System.Collections.ArrayList arData = new System.Collections.ArrayList();
            List<ArrayList> arDatas = new List<ArrayList>();
            System.Collections.Hashtable ht = new System.Collections.Hashtable();

            int oldID = 0;
            int oldCustomerID = 0;
            string oldCustomerName = "";
            string oldTaxID = "";
            string oldTaxName = "";
            string oldTaxNumber = "";
            string oldCuType = "";
            string oldMobile = "";
            string oldPersonID = "";
            string oldDescription = "";
            int oldHouseID = 0;
            decimal Poundage = 0m;
            DateTime oldMinDate = new DateTime(1799, 1, 2);
            DateTime oldMaxDate = new DateTime(1799, 1, 2);
            decimal sumAmount = 0m;
            System.Collections.ArrayList arDetailData = new System.Collections.ArrayList();
            int flag = 0;
            decimal eachAmount = 0m;
            foreach (DataRow row in dt.Rows)
            {
                //如果当前记录的客户ID或开票项目不同于前一条，则表示前面采集的数据集合为一张发票。
                if (iType == 1 && oldCustomerID != 0 && (oldCustomerID != (int)row["CustomerID"] || oldCustomerName != (string)row["CustomerName"] || oldTaxID != row["TaxID"].ToString()))
                {
                    //将采集的数据记录分组汇总
                    while (true)
                    {
                        
                        ht["Name"] = oldCustomerName;
                        ht["ID"] = oldID;
                        ht["EntityID"] = EntityID;
                        ht["CustomerID"] = oldCustomerID;
                        ht["Description"] = oldDescription;
                        ht["HouseID"] = oldHouseID;
                        ht["TaxName"] = oldTaxName;
                        ht["TaxNumber"] = oldTaxNumber;
                        ht["Mobile"] = oldMobile;
                        ht["PersonID"] = "";// oldPersonID;
                        ht["CuType"] = oldCuType;

                        if (iType == 2)
                        {
                            ht["Year"] = oldMaxDate.Year;
                            ht["Month"] = oldMaxDate.Month;
                        }
                        else
                        {
                            ht["Year"] = 0;
                            ht["Month"] = 0;
                        }
                        ht["Poundage"] = Poundage;
                        ht["Detail"] = CombineDetailDataByOut(arDetailData, ref eachAmount, ref flag);//汇总发票明细
                        ht["Amount"] = System.Decimal.Round(eachAmount, 2);
                        eachAmount = 0m;
                        arData.Add(ht);
                        ht = new System.Collections.Hashtable();
                        if (flag < arDetailData.Count)
                            continue;
                        
                        arDetailData = new System.Collections.ArrayList();
                        oldHouseID = 0;
                        sumAmount = 0m;
                        Poundage = 0m;
                        flag = 0;
                        break;
                    }
                    //如果开票项目已改，则分组已保存下来的开票记录，每个分组都作为一个EXCEL文件导出 20110512
                    if (oldTaxID != row["TaxID"].ToString())
                    {
                    ArrayList newData = new ArrayList();
                        newData.AddRange(arData);
                        arDatas.Add(newData);
                        arData.Clear();
                    }
                }
                
                sumAmount += (decimal)row["Amount"];
                System.Collections.Hashtable ht1 = new System.Collections.Hashtable();
                ht1["jysm"] = row["Name"].ToString();
                ht1["tddDesc"] = row["tddDesc"].ToString();
                ht1["sfsm"] = row["Name"].ToString();
                ht1["CalcStartDate"] = row["CalcStartDate"].ToString();
                ht1["CalcEndDate"] = row["CalcEndDate"].ToString();
                ht1["LastReadCount"] = System.Decimal.Round(Convert.ToDecimal(row["LastReadCount"]), 2);
                ht1["CurrentReadCount"] = System.Decimal.Round(Convert.ToDecimal(row["CurrentReadCount"]), 2);
                ht1["totalCount"] = System.Decimal.Round(Convert.ToDecimal(row["totalCount"]), 2); 
                ht1["Money"] = System.Decimal.Round(Convert.ToDecimal(row["Amount"]), 2);
                arDetailData.Add(ht1);
                oldID = (int)row["ID"];
                oldCustomerID = (int)row["CustomerID"];
                oldCustomerName = row["CustomerName"].ToString();
                oldTaxID = row["TaxID"].ToString();
                oldTaxName = row["TaxName"].ToString();
                oldTaxNumber = row["TaxNumber"].ToString();
                oldCuType = row["CuType"].ToString();
                oldMobile = row["Mobile"].ToString();
                oldPersonID = row["PersonID"].ToString();
                oldDescription = row["Description"].ToString();
                if ((int)row["HouseID"] > 0)
                {
                    oldHouseID = (int)row["HouseID"];
                    oldMinDate = (DateTime)row["BeginDate"];
                    oldMaxDate = (DateTime)row["EndDate"];
                }
                if ((int)row["IsPoundage"] > 0)
                {
                    Poundage = System.Decimal.Round((decimal)row["Amount"], 2);
                }
            }
            if (sumAmount > 0)
            {
                while (true)
                {
                    ht["Name"] = oldCustomerName;

                    ht["ID"] = oldID;
                    ht["EntityID"] = EntityID;
                    ht["CustomerID"] = oldCustomerID;
                    ht["HouseID"] = oldHouseID;
                    ht["TaxName"] = oldTaxName;
                    ht["TaxNumber"] = oldTaxNumber;
                    ht["Mobile"] = oldMobile;
                    ht["PersonID"] = "";// oldPersonID;
                    ht["CuType"] = oldCuType;
                    ht["Description"] = oldDescription;
                    if (iType == 2)
                    {
                        ht["Year"] = oldMaxDate.Year;
                        ht["Month"] = oldMaxDate.Month;
                    }
                    else
                    {
                        ht["Year"] = 0;
                        ht["Month"] = 0;
                    }
                    ht["Poundage"] = Poundage;
                    ht["Detail"] = CombineDetailDataByOut(arDetailData, ref eachAmount, ref flag);
                    ht["Amount"] = System.Decimal.Round(eachAmount, 2);
                    eachAmount = 0m;
                    arData.Add(ht);
                    ht = new System.Collections.Hashtable();
                    if (flag < arDetailData.Count)
                        continue;
                    break;
                }
                //如果开票项目已改，则分组已保存下来的开票记录，每个分组都作为一个EXCEL文件导出 20110512

                ArrayList newData = new ArrayList();
                newData.AddRange(arData);
                arDatas.Add(newData);
                arData.Clear();
                
            }
            return arDatas;
        }

        /// <summary>
        /// 汇总发票明细
        /// </summary>
        /// <param name="detailData">待汇总的数据集合</param>
        /// <param name="eachAmount"></param>
        /// <param name="flag"></param>
        /// <returns></returns>
        /// written by linewei 20110509
        private static ArrayList CombineDetailDataByOut(ArrayList detailData, ref decimal eachAmount, ref int flag)
        {
            string ItemName = String.Empty;
            Decimal amount = 0m;
            ArrayList newDetails = new ArrayList();
            System.Collections.Hashtable item;
            System.Collections.Hashtable preItem;
            System.Collections.Hashtable temp;
            string exp = string.Empty;
            string sfsm = string.Empty;
            List<DateTime> listStart = new List<DateTime>();
            List<DateTime> listEnd = new List<DateTime>();
            Decimal totalCount = 0m;
            Decimal LastReadCount = 0m;
            Decimal CurrentReadCount = 0m;
            DateTime MinReadDate = DateTime.Today;
            DateTime MaxReadDate = Kf.Core.Toolkit.GetDateTimeDefaultValue();
            for (; flag < detailData.Count; flag++)
            {
                item = detailData[flag] as Hashtable;

                if (ItemName != String.Empty && ItemName != item["sfsm"].ToString())
                {
                    preItem = detailData[flag - 1] as Hashtable;
                    temp = new System.Collections.Hashtable();
                    string jysm = "";
                    if (totalCount > 0m)
                    {
                       // jysm = string.Format("{0}从{1}到{2}期间内上次读数：{3},本次读数：{4},总行度：{5}",preItem["sfsm"].ToString(), MinReadDate.ToString("yyyy-MM-dd"), MaxReadDate.ToString("yyyy-MM-dd"), LastReadCount, CurrentReadCount, totalCount);
                        if (LastReadCount != 0 && CurrentReadCount != 0)
                        {
                            jysm = string.Format("{0}{1}-{2}({3}-{4})", preItem["sfsm"].ToString(), MinReadDate.ToString("yyyyMMdd"), MaxReadDate.ToString("yyyyMMdd"), LastReadCount, CurrentReadCount);
                        }
                        else
                        {
                            jysm = string.Format("{0}{1}-{2}({3}-{4})", preItem["sfsm"].ToString(), MinReadDate.ToString("yyyyMMdd"), MaxReadDate.ToString("yyyyMMdd"), LastReadCount, CurrentReadCount);
                        }
                    }
                    else
                    {
                        jysm=GetMonthByDates(listStart, listEnd) + preItem["sfsm"].ToString();
                    }
                    if (jysm.Length > 110)
                        jysm = jysm.Substring(109);
                    temp["jysm"] = jysm;
                    temp["sfsm"] = preItem["sfsm"];
                    temp["tddDesc"] = preItem["tddDesc"];
                    temp["Money"] = amount;
                    eachAmount += amount;

                    newDetails.Add(temp);

                    amount = 0m;
                    totalCount = 0m;
                    LastReadCount = 0m;
                    CurrentReadCount = 0m;
                    MinReadDate = DateTime.Today;
                    MaxReadDate = Kf.Core.Toolkit.GetDateTimeDefaultValue();
                    exp = string.Empty;
                    sfsm = string.Empty;
                    listStart.Clear();
                    listEnd.Clear();

                    if (newDetails.Count == 5)//每一张发票最多只能有5条明细
                        return newDetails;
                }

                listStart.Add(DateTime.Parse(item["CalcStartDate"].ToString()));
                listEnd.Add(DateTime.Parse(item["CalcEndDate"].ToString()));
                amount += Convert.ToDecimal(item["Money"]);
                totalCount += Convert.ToDecimal(item["totalCount"]);
                if (Convert.ToDecimal(item["LastReadCount"]) < LastReadCount)
                {
                    LastReadCount = Convert.ToDecimal(item["LastReadCount"]);
                }
                if (Convert.ToDecimal(item["CurrentReadCount"]) > CurrentReadCount)
                {
                    CurrentReadCount = Convert.ToDecimal(item["CurrentReadCount"]);
                }
                if (DateTime.Parse(item["CalcStartDate"].ToString()) < MinReadDate)
                {
                    MinReadDate = DateTime.Parse(item["CalcStartDate"].ToString());
                }
                if (DateTime.Parse(item["CalcEndDate"].ToString()) > MaxReadDate)
                {
                    MaxReadDate = DateTime.Parse(item["CalcEndDate"].ToString());
                }
                ItemName = item["sfsm"].ToString();
            }

            if (amount > 0m)
            {
                preItem = detailData[detailData.Count - 1] as Hashtable;
                temp = new System.Collections.Hashtable();
                string jysm = "";
                if (totalCount > 0m)
                {
                    //jysm = string.Format("{0}从{1}到{2}期间内上次读数：{3},本次读数：{4},总行度：{5}", preItem["sfsm"].ToString(), MinReadDate.ToString("yyyy-MM-dd"), MaxReadDate.ToString("yyyy-MM-dd"), LastReadCount, CurrentReadCount,  totalCount);
                    if (LastReadCount != 0 && CurrentReadCount != 0)
                    {
                        jysm = string.Format("{0}{1}-{2}({3}-{4})", preItem["sfsm"].ToString(), MinReadDate.ToString("yyyyMMdd"), MaxReadDate.ToString("yyyyMMdd"), LastReadCount, CurrentReadCount);
                    }
                    else
                    {
                        jysm = string.Format("{0}{1}-{2}({3}-{4})", preItem["sfsm"].ToString(), MinReadDate.ToString("yyyyMMdd"), MaxReadDate.ToString("yyyyMMdd"), LastReadCount, CurrentReadCount);
                    }
                }
                else
                {
                    jysm = GetMonthByDates(listStart, listEnd) + preItem["sfsm"].ToString();
                }
                
                if (jysm.Length > 110)//开票项目说明最大只能110个字符
                    jysm = jysm.Substring(109);
                temp["jysm"] = jysm;
                temp["sfsm"] = preItem["sfsm"];
                temp["tddDesc"] = preItem["tddDesc"];
                temp["Money"] = amount;
                eachAmount += amount;
                newDetails.Add(temp);
            }

            return newDetails;
        }

        /// <summary>
        /// 组合费用单中的日期集合
        /// </summary>
        /// <param name="listStart">开始日期集合（开始日期和结束日期集合中的元素形成一对一关系）</param>
        /// <param name="listEnd">结束日期集合</param>
        /// <returns></returns>
        /// written by linewei 20110509
        private static string GetMonthByDates(List<DateTime> listStart, List<DateTime> listEnd)
        {
            if (listStart.Count != listEnd.Count)
                return string.Empty;
            List<DateTime> newStart = new List<DateTime>();
            List<DateTime> newEnd = new List<DateTime>();
            for (int i = 0; i < listStart.Count; i++)
            {
                if (listStart[i].Year == 1799 || listEnd[i].Year == 1799)
                    continue;
                //消除日期集合中每对日期重叠的区间。
                if (newStart.Count <= 0)
                {
                    newStart.Add(listStart[i]);
                    newEnd.Add(listEnd[i]);
                }
                else
                {
                    int j = 0;
                    for (j = 0; j < newStart.Count; j++)
                    {
                        if ((listStart[i].Year * 100 + listStart[i].Month <= newEnd[j].Year * 100 + newEnd[j].Month)
                            ||(listStart[i].Year-newEnd[j].Year==1 && listStart[i].Month==1 && newEnd[j].Month==12) 
                            || (listStart[i].Year==newEnd[j].Year && listStart[i].Month-newEnd[j].Month==1))
                        {
                            if (listEnd[i].Year * 100 + listEnd[i].Month > newEnd[j].Year * 100 + newEnd[j].Month)
                                newEnd[j] = listEnd[i];
                            break;
                        }
                    }
                    if (j == newStart.Count)
                    {
                        newStart.Add(listStart[i]);
                        newEnd.Add(listEnd[i]);
                    }
                }
            }

            StringBuilder sb = new StringBuilder();
            DateTime dtStart, dtEnd;

            //字符化日期
            for (int m = 0; m < newStart.Count; m++)
            {
                dtStart = newStart[m];
                dtEnd = newEnd[m];
                if (dtStart.Year == dtEnd.Year)
                {
                    if (dtEnd.Month - dtStart.Month > 1)
                        sb.AppendFormat("{0}年{1}-{2}月", dtStart.Year, dtStart.Month, dtEnd.Month);
                    else if (dtEnd.Month - dtStart.Month == 1)
                        sb.AppendFormat("{0}年{1}、{2}月", dtStart.Year, dtStart.Month, dtEnd.Month);
                    else
                        sb.AppendFormat("{0}年{1}月", dtStart.Year, dtStart.Month);
                }
                else
                {
                    for (int i = dtStart.Year; i <= dtEnd.Year; i++)
                    {
                        if (i == dtStart.Year)
                        {
                            if (dtStart.Month < 11)
                                sb.AppendFormat("{0}年{1}-{2}月", i, dtStart.Month, 12);
                            else if (dtStart.Month == 11)
                                sb.AppendFormat("{0}年{1}、{2}月", i, dtStart.Month, 12);
                            else
                                sb.AppendFormat("{0}年{1}月", i, dtStart.Month);
                        }
                        else if (i == dtEnd.Year)
                        {
                            if (dtEnd.Month > 2)
                                sb.AppendFormat("{0}年{1}-{2}月", i, 1, dtEnd.Month);
                            else if (dtStart.Month == 2)
                                sb.AppendFormat("{0}年{1}、{2}月", i, 1, dtEnd.Month);
                            else
                                sb.AppendFormat("{0}年{1}月", i, dtEnd.Month);
                        }
                        else
                        {
                            sb.AppendFormat("{0}年{1}-{2}月", i, 1, 12);
                        }
                        sb.Append(",");
                    }
                    sb.Remove(sb.Length - 1, 1);
                }
                sb.Append(",");
            }
            if(sb.Length > 0)
                sb.Remove(sb.Length - 1, 1);
            return sb.ToString();
        }
        private static string _Noteinfotip = "-1";
        public static string getNoteInfoTip()
        {
            try
            {
                if (_Noteinfotip == "-1")
                {
                    _Noteinfotip = "";
                    string fileName = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "OnLineUpdateConfig.xml";
                    if (System.IO.File.Exists(fileName))
                    {
                        System.Xml.XmlDocument xd = new System.Xml.XmlDocument();
                        xd.Load(fileName);
                        if (xd.SelectSingleNode("/root/data/NoteInfoTip") != null)
                        {
                            _Noteinfotip = xd.SelectSingleNode("/root/data/NoteInfoTip").InnerText;
                        }
                    }
                }
            }
            catch
            {
                _Noteinfotip = "";
            }
            return _Noteinfotip;
        }
        private static string _Smsinfotip = "-1";
        public static string getSmsInfoTip()
        {
            try
            {
                if (_Smsinfotip == "-1")
                {
                    _Smsinfotip = "";
                    string fileName = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "OnLineUpdateConfig.xml";
                    if (System.IO.File.Exists(fileName))
                    {
                        System.Xml.XmlDocument xd = new System.Xml.XmlDocument();
                        xd.Load(fileName);
                        if (xd.SelectSingleNode("/root/data/SmsInfoTip") != null)
                        {
                            _Smsinfotip = xd.SelectSingleNode("/root/data/SmsInfoTip").InnerText;
                        }
                    }
                }
            }
            catch
            {
                _Smsinfotip = "";
            }
            return _Smsinfotip;
        }
        public static int isSupportTaxInvoice(ObjectContext objctx)
        {
            if (_IsSupportTaxInvocie == 0)
            {
                object eosys = Kf.BllObj.SystemProfile.GetSystemProfileValueByID(objctx, 221);
                if (eosys != null)
                {
                    if (Convert.ToInt32(eosys.ToString()) == 0)
                        _IsSupportTaxInvocie = 3;
                    else
                        _IsSupportTaxInvocie = Convert.ToInt32(eosys.ToString());
                }
            }
            return _IsSupportTaxInvocie;
        }
        public static void ShowInoviceForm(Form MainUI, System.Collections.ArrayList UserData)
        {
            if (UserData == null || UserData.Count == 0)
            {
                Kf.Util.UI.MsgBox.Warn("没有需要开发票的金额，请检查是否已对收费项目设定税目");
                return;
            }
            Kf.Entity.Base.JeezUIMainContainerFactory eof = new Kf.Entity.Base.JeezUIMainContainerFactory(Kf.Login.Environment.objContext.Clone());
            Kf.Entity.Base.JeezUIMainContainer dataEntityObject = eof.FindObject(2271121);
            Jeez.Runtime.Base.BaseForm f = Jeez.Runtime.Base.Base.OpenBaseUIByShowDialog(MainUI, Kf.Login.Environment.objContext.Clone(), null, dataEntityObject, null, UserData);
            if (MainUI is Jeez.Runtime.Base.General.BillUI)
            {
                ((Kf.Equipment.Toll.SetInvoice)f).ParentUiMainID = ((Jeez.Runtime.Base.General.BillUI)MainUI).UIMainID;
            }
            else if (((Jeez.Runtime.Base.General.frmList)MainUI).UIMainID == 287)
            {
                ((Kf.Equipment.Toll.SetInvoice)f).ParentUiMainID = 286;
            }
            else if (((Jeez.Runtime.Base.General.frmList)MainUI).UIMainID == 272)
            {
                ((Kf.Equipment.Toll.SetInvoice)f).ParentUiMainID = 271;
            }
            else if (((Jeez.Runtime.Base.General.frmList)MainUI).UIMainID == 425)
            {
                ((Kf.Equipment.Toll.SetInvoice)f).ParentUiMainID = 424;
            }
            if (f != null)
            {
                f.Visible = false;
                f.ShowDialog();
            }
        }
        private static bool GetFTaxControlMachineInfo(Form frm, ref string sMachineCode)
        {
            AxFiscalOcx.AxTFiscal fiscal = GetFiscal(frm);
            try
            {
                string com = Kf.Util.Constants.GetConfigData("FCom");
                if (!string.IsNullOrEmpty(com))
                {
                    if (GetFTaxMachineInfo(fiscal, Convert.ToInt32(com), ref sMachineCode))
                    {
                        return true;
                    }
                }
                List<int> comList = BaseFunc.GetAllComInterface();
                foreach (int item in comList)
                {
                    if (GetFTaxMachineInfo(fiscal, item, ref sMachineCode))
                    {
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                Kf.Util.UI.MsgBox.ShowErrMsg(ex);
            }
            finally
            {
                RemoveFiscal(frm, fiscal);
            }

            return false;
        }

        //private static bool GetFTaxMachineInfo(AxFiscalOcx.AxTFiscal fiscal, int com, ref string sMachineCode)
        //{
        //    string str = Kf.Util.Constants.GetConfigData("FMachineNumber");
        //    if (!string.IsNullOrEmpty(str))
        //    {
        //        sMachineCode = str;
        //        return true;
        //    }

        //    int ret;
        //    if (fiscal == null)
        //    {
        //        return false;
        //    }
        //    try
        //    {
        //        ret = Convert.ToInt32(fiscal.CommOpen(com, 57600, 60));
        //        BaseFunc.WriteTaxLog("fiscal.CommOpen " + ret.ToString());
        //        if (ret == 1)
        //        {
        //            ret = fiscal.LinkMachine();
        //            BaseFunc.WriteTaxLog("fiscal.LinkMachine " + ret.ToString());
        //            if (ret == 0)
        //            {
        //                string sTaxName = String.Empty, sTaxreg = string.Empty;
        //                Kf.Util.Constants.SetConfigData("FCom", com.ToString());
        //                T_rMachineInfo rm = new T_rMachineInfo();

        //                ret = fiscal.GetMachineInfo(ref rm);
        //                BaseFunc.WriteTaxLog("fiscal.GetMachineInfo " + ret.ToString());
        //                if (ret == 0)
        //                {
        //                    Kf.Util.Constants.SetConfigData("FMachineNumber", rm.sMachineNumber);
        //                    return true;
        //                }

        //            }
        //        }
        //    }
        //    finally
        //    {
        //        fiscal.CommClose();
        //    }

        //    return false;
        //}

        public static bool GetCurrentInvoiceNo(Form frm, ref string currNo, ref string currCode, ref string startNo, ref string endNo)
        {
            switch (GetTaxControlMachineType(frm))
            {
                case TaxMachineType.FISCAL:
                    return GetFCurrentInvoiceNo(frm, ref currNo, ref currCode, ref startNo, ref endNo);
                case TaxMachineType.ZHONGDING:
                    return GetZCurrentInvoiceNo(ref currNo, ref currCode, ref startNo, ref endNo);
                case TaxMachineType.None:
                    Kf.Util.UI.MsgBox.Show("没找到税控设备.");
                    break;
                default:
                    break;
            }
            return false;
        }

        private static bool GetZCurrentInvoiceNo(ref string currNo, ref string currCode, ref string startNo, ref string endNo)
        {
            try
            {
                string com = Kf.Util.Constants.GetConfigData("ZCom");
                if (!string.IsNullOrEmpty(com))
                {
                    if (GetZCurrentInvoiceNo(Convert.ToByte(com), ref currNo, ref currCode, ref startNo, ref endNo))
                    {
                        return true;
                    }
                }
                List<int> comList = BaseFunc.GetAllComInterface();
                foreach (int item in comList)
                {
                    if (GetZCurrentInvoiceNo((byte)item, ref currNo, ref currCode, ref startNo, ref endNo))
                    {
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                Kf.Util.UI.MsgBox.ShowErrMsg(ex);
            }
            finally
            {
            }

            return false;
        }

        private static bool GetZCurrentInvoiceNo(byte com, ref string currNo, ref string currCode, ref string startNo, ref string endNo)
        {
            int ret;
            ZTaxControl.TaxControlClass taxControl = new ZTaxControl.TaxControlClass();
            try
            {
                //ret = Jeez.TaxControl.ZTaxControl.InitReader(Convert.ToByte(com));
                ret = taxControl.InitReader1(com);
                BaseFunc.WriteTaxLog("Jeez.TaxControl.ZTaxControl.InitReader " + ret.ToString());
                if (ret == 0)
                {
                    Kf.Util.Constants.SetConfigData("ZCom", com.ToString());
                    //ret = Jeez.TaxControl.ZTaxControl.GetCurrInvNO(ref currNo);
                    ret = taxControl.GetCurrInvNO1(ref currNo);
                    BaseFunc.WriteTaxLog("Jeez.TaxControl.ZTaxControl.GetCurrInvNO " + ret.ToString());
                    if (ret == 0)
                    {
                        return true;
                    }
                }

            }
            finally
            {
                //Jeez.TaxControl.ZTaxControl.CloseReader();
                taxControl.CloseReader1();
            }
            return false;
        }

        private static bool GetFCurrentInvoiceNo(Form frm, ref string currNo, ref string currCode, ref string startNo, ref string endNo)
        {
            AxFiscalOcx.AxTFiscal fiscal = GetFiscal(frm);
            try
            {
                string com = Kf.Util.Constants.GetConfigData("FCom");
                if (!string.IsNullOrEmpty(com))
                {
                    if (GetFCurrentInvoiceNo(fiscal, Convert.ToInt32(com), ref currNo, ref currCode, ref startNo, ref endNo))
                    {
                        return true;
                    }
                }
                List<int> comList = BaseFunc.GetAllComInterface();
                foreach (int item in comList)
                {
                    if (GetFCurrentInvoiceNo(fiscal, item, ref currNo, ref currCode, ref startNo, ref endNo))
                    {
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                Kf.Util.UI.MsgBox.ShowErrMsg(ex);
            }
            finally
            {
                RemoveFiscal(frm, fiscal);
            }

            return false;
        }
        
        private static bool GetFCurrentInvoiceNo(AxFiscalOcx.AxTFiscal fiscal, int com, ref string currNo, ref string currCode, ref string startNo, ref string endNo)
        {
            int ret;
            if (fiscal == null)
            {
                return false;
            }
            try
            {
                ret = Convert.ToInt32(fiscal.CommOpen(com, 57600, 60));
                BaseFunc.WriteTaxLog("fiscal.CommOpen " + ret.ToString());
                if (ret == 1)
                {

                    ret = fiscal.LinkMachine();
                    BaseFunc.WriteTaxLog("fiscal.LinkMachine " + ret.ToString());
                    if (ret == 0)
                    {
                        Kf.Util.Constants.SetConfigData("FCom", com.ToString());

                        ret = fiscal.GetCurrentInvNumber(ref currNo, ref currCode, ref startNo, ref endNo);

                        BaseFunc.WriteTaxLog("fiscal.GetCurrentInvNumber " + ret.ToString());
                        if (ret == 0)
                        {
                            return true;
                        }
                    }

                }
            }
            finally
            {
                fiscal.CommClose();
            }

            return false;
        }

        public static bool MakeInvoice(Form frm, string playName, DataTable details, ref string InvoiceNo, ref string forgeryCode)
        {
            switch (GetTaxControlMachineType(frm))
            {
                case TaxMachineType.FISCAL:
                    return FMakeInvoice(frm, playName, details, ref InvoiceNo, ref forgeryCode);
                case TaxMachineType.ZHONGDING:
                    return ZMakeInvoice(playName, details, ref InvoiceNo, ref forgeryCode);
                case TaxMachineType.None:
                    Kf.Util.UI.MsgBox.Show("没找到税控设备.");
                    break;
                default:
                    break;
            }

            return false;
        }

        private static bool ZMakeInvoice(string playName, DataTable details, ref string InvoiceNo, ref string forgeryCode)
        {
            BaseFunc.WriteTaxLog("ZMakeInvoice");
            try
            {
                string com = Kf.Util.Constants.GetConfigData("ZCom");
                if (!string.IsNullOrEmpty(com))
                {
                    if (ZMakeInvoice(Convert.ToByte(com), playName, details, ref InvoiceNo, ref forgeryCode))
                    {
                        return true;
                    }
                }
                List<int> comList = BaseFunc.GetAllComInterface();
                foreach (int item in comList)
                {
                    if (ZMakeInvoice((byte)item, playName, details, ref InvoiceNo, ref forgeryCode))
                    {
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                Kf.Util.UI.MsgBox.ShowErrMsg(ex);
            }
            finally
            {
            }
            return false;
        }

        private static bool ZMakeInvoice(byte com, string playName, DataTable details, ref string InvoiceNo, ref string forgeryCode)
        {
            int ret;
            ZTaxControl.TaxControlClass taxControl = new ZTaxControl.TaxControlClass();
            ret = taxControl.InitReader1(Convert.ToByte(com));
            //ret = Jeez.TaxControl.ZTaxControl.InitReader(Convert.ToByte(com));
            BaseFunc.WriteTaxLog("Jeez.TaxControl.ZTaxControl.InitReader " + ret.ToString());
            try
            {
                if (ret == 0)
                {
                    DataView dv = new DataView(details);
                    dv.Sort = "TaxIndex asc";
                    {
                        //Jeez.TaxControl.TypeDefine.TRecordIn normlInvInfo = new TRecordIn();
                        ZTaxControl.TRecordIn1 normlInvInfo = new ZTaxControl.TRecordIn1();
                        string s = playName + (playName.Length < 50 ? new String(' ', 50 - playName.ToString().Length) : "");
                        normlInvInfo.PayUserName = System.Text.Encoding.Default.GetString(System.Text.Encoding.Default.GetBytes(s));

                        //Jeez.TaxControl.TypeDefine.TInvoiceDTL [] items = new TInvoiceDTL[4];
                        ZTaxControl.TInvoiceDTL[] items = new ZTaxControl.TInvoiceDTL[4];
                        int ii = 0;
                        int iCount = 0;
                        for (int i = 0; i < 4 && i < dv.Count; i++)
                        {
                            bool bfind = false;

                            for (int j = 0; j < ii; j++)
                            {
                                if (items[j].TaxItemIndex == dv[i]["TaxIndex"].ToString())
                                {
                                    items[j].Money = Convert.ToString((Convert.ToDouble(items[j].Money) + Convert.ToDouble(dv[i]["Money"])));
                                    bfind = true;
                                    break;
                                }
                            }

                            if (!bfind)
                            {
                                iCount++;
                                s = dv[i]["ItemName"].ToString() + (dv[i]["ItemName"].ToString().Length < 20 ? new String(' ', 20 - dv[i]["ItemName"].ToString().Length) : "");
                                items[ii].ItemName = System.Text.Encoding.Default.GetString(System.Text.Encoding.Default.GetBytes(s));
                                items[ii].TaxItemIndex = dv[i]["TaxIndex"].ToString().Trim();
                                items[ii].Money = Convert.ToString(Math.Round(Convert.ToDecimal(dv[i]["Money"]) * 100, 0, MidpointRounding.AwayFromZero));
                                s = dv[i]["Name"].ToString() + (dv[i]["Name"].ToString().Length < 20 ? new String(' ', 20 - dv[i]["Name"].ToString().Length) : "");
                                items[ii].ChargeExp = System.Text.Encoding.Default.GetString(System.Text.Encoding.Default.GetBytes(s));
                                items[ii].TaxItemNO = dv[i]["TaxNumber"].ToString().Trim();
                                ii++;
                            }
                        }
                        ii--;
                        normlInvInfo.ItemNum = iCount.ToString();
                        s = Kf.Login.Environment.UserName + (Kf.Login.Environment.UserName.Length < 50 ? new String(' ', 50 - Kf.Login.Environment.UserName.ToString().Length) : "");
                        normlInvInfo.UserName = System.Text.Encoding.Default.GetString(System.Text.Encoding.Default.GetBytes(s));
                        normlInvInfo.UserDTLNO = "0000";
                        normlInvInfo.InvoiceDTL0 = items[0];
                        normlInvInfo.InvoiceDTL1 = items[1];
                        normlInvInfo.InvoiceDTL2 = items[2];
                        normlInvInfo.InvoiceDTL3 = items[3];
                        //normlInvInfo.InvoiceDtl = items;                        
                        //normlInvInfo.InvoiceNO = InvoiceNo;

                        //Jeez.TaxControl.TypeDefine.TRecordOut outInfo = new TRecordOut();
                        ZTaxControl.TRecordOut outInfo = new ZTaxControl.TRecordOut();
                        ret = taxControl.WriteRecord1(ref normlInvInfo, ref outInfo);
                        //ret = Jeez.TaxControl.ZTaxControl.WriteRecord(ref normlInvInfo, ref outInfo);
                        BaseFunc.WriteTaxLog("Jeez.TaxControl.ZTaxControl.WriteRecord : " + ret.ToString());
                        forgeryCode = "";
                        if (ret == 0)
                        {
                            InvoiceNo = normlInvInfo.InvoiceNO;
                            forgeryCode = outInfo.TaxDeviceChkCode.Trim();
                            Kf.Util.Constants.SetConfigData("ZMachineNumber", outInfo.MacNO.Trim());
                            return true;
                        }
                    }
                }
            }
            finally
            {
                //Jeez.TaxControl.ZTaxControl.CloseReader();
                taxControl.CloseReader1();
                //GC.Collect();
            }
            return false;
        }

        private static bool FMakeInvoice(Form frm, string playName, DataTable details, ref string InvoiceNo, ref string forgeryCode)
        {
            BaseFunc.WriteTaxLog("FMakeInvoice");
            AxFiscalOcx.AxTFiscal fiscal = GetFiscal(frm);
            try
            {
                string com = Kf.Util.Constants.GetConfigData("FCom");
                if (!string.IsNullOrEmpty(com))
                {
                    if (FMakeInvoice(fiscal, Convert.ToInt32(com), playName, details, ref InvoiceNo, ref forgeryCode))
                    {
                        return true;
                    }
                }
                List<int> comList = BaseFunc.GetAllComInterface();
                foreach (int item in comList)
                {
                    if (FMakeInvoice(fiscal, item, playName, details, ref InvoiceNo, ref forgeryCode))
                    {
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                Kf.Util.UI.MsgBox.ShowErrMsg(ex);
            }
            finally
            {
                RemoveFiscal(frm, fiscal);
            }
            return false;
        }

        private static bool FMakeInvoice(AxFiscalOcx.AxTFiscal fiscal, int com, string playName, DataTable details, ref string InvoiceNo, ref string forgeryCode)
        {
            int ret;
            if (fiscal == null)
            {
                return false;
            }
            try
            {
                ret = Convert.ToInt32(fiscal.CommOpen(com, 57600, 60));
                BaseFunc.WriteTaxLog("fiscal.CommOpen " + ret.ToString());

                if (ret == 1)
                {
                    ret = fiscal.LinkMachine();
                    BaseFunc.WriteTaxLog("fiscal.LinkMachine " + ret.ToString());
                    if (ret == 0)
                    {

                        DataView dv = new DataView(details);
                        dv.Sort = "TaxIndex asc";

                        FiscalOcx.T_wNormalInvInfo normlInvInfo = new FiscalOcx.T_wNormalInvInfo();
                        normlInvInfo.sPayer = playName;
                        FiscalOcx.T_InvItem[] items = new FiscalOcx.T_InvItem[6];
                        int ii = 0;
                        int iCount = 0;
                        for (int i = 0; i < 7 && i < dv.Count; i++)
                        {
                            bool bfind = false;

                            for (int j = 0; j < ii; j++)
                            {
                                if (items[j].sTaxindex == dv[i]["TaxIndex"].ToString())
                                {
                                    items[j].fsum += Convert.ToDouble(dv[i]["Money"]);
                                    bfind = true;
                                    break;
                                }
                            }

                            if (!bfind)
                            {
                                iCount++;
                                items[ii].sTaxindex = dv[i]["TaxIndex"].ToString();
                                items[ii].fsum = Convert.ToDouble(dv[i]["Money"]);
                                ii++;
                            }
                        }
                        normlInvInfo.nItemCount = iCount;
                        normlInvInfo.cInvItems = items;

                        FiscalOcx.T_rNormalInvInfo outInfo = new FiscalOcx.T_rNormalInvInfo();
                        ret = fiscal.MakeInvoice(ref normlInvInfo, ref outInfo);
                        BaseFunc.WriteTaxLog("fiscal.MakeInvoice return : " + ret.ToString());
                        if (ret == 0)
                        {
                            InvoiceNo = outInfo.sPrintno;
                            forgeryCode = outInfo.sAntiForgeryCode;
                            return true;
                        }

                    }

                }
            }
            finally
            {
                fiscal.CommClose();
            }
            return false;
        }
        
        public static DataTable GetTaxIndexInfo(Form frm)
        {
            switch (GetTaxControlMachineType(frm))
            {
                case TaxMachineType.FISCAL:
                    return GetFTaxIndexInfo(frm);
                case TaxMachineType.ZHONGDING:
                    return GetZTaxIndexInfo();
                case TaxMachineType.None:
                    Kf.Util.UI.MsgBox.Show("没找到税控设备.");
                    break;
                default:
                    break;
            }
            return new DataTable();
        }
        */
        /*
        private static DataTable GetZTaxIndexInfo()
        {
            DataTable dt = new DataTable();
            try
            {
                string com = Kf.Util.Constants.GetConfigData("ZCom");

                if (!string.IsNullOrEmpty(com) && GetZTaxIndexInfo(Convert.ToByte(com), dt))
                {
                    return dt;
                }

                List<int> comList = BaseFunc.GetAllComInterface();
                foreach (int item in comList)
                {
                    if (GetZTaxIndexInfo((byte)item, dt))
                    {
                        return dt;
                    }
                }
            }
            catch (Exception ex)
            {
                Kf.Util.UI.MsgBox.ShowErrMsg(ex);
            }
            finally
            {

            }

            return dt;
        }
        
       private static bool GetZTaxIndexInfo(byte com, DataTable dt)
       {
           ZTaxControl.TaxControlClass taxControl = new ZTaxControl.TaxControlClass();
           try
           {
               int ret;
               ret = taxControl.InitReader1(com);
               //ret = Jeez.TaxControl.ZTaxControl.InitReader(Convert.ToByte(com));
               if (ret == 0)
               {
                   Kf.Util.Constants.SetConfigData("ZCom", com.ToString());
                   //TTaxItem taxIndexList = new TTaxItem();
                   //taxIndexList.TaxItemDTL = new TTaxItemDTL[6];
                   //ret = Jeez.TaxControl.ZTaxControl.GetTaxItem(ref taxIndexList);
                   ZTaxControl.TTaxItem1 taxIndexList = new ZTaxControl.TTaxItem1();
                   ret = taxControl.GetTaxItem1(ref taxIndexList);
                   BaseFunc.WriteTaxLog("Jeez.TaxControl.ZTaxControl.GetTaxItem :" + ret.ToString());
                   if (ret == 0)
                   {
                       dt.Columns.Add("TaxIndex");
                       dt.Columns.Add("Name");
                       dt.Columns.Add("Number");
                       dt.Columns.Add("TaxRate");
                       dt.Columns.Add("EName");
                       //foreach (TTaxItemDTL item in taxIndexList.TaxItemDTL)
                       //{
                       //    BaseFunc.WriteTaxLog(item.TaxItemIndex + item.TaxCName);
                       //    if (!string.IsNullOrEmpty(item.TaxCName) && !string.IsNullOrEmpty(item.TaxItemIndex) && Convert.ToInt32(item.TaxItemIndex) > 0)
                       //    {
                       //        DataRow dr = dt.NewRow();
                       //        dr["TaxIndex"] = item.TaxItemIndex.Trim();
                       //        dr["Name"] = item.TaxCName.Trim();
                       //        dr["Number"] = item.TaxItemNO.Trim();
                       //        dr["TaxRate"] = item.TaxRate;
                       //        dr["EName"] = item.TaxEName;
                       //        dt.Rows.Add(dr);
                       //    }
                       //}
                       ZTaxControl.TTaxItemDTL item = taxIndexList.TaxItemDTL0;
                       if (!string.IsNullOrEmpty(item.TaxCName) && !string.IsNullOrEmpty(item.TaxItemIndex) && Convert.ToInt32(item.TaxItemIndex) > 0)
                       {
                           DataRow dr = dt.NewRow();
                           dr["TaxIndex"] = item.TaxItemIndex.Trim();
                           dr["Name"] = item.TaxCName.Trim();
                           dr["Number"] = item.TaxItemNO.Trim();
                           dr["TaxRate"] = item.TaxRate;
                           dr["EName"] = item.TaxEName;
                           dt.Rows.Add(dr);
                       }

                       item = taxIndexList.TaxItemDTL1;
                       if (!string.IsNullOrEmpty(item.TaxCName) && !string.IsNullOrEmpty(item.TaxItemIndex) && Convert.ToInt32(item.TaxItemIndex) > 0)
                       {
                           DataRow dr = dt.NewRow();
                           dr["TaxIndex"] = item.TaxItemIndex.Trim();
                           dr["Name"] = item.TaxCName.Trim();
                           dr["Number"] = item.TaxItemNO.Trim();
                           dr["TaxRate"] = item.TaxRate;
                           dr["EName"] = item.TaxEName;
                           dt.Rows.Add(dr);
                       }

                       item = taxIndexList.TaxItemDTL2;
                       if (!string.IsNullOrEmpty(item.TaxCName) && !string.IsNullOrEmpty(item.TaxItemIndex) && Convert.ToInt32(item.TaxItemIndex) > 0)
                       {
                           DataRow dr = dt.NewRow();
                           dr["TaxIndex"] = item.TaxItemIndex.Trim();
                           dr["Name"] = item.TaxCName.Trim();
                           dr["Number"] = item.TaxItemNO.Trim();
                           dr["TaxRate"] = item.TaxRate;
                           dr["EName"] = item.TaxEName;
                           dt.Rows.Add(dr);
                       }

                       item = taxIndexList.TaxItemDTL3;
                       if (!string.IsNullOrEmpty(item.TaxCName) && !string.IsNullOrEmpty(item.TaxItemIndex) && Convert.ToInt32(item.TaxItemIndex) > 0)
                       {
                           DataRow dr = dt.NewRow();
                           dr["TaxIndex"] = item.TaxItemIndex.Trim();
                           dr["Name"] = item.TaxCName.Trim();
                           dr["Number"] = item.TaxItemNO.Trim();
                           dr["TaxRate"] = item.TaxRate;
                           dr["EName"] = item.TaxEName;
                           dt.Rows.Add(dr);
                       }

                       item = taxIndexList.TaxItemDTL4;
                       if (!string.IsNullOrEmpty(item.TaxCName) && !string.IsNullOrEmpty(item.TaxItemIndex) && Convert.ToInt32(item.TaxItemIndex) > 0)
                       {
                           DataRow dr = dt.NewRow();
                           dr["TaxIndex"] = item.TaxItemIndex.Trim();
                           dr["Name"] = item.TaxCName.Trim();
                           dr["Number"] = item.TaxItemNO.Trim();
                           dr["TaxRate"] = item.TaxRate;
                           dr["EName"] = item.TaxEName;
                           dt.Rows.Add(dr);
                       }

                       item = taxIndexList.TaxItemDTL5;
                       if (!string.IsNullOrEmpty(item.TaxCName) && !string.IsNullOrEmpty(item.TaxItemIndex) && Convert.ToInt32(item.TaxItemIndex) > 0)
                       {
                           DataRow dr = dt.NewRow();
                           dr["TaxIndex"] = item.TaxItemIndex.Trim();
                           dr["Name"] = item.TaxCName.Trim();
                           dr["Number"] = item.TaxItemNO.Trim();
                           dr["TaxRate"] = item.TaxRate;
                           dr["EName"] = item.TaxEName;
                           dt.Rows.Add(dr);
                       }
                       return true;
                   }
               }
           }
           finally
           {
               //Jeez.TaxControl.ZTaxControl.CloseReader();
               taxControl.CloseReader1();
           }

           return false;
       }

       private static DataTable GetFTaxIndexInfo(Form frm)
       {
           DataTable dt = new DataTable();
           AxFiscalOcx.AxTFiscal fiscal = GetFiscal(frm);
           try
           {
               string com = Kf.Util.Constants.GetConfigData("FCom");

               if (!string.IsNullOrEmpty(com) && GetFTaxIndexInfo(fiscal, Convert.ToInt32(com), dt))
               {
                   return dt;
               }

               List<int> comList = BaseFunc.GetAllComInterface();
               foreach (int item in comList)
               {
                   if (GetFTaxIndexInfo(fiscal, item, dt))
                   {
                       return dt;
                   }
               }
           }
           catch (Exception ex)
           {
               Kf.Util.UI.MsgBox.ShowErrMsg(ex);
           }
           finally
           {
               RemoveFiscal(frm, fiscal);
           }

           return dt;
       }
       
       private static bool GetFTaxIndexInfo(AxFiscalOcx.AxTFiscal fiscal, int com, DataTable dt)
       {
           Kf.Util.UI.MessageTip tip = new Kf.Util.UI.MessageTip("正在接连税控设备...");
           if (fiscal == null)
           {
               return false;
           }
           try
           {
               try
               {
                   if (fiscal.CommOpen(com, 57600, 60))
                   {
                       if (fiscal.LinkMachine() == 0)
                       {
                           Kf.Util.Constants.SetConfigData("FCom", com.ToString());
                           FiscalOcx.T_rTaxkindInfo[] taxIndexList = new FiscalOcx.T_rTaxkindInfo[6];
                           System.Array ay = taxIndexList;
                           int ret = fiscal.GetTaxkindInfo(ref ay);
                           BaseFunc.WriteTaxLog("GetTaxkindInfo return :" + ret.ToString());
                           if (ret == 0)
                           {

                               BaseFunc.WriteTaxLog(ay.Length.ToString());

                               dt.Columns.Add("TaxIndex");
                               dt.Columns.Add("Name");
                               dt.Columns.Add("Number");
                               dt.Columns.Add("TaxRate");
                               dt.Columns.Add("EName");
                               foreach (Object obj in ay)
                               {
                                   if (obj is FiscalOcx.T_rTaxkindInfo)
                                   {
                                       FiscalOcx.T_rTaxkindInfo info = (FiscalOcx.T_rTaxkindInfo)obj;
                                       BaseFunc.WriteTaxLog(info.sTaxindex + info.sTaxname);
                                       if (!string.IsNullOrEmpty(info.sTaxindex) && !string.IsNullOrEmpty(info.sTaxname))
                                       {
                                           DataRow dr = dt.NewRow();
                                           dr["TaxIndex"] = info.sTaxindex.Trim();
                                           dr["Name"] = info.sTaxname.Trim();
                                           dr["Number"] = info.sTaxno;
                                           dr["TaxRate"] = info.fTaxrate;
                                           dr["EName"] = "";
                                           dt.Rows.Add(dr);
                                       }
                                   }
                               }


                               return true;
                           }
                       }
                   }
               }
               finally
               {
                   fiscal.CommClose();
               }
           }
           finally
           {
               tip.CloseMessageTip();

           }

           return false;
       }
       
        public static TaxMachineType GetTaxControlMachineType(Form frm)
        {
            TaxMachineType tmType;
            string type = Kf.Util.Constants.GetConfigData("TMType");
            if (!string.IsNullOrEmpty(type))
            {
                tmType = (TaxMachineType)int.Parse(type);
                return tmType;
            }

            List<int> comList = GetAllComInterface();
            BaseFunc.WriteTaxLog("Com Conut :" + comList.Count.ToString());
            AxFiscalOcx.AxTFiscal fiscal = GetFiscal(frm);
            try
            {
                foreach (int com in comList)
                {
                    if (LinkFMachine(fiscal, com))
                    {
                        Kf.Util.Constants.SetConfigData("TMType", "1");
                        return TaxMachineType.FISCAL;
                    }
                    else if (LinkZMachine(com))
                    {
                        Kf.Util.Constants.SetConfigData("TMType", "2");
                        return TaxMachineType.ZHONGDING;
                    }
                }
            }
            catch (Exception ex)
            {
                Kf.Util.UI.MsgBox.ShowErrMsg(ex);
            }
            finally
            {
                RemoveFiscal(frm, fiscal);
            }
            return TaxMachineType.None;
        }

        private static bool LinkZMachine(int com)
        {
            ZTaxControl.TaxControlClass taxControl = new ZTaxControl.TaxControlClass();
            try
            {
                BaseFunc.WriteTaxLog("Com: " + Convert.ToByte(com).ToString());
                int ret;
                //ret = Jeez.TaxControl.ZTaxControl.InitReader(Convert.ToByte(com));
                ret = taxControl.InitReader1(Convert.ToByte(com));
                BaseFunc.WriteTaxLog("Jeez.TaxControl.ZTaxControl.InitReader " + ret.ToString());
                if (ret == 0)
                {
                    Kf.Util.Constants.SetConfigData("ZCom", com.ToString());
                    return true;
                }
            }

            finally
            {
                //Jeez.TaxControl.ZTaxControl.CloseReader();
                taxControl.CloseReader1();
                BaseFunc.WriteTaxLog("Jeez.TaxControl.ZTaxControl.CloseReader ");
            }
            return false;
        }
         */ 
        /*
        private static bool LinkFMachine(AxFiscalOcx.AxTFiscal fiscal, int com)
        {

            int ret;
            if (fiscal == null)
            {
                return false;
            }
            try
            {
                ret = Convert.ToInt32(fiscal.CommOpen(com, 57600, 60));
                BaseFunc.WriteTaxLog("fiscal.CommOpen " + ret.ToString());
                if (ret == 1)
                {
                    ret = fiscal.LinkMachine();
                    BaseFunc.WriteTaxLog("fiscal.LinkMachine " + ret.ToString());
                    if (ret == 0)
                    {
                        Kf.Util.Constants.SetConfigData("FCom", com.ToString());
                        return true;
                    }

                }

            }
            finally
            {
                fiscal.CommClose();
            }

            return false;
        }

        private static AxFiscalOcx.AxTFiscal GetFiscal(Form frm)
        {
            try
            {
                AxFiscalOcx.AxTFiscal fiscal = new AxFiscalOcx.AxTFiscal();
                frm.Controls.Add(fiscal);
                fiscal.Visible = false;

                return fiscal;
            }
            catch
            {
            }
            return null;
        }

        private static void RemoveFiscal(Form frm, AxFiscalOcx.AxTFiscal fiscal)
        {
            frm.Controls.Remove(fiscal);
        }
        */
        /// <summary>
        /// 获取当前组织机构的IsStart=true启用期间,或IsStart=false当前期间
        /// </summary>
        /// <param name="OrgnizationID"></param>
        /// <returns></returns>
        static public DateTime GetCurPeriod(int OrganizationID, bool IsStart)
        {
            EntityObjectFactory factory = EntityObjectFactory.GetInstance(Kf.Login.Environment.objContext.Clone(), Kf.Util.EntityIDEnum.CurrentPeriod);
            EntityObject curPeriod = factory.FindFirst("OrganizationID={0} and SystemName = {1} and IsStart={2}", OrganizationID, "PMS", IsStart);
            if (curPeriod == null && IsStart == false)
            {
                return GetCurPeriod(OrganizationID, true);
            }
            else if (curPeriod == null && IsStart == true)
            {
                return Kf.Core.Toolkit.GetDateTimeDefaultValue();
            }
            else
            {
                return new DateTime((int)curPeriod.GetProperty("Year"), (int)curPeriod.GetProperty("Period"), 1);
            }
        }


        static public string GetDefaultValueStringByDataType(int DataTypeID, int BaseDataTypeID)
        {
            if (BaseDataTypeID == 1)
            {
                switch (DataTypeID)
                { 
                    case 101:
                    case 102:
                    case 103:
                    case 106:
                        return "0";
                        break;
                    case 107:
                    case 108:
                    case 109:
                    case 110:
                    case 111:
             
                    case 104:
                  
                        return "''";
                    case 105:
                    case 112:
                    case 113:
                        return "null";
                    default:
                        return "''";
                }
            }
            else if (BaseDataTypeID == 2)
            {
                return "0";
            }
            else
            {
                return "''";
            }
        }

        public static void WriteAndShowLogFile(string fileName, string logContent)
        {
            using (System.IO.StreamWriter sw = new System.IO.StreamWriter(fileName))
            {
                sw.Write(logContent);
                sw.Flush();
                sw.Close();
            }
            System.Diagnostics.Process.Start(fileName);
        }

        /// <summary>
        /// 判断单据是否已经开过地税发票
        /// </summary>
        /// <param name="objContext"></param>
        /// <param name="entityId">单据实体ID</param>
        /// <param name="sourceId">单据ID</param>
        /// <returns></returns>
        public static bool HasDInvoice(ObjectContext objContext, int entityId, int sourceId)
        {
            EntityObjectFactory eof = EntityObjectFactory.GetInstance(objContext, Kf.Util.EntityIDEnum.DInvoiceInfo);
            EntityObject eo = eof.FindFirst("EntityID = {0} and SourceID = {1}", entityId, sourceId);
            return eo != null;
        }


        public static string GetSMSTemplate(int Type)
        {
            String Context = "";
            try
            {
                string fileName = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "SMSTemplateConfig.xml";
                if (System.IO.File.Exists(fileName))
                {
                    System.Xml.XmlDocument xd = new System.Xml.XmlDocument();
                    xd.Load(fileName);

                    XmlNode node = xd.SelectSingleNode("/root");
                    XmlNodeList nodelist = xd.SelectSingleNode("/root").ChildNodes;

                    for (int i = 0; i < nodelist.Count; i++)
                    {
                        XmlElement xel = nodelist[i] as XmlElement;

                        XmlNodeList xnlist = xel.ChildNodes;

                        if (xnlist[0].InnerText == Type.ToString())
                        {
                            Context = xnlist[1].InnerText;
                            break;
                        }
                    }
                }
            }
            catch
            {
                Context = "";
            }
            return Context;
        }

     
        public static void SaveSMSTemplate(int Type,string content)
        {
            string fileName = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "SMSTemplateConfig.xml";
            System.Xml.XmlDocument xd = new System.Xml.XmlDocument();
            StringBuilder sb = new StringBuilder();
            if (System.IO.File.Exists(fileName))
            {
                bool isHave = false;
                xd.Load(fileName);
                XmlNodeList nodelist = xd.SelectSingleNode("/root").ChildNodes;

                for (int i = 0; i < nodelist.Count; i++)
                {
                    XmlElement xel = nodelist[i] as XmlElement;
                    XmlNodeList xnlist = xel.ChildNodes;
                    if (xnlist[0].InnerText == Type.ToString())
                    {
                        xnlist[1].InnerText = content;
                        isHave = true;
                        break;
                    }
                }


                if (!isHave)
                {
                    System.Xml.XmlNode xmlnode = xd.SelectSingleNode("/root");

                    //新增节点
                    XmlNode xn = xd.CreateNode(XmlNodeType.Element, "SMS", null);
                    //添加属性
                    XmlNode xa = xd.CreateNode(XmlNodeType.Element, "Type", null);
                    xa.InnerText = Type.ToString();
                    XmlNode xa2 = xd.CreateNode(XmlNodeType.Element, "Content", null);
                    xa2.InnerText = content;

                    xn.AppendChild(xa);
                    xn.AppendChild(xa2);
                    xmlnode.AppendChild(xn);
                }

            }
            else
            {
                sb.Append("<?xml version='1.0' encoding='utf-8'?>");
                sb.Append("<root><SMS>");
                sb.Append(string.Format("<Type>{0}</Type>", Type.ToString()));
                sb.Append(string.Format("<Content>{0}</Content>", content));
                sb.Append("</SMS></root>");
                xd.LoadXml(sb.ToString());
            }

            xd.Save(fileName);
        }

        public static decimal ToDecimal(object o,decimal defaultValue=0m)
        {
            if (o == DBNull.Value || o == null) return defaultValue;
            return Convert.ToDecimal(o);
        }

        public static int ToInt(object o, int defaultValue = 0)
        {
            if (o == DBNull.Value || o == null) return defaultValue;
            int.TryParse(o.ToString(), out defaultValue);
            return defaultValue;
        }

        public static string ToStr(object o, string defaultValue = "")
        {
            if (o == DBNull.Value || o == null) return defaultValue;
            return o.ToString();
        }

        public static bool IsNullOrWhiteSpace(object o, string defaultValue = "")
        {
            if (o == DBNull.Value || o == null) defaultValue = "";
            else defaultValue = o.ToString();
            return string.IsNullOrWhiteSpace(defaultValue);
        }

        public static bool GridHasCol(Kf.Control.KfGrids.KfGrid grid, string colKey)
        {
            return grid.Rows.Band.Columns.Exists(colKey);
        }

        static public string GetOrganizationTable(ObjectContext objContext)
        {
            return "(select kfOrganization.* from kfOrganization " +
                (new EntityMap(Kf.Util.EntityIDEnum.Organization, objContext)).DataEntity.GetTableName(Kf.Login.Environment.UserID)
                + ")";
        }
        static public string GetMaterialTable(ObjectContext objContext)
        {
            return "(select kfMaterial.* from kfMaterial " +
                (new EntityMap(Kf.Util.EntityIDEnum.Material, objContext)).DataEntity.GetTableName(Kf.Login.Environment.UserID)
                + ")";
        }
        static public string GetRelatedOrgTable(ObjectContext objContext)
        {
            return "(select kfRelatedOrg.* from kfRelatedOrg " +
                (new EntityMap(Kf.Util.EntityIDEnum.RelatedOrg, objContext)).DataEntity.GetTableName(Kf.Login.Environment.UserID)
                + ")";
        }
        static public string GetDeparetMentTable(ObjectContext objContext)
        {
            return "(select kfDepartment.ID from kfDepartment " +
                (new EntityMap(Kf.Util.EntityIDEnum.DepartMent, objContext)).DataEntity.GetTableName(Kf.Login.Environment.UserID)
                + " union all select 0)";
        }
        static public string GetDeparetMentTable2(ObjectContext objContext)
        {
            return "(select kfDepartment.* from kfDepartment " +
                (new EntityMap(Kf.Util.EntityIDEnum.DepartMent, objContext)).DataEntity.GetTableName(Kf.Login.Environment.UserID)
                + ")";
        }
        static public string GetWareHouseTable(ObjectContext objContext)
        {
            return " (select kfWareHouse.* from kfWareHouse where ID in (select kfWareHouse.ID from kfWareHouse " +
                (new EntityMap(Kf.Util.EntityIDEnum.Warehouse, objContext)).DataEntity.GetTableName(Kf.Login.Environment.UserID)
                + "))";
        }
        private static int bSaveUpdateQty = 0;
        static public bool SaveUpdateQty
        {
            get
            {
                if (bSaveUpdateQty == 0)
                {
                    ObjectContext objContext = Kf.Login.Environment.objContext.Clone();
                    object oSysProfile = Kf.BllObj.SystemProfile.GetSystemProfileValueByID(objContext, Kf.Util.SysParamEnum.SYS165);
                    if (oSysProfile != null && oSysProfile.ToString() == "1")
                    {
                        bSaveUpdateQty = 1;
                    }
                    else
                    {
                        bSaveUpdateQty = 2;
                    }
                }
                return bSaveUpdateQty == 1;
            }
        }
    }

    public enum TaxMachineType
    {
        /// <summary>
        /// 没有税控机
        /// </summary>
        None = 0,
        /// <summary>
        /// 奥格立
        /// </summary>
        FISCAL = 1,
        /// <summary>
        /// 中鼎
        /// </summary>
        ZHONGDING = 2
    }

    public enum TreeType : int
    {
        Park = 1,
        Build = 2,
        House = 3,
        ParkingSpace = 4,
        Meter = 5,
        MeterType = 6,
        Organization = 7,
        DepartMent = 8,
        EquipMentType = 9,
        Floor = 10

    }

    public class KfSqlHelper
    {
        private INativeQuery query;
        private NativeQueryCommand cmd;
        private ObjectContext objContext;

        private bool isNeedTransaction;

        public KfSqlHelper(ObjectContext objContext)
            : this(objContext, false) { }

        public KfSqlHelper(ObjectContext objContext, bool isNeedTransaction)
        {
            this.isNeedTransaction = isNeedTransaction;
            this.query = Kf.Login.RemoteCall.GetNativeQuery();
            this.cmd = new NativeQueryCommand();
            this.objContext = objContext;
        }

        public DataTable GetDataTable(string sqlStr)
        {
            cmd.CommandText = sqlStr;
            return query.GetDataTable(objContext.ConnectionString, cmd, isNeedTransaction);
        }

        public DataSet GetDataSet(string sqlStr)
        {
            cmd.CommandText = sqlStr;
            ArrayList cmdList = new ArrayList();
            cmdList.Add(cmd);
            return query.GetDataSet(objContext.ConnectionString, cmdList, isNeedTransaction);
        }

        public object ExecuteScalar(string sqlStr)
        {
            cmd.CommandText = sqlStr;
            return query.ExecuteScalar(objContext.ConnectionString, cmd, isNeedTransaction);
        }

        public void ExecuteNonQuery(string sqlStr)
        {
            cmd.CommandText = sqlStr;
            query.ExecuteNonQuery(objContext.ConnectionString, cmd, isNeedTransaction);
        }
    }
}
