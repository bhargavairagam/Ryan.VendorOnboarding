using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Office.Interop.Excel;
using LinqToExcel;
using BuildingConnected.API.VendorList.Domain.Entities;
using System.Data;
using BuildingConnected.API.VendorList.Domain.Concrete;

namespace BuildingConnected.API.VendorList
{
    public static class JDEdwardsVendorInfo
    {
        public static async Task<bool> ImportJDEVendorData()
        {
            //string csv_file_path = "C:\\Users\\biragam\\Desktop\\JDE_Vendors_New.xslx";
             string pathToExcelFile = @"C:\Users\biragam\Documents\VendorDocs\JDEVendors.xls";
             VendorAddressContactsRespositary varep = new VendorAddressContactsRespositary();

            System.Data.DataTable dt = new System.Data.DataTable();
            dt.Columns.Add("Name", typeof(String));
            dt.Columns.Add("PHONE", typeof(String));
            dt.Columns.Add("AREA", typeof(String));
            dt.Columns.Add("ADDR1", typeof(String));
            dt.Columns.Add("ADDR2", typeof(String));
            dt.Columns.Add("CITY", typeof(String));
            dt.Columns.Add("ST", typeof(String));
            dt.Columns.Add("ZIP", typeof(String));
            dt.Columns.Add("TAX", typeof(String));
            dt.Columns.Add("VendorID", typeof(String));
            dt.Columns.Add("CONTACT", typeof(String));
            dt.Columns.Add("EMAIL", typeof(String));


            try
            {
                //var excelFile = new ExcelQueryFactory(pathToExcelFile);
                //var artistAlbums = from a in excelFile.Worksheet<JDEVendor>() select a;

                ConnexionExcel ConxObject = new ConnexionExcel(pathToExcelFile);
                dynamic query = from m in ConxObject.UrlConnexion.Worksheet<JDEVendor>().ToList()
                             group m by new { m.TAX } into g
                             select new { NAME = g.First().NAME, TAX = g.First().TAX, ST = g.First().ST, CITY = g.First().CITY, ZIP = g.First().ZIP, ADDR1 = g.First().ADDR1, ADDR2 = g.First().ADDR2, VendorID = g.First().VendorID, AREA = g.First().AREA, PHONE = g.First().PHONE, KeyCols = g.ToList() };


                //var product = from x in ConxObject.UrlConnexion.Worksheet<JDEVendor>().ToList()
                //              group x x.VendorID into g_pn
                //              select g_pn.First().InternalPN, g_pn.First().Name, g_pn.Sum(x => x.Qty);

                //var query = from m in ConxObject.UrlConnexion.Worksheet<JDEVendor>()
                //            select m;

                //var results = from p in ConxObject.UrlConnexion.Worksheet<JDEVendor>().ToList()
                //              group p by p.TAX into g

                //              select new { TAX = g.Key, records = g.ToList() };

                //var result = query.ToList().GroupBy(t => new { t.VendorID, t.TAX }).Select(t => new
                //{
                //    T = t.Key.TAX,
                //    VID = t.Key.VendorID,
                //    njk = t.Select(x=>x.NAME.FirstOrDefault()),

                //    contactemails = string.Join("|", t.Select(x => x.EMAIL))
                //}).ToList();
                int j = 0;

                foreach (var result in query)
                {
                    j++;
                    if (result.TAX.Trim() == "")
                    {
                        continue;
                    }
                    
                    string products = "Counter : {2} ,TAXID : {0}, VendorName: {1} ";
                    Console.WriteLine(string.Format(products, result.TAX, result.NAME , j));

                    string taxid = Convert.ToString(result.TAX);

                    // check if this 
                    VendorAddressContacts vacheck = varep.GetVendorAddressContacts(taxid.Trim());

                    if (vacheck is null)
                    {

                        VendorAddressContacts vc = new VendorAddressContacts();
                        vc.VendorName = result.NAME != null ? result.NAME.Trim() : string.Empty;
                        vc.VendorPhone = "(" + (result.AREA != null ? result.AREA.Trim() : string.Empty) + ") " + (result.PHONE != null ? result.PHONE.Trim() : string.Empty);
                        vc.City = result.CITY != null ? result.CITY.Trim() : string.Empty;
                        vc.StName = result.ADDR1 != null ? result.ADDR1.Trim() : string.Empty;
                        vc.StNumber = result.ADDR2 != null ? result.ADDR2.Trim() : string.Empty;
                        vc.Stat = result.ST != null ? result.ST.Trim() : string.Empty;
                        vc.VendorEIN = result.TAX != null ? result.TAX.Trim() : string.Empty;
                        vc.Zip = result.ZIP != null ? result.ZIP.Trim() : string.Empty;
                        vc.JDEVendorID = result.VendorID != null ? result.VendorID.Trim() : string.Empty;
                        vc.SourceType = "JDE";

                        int i = 0;
                        foreach (var res in result.KeyCols)
                        {
                            i++;

                            if (i > 5)
                            {
                                break;
                            }

                            if(i==1)
                            {
                               
                                vc.FirstContactEmail = res.EMAIL;
                                
                                vc.FirstContactName = res.CONTACT;
                                vc.FirstContactPhone = "(" + (res.AREA != null ? res.AREA.Trim() : string.Empty) + ") " + (res.PHONE != null ? res.PHONE.Trim() : string.Empty);

                            }

                            if(i==2)
                            {
                                
                                vc.SecondContactEmail = res.EMAIL;
                                vc.SecondContactName = res.CONTACT;
                                vc.SecondContactPhone = "(" + (res.AREA != null ? res.AREA.Trim() : string.Empty) + ") " + (res.PHONE != null ? res.PHONE.Trim() : string.Empty);
                            }

                            if (i == 3)
                            {
                                
                                vc.ThirdContactEmail = res.EMAIL;
                               vc.ThirdContactPhone = "(" + (res.AREA != null ? res.AREA.Trim() : string.Empty) + ") " + (res.PHONE != null ? res.PHONE.Trim() : string.Empty);
                                vc.ThirdContactName = res.CONTACT;
                               
                            }

                            if (i == 4)
                            {

                                vc.FourthContactEmail = res.EMAIL;
                                vc.FourthContactPhone = "(" + (res.AREA != null ? res.AREA.Trim() : string.Empty) + ") " + (res.PHONE != null ? res.PHONE.Trim() : string.Empty);
                                vc.FourthContactName = res.CONTACT;

                            }

                            if (i == 5)
                            {

                                vc.FifthContactEmail = res.EMAIL;
                                vc.FifthContactPhone = "(" + (res.AREA != null ? res.AREA.Trim() : string.Empty) + ") " + (res.PHONE != null ? res.PHONE.Trim() : string.Empty);
                                vc.FifthContactName = res.CONTACT;

                            }

                        }


                        // first contact eamil
                        var x = await varep.SaveVendorAddressContactsAsync(vc);

                    }
                    else
                    {
                        DataRow dtr = dt.NewRow();
                        dtr["Name"] = result.NAME;
                        
                        dtr["VendorID"] = result.VendorID;
                        dtr["ADDR1"] = result.ADDR1;
                        dtr["ADDR2"] = result.ADDR2;
                        dtr["CITY"] = result.CITY;
                        dtr["ZIP"] = result.ZIP;
                        dtr["TAX"] = result.TAX;
                        dtr["ST"] = result.ST;
                        
                        
                        dt.Rows.Add(dtr);

                    }

                }
                Console.ReadKey();


            }
            catch (Exception ex)
            {
            }


            return true;

        }




    }


    public class ConnexionExcel
    {
        public string _pathExcelFile;
        public ExcelQueryFactory _urlConnexion;
        public ConnexionExcel(string path)
        {
            this._pathExcelFile = path;
            this._urlConnexion = new ExcelQueryFactory(_pathExcelFile);
        }
        public string PathExcelFile
        {
            get
            {
                return _pathExcelFile;
            }
        }
        public ExcelQueryFactory UrlConnexion
        {
            get
            {
                return _urlConnexion;
            }
        }
    }


    public class JDEVendor
    {
        public string VendorID
        {
            get;
            set;
        }
       
        public string TAX
        {
            get;
            set;
        }
        public string NAME
        {
            get;
            set;
        }
        public string ADDR1
        {
            get;
            set;
        }
        public string ADDR2
        {
            get;
            set;
        }
        public string CITY
        {
            get;
            set;
        }
        public string ST
        {
            get;
            set;
        }
        public string ZIP
        {
            get;
            set;
        }
        public string AREA
        {
            get;
            set;
        }
        public string PHONE
        {
            get;
            set;
        }
        public string CONTACT
        {
            get;
            set;
        }
        public string EMAIL
        {
            get;
            set;
        }
    }
}
