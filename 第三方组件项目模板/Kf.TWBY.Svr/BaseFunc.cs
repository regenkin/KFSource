using System;
using Kf.Core;
using Kf.Core.Qualifiers;
using System.Collections;
using Kf.Util.FormulaCal;
using System.Text;

namespace Kf.TWBY.Svr
{
    internal class InOutType
    {
        public const int Adjust = 1;//成本调整
        public const int Purchase = 2;//采购
        public const int Sale = 3;//销售
        public const int Pick = 4;//领料
        public const int Initial = 101;//初始化
        public const int CheckIn = 102;//盘赢
        public const int CheckOut = 103;//盘亏
    }

    internal class BaseFunc
    {
        public const decimal NormalTaxRate = 6m;

        //把2008-8-1等类型 转为20080801 类型 
        static public string ConvertDateToString(DateTime dtDate)
        {
            string strYear = dtDate.Year.ToString();
            string strMonth = dtDate.Month > 9 ? dtDate.Month.ToString() : "0" + dtDate.Month.ToString();
            string strDay = dtDate.Day > 9 ? dtDate.Day.ToString() : "0" + dtDate.Day.ToString();
            return strYear + strMonth + strDay;
        }

        static public bool isSuppierAssest(ObjectContext objContext)
        {
           object oSysProfile = Kf.BllObj.SystemProfile.GetSystemProfileValueByID(objContext, 316);
            if (oSysProfile != null && oSysProfile.ToString() == "1")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        static public Decimal ToDecimal(ObjectContext objContext, Object dataValue)
        {
            if (dataValue == null) return 0.0M;
            if (String.IsNullOrEmpty(dataValue.ToString())) return 0.0M;
            try
            {
                return Decimal.Parse(dataValue.ToString());
            }
            catch
            {
                return 0.00M;
            }
        }


        static public bool isPucrchInControl(ObjectContext objContext)
        {
            object oSysProfile = Kf.BllObj.SystemProfile.GetSystemProfileValueByID(objContext, 357);
            if (oSysProfile != null && oSysProfile.ToString() == "1")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        static public bool isConOutControl(ObjectContext objContext)
        {
            object oSysProfile = Kf.BllObj.SystemProfile.GetSystemProfileValueByID(objContext, 358);
            if (oSysProfile != null && oSysProfile.ToString() == "1")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        static public bool isCalcAssestMaterial(ObjectContext obj, int OrgID)
        {
            object oSysProfile = Kf.BllObj.SystemProfile.GetSystemProfileValueByID(obj, 343,OrgID);
            if (oSysProfile != null && oSysProfile.ToString() == "1")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        static public bool isToGenCard(ObjectContext objContext)
        {
            object oSysProfile = Kf.BllObj.SystemProfile.GetSystemProfileValueByID(objContext, 334);
            if (oSysProfile != null && oSysProfile.ToString() == "1")
            {
                return true;
            }
            else
            {
                return false;
            }
        }



        static public bool isSupperControl(ObjectContext objContext)
        {
            
                object oSysProfile = Kf.BllObj.SystemProfile.GetSystemProfileValueByID(objContext, 317);
               
                if (oSysProfile != null && oSysProfile.ToString() == "1")
                {
                    return true;                    
                }
                return false;
         
        }

        static public int QtyPrecision(ObjectContext objContext)
        {

            object oSysProfile = Kf.BllObj.SystemProfile.GetSystemProfileValueByID(objContext, 186);
                int iQtyPrecision = 4;
                if (oSysProfile != null)
                {
                    try
                    {
                        iQtyPrecision = Convert.ToInt32(oSysProfile);
                    }
                    catch
                    {
                    }                    
                }
                return iQtyPrecision;
            
        }
        static public int AmountPrecision(ObjectContext objContext)
        {
            int iAmountPrecision;
            object oSysProfile = Kf.BllObj.SystemProfile.GetSystemProfileValueByID(objContext, 184);
            if (oSysProfile == null)
            {
                iAmountPrecision = 2;
            }
            else
            {
                iAmountPrecision = Convert.ToInt32(oSysProfile);
            }

            return iAmountPrecision;
        }
        
        static public int PricePrecision(ObjectContext objContext)
        {
            int iPricePrecision;
            object oSysProfile = Kf.BllObj.SystemProfile.GetSystemProfileValueByID(objContext, 203);
            if (oSysProfile == null)
            {
                iPricePrecision = 2;
            }
            else
            {
                iPricePrecision = Convert.ToInt32(oSysProfile);
            }

            return iPricePrecision;
        }

        static public bool CanUnderZeo(ObjectContext objContext)
        {
            bool bRet;
            object oSysProfile = Kf.BllObj.SystemProfile.GetSystemProfileValueByID(objContext, 163);
            if (oSysProfile == null)
            {
                bRet = false;
            }
            else
            {
                bRet = (oSysProfile.ToString() == "1");
            }

            return bRet;
        }

        static public decimal CalcQty(EntityObject eoSourceU, EntityObject eoTargetU, decimal SourceQuantity,ObjectContext obj)
        {
            if ((int)eoSourceU.GetProperty("UnitGroupID") != (int)eoTargetU.GetProperty("UnitGroupID"))
            {
                return 0m;
            }
            if ((int)eoSourceU.PrimaryKeyValue == (int)eoTargetU.PrimaryKeyValue)
            {
                return SourceQuantity;
            }

            decimal qtyRet = SourceQuantity * (decimal)eoSourceU.GetProperty("ConversionRate") / (decimal)eoTargetU.GetProperty("ConversionRate");
            return System.Math.Round(qtyRet, BaseFunc.QtyPrecision(obj), MidpointRounding.AwayFromZero);
        }

        static public bool SaveUpdateQty(ObjectContext objContext)
        {
            bool bRet;
            object oSysProfile = Kf.BllObj.SystemProfile.GetSystemProfileValueByID(objContext, 165);
            if (oSysProfile == null)
            {
                bRet = false;
            }
            else
            {
                bRet = (oSysProfile.ToString() == "1");
            }

            return bRet;
        }

        static public bool CanModifyPurchasePrice(ObjectContext objContext)
        {
            bool bRet;
            object oSysProfile = Kf.BllObj.SystemProfile.GetSystemProfileValueByID(objContext, 199);
            if (oSysProfile == null)
            {
                bRet = false;
            }
            else
            {
                bRet = (oSysProfile.ToString() == "1");
            }

            return bRet;
        }

        static public bool CanMaxPrice(ObjectContext objContext)
        {
            bool bRet;
            object oSysProfile = Kf.BllObj.SystemProfile.GetSystemProfileValueByID(objContext, 167);
            if (oSysProfile == null)
            {
                bRet = true;
            }
            else
            {
                bRet = (oSysProfile.ToString() == "0");
            }

            return bRet;
        }

        static public bool CanMinPrice(ObjectContext objContext)
        {
            bool bRet;
            object oSysProfile = Kf.BllObj.SystemProfile.GetSystemProfileValueByID(objContext, 168);
            if (oSysProfile == null)
            {
                bRet = true;
            }
            else
            {
                bRet = (oSysProfile.ToString() == "0");
            }

            return bRet;
        }

        static public bool MatControl(ObjectContext objContext)//物料购销管控
        {
            bool bRet;
            object oSysProfile = Kf.BllObj.SystemProfile.GetSystemProfileValueByID(objContext, 170);
            if (oSysProfile == null)
            {
                bRet = true;
            }
            else
            {
                bRet = (oSysProfile.ToString() == "1");
            }

            return bRet;
        }


        public static void SetBillNo(ObjectContext objCtx, Kf.Runtime.BusiLogic.BillNumber billNum, EntityObjectList newObjLst, int dataEntityID, int currYear, int currPeriod)
        {
            if (newObjLst.Count > 0)
            {
                Kf.Core.ObjectContext objContext = objCtx.Clone();
                Hashtable ht = billNum.GetCurrentBillNo(dataEntityID, currYear, currPeriod, newObjLst.Count);
                string tranPrefix = ht["TranPrefix"].ToString();
                string tranPostfix = ht["TranPostfix"].ToString();
                int cn = (int)ht["CurrentNumber"];
                int dl = (int)ht["DigitalCount"];
                foreach (EntityObject newObj in newObjLst)
                {
                    string currentNo = cn.ToString();
                    if (currentNo.Length < dl)
                    {
                        for (int i = currentNo.Length; i < dl; i++)
                        {
                            currentNo = "0" + currentNo;
                        }
                    }
                    newObj.SetProperty("BillNO", tranPrefix + currentNo + tranPostfix);
                    cn++;
                }

            }
        }

        public static int GetUserID(ObjectContext context)
        {
            Kf.Util.Base.EncryptTripleDes encry = new Kf.Util.Base.EncryptTripleDes();
            string connectionString = encry.Decrypt(context.ConnectionString);

            string[] cs = connectionString.Split(Kf.Util.Constants.KfSplitChar1);
            if (cs.Length >= 2)
            {
                return int.Parse(cs[1]);
            }
            return 2;
        }

        /// <summary>
        /// 通过当前对象空间获取当前年度期间
        /// </summary>
        /// <param name="objctx"></param>
        /// <returns></returns>
        public static Hashtable GetYearPeriod(Kf.Core.ObjectContext objctx)
        {
            int CurrYear = 5000;
            int CurrPeriod = 1;
            int StartYear = 2006;
            int StartPeriod = 1;
            EntityObjectFactory eof = EntityObjectFactory.GetInstance(objctx, 414);
            EntityObject eoStartYear = eof.FindFirst("Key = {0}",Kf.Util.MultiLanguage.MultiLangTools.LoadString( "启用年度",objctx));
            if (eoStartYear != null)
            {
                StartYear = Convert.ToInt32(eoStartYear.GetProperty("Value"));
                EntityObject eoStartPeriod = eof.FindFirst("Key = {0}", "启用期间");
                StartPeriod = Convert.ToInt32(eoStartPeriod.GetProperty("Value"));
            }
            EntityObjectFactory factory = EntityObjectFactory.GetInstance(objctx, Kf.Util.EntityIDEnum.CurrentPeriod);
            EntityObjectList periodlist = factory.Find("SystemName = {0}", "SCM");
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
                Kf.Core.NativeQueryCommand cmd = new NativeQueryCommand();
                cmd.AppendCommandTextLine(" Select Top 1 * From kfCurrentPeriod order by [Year],[Period]");

                System.Data.DataTable dt = objctx.DataStore.GetDataTable(objctx.ConnectionString, cmd);

                if (dt.Rows.Count > 0)
                {
                    CurrYear = (int)dt.Rows[0]["Year"];
                    CurrPeriod = (int)dt.Rows[0]["Period"];
                }
                else
                {
                    CurrYear = StartYear;
                    CurrPeriod = StartPeriod;
                }
            }
            Hashtable ht = new Hashtable();
            ht.Add("StartYear", StartYear);
            ht.Add("StartPeriod", StartPeriod);
            ht.Add("CurrYear", CurrYear);
            ht.Add("CurrPeriod", CurrPeriod);
            return ht;
        }

        static public string GetMaterialTable(ObjectContext objContext)
        {
            return "(select kfMaterial.* from kfMaterial " +
                (new EntityMap(Kf.Util.EntityIDEnum.Material, objContext)).DataEntity.GetTableName(objContext.UserID)
                + ")";
        }

        /// <summary>
        /// 判断当前组织机构是否设置了启用期间
        /// </summary>
        /// <param name="OrgnizationID"></param>
        /// <returns></returns>
        static public bool IsSetPeriod(ObjectContext objContext, Object OrgnizationID)
        {
            EntityObjectFactory factory = EntityObjectFactory.GetInstance(objContext, Kf.Util.EntityIDEnum.CurrentPeriod);
            EntityObject curPeriod = factory.FindFirst("OrganizationID={0} and SystemName = {1} and IsStart={2}", OrgnizationID, "SCM", true);

            if (curPeriod == null)
            {
                return false;
            }
            return true;

        }


        static public bool IsSetPeriod(ObjectContext objContext, Object OrgnizationID, string SystemName)
        {
            EntityObjectFactory factory = EntityObjectFactory.GetInstance(objContext, Kf.Util.EntityIDEnum.CurrentPeriod);
            EntityObject curPeriod = factory.FindFirst("OrganizationID={0} and SystemName = {1} and IsStart={2}", OrgnizationID, SystemName, true);

            if (curPeriod == null)
            {
                return false;
            }
            return true;

        }
        /// <summary>
        /// 获取当前组织机构的IsStart=true启用期间,或IsStart=false当前期间
        /// </summary>
        /// <param name="OrgnizationID"></param>
        /// <returns></returns>
        static public DateTime GetCurPeriod(ObjectContext objContext, int OrganizationID, bool IsStart)
        {
            EntityObjectFactory factory = EntityObjectFactory.GetInstance(objContext, Kf.Util.EntityIDEnum.CurrentPeriod);
            EntityObject curPeriod = factory.FindFirst("OrganizationID={0} and SystemName = {1} and IsStart={2}", OrganizationID, "SCM", IsStart);
            if (curPeriod == null && IsStart == false)
            {
                return GetCurPeriod(objContext, OrganizationID, true);
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

        static public DateTime GetCurPeriod(ObjectContext objContext, int OrganizationID, bool IsStart, string SystemName)
        {
            EntityObjectFactory factory = EntityObjectFactory.GetInstance(objContext, Kf.Util.EntityIDEnum.CurrentPeriod);
            EntityObject curPeriod = factory.FindFirst("OrganizationID={0} and SystemName = {1} and IsStart={2}", OrganizationID, SystemName, IsStart);
            if (curPeriod == null && IsStart == false)
            {
                return GetCurPeriod(objContext, OrganizationID, true, SystemName);
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

        static public DateTime GetCurPeriodBeginDate(ObjectContext objContext, int OrganizationID)
        {
            EntityObjectFactory factory = EntityObjectFactory.GetInstance(objContext, Kf.Util.EntityIDEnum.CurrentPeriod);
            EntityObject curPeriod = factory.FindFirst("OrganizationID={0} and SystemName = {1} and IsStart={2}", OrganizationID, "SCM", false);
            if (curPeriod == null || (bool)curPeriod.GetProperty("IsStart"))
            {
                return GetCurPeriod(objContext, OrganizationID, true);
            }
            else
            {
                return new DateTime((int)curPeriod.GetProperty("Year"), (int)curPeriod.GetProperty("Period"), 1);
            }
        }

        static public DateTime GetCurPeriodBeginDate(ObjectContext objContext, EntityObject eo)
        {
            if (eo != null)
            {
                if (eo.EntityMap.EntityID == Kf.Util.EntityIDEnum.Organization)
                {
                    return GetCurPeriodBeginDate(objContext, (int)eo.PrimaryKeyValue);
                }
                else
                {
                    return GetCurPeriodBeginDate(objContext, (int)eo.GetProperty("OrganizationID"));
                }
            }
            else
            {
                return Toolkit.GetDateTimeDefaultValue();
            }
        }

        #region Old
        //static public DateTime GetCurPeriodBeginDate(ObjectContext objContext, int OrgnizationID)
        //{
        //    EntityObjectFactory factory = EntityObjectFactory.GetInstance(objContext, Kf.Util.EntityIDEnum.CurrentPeriod);
        //    EntityObject curPeriod = factory.FindFirst("OrganizationID={0} and SystemName = {1}", OrgnizationID,"SCM");
        //    if (curPeriod == null || (bool)curPeriod.GetProperty("IsStart"))
        //    {
        //        return Toolkit.GetDateTimeDefaultValue();
        //    }
        //    else
        //    {
        //        return new DateTime((int)curPeriod.GetProperty("Year"), (int)curPeriod.GetProperty("Period"), 1);
        //    }
        //}

        //static public DateTime GetCurPeriodBeginDate(ObjectContext objContext, EntityObject eo)
        //{
        //    if (eo != null)
        //    {
        //        if (eo.EntityMap.EntityID == Kf.Util.EntityIDEnum.Organization)
        //        {
        //            return GetCurPeriodBeginDate(objContext, (int)eo.PrimaryKeyValue);
        //        }
        //        else
        //        {
        //            return GetCurPeriodBeginDate(objContext, (int)eo.GetProperty("OrganizationID"));
        //        }
        //    }
        //    else
        //    {
        //        return Toolkit.GetDateTimeDefaultValue();
        //    }
        //}
        #endregion

        public static void SetCurrentQty(ObjectContext objContext, int WarehouseID, EntityObjectList lstDetail, bool bIn, EntityObjectList lstChanged, bool bRaiseError)
        {
            BaseFunc.SetCurrentQty(objContext, WarehouseID, lstDetail, bIn, lstChanged, "WHDistrictID", bRaiseError);
        }

        public static void SetCurrentQty(ObjectContext objContext, int WarehouseID, EntityObjectList lstDetail, bool bIn, EntityObjectList lstChanged, string WHDFieldName, bool bRaiseError)
        {
            int _WarehouseID = WarehouseID;
            EntityObjectFactory eof = EntityObjectFactory.GetInstance(objContext, Kf.Util.EntityIDEnum.WHCurrentQty);
            eof.IsUpdLock = true;
            EntityObject eoParent = null;
            foreach (EntityObject eoDetail in lstDetail)
            {
                eoParent = eoDetail.GetRelatedObject("WareHouseBillID");
                int MaterialID = (int)BaseFunc.IsNull(eoDetail.GetProperty("MaterialID"), 0);
                //int WHDistrictID = (int)BaseFunc.IsNull(eoDetail.GetProperty("WHDistrictID"), 0);
                if(WHDFieldName.ToLower()=="inwhdistrictid")
                    _WarehouseID = (int)BaseFunc.IsNull(eoDetail.GetProperty("InWarehouseID"), 0);
                else
                    _WarehouseID = (int)BaseFunc.IsNull(eoDetail.GetProperty("WarehouseID"), 0);
                int WHDistrictID = (int)BaseFunc.IsNull(eoDetail.GetProperty(WHDFieldName), 0);
                string BatchNO = BaseFunc.IsNull(eoDetail.GetProperty("BatchNO"), string.Empty).ToString();
                DateTime ProductDate = (DateTime)BaseFunc.IsNull(eoDetail.GetProperty("ProductDate"), Kf.Core.Toolkit.GetDateTimeDefaultValue());
                DateTime LimitedDate = (DateTime)BaseFunc.IsNull(eoDetail.GetProperty("LimitedDate"), Kf.Core.Toolkit.GetDateTimeDefaultValue());
                decimal Quantity = (decimal)BaseFunc.IsNull(eoDetail.GetProperty("Quantity"), 0m);
                if (Quantity == 0m && bRaiseError && (int)eoParent.GetProperty("InOutTypeID") == 208)
                {
                    eoParent = eoDetail.GetRelatedObject("WareHouseBillID");
                    eoDetail.Delete();
                } 
                else
                {
                    //更新及时库存
                    EntityObject eoQty = lstChanged.FindFirst("MaterialID={0} And WHDistrictID={1} And BatchNO={2} And ProductDate={3} And LimitedDate={4} And WareHouseID={5}",
                                            MaterialID, WHDistrictID, BatchNO, ProductDate, LimitedDate, _WarehouseID);
                    if (eoQty == null)
                    {
                        eoQty = eof.FindFirst("MaterialID={0} And WHDistrictID={1} And BatchNO={2} And ProductDate={3} And LimitedDate={4} And WareHouseID={5}",
                                            MaterialID, WHDistrictID, BatchNO, ProductDate, LimitedDate, _WarehouseID);
                        if (eoQty == null)
                        {
                            eoQty = eof.CreateObject();
                            eoQty.SetProperty("WarehouseID", _WarehouseID);
                            eoQty.SetProperty("MaterialID", MaterialID);
                            eoQty.SetProperty("WHDistrictID", WHDistrictID);
                            eoQty.SetProperty("BatchNO", BatchNO);
                            eoQty.SetProperty("ProductDate", ProductDate);
                            eoQty.SetProperty("LimitedDate", LimitedDate);
                            eoQty.SetProperty("Quantity", 0m);
                        }
                        lstChanged.Add(eoQty);
                    }

                    eoQty.SetProperty("Quantity", (decimal)eoQty.GetProperty("Quantity") + (bIn ? 1 : -1) * Quantity);
                }

                Kf.MetaData.DataEntity de = new Kf.MetaData.DataEntityFactory(objContext).FindObject(Kf.Util.EntityIDEnum.SaleWareHouseLock);
                if (de != null)
                {
                    if ((int)eoParent.GetProperty("InOutTypeID") == 3 
                        )
                    {
                        if (Convert.ToInt32(BaseFunc.IsNull(eoDetail.GetProperty("EntityID"), 0)) == Kf.Util.EntityIDEnum.SaleOrderDetail)
                        {
                            EntityObject eoLockQty = EntityObjectFactory.GetInstance(objContext, Kf.Util.EntityIDEnum.SaleWareHouseLock).FindFirst("MaterialID={0} And WarehouseID={1} and SaleOrderDetailID={2}", MaterialID, _WarehouseID, eoDetail.GetProperty("RefID"));
                            if (eoLockQty != null)
                            {
                                eoLockQty.SetProperty("LockQty", (decimal)eoLockQty.GetProperty("LockQty") + (bIn ? 1 : -1) * Quantity);
                                if ((decimal)eoLockQty.GetProperty("LockQty") < 0m)
                                    eoLockQty.SetProperty("LockQty", 0m);
                            }
                        }
                        else
                        {
                            if (bRaiseError && !bIn)
                            {
                                decimal lockQty = 0m;
                                decimal WHQty = 0m;
                                EntityObjectList eolLockQty = EntityObjectFactory.GetInstance(objContext, 2271493).Find("MaterialID={0} and WarehouseID={1}", MaterialID, _WarehouseID);
                                foreach (EntityObject eoLockQty in eolLockQty)
                                {
                                    lockQty += Convert.ToDecimal(eoLockQty.GetProperty("LockQty"));
                                }
                                EntityObjectList eolQty =EntityObjectFactory.GetInstance(objContext.Clone(),Kf.Util.EntityIDEnum.WHCurrentQty).Find("MaterialID={0} and WarehouseID={1}", MaterialID, _WarehouseID);
                                foreach (EntityObject eowhQty in eolQty)
                                {
                                    WHQty += Convert.ToDecimal(eowhQty.GetProperty("Quantity"));
                                }

                                if (WHQty - Quantity < lockQty && lockQty>0m)
                                {
                                    throw new Exception(string.Format(Kf.Util.MultiLanguage.MultiLangTools.LoadString("由于物料{0}被销售订单锁定库存{1}{2}，出现库存不足，操作失败！", objContext),eoDetail.GetRelatedObject("MaterialID").ToString(),Math.Round(lockQty,4),eoDetail.GetRelatedObject("MaterialID").GetRelatedObject("MeasureUnitID").GetProperty("Name")));
       
                                }
                            }
                        }
                    }
                }
            }

            if (bRaiseError)
            {
                StringBuilder sb = new StringBuilder();
                for (int i = lstChanged.Count - 1; i >= 0; i--)
                {
                    EntityObject eoChanged = lstChanged[i];

                    if ((decimal)eoChanged.GetProperty("Quantity") < 0m)
                    {
                        EntityObject eoMat = eoChanged.GetRelatedObject("MaterialID");
                        if (eoMat != null)
                        {
                            sb.Append("[");
                            sb.Append(eoMat.ToString());
                            sb.Append("],");
                        }
                    }
                    else if ((decimal)eoChanged.GetProperty("Quantity") == 0m)
                    {
                        eoChanged.Delete();
                    }
                }
                if (sb.Length > 0 && (!BaseFunc.CanUnderZeo(objContext)))
                {
                    throw new Exception(string.Format(Kf.Util.MultiLanguage.MultiLangTools.LoadString("物料{0}出现负库存，操作失败！",objContext), sb.Remove(sb.Length - 1, 1)));
                }
            }
        }

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

        static public bool BillCheckValidate(ObjectContext objContext, EntityObject eo, ref string Msg)
        {
            if ((int)eo.GetProperty("CheckID") != 0)
            {
                EntityObjectFactory factory = EntityObjectFactory.GetInstance(objContext, Kf.Util.EntityIDEnum.BillGroup);
                MetaData.DataEntityList dataEntityList = eo.EntityMap.DataEntity.ChildDataEntityList;
                dataEntityList.Sort("EntityID", SortDirection.Ascending);
                //foreach (EntityObject childEO in eo.GetChildEntityObjects(eo.EntityMap.DataEntity.ChildDataEntityList[0].EntityID))
                foreach (EntityObject childEO in eo.GetChildEntityObjects(dataEntityList[0].EntityID))
                {
                    EntityObject groupEO = factory.FindFirst("SourceEntityID={0} and SourceObjectID={1}", childEO.EntityMap.EntityID, childEO.PrimaryKeyValue);
                    if (groupEO != null)
                    {
                        EntityObject targetEO = EntityObjectFactory.GetInstance(objContext, (int)groupEO.GetProperty("TargetEntityID")).FindObject(groupEO.GetProperty("TargetObjectID"));
                        if (targetEO == null)
                            return true;
                        else
                        {
                            Msg = string.Format(Kf.Util.MultiLanguage.MultiLangTools.LoadString("单据已经产生关联单据[{0}],不能反审核！", objContext), targetEO.ParentEntityObject.ToString());
                            return false;
                        }
                    }
                }
            }

            return true;
        }

        static public void AdjustDiff(ObjectContext objContext, EntityObject eo, int UserID, int CurrYear, int CurrPeriod)
        {
        //    Hashtable htWHBill = new Hashtable();
        //    Hashtable htGroup = new Hashtable();

        //    EntityObjectFactory factory = EntityObjectFactory.GetInstance(objContext, Kf.Util.EntityIDEnum.BillGroup);
        //    foreach (EntityObject childEO in eo.GetChildEntityObjects(eo.EntityMap.DataEntity.ChildDataEntityList[0].EntityID))
        //    {
        //        EntityObject groupEO = factory.FindFirst("TargetEntityID={0} and TargetObjectID={1}", childEO.EntityMap.EntityID, childEO.PrimaryKeyValue);
        //        if (groupEO != null)
        //        {
        //            EntityObjectFactory factory1 = EntityObjectFactory.GetInstance(objContext, Kf.Util.EntityIDEnum.PIDetail);
        //            FetchSpecification fs = new FetchSpecification(factory1.EntityMap);
        //            fs.Qualifier = new InQualifier("ID", factory.EntityMap, new Kf.Core.QueryField("TargetObjectID"), new AndQualifier(
        //                         new ColumnQualifier("TargetEntityID", new EqualsPredicate(Kf.Util.EntityIDEnum.PIDetail)),
        //                         new ColumnQualifier("GroupID", new EqualsPredicate(groupEO.GetProperty("GroupID")))
        //                         ));

        //            EntityObjectList lstInvoice = factory1.Find(fs);
        //            if ((eo.EntityMap.EntityID == Kf.Util.EntityIDEnum.PurchaseInvoice && lstInvoice.FindFirst("PurchaseInvoiceObject.CheckID={0} And PurchaseInvoiceID!={1}", 0, eo.PrimaryKeyValue) != null) ||
        //                (eo.EntityMap.EntityID != Kf.Util.EntityIDEnum.PurchaseInvoice && lstInvoice.FindFirst("PurchaseInvoiceObject.CheckID={0}", 0) != null))
        //            {
        //                continue;
        //            }

        //            factory1 = EntityObjectFactory.GetInstance(objContext, Kf.Util.EntityIDEnum.WHBillDetail);
        //            fs = new FetchSpecification(factory1.EntityMap);
        //            fs.Qualifier = new InQualifier("ID", factory.EntityMap, new Kf.Core.QueryField("TargetObjectID"), new AndQualifier(
        //                         new ColumnQualifier("TargetEntityID", new EqualsPredicate(Kf.Util.EntityIDEnum.WHBillDetail)),
        //                         new ColumnQualifier("GroupID", new EqualsPredicate(groupEO.GetProperty("GroupID")))
        //                         ));

        //            EntityObjectList lstWHBill = factory1.Find(fs);
        //            if ((eo.EntityMap.EntityID == Kf.Util.EntityIDEnum.WareHouseBill && lstWHBill.FindFirst("WareHouseBillObject.CheckID={0} And WareHouseBillID!={1}", 0, eo.PrimaryKeyValue) != null) ||
        //                (eo.EntityMap.EntityID != Kf.Util.EntityIDEnum.WareHouseBill && lstWHBill.FindFirst("WareHouseBillObject.CheckID={0}", 0) != null))
        //            {
        //                continue;
        //            }

        //            decimal dAmount = 0, dQuantity = 0;
        //            foreach (EntityObject eoInvioce in lstInvoice)
        //            {
        //                dAmount += Kf.BllObj.Currency.CalcAmountByExchangeRate(objContext,
        //                            (int)eoInvioce.ParentEntityObject.GetProperty("CurrencyID"),
        //                            (decimal)eoInvioce.ParentEntityObject.GetProperty("ExchangeRate"),
        //                            (decimal)eoInvioce.GetProperty("Amount")) * ((bool)eoInvioce.ParentEntityObject.GetProperty("IsRed") ? -1 : 1);
        //                dQuantity += (decimal)eoInvioce.GetProperty("Quantity") * ((bool)eoInvioce.ParentEntityObject.GetProperty("IsRed") ? -1 : 1);
        //            }

        //            EntityObject eoLastWHBill = null;
        //            foreach (EntityObject eoWHBill in lstWHBill)
        //            {
        //                dAmount -= (decimal)eoWHBill.GetProperty("Amount") * ((bool)eoWHBill.ParentEntityObject.GetProperty("IsRed") ? -1 : 1);
        //                dQuantity -= (decimal)eoWHBill.GetProperty("Quantity") * ((bool)eoWHBill.ParentEntityObject.GetProperty("IsRed") ? -1 : 1);
        //                eoLastWHBill = eoWHBill;
        //            }

        //            if (dQuantity == 0m && dAmount != 0m)
        //            {
        //                EntityObject eoWHBill;
        //                if (!htWHBill.Contains(eoLastWHBill.ParentEntityObject.GetProperty("WareHouseID")))
        //                {
        //                    eoWHBill = EntityObjectFactory.GetInstance(objContext, Kf.Util.EntityIDEnum.WareHouseBill).CreateObject();
        //                    eoWHBill.SetProperty("PreparerID", UserID);
        //                    eoWHBill.SetProperty("PreparerDate", Toolkit.ServerCurrentTime());
        //                    eoWHBill.SetProperty("WareHouseID", eoLastWHBill.ParentEntityObject.GetProperty("WareHouseID"));
        //                    eoWHBill.SetProperty("Date", eo.GetProperty("Date"));
        //                    eoWHBill.SetProperty("Description", "暂估补差系统自动生成");
        //                    eoWHBill.SetProperty("BillNO", Kf.BllObj.BillWithAutoNO.GetCurrentBillNo(objContext, Kf.Util.EntityIDEnum.WareHouseBill, CurrYear, CurrPeriod, 1)["CurrentBillNo"]);
        //                    eoWHBill.SetProperty("InOrOutID", 1);
        //                    eoWHBill.SetProperty("InOutTypeID", InOutType.Adjust);

        //                    htWHBill.Add(eoLastWHBill.ParentEntityObject.GetProperty("WareHouseID"), eoWHBill);
        //                }
        //                else
        //                {
        //                    eoWHBill = (EntityObject)htWHBill[eoLastWHBill.ParentEntityObject.GetProperty("WareHouseID")];
        //                }

        //                EntityObject eoWHDetail = EntityObjectFactory.GetInstance(objContext, Kf.Util.EntityIDEnum.WHBillDetail).CreateObject();
        //                eoWHDetail.SetProperty("RecordNum", eoWHBill.GetChildEntityObjects(Kf.Util.EntityIDEnum.WHBillDetail).Count + 1);
        //                eoWHDetail.SetProperty("WareHouseBillID", eoWHBill.PrimaryKeyValue);
        //                eoWHDetail.SetProperty("MaterialID", eoLastWHBill.GetProperty("MaterialID"));
        //                eoWHDetail.SetProperty("BatchNO", eoLastWHBill.GetProperty("BatchNO"));
        //                eoWHDetail.SetProperty("ProductDate", eoLastWHBill.GetProperty("ProductDate"));
        //                eoWHDetail.SetProperty("LimitedDate", eoLastWHBill.GetProperty("LimitedDate"));
        //                eoWHDetail.SetProperty("WHDistrictID", eoLastWHBill.GetProperty("WHDistrictID"));
        //                eoWHDetail.SetProperty("EntityID", childEO.EntityMap.EntityID);
        //                eoWHDetail.SetProperty("RefID", childEO.PrimaryKeyValue);
        //                eoWHDetail.SetProperty("Amount", dAmount);

        //                EntityObject newGroupEO = EntityObjectFactory.GetInstance(objContext, Kf.Util.EntityIDEnum.BillGroup).CreateObject();
        //                newGroupEO.SetProperty("GroupID", groupEO.GetProperty("GroupID"));
        //                newGroupEO.SetProperty("SourceEntityID", childEO.EntityMap.EntityID);
        //                newGroupEO.SetProperty("SourceObjectID", childEO.PrimaryKeyValue);
        //                newGroupEO.SetProperty("TargetEntityID", Kf.Util.EntityIDEnum.WHBillDetail);
        //                newGroupEO.SetProperty("TargetObjectID", eoWHDetail.PrimaryKeyValue);

        //                htGroup.Add(eoWHDetail, newGroupEO);
        //            }
        //        }
        //    }

        //    objContext.SaveChanges();

        //    foreach (DictionaryEntry dic in htGroup)
        //    {
        //        EntityObject eoWHBillDetail = (EntityObject)dic.Key;
        //        EntityObject eoGroup = (EntityObject)dic.Value;
        //        eoGroup.SetProperty("TargetObjectID", eoWHBillDetail.PrimaryKeyValue);
        //    }
        //    objContext.SaveChanges();
        }

        static public bool DeleteDiffBill(ObjectContext objContext, EntityObject eo, ref string Msg)
        {
            //if ((int)eo.GetProperty("CheckID") != 0)
            //{
            //    Hashtable htAdBill = new Hashtable();

            //    EntityObjectFactory factory = EntityObjectFactory.GetInstance(objContext, Kf.Util.EntityIDEnum.BillGroup);
            //    foreach (EntityObject childEO in eo.GetChildEntityObjects(eo.EntityMap.DataEntity.ChildDataEntityList[0].EntityID))
            //    {
            //        EntityObject groupEO = factory.FindFirst("TargetEntityID={0} and TargetObjectID={1}", childEO.EntityMap.EntityID, childEO.PrimaryKeyValue);
            //        if (groupEO != null)
            //        {
            //            EntityObjectFactory factory1 = EntityObjectFactory.GetInstance(objContext, Kf.Util.EntityIDEnum.WHBillDetail);
            //            FetchSpecification fs = new FetchSpecification(factory1.EntityMap);
            //            fs.Qualifier = new InQualifier("ID", factory.EntityMap, new Kf.Core.QueryField("TargetObjectID"), new AndQualifier(
            //                         new ColumnQualifier("TargetEntityID", new EqualsPredicate(Kf.Util.EntityIDEnum.WHBillDetail)),
            //                         new ColumnQualifier("GroupID", new EqualsPredicate(groupEO.GetProperty("GroupID")))
            //                         ));
            //            fs.Qualifier = new AndQualifier(fs.Qualifier, new ColumnQualifier("Quantity", new EqualsPredicate(0)));

            //            EntityObjectList lstAdBillDetail = factory1.Find(fs);

            //            foreach (EntityObject eoAdBillDetail in lstAdBillDetail)
            //            {
            //                if ((int)eoAdBillDetail.ParentEntityObject.GetProperty("CheckID") != 0)
            //                {
            //                    Msg = string.Format("关联成本调整单[{0}]已经审核,不能反审核！", eoAdBillDetail.ParentEntityObject.ToString());
            //                    objContext.RejectChanges();
            //                    return false;
            //                }
            //                if (!htAdBill.Contains(eoAdBillDetail.ParentEntityObject))
            //                {
            //                    htAdBill.Add(eoAdBillDetail.ParentEntityObject, null);
            //                }
            //                eoAdBillDetail.Delete();
            //            }
            //        }

            //        foreach (DictionaryEntry dic in htAdBill)
            //        {
            //            EntityObject eoAdBill = (EntityObject)dic.Key;
            //            if (eoAdBill.GetChildEntityObjects(Kf.Util.EntityIDEnum.WHBillDetail).Count == 0)
            //            {
            //                eoAdBill.Delete();
            //            }
            //        }
            //    }
            //}
            return true;
        }

        static public bool IsIncludeCol(ObjectContext objContext, int EntityID, string FieldName)
        {
            EntityObject eoCol = EntityObjectFactory.GetInstance(objContext, Kf.Util.EntityIDEnum.DataEntityCol).FindFirst("EntityID = {0} and FieldName = {1}", EntityID, FieldName);
            return eoCol != null;
        }

        /// <summary>
        /// 比较两个日期的大小，eoDate比arDate大 就返回 False
        /// </summary>
        /// <param name="eoDate"></param>
        /// <param name="arDate"></param>
        /// <returns></returns>
        static public  bool IsValidDate(object eoDate, object arDate)
        {
            if (eoDate != null && arDate != null)
            {
                if (DateTime.Compare(Convert.ToDateTime(eoDate.ToString()), Convert.ToDateTime(arDate.ToString())) > 0)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 判断当前期间是否为启用期间,返回true 就为启用期间
        /// </summary>
        /// <param name="objContext"></param>
        /// <param name="OrgnizationID"></param>
        /// <param name="SystemName"></param>
        /// <returns></returns>
        static public bool IsStartPeriod(ObjectContext objContext, Object OrgnizationID)
        {
            EntityObjectFactory factory = EntityObjectFactory.GetInstance(objContext, Kf.Util.EntityIDEnum.CurrentPeriod);
            EntityObjectList eol = factory.Find("OrganizationID={0} and SystemName = {1}", OrgnizationID, "SCM");

            if (eol.Count < 2)
            {
                return false;
            }
            else
            {
                if ((int)eol[0].GetProperty("Year") == (int)eol[1].GetProperty("Year")
                    && (int)eol[0].GetProperty("Period") == (int)eol[1].GetProperty("Period"))
                {
                    return true;
                }
            }
            return false;
        }

     
    }
}
