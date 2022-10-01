using System.ComponentModel.DataAnnotations;
namespace CrmTracker.Models.EntityModels
{
    public class NewEnquiries
    {

        
        [Required]
        public int customer_id { get; set; }
        [Required]
        public string enquiry_desc { get; set; }
        [Required]
        public string enquiry_subject { get; set; }

        public List<enquiries_documents> enquiries_documents { get; set; }

    }


    public class enquiries_documents
    {
         
        // public int enquiry_id { get; set; }        
        public string document_path { get; set; }
        public string document_desc { get; set; }
    }
}

