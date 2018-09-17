using System;
using System.Collections.Generic;
using System.IO;
using BookMyShow.CommonLibrary;

class Program
{
    const string udcErrorSource = "Program";

    static void Main(string[] args)
    {
        const string udcErrorMethod = "Main";

        bool blnMailFiles = true;
        string strFileType = "S";      

        try
        {
            long lngimintUpdateStamp = 0;
            if (clsimintSettlementProcessor.blnGenerateimintSettlementFile(out lngimintUpdateStamp) == true)
            {
                clsDBEngine objDB = new clsDBEngine();

                string strSQL = "UPDATE tblControl SET Control_intValue = " + lngimintUpdateStamp.ToString() + " WHERE UPPER(Control_strName) = 'IMINTUPDATESTAMP'";
                if (objDB.blnExecute(strSQL) == false)
                {
                    clsLog.blnLogError(udcErrorSource, udcErrorMethod, "Error updating Control Value :: " + lngimintUpdateStamp.ToString(), objDB.strErrorMessage, false);
                }

                objDB.blnCloseConnection();
            }

            string strDirectoryPath = System.AppDomain.CurrentDomain.BaseDirectory + "imint\\Settlement\\";
            string strRefundDirectoryPath = System.AppDomain.CurrentDomain.BaseDirectory + "imint\\Refund\\";
            string strDestinationPath = System.AppDomain.CurrentDomain.BaseDirectory + "imint\\Mail\\";

            while (blnMailFiles == true)
            {
                DirectoryInfo objDir = new DirectoryInfo(strDirectoryPath);
                FileInfo[] objFileInfo = objDir.GetFiles();

                for (int intCount = 0; intCount < objFileInfo.Length; intCount++)
                {
                    if (clsimintSettlementProcessor.blnMailimintSettlementFile(objFileInfo[intCount].Name, objFileInfo[intCount].FullName, strFileType) == true)
                    {
                        objFileInfo[intCount].MoveTo(strDestinationPath + objFileInfo[intCount].Name);
                    }
                }

                if (strDirectoryPath != strRefundDirectoryPath)
                {
                    strDirectoryPath = strRefundDirectoryPath;
                    strFileType = "R";
                }
                else
                {
                    blnMailFiles = false;
                }

                objFileInfo = null;
                objDir = null;
            }
        }
        catch (Exception ex)
        {
            clsLog.blnLogError(udcErrorSource, udcErrorMethod, "Error in Generating imintSettlement File", ex.ToString(), false);
        }
    }
}
