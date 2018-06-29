using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Filters;
using System.Net.Http.Headers;
using Ryan.VendorOnboarding.API.Services;
using Ryan.VendorOnboarding.API.Domain.Entities;
using System.Net.Http;
using System.Net;

namespace Ryan.VendorOnboarding.API.Controllers
{
    public class VendorController : ApiController
    {
        private VendorOnboardingService vendorservice;

        public VendorController() {
            vendorservice = new VendorOnboardingService();

        }


        [HttpGet]
        public async Task<VendorProfile> GetProfile(string id)
        {
           return await vendorservice.GetVendorDetails(id);
        }


        [HttpGet]
        public VendorProfile GetProfile1(string id)
        {
            VendorProfile c = new VendorProfile();
            try
            {
                c = vendorservice.GetVendorDetails1(id);

                if (c == null)
                {
                    var message = string.Format("Product with id = {0} not found", id);
                    throw new HttpResponseException(
                        Request.CreateErrorResponse(HttpStatusCode.BadGateway, message));
                }
               

            }
            catch(Exception ex)
            {
                throw new HttpResponseException(
                       Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex));
            }

            return c;
            
        }


        [HttpGet]
        public async Task<VendorProfile> GetProfileBtGuid(string guid)
        {
            return await vendorservice.GetVendorDetails(guid);
        }


        [HttpGet]
        public async Task<bool> SaveVendorProfile(VendorProfile profile)
        {
            return await vendorservice.SaveVendorDetails(profile);
        }


    }
}