

using CrmTracker.Models.EntityModels;
using CrmTracker.Models.ResourceModels;

namespace CrmTracker.Contracts
{
    public interface IEnquiry
    {
        public bool NewEnquiry(NewEnquiries enquiries);
       public List<Enquiry> getAllEnquires();
       public List<EnquiryDocument> getAllEnquiresDocuments(int id);
    }
}
