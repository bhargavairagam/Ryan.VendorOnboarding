using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Web;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using RestSharp;
using RestSharp.Authenticators;
using Newtonsoft.Json.Linq;
using BuildingConnected.API.VendorList.Domain.Entities;
using BuildingConnected.API.VendorList.Domain.Concrete;
using System.Diagnostics;



namespace BuildingConnected.API.VendorList
{
    class Program
    {
        static string clientId = "5a832c56174e9b000f08e6ee";
        static string clientSecret = "8455ead5-af80-4789-85ca-e72d5a0cfd98";

       static VendorProfileRepositary vf = new VendorProfileRepositary();
       static VendorAddressContactsRespositary va = new VendorAddressContactsRespositary();

        // The server base address
        static string baseUrl = "https://app.buildingconnected.com/api-beta/auth/token/";

        // this will hold the Access Token returned from the server.
        static string accessToken = null;

        static int i = 0;

        // starting here
        static  void Main(string[] args)
        {
            //VendorProfileRepositary vf = new VendorProfileRepositary();
            //var prof = vf.GetVendorDetials("2322333");

            Console.WriteLine("Starting ...");
            try
            {
                // load data into VendorProfileNew table
              //  LoadVendorInfoTable.LoadVendorInfoFromMaster();


                // Import data from CSV to SQL
                // JDEdwardsVendorInfo.ImportJDEVendorData();

                // adress contacts from building connected
                SaveVendorAddressContactstoMasterDB();


                // Modified Not needed at this time
                 //GetAccessTokenFromBuildingConnected();

            }
            catch(Exception ex)
            {

            }
            Console.ReadLine();
        }



        private static async Task<bool> SaveVendorAddressContactstoMasterDB()
        {


            // Get the Access Token.
            accessToken = GetAccessToken();
            Console.WriteLine(accessToken != null ? "Got Token" : "No Token found");

            // Get the Articles
            Console.WriteLine();
            Console.WriteLine("Getting contacts from building connected.!!");

            // loop until you reach end of page... we have 30k records in BC as of today sep,2018
            for (var page = 1; page < 340; page++)
            {
                Console.WriteLine("Getting contacts from building connected.!! Page ======= " + page.ToString());
                var client = new RestClient("https://app.buildingconnected.com/api-beta/contacts?limit=100&page=" + page.ToString());
                RestRequest request = new RestRequest() { Method = Method.GET };
                request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
                request.AddParameter("Authorization", string.Format("Bearer {0}", accessToken), ParameterType.HttpHeader);
                request.AddHeader("Accept", "application/json");
                var response = client.Execute(request);

                // dynamic data1 = JObject.Parse(response.Content);
                dynamic data = JsonConvert.DeserializeObject(response.Content);

                if (data.results.Count > 0)
                {


                    foreach (var Token in data.results)
                    {
                        VendorAddressContacts vc = new VendorAddressContacts();
                        i++;
                        Console.WriteLine(" {0}  record added to DB", i.ToString());
                      
                    
                        try
                        {
                            if(Token == null)
                            {
                                continue;
                            }

                            string pqrelationshipid = string.Empty;

                            if (Token.prequal != null)
                            {
                                dynamic d = Token.prequal;
                                pqrelationshipid = d.pqRelationshipId;
                            }

                            // dynamic pqdetails = Token.qualification;
                            dynamic office = Token.vendorOffice;
                            dynamic stats = Token.stats;
                            dynamic BusinessType = Token.stats;

                            dynamic keywords = office["keywords"];
                            string keys1 = "";
                            string keys2 = "";
                           

                            for(int i=0;i< keywords.Count; i++)
                            {
                                if(i< ( keywords.Count/2))
                                {
                                    keys1 = keys1 + (string) keywords[i] + ",";
                                }
                                else
                                {
                                    keys2 = keys2 + (string)keywords[i] + ",";
                                }

                            }

                            dynamic bustype = Token["vendorCompany"]["businessType"];
                            string strbus = "";
                            foreach(var bus in bustype)
                            {
                                strbus = strbus + bus + ", ";
                            }


                            vc.VendorAddress = (string)office["location"]["complete"];
                            vc.VendorName = (string)Token["vendorCompany"]["name"];
                            vc.VendorPhone = (string)office["phone"];
                            vc.City = (string)office["location"]["city"];
                            vc.StName = (string)office["location"]["streetName"];
                            vc.StNumber = (string)office["location"]["streetNumber"];
                            vc.Stat = (string)office["location"]["state"];
                            vc.VendBusiness1 = keys1;
                            vc.VendBusiness2 = keys2;
                            vc.CoordsLat = (string)office["location"]["coords"]["lat"];
                            vc.CoordsLong = (string)office["location"]["coords"]["lng"];
                            vc.Zip = (string)office["location"]["zip"];
                            vc.VendorAverageRating = Token.vendorAverageRating;
                            vc.Website = ((string)Token["vendorCompany"]["website"]);
                            vc.BusinessType = strbus;
                            // stats details
                            vc.InviteCount = (string)Token["stats"]["inviteCount"];
                            vc.ViewedCount = (string)Token["stats"]["viewedCount"];
                            vc.ViewedPercent = (string)Token["stats"]["viewedPercent"];
                            vc.BiddingCount = (string)Token["stats"]["biddingCount"];
                            vc.BiddingPercent = (string)Token["stats"]["biddingPercent"];
                            vc.BidCount = (string)Token["stats"]["bidCount"];
                            vc.BidPercent = (string)Token["stats"]["bidPercent"];
                            vc.AwardedCount = (string)Token["stats"]["awardedCount"];
                            vc.AwardedPercent = (string)Token["stats"]["awardedPercent"];
                            vc.DeclinedPercent = (string)Token["stats"]["declinedPercent"];
                            vc.DeclinedCount = (string)Token["stats"]["declinedCount"];


                            List<string> qualifInfo = new List<string>();
                            string federalid = "";


                            if (!string.IsNullOrEmpty(pqrelationshipid))
                            {

                                string qualificationUrl = "https://app.buildingconnected.com/api-beta/qm-submissions?pqRelationshipId=" + pqrelationshipid;
                                var pqclient = new RestClient(qualificationUrl);
                                RestRequest pqrequest = new RestRequest() { Method = Method.GET };
                                pqrequest.AddHeader("Content-Type", "application/x-www-form-urlencoded");
                                pqrequest.AddParameter("Authorization", string.Format("Bearer {0}", accessToken), ParameterType.HttpHeader);
                                pqrequest.AddHeader("Accept", "application/json");
                                dynamic pqresponse = pqclient.Execute(pqrequest);

                               // dynamic data = JsonConvert.DeserializeObject(pqresponse.Content);
                                

                                if (pqresponse.StatusDescription == "OK")   
                                {

                                    JObject data1 = JObject.Parse(pqresponse.Content);
                                    // federal id
                                    JToken acme = data1.SelectToken("$.sections[?(@.sectionType == 'FINANCIALS')]");
                                    dynamic qu = acme["questions"][0];
                                    federalid = qu.value;
                                    vc.VendorEIN = federalid;

                                    if (federalid == "06-1738359" || federalid == "87-0572162" || federalid == "26-4661988")
                                    {
                                        federalid = qu.value;
                                    }

                                    // email id
                                    JToken compProf = data1.SelectToken("$.sections[?(@.sectionType == 'COMPANY_PROFILE')]");
                                    dynamic qclist = compProf.SelectToken("$.questions[?(@.responseType == 'CONTACT_LIST')]");

                                    if (qclist["value"].Count > 0)
                                    {
                                        
                                        
                                       
                                        vc.FirstContactEmail = (string)qclist["value"][0]["email"];
                                        vc.FirstContactName = (string)qclist["value"][0]["name"];
                                        vc.FirstContactPhone = (string)qclist["value"][0]["phone"];


                                    }

                                    if (qclist["value"].Count > 1)
                                    {
                                        vc.SecondContactEmail = (string)qclist["value"][1]["email"];
                                        vc.SecondContactName = (string)qclist["value"][1]["name"];
                                        vc.SecondContactPhone = (string)qclist["value"][1]["phone"];
                                    }


                                    if (qclist["value"].Count > 2)
                                    {
                                        vc.ThirdContactEmail = (string)qclist["value"][2]["email"];
                                        vc.ThirdContactName = (string)qclist["value"][2]["name"];
                                        vc.ThirdContactPhone = (string)qclist["value"][2]["phone"];
                                    }

                                    if (qclist["value"].Count > 3)
                                    {
                                        vc.FourthContactEmail = (string)qclist["value"][3]["email"];
                                        vc.FourthContactName = (string)qclist["value"][3]["name"];
                                        vc.FourthContactPhone = (string)qclist["value"][3]["phone"];
                                    }

                                    if (qclist["value"].Count > 4)
                                    {
                                        vc.FifthContactEmail = (string)qclist["value"][4]["email"];
                                        vc.FifthContactName = (string)qclist["value"][4]["name"];
                                        vc.FifthContactPhone = (string)qclist["value"][4]["phone"];
                                    }

                                

                                    try
                                    {
                                         var x = await va.SaveVendorAddressContactsAsync(vc);
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine(" {0}  Exception occured", ex.Message);
                                        Console.WriteLine(" address ", vc.BusinessType);
                                        Console.WriteLine(" name ", vc.VendorName);
                                        Console.WriteLine(" web", vc.Website);
                                        Console.WriteLine(" bus2 ", vc.VendBusiness1);
                                        Console.WriteLine(" bus1 ", vc.VendBusiness2);

                                    }

                                }
                                else
                                {
                                    try
                                    {
                                        var x = await va.SaveVendorAddressContactsAsync(vc);
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine(" {0}  Exception occured", ex.Message);
                                        Console.WriteLine(" address ", vc.BusinessType);
                                        Console.WriteLine(" name ", vc.VendorName);
                                        Console.WriteLine(" web", vc.Website);
                                        Console.WriteLine(" bus2 ", vc.VendBusiness1);
                                        Console.WriteLine(" bus1 ", vc.VendBusiness2);

                                    }




                                     
                                }


                            }
                            else
                            {

                                try
                                {
                                    var x = await va.SaveVendorAddressContactsAsync(vc);
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(" {0}  Exception occured", ex.Message);
                                    Console.WriteLine(" address ", vc.BusinessType);
                                    Console.WriteLine(" name ", vc.VendorName);
                                    Console.WriteLine(" web", vc.Website);
                                    Console.WriteLine(" bus2 ", vc.VendBusiness1);
                                    Console.WriteLine(" bus1 ", vc.VendBusiness2);

                                }
                            }

                        }
                        catch (Exception ex)
                        {
                            Console.Write("Exception occured !! ----  {0}", ex.Message);
                         
                            continue;

                        }
                    }




                }

            }
            Console.WriteLine(" End of program..!!!!");

            return true;
           

        }




        /// <summary>
        /// This method does all the work to get an Access Token and read the first page of
        /// Articles from the server.
        /// </summary>
        /// <returns></returns>
        private static string GetAccessTokenFromBuildingConnected()
        {
            // Get the Access Token.
            accessToken =  GetAccessToken();
            Console.WriteLine(accessToken != null ? "Got Token" : "No Token found");

            // Get the Articles
            Console.WriteLine();
            Console.WriteLine("Getting contacts from building connected.!!");

            // loop until you reach end of page... we have 30k records in BC as of today sep,2018
            for( var page =1;page<1000; page++)
            {
                Console.WriteLine("Getting contacts from building connected.!! Page ======= " + page.ToString());
                var client = new RestClient("https://app.buildingconnected.com/api-beta/contacts?limit=31&page=" + page.ToString());
                RestRequest request = new RestRequest() { Method = Method.GET };
                request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
                request.AddParameter("Authorization", string.Format("Bearer {0}", accessToken), ParameterType.HttpHeader);
                request.AddHeader("Accept", "application/json");
                var response = client.Execute(request);

                // dynamic data1 = JObject.Parse(response.Content);
                dynamic data = JsonConvert.DeserializeObject(response.Content);

                if (data.results.Count > 0)
                {
                   

                    foreach (var Token in data.results)
                    {
                        try
                        {
                            dynamic d = Token.prequal;

                            
                                // dynamic pqdetails = Token.qualification;
                                string pqrelationshipid = d.pqRelationshipId;

                                dynamic office = Token.vendorOffice;
                                string name = (string)Token["vendorCompany"]["name"];
                                string addre = (string)office["location"]["complete"] ;
                                string phone = (string)office["phone"];
                           

                            SaveAddressContactsTpDB(accessToken, pqrelationshipid, name, addre,phone);

                        }
                        catch (Exception ex)
                        {
                            Console.Write("Exception occured !! ----  {0}", ex.Message);
                            continue;
                           
                        }
                    }



                   
                }

            }


            Console.WriteLine(" End of program..!!!!");

            return accessToken;
        }


        /// <summary>
        /// Get Federal ID
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="pqrelationshipid"></param>
        /// <returns></returns>
        private static List<string> GetFederalIDFromAPI(string accessToken,string pqrelationshipid,string vendName, string addr)
        {
            List<string> qualifInfo = new List<string>();
            string federalid = "";
            string qualificationUrl = "https://app.buildingconnected.com/api-beta/qm-submissions?pqRelationshipId=" + pqrelationshipid;
            var client = new RestClient(qualificationUrl);
            RestRequest request = new RestRequest() { Method = Method.GET };
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            request.AddParameter("Authorization", string.Format("Bearer {0}", accessToken), ParameterType.HttpHeader);
            request.AddHeader("Accept", "application/json");
            dynamic response = client.Execute(request);

            dynamic data = JsonConvert.DeserializeObject(response.Content);

            if (response.StatusDescription == "OK" )
            {
                VendorAddressContacts vc = new VendorAddressContacts();
                JObject data1 = JObject.Parse(response.Content);
                // federal id
                JToken acme = data1.SelectToken("$.sections[?(@.sectionType == 'FINANCIALS')]");
                dynamic qu = acme["questions"][0];
                federalid = qu.value;
                

                // email id
                JToken compProf = data1.SelectToken("$.sections[?(@.sectionType == 'COMPANY_PROFILE')]");
                dynamic qclist = compProf.SelectToken("$.questions[?(@.responseType == 'CONTACT_LIST')]");

                if(qclist.Count > 0)
                {
                    vc.VendorAddress = addr;
                    vc.VendorEIN = "";
                    vc.VendorName = vendName;
                    vc.FirstContactEmail = (string)qclist["value"][0]["email"];
                    vc.FirstContactName = (string)qclist["value"][0]["name"];
                    vc.FirstContactPhone = (string)qclist["value"][0]["phone"];

                }

                if (qclist.Count > 1)
                {
                    vc.SecondContactEmail = (string)qclist["value"][1]["email"];
                    vc.SecondContactName = (string)qclist["value"][1]["name"];
                    vc.SecondContactPhone = (string)qclist["value"][1]["phone"];
                }


                if (qclist.Count > 2)
                {
                    vc.SecondContactEmail = (string)qclist["value"][2]["email"];
                    vc.SecondContactName = (string)qclist["value"][2]["name"];
                    vc.SecondContactPhone = (string)qclist["value"][2]["phone"];
                }

                if (qclist.Count > 2)
                {
                    vc.SecondContactEmail = (string)qclist["value"][2]["email"];
                    vc.SecondContactName = (string)qclist["value"][2]["name"];
                    vc.SecondContactPhone = (string)qclist["value"][2]["phone"];
                }

                //string email = (string)qclist["value"][0]["email"];
                //string name = (string)qclist["value"][0]["name"];
                //string phone = (string)qclist["value"][0]["phone"];



                //qualifInfo.Add(federalid);
                //qualifInfo.Add(email);
                //qualifInfo.Add(name);
                //qualifInfo.Add(phone);


                return qualifInfo;
            }

            return qualifInfo;
        }





        /// <summary>
        /// Get Federal ID
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="pqrelationshipid"></param>
        /// <returns></returns>
        private static void SaveAddressContactsTpDB(string accessToken, string pqrelationshipid, string vendName, string addr, string phone)
        {
            List<string> qualifInfo = new List<string>();
            string federalid = "";

            if (i == 1466)
            {
                int j = 0;
            }
           

            if(! string.IsNullOrEmpty( pqrelationshipid)) {

                string qualificationUrl = "https://app.buildingconnected.com/api-beta/qm-submissions?pqRelationshipId=" + pqrelationshipid;
                var client = new RestClient(qualificationUrl);
                RestRequest request = new RestRequest() { Method = Method.GET };
                request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
                request.AddParameter("Authorization", string.Format("Bearer {0}", accessToken), ParameterType.HttpHeader);
                request.AddHeader("Accept", "application/json");
                dynamic response = client.Execute(request);

                dynamic data = JsonConvert.DeserializeObject(response.Content);
                VendorAddressContacts vc = new VendorAddressContacts();

                if (response.StatusDescription == "OK")
                {
                  
                    JObject data1 = JObject.Parse(response.Content);
                    // federal id
                    JToken acme = data1.SelectToken("$.sections[?(@.sectionType == 'FINANCIALS')]");
                    dynamic qu = acme["questions"][0];
                    federalid = qu.value;


                    // email id
                    JToken compProf = data1.SelectToken("$.sections[?(@.sectionType == 'COMPANY_PROFILE')]");
                    dynamic qclist = compProf.SelectToken("$.questions[?(@.responseType == 'CONTACT_LIST')]");

                    if (qclist["value"].Count > 0)
                    {
                        vc.VendorAddress = addr;
                        vc.VendorEIN = federalid;
                        vc.VendorName = vendName;
                        vc.FirstContactEmail = (string)qclist["value"][0]["email"];
                        vc.FirstContactName = (string)qclist["value"][0]["name"];
                        vc.FirstContactPhone = (string)qclist["value"][0]["phone"];


                    }

                    if (qclist["value"].Count > 1)
                    {
                        vc.SecondContactEmail = (string)qclist["value"][1]["email"];
                        vc.SecondContactName = (string)qclist["value"][1]["name"];
                        vc.SecondContactPhone = (string)qclist["value"][1]["phone"];
                    }


                    if (qclist["value"].Count > 2)
                    {
                        vc.ThirdContactEmail = (string)qclist["value"][2]["email"];
                        vc.ThirdContactName = (string)qclist["value"][2]["name"];
                        vc.ThirdContactPhone = (string)qclist["value"][2]["phone"];
                    }

                    if (qclist["value"].Count > 3)
                    {
                        vc.FourthContactEmail = (string)qclist["value"][3]["email"];
                        vc.FourthContactName = (string)qclist["value"][3]["name"];
                        vc.FourthContactPhone = (string)qclist["value"][3]["phone"];
                    }

                    if (qclist["value"].Count > 4)
                    {
                        vc.FifthContactEmail = (string)qclist["value"][4]["email"];
                        vc.FifthContactName = (string)qclist["value"][4]["name"];
                        vc.FifthContactPhone = (string)qclist["value"][4]["phone"];
                    }

                    i++;
                    Console.WriteLine(" {0}  record added to DB", i.ToString());

                    try
                    {
                        var x = va.SaveVendorAddressContacts(vc);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(" {0}  Exception occured", i.ToString());
                        Console.WriteLine(" address ", vc.VendorAddress);
                        Console.WriteLine(" name ", vc.VendorName);
                        Console.WriteLine(" address ", vc.FirstContactPhone);
                        Console.WriteLine(" v name ", vc.FirstContactName);
                        Console.WriteLine(" email ", vc.FirstContactEmail);

                    }

                }
                else {
                    i++;
                    Console.WriteLine(" {0}  record added to DB", i.ToString());

                   
                    vc.VendorAddress = addr;
                    vc.VendorName = vendName;
                    vc.VendorPhone = phone;
                    var x = va.SaveVendorAddressContacts(vc);
                }


            }else
            {
                i++;
                Console.WriteLine(" {0}  record added to DB", i.ToString());

                VendorAddressContacts vc = new VendorAddressContacts();
                vc.VendorAddress = addr;
                vc.VendorName = vendName;
                vc.VendorPhone = phone;
                var x = va.SaveVendorAddressContacts(vc);
            }

        

        
        }



        /// <summary>
        /// This method uses the OAuth Client Credentials Flow to get an Access Token to provide
        /// Authorization to the APIs.
        /// </summary>
        /// <returns></returns>
        private static  string GetAccessToken()
        {

            var client = new RestClient(baseUrl);
            RestRequest request = new RestRequest() { Method = Method.POST };

            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            request.AddParameter("grant_type", "client_credentials");
            request.AddHeader("Accept", "application/json");
            client.Authenticator = new HttpBasicAuthenticator(clientId, clientSecret);
            dynamic response = client.Execute(request);

          


            

            object responseData = JsonConvert.DeserializeObject(response.Content);

            // return the Access Token.
            return ((dynamic)responseData).access_token;

        }


    }
}
