using System;
using System.Collections;
using System.IO;
using BookMyShow.CommonLibrary;

internal static class clsimintSettlementProcessor
{
    const string udcErrorSource = "clsimintSettlementProcessor";
    static string strFolder = System.AppDomain.CurrentDomain.BaseDirectory + "imint\\Settlement\\";

    internal static bool blnGenerateimintSettlementFile(out long lngimintUpdateStamp)
    {
        const string udcErrorMethod = "blnGenerateimintSettlementFile";

        string udcMerchantId = clsSettings.Get("IMINT", "MerchantId", "90009796");
        string udcTerminalId = clsSettings.Get("IMINT", "TerminalId", "64211677");
        const string udcActionCode = "EARN";
        //const string udcClassificationCode = "CASH_CASH";
        const string udcPaymentType = "CASH";
        const string udcUnits = "AMOUNT";
        const string udcHasSKUActivity = "0";

        lngimintUpdateStamp = 0;

        try
        {
            if (System.IO.Directory.Exists(strFolder) == false)
            {
                System.IO.Directory.CreateDirectory(strFolder);
            }
        }
        catch (Exception ex)
        {
            clsLog.blnLogError(udcErrorSource, udcErrorMethod, "Error creating imint Directory : " + strFolder, ex.ToString());
            return false;
        }

        string strFileName = "", strSettlementDate = "";
        ArrayList arlSettlement = new ArrayList();
        ArrayList arlPaymentId = new ArrayList();
        clsDBEngine objDB = new clsDBEngine();
        StreamWriter stwSettlement;
        System.Text.StringBuilder sbrData;
        string strSQL = "EXECUTE spGetDataForIMINT";

        if (objDB.blnOpenResultSet(strSQL) == true)
        {
            if (objDB.blnHasRecords == true)
            {
                strSettlementDate = DateTime.Now.ToString("ddMMyyyy");
            }
            
            while (objDB.blnResultsMoveNextRow() == true)
            {

                if (objDB.objResultsValue("Trans_MemberType").ToString() == "IMINT")
                {
                    sbrData = new System.Text.StringBuilder();
                    sbrData.Append(objDB.objResultsValue("Trans_MemberNo").ToString() + "|");
                    sbrData.Append("|||");
                    sbrData.Append(udcMerchantId + "|");
                    sbrData.Append("|");
                    sbrData.Append(udcTerminalId + "|");
                    sbrData.Append("|");
                    sbrData.Append((objDB.objResultsValue("Trans_lngId").ToString() + "|"));
                    sbrData.Append("||");
                    sbrData.Append(udcActionCode + "|");
                    //sbrData.Append(udcClassificationCode + "|");
                    sbrData.Append(objDB.objResultsValue("Trans_strIMINTEventType").ToString() + "|");
                    sbrData.Append("||");
                    sbrData.Append(udcPaymentType + "|");
                    sbrData.Append(objDB.objResultsValue("Trans_strDate").ToString() + "|");
                    sbrData.Append("|");
                    sbrData.Append(float.Parse(objDB.objResultsValue("Trans_mnyTotal").ToString()).ToString("#") + "|");
                    sbrData.Append(udcUnits + "|");
                    sbrData.Append("||");
                    sbrData.Append(udcHasSKUActivity + "|");
                    sbrData.Append(strSettlementDate + "|");
                    sbrData.Append((objDB.objResultsValue("Trans_lngId").ToString() + "|"));
                    sbrData.Append("|||");
                    sbrData.Append((objDB.objResultsValue("Trans_lngId").ToString() + "|"));
                    sbrData.Append("||||||||");
                    arlSettlement.Add(sbrData);
                }
           
                else if (objDB.objResultsValue("Trans_MemberType").ToString() == "PB_SAM")
                {
                    //Add Payback Samsung Base points

                    sbrData = new System.Text.StringBuilder();
                    sbrData.Append(objDB.objResultsValue("Trans_MemberNo").ToString() + "|");
                    sbrData.Append("|||");
                    sbrData.Append(udcMerchantId + "|");
                    sbrData.Append("|");
                    sbrData.Append(udcTerminalId + "|");
                    sbrData.Append("|");
                    sbrData.Append((objDB.objResultsValue("Trans_lngId").ToString() + "|"));
                    sbrData.Append("||");
                    sbrData.Append(udcActionCode + "|");
                    sbrData.Append(objDB.objResultsValue("Trans_strIMINTEventType").ToString() + "|");
                    sbrData.Append("||");
                    sbrData.Append(udcPaymentType + "|");
                    sbrData.Append(objDB.objResultsValue("Trans_strDate").ToString() + "|");
                    sbrData.Append("|");
                    sbrData.Append(float.Parse(objDB.objResultsValue("Trans_mnyTotal").ToString()).ToString("#") + "|");
                    sbrData.Append(udcUnits + "|");
                    sbrData.Append("||");
                    sbrData.Append(udcHasSKUActivity + "|");
                    sbrData.Append(strSettlementDate + "|");
                    sbrData.Append((objDB.objResultsValue("Trans_lngId").ToString()+"_Samsung_base_point|"));
                    sbrData.Append("|||");
                    sbrData.Append((objDB.objResultsValue("Trans_lngId").ToString()+"|"));
                    sbrData.Append("||||||||");
                    arlSettlement.Add(sbrData);


                    //Add Payback Samsung 2X points

                    sbrData = new System.Text.StringBuilder();
                    sbrData.Append(objDB.objResultsValue("Trans_MemberNo").ToString() + "|");
                    sbrData.Append("|||");
                    sbrData.Append(udcMerchantId + "|");
                    sbrData.Append("|");
                    sbrData.Append(udcTerminalId + "|");
                    sbrData.Append("|");
                    sbrData.Append((objDB.objResultsValue("Trans_lngId").ToString() + "|"));
                    sbrData.Append("||");
                    sbrData.Append(udcActionCode + "|");
                    //sbrData.Append(udcClassificationCode + "|");
                    sbrData.Append("BONUS_POINTS|");

                    sbrData.Append("||");
                    sbrData.Append(udcPaymentType + "|");
                    sbrData.Append(objDB.objResultsValue("Trans_strDate").ToString() + "|");
                    sbrData.Append("|");
                    sbrData.Append(float.Parse(objDB.objResultsValue("Trans_mnyTotal").ToString()).ToString("#") + "|");
                    sbrData.Append(udcUnits + "|");
                    sbrData.Append("||");
                    sbrData.Append(udcHasSKUActivity + "|");
                    sbrData.Append(strSettlementDate + "|");

                    sbrData.Append((objDB.objResultsValue("Trans_lngId").ToString() + "_Samsung_bonus_point|"));
                    sbrData.Append("|||");
                    sbrData.Append((objDB.objResultsValue("Trans_lngId").ToString() + "|"));
                    sbrData.Append("||||||||");
                    arlSettlement.Add(sbrData);
                }


            }

            objDB.blnMoveNextResultSet();

            objDB.blnResultsMoveNextRow();

            lngimintUpdateStamp = long.Parse(objDB.objResultsValue("Trans_intUpdateStamp").ToString());

            if (strSettlementDate.Length > 0)
            {
                strFileName = strFolder + "ACTIVITY_IMINT_BIGTREE_" + strSettlementDate + ".txt";
            }
        }
        else
        {
            objDB.blnCloseConnection();
            clsLog.blnLogError(udcErrorSource, udcErrorMethod, "Error fetching imint Settlement Data from DB", objDB.strErrorMessage);
            return false;
        }

        objDB.blnCloseConnection();

        if (strFileName.Length > 0)
        {
            try
            {
                stwSettlement = new StreamWriter(strFileName, false);

                for (int intCount = 0; intCount < arlSettlement.Count; intCount++)
                {
                    stwSettlement.WriteLine(arlSettlement[intCount].ToString());
                }
                stwSettlement.Close();
                stwSettlement = null;
            }
            catch (Exception ex)
            {
                stwSettlement = null;
                clsLog.blnLogError(udcErrorSource, udcErrorMethod, "Error creating imint Settlement File", ex.ToString());
                return false;
            }
        }

        return true;
    }

    internal static bool blnMailimintSettlementFile(string strFileName, string strFilePath, string strFileType)
    {
        const string udcErrorMethod = "blnMailimintSettlementFile";

        string strEmailList = "", strEmailListCC = "";
        bool blnReturn = false;

        try
        {
            clsMail objMail = new clsMail();
            if (strFileType == "S")
            {
                strEmailList = clsSettings.Get("IMINT", "EmailList", "");
                strEmailListCC = clsSettings.Get("IMINT", "EmailListCC", "");
            }
            else if (strFileType == "R")
            {
                strEmailList = clsSettings.Get("IMINT", "RefundEmailList", "");
                strEmailListCC = clsSettings.Get("IMINT", "RefundEmailListCC", "");
            }

            foreach (string strEmailId in strEmailList.Split(new char[] { '|' }))
            {
                if (strEmailId.Length > 0)
                {
                    objMail.blnAddTo("3i-infotech", strEmailId);
                }
            }

            foreach (string strEmailIdCC in strEmailListCC.Split(new char[] { '|' }))
            {
                if (strEmailIdCC.Length > 0)
                {
                    objMail.blnAddTo("imint", strEmailIdCC);
                }
            }

            objMail.blnAddFrom("BookMyShow", "tickets@bookmyshow.com");
            objMail.MailPriority = System.Net.Mail.MailPriority.High;
            objMail.blnAddAttachment(strFilePath);
            blnReturn = objMail.blnSendMail(strFileName, strFileName);
            objMail = null;
        }
        catch (Exception ex)
        {
            blnReturn = false;
            clsLog.blnLogError(udcErrorSource, udcErrorMethod, "Error mailing imint Settlement File : " + strFileName, ex.ToString());
        }
        return blnReturn;
    }
}
