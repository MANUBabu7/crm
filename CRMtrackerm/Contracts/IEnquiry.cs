

using CrmTracker.Models.EntityModels;
using CrmTracker.Models.ResourceModels;

namespace CrmTracker.Contracts
{
    public interface IEnquiry
    {
        public bool NewEnquiry(NewEnquiries enquiries);
        public bool updatesatusEnq(UpdateStatusEnq statusEnq);
        public List<Enquiry> getAllEnquires();
        public List<Enquiry> getAllActiveEnquires();
        public List<Rfpcategory> getAllRfpcategory();
        public List<EnquiryDocument> getAllEnquiresDocuments(int id);
    }
}
