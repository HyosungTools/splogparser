using Samples;
using System.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Xml;
using System;
using System.Collections.Generic;

namespace SPLogParserTests
{
   [TestClass]
   public class DataTablesOpsTests
   {
      string cdmTestDataXml =
@"<?xml version=""1.0"" standalone=""yes""?>
<CDM>
  <xs:schema id = ""CDM"" xmlns="""" xmlns:xs=""http://www.w3.org/2001/XMLSchema"" xmlns:msdata=""urn:schemas-microsoft-com:xml-msdata"">
    <xs:element name = ""CDM"" msdata:IsDataSet=""true"" msdata:UseCurrentLocale=""true"">
      <xs:complexType>
        <xs:choice minOccurs = ""0"" maxOccurs=""unbounded"">
          <xs:element name = ""Status"" >
            <xs:complexType>
              <xs:sequence>
                <xs:element name = ""file"" type=""xs:string"" minOccurs=""0"" />
                <xs:element name = ""time"" type=""xs:string"" minOccurs=""0"" />
                <xs:element name = ""error"" type=""xs:string"" minOccurs=""0"" />
                <xs:element name = ""status"" type=""xs:string"" minOccurs=""0"" />
                <xs:element name = ""dispenser"" type=""xs:string"" minOccurs=""0"" />
                <xs:element name = ""intstack"" type=""xs:string"" minOccurs=""0"" />
              </xs:sequence>
            </xs:complexType>
          </xs:element>
        </xs:choice>
      </xs:complexType>
    </xs:element>
  </xs:schema>
  <Status>
    <file>BSTrace.nwlog</file>
    <time>2022/12/01 23:15 18.297</time>
    <error>0</error>
    <status>0</status>
    <dispenser>0</dispenser>
    <intstack>0</intstack>
  </Status>
  <Status>
    <file>BSTrace.nwlog</file>
    <time>2022/12/11 23:15 18.303</time>
    <error>0</error>
    <status>0</status>
    <dispenser>0</dispenser>
    <intstack>1</intstack>
  </Status>
  <Status>
    <file>BSTrace.nwlog</file>
    <time>2022/12/05 23:15 18.312</time>
    <error>0</error>
    <status>0</status>
    <dispenser>1</dispenser>
    <intstack>0</intstack>
  </Status>
  <Status>
    <file>BSTrace.nwlog</file>
    <time>2022/12/17 23:15 18.314</time>
    <error>0</error>
    <status>0</status>
    <dispenser>1</dispenser>
    <intstack>1</intstack>
  </Status>
  <Status>
    <file>BSTrace.nwlog</file>
    <time>2022/12/09 23:15 18.318</time>
    <error>0</error>
    <status>1</status>
    <dispenser>0</dispenser>
    <intstack>0</intstack>
  </Status>
  <Status>
    <file>BSTrace.nwlog</file>
    <time>2022/12/05 23:15 18.451</time>
    <error>0</error>
    <status>1</status>
    <dispenser>0</dispenser>
    <intstack>1</intstack>
  </Status>
  <Status>
    <file>BSTrace.nwlog</file>
    <time>2022/12/23 23:15 18.459</time>
    <error>0</error>
    <status>1</status>
    <dispenser>1</dispenser>
    <intstack>0</intstack>
  </Status>
  <Status>
    <file>BSTrace.nwlog</file>
    <time>2022/12/02 23:15 18.545</time>
    <error>0</error>
    <status>1</status>
    <dispenser>1</dispenser>
    <intstack>1</intstack>
  </Status>
  <Status>
    <file>BSTrace.nwlog</file>
    <time>2022/12/13 23:15 21.679</time>
    <error>1</error>
    <status>0</status>
    <dispenser>0</dispenser>
    <intstack>0</intstack>
  </Status>
  <Status>
    <file>BSTrace.nwlog</file>
    <time>2022/12/18 23:15 21.681</time>
    <error>1</error>
    <status>0</status>
    <dispenser>0</dispenser>
    <intstack>1</intstack>
  </Status>
</CDM>
";
      DataSet dTableSet = null;

      [TestInitialize]
      public void TestInitialize()
      {
         dTableSet = new DataSet();
         dTableSet.ReadXml(XmlReader.Create(new StringReader(cdmTestDataXml)));
      }

      [TestMethod]
      public void TestDataSetSort()
      {

         // sort the status table by time
         dTableSet.Tables["Status"].DefaultView.Sort = "time ASC";

         int rowCount = dTableSet.Tables["status"].Rows.Count;
         DataRowView lastRow;
         DataRowView secondLastRow;

         DataView dataView = new DataView(dTableSet.Tables["Status"]);
         dataView.Sort = "time ASC";

         // prove the sort by walking back up this list and testing that for each row
         // time decreases
         for (int i = rowCount - 1; i > 1; i--)
         {
            lastRow = dataView[i];
            secondLastRow = dataView[i - 1];
            string testline = String.Format("lastRow[{0}] time '{1}', secondlastRow[{2}] time '{3}'!!", i, lastRow["time"].ToString(), i - 1, secondLastRow["time"].ToString());
            Assert.IsTrue(string.Compare(secondLastRow["time"].ToString(), lastRow["time"].ToString()) < 0);
         }
      }

      [TestMethod]
      public void TestDataSetDeleteRows()
      {
         // sort the status table by time
         dTableSet.Tables["Status"].DefaultView.Sort = "time ASC";

         int rowCount = dTableSet.Tables["status"].Rows.Count;

         // Use a DataView to delete rows > 2022/12/15 00:00 00.000
         // Show that delete rows through the DataView, deletes rows in the
         // DataTable. 

         // sort the data view
         DataView dataView = new DataView(dTableSet.Tables["Status"]);
         dataView.Sort = "time ASC";

         // identify the rows to delete
         List<DataRow> deleteRows = new List<DataRow>();
         for (int i = 0; i < rowCount; i++)
         {
            DataRowView viewRow = dataView[i];
            string thisTime = viewRow["time"].ToString();
            if (string.Compare(viewRow["time"].ToString(), "2022/12/15 00:00 00.000") > 0)
            {
               // delete this row
               deleteRows.Add(viewRow.Row);
            }
         }

         // delete the rows - You cannot modify a collection while you're iterating
         // on it using a foreach statement. The MS document suggests you can call delete within
         // the above loop but in testing you can't
         foreach (DataRow dataRow in deleteRows)
         {
            dataRow.Delete();
         }

         // Delete 'marks' for delete, AcceptChanges actually deletes. 
         dTableSet.Tables["Status"].AcceptChanges();

         // assert that there are no rows with a date > > 2022/12/15 00:00 00.000 in the data table
         int rowCount2 = dTableSet.Tables["status"].Rows.Count;
         foreach (DataRow dataRow in dTableSet.Tables["status"].Rows)
         {
            string thisTime = dataRow["time"].ToString();
            Assert.IsTrue(string.Compare(dataRow["time"].ToString(), "2022/12/15 00:00 00.000") < 0); 
         }
      }

      private void PassTableByReference(DataTable localTable)
      {
         DataRow dataRow = localTable.Rows[0];
         dataRow.Delete();
         localTable.AcceptChanges(); 
      }

      [TestMethod]
      public void TestDataSetPassTableByReferece()
      {
         // show that you can pass a table by reference
         DataSet dLocalTableSet = new DataSet();
         dLocalTableSet.ReadXml(XmlReader.Create(new StringReader(cdmTestDataXml)));

         int rowCountBefore = dLocalTableSet.Tables["Status"].Rows.Count;
         PassTableByReference(dLocalTableSet.Tables["Status"]);
         
         int rowCountAfter = dLocalTableSet.Tables["Status"].Rows.Count;
         Assert.IsTrue(rowCountBefore > rowCountAfter);
      }
   }
}
