using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CrmTracker.Models.EntityModels;
using CrmTracker.Contracts;
using Microsoft.AspNetCore.Authorization;

namespace CrmTracker.Controllers.V1
{
    //[Authorize(Roles ="Manager")]
    [Route("api/[controller]")]
    [ApiController]
    public class EnquiriesController : ControllerBase
    {
        ILoggerManager log = null;
        IEnquiry enquiries;
        public EnquiriesController(ILoggerManager log, IEnquiry enquiries)
        {
            this.log = log;
            this.enquiries = enquiries;
        }
        [HttpPost]
        [Route("/[controller]/V1/AddEnquiry")]
        // geting data from form
        // adding new enquiry

        public IActionResult NewEnquiry([FromBody] NewEnquiries add)
        {
            log.LogInfo("ADDING NEW Enquiry ");
            return Ok(enquiries.NewEnquiry(add));
        }

        [HttpGet]
        [Route("/[controller]/V1/ToGetAllActiveEnquiries")]
        public IActionResult ToGetAllActiveEnquiries()
        {
            log.LogInfo("GETIING ALL EXISTING CUSTOMERS");
            return Ok(enquiries.getAllEnquires());
        }

        [HttpPost]
        [Route("/[controller]/V1/GetAllEnquiryDocuments")]
        // geting data from form
        // adding new enquiry

        public IActionResult GetAllEnquiryDocuments([FromBody] int enquiryid)
        {
            log.LogInfo("ADDING NEW Enquiry ");
            return Ok(enquiries.getAllEnquiresDocuments(enquiryid));
        }
    }

}
