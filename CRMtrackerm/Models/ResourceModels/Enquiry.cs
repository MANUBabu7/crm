using System.ComponentModel.DataAnnotations;
namespace CrmTracker.Models.ResourceModels
{
    public class Enquiry
    {

        public int enquiry_id { get; set; }
        public DateTime enquiry_date { get; set; }

        public int customer_id { get; set; }
        public string enquiry_type { get; set; }
        public string enquiry_product { get; set; }
        public string status { get; set; }



    }



}

