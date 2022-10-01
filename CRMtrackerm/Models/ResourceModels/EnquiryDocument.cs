using System.ComponentModel.DataAnnotations;
namespace CrmTracker.Models.ResourceModels
{



    public class EnquiryDocument
    { public int enquiry_id { get; set; }
        public int document_index { get; set; }
        public string document_path { get; set; }
        public string document_desc { get; set; }
    }
}

