using CrmTracker.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CrmTracker.Models.EntityModels;
using Microsoft.AspNetCore.Authorization;

namespace CrmTracker.Controllers.V1
{
    [Authorize(Roles = "Technical")]
    [Route("api/[controller]")]
    [ApiController]
    public class RfpController : ControllerBase
    {
        ILoggerManager log = null;
        IRfpContract rfpContract;
        public RfpController(ILoggerManager log, IRfpContract rfpContract)
        {
            this.log = log;
            this.rfpContract = rfpContract;
        }
        [HttpPost]
        [Route("/[controller]/V1/AddRfp")]
        // geting data from form
        // adding new Rfps

        public bool NewRFP([FromBody] RFPs add)
        {
            log.LogInfo("ADDING NEW Rfps ");
            return rfpContract.NewRfp(add);

        }
        [HttpGet]
        // to get all active rfps
        [Route("/[controller]/V1/ToGetAllActiveRFPs")]
        public IActionResult ToGetAllActiveEnquiries()
        {
            log.LogInfo("GETIING ALL EXISTING CUSTOMERS");
            return Ok(rfpContract.GetAllRFPs());
        }

        [HttpPost]
        [Route("/[controller]/V1/GetAllEnquiryDocuments")]
        // geting data from form


        public IActionResult GetAllEnquiryDocuments([FromBody] int RPFrid)
        {
            // to get all 
            log.LogInfo("geting all documents based on rfpid ");
            return Ok(rfpContract.GetAllRFPrDocuments(RPFrid));
        }

    }

}
