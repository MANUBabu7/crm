using System.ComponentModel.DataAnnotations;
namespace CrmTracker.Models.EntityModels
{
    public class CRFPs
    {
        public List<rfpdoc> RfpDoc { get; set; }
       

        
    }

    public class rfpdoc
    {
        public string rfpsubject { get; set; }

        public string rfpintro { get; set; }
        public string Rfpdcat { get; set; }

        public string Rfptype { get; set; }
        public string Rfpcatg { get; set; }
        public int enquiry_id { get; set; }
        public int document_index { get; set; }
        public string document_desc { get; set; }
        public string document_path { get; set; }



    }


}

