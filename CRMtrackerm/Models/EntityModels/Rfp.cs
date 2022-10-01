using System.ComponentModel.DataAnnotations;
namespace CrmTracker.Models.EntityModels
{
    public class RFPs
    {

        //public int rfpr_id { get; set; }

        public int rfpr_enqr_id { get; set; }
        public string rfpr_assignedto { get; set; }

        public string rfpr_subject { get; set; }
        public string rfpr_intro_note { get; set; }
        public string rpfr_rfpc_id { get; set; }
        public string rfpr_created_ausr_id { get; set; }
        public string rfpr_status { get; set; }
        public DateTime rfpr_ludatetime { get; set; }


        // public IFormFile enquiry_subject1 { get; set; }
        //public List<RfpDocument> RfpDoc { get; set; }
      

    }
    public class RfpDocument
    {
        //  public int rfpr_id { get; set; }
        public int rfpd_docindex { get; set; }
        public string rfpd_documentpath { get; set; }
        public string rfpd_version { get; set; }
        public string rpfd_reviewed_by { get; set; }
        public string rfpd_desc { get; set; }
    }

}

