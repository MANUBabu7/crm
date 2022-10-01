using CrmTracker.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CrmTracker.Models.EntityModels;
using AutoMapper;
using CrmTracker.Models.ResourceModels;
using CrmTracker.AutoMapper;
using Microsoft.AspNetCore.Cors;

namespace CrmTracker.Controllers.V1
{
  //  [EnableCors]

    [Route("api/[controller]")]
    [ApiController]
    public class CustomersMasterEntry : ControllerBase
    {
        ILoggerManager log = null;
        IcustomerMasterEntrty icustomer;
        IMapper _mapper;

        public CustomersMasterEntry(ILoggerManager log, IcustomerMasterEntrty icustomer, IMapper mapper)
        {
            this.log = log;
            this.icustomer = icustomer;
            _mapper = mapper;
        }
        [HttpPost]
        [Route("/[controller]/V1/AddCustomer")]

        //ADDING NEW CUSTOMER
        public IActionResult Addcustomer([FromBody] NewCustomer add)
        {
            log.LogInfo("ADDING NEW CUSTOMER ");
            return Ok(icustomer.NewCustomer(add));
        }
        [HttpPost]
        [Route("/[controller]/V1/EDITCustomer")]
        //UPDATE EXISTING CUSTOMER
        public IActionResult EDITcustomer([FromBody] Customer edit)
        {
            log.LogInfo("UPDATE EXISTING CUSTOMER ");
            return Ok(icustomer.EditCustomer(edit));
        }
        [HttpPost]
        [Route("/[controller]/V1/DELETECustomer")]
        //DELETING(SOFT) EXISTING CUSTOMER 
        public IActionResult DELETEcustomer([FromBody] int customer_id)
        {
            log.LogInfo("DELETING(SOFT) EXISTING CUSTOMER ");
            return Ok(icustomer.DeleteCustomer(customer_id));
        }
        [HttpGet]
        [Route("/[controller]/V1/ToGetAllCustomers")]

        //GETIING ALL EXISTING CUSTOMERS
        public IActionResult ToGetAllcustomers()
        {
            log.LogInfo("GETIING ALL EXISTING CUSTOMERS");
            return Ok(_mapper.Map<List<CustomerResource>>(icustomer.getAllCustomer()));

        }

    }

}
