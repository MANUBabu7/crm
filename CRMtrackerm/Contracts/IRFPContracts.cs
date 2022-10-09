

using CrmTracker.Models.EntityModels;
using static CrmTracker.Models.EntityModels.RFPs;

namespace CrmTracker.Contracts
{
    public interface IRfpContract
    {
        public bool NewRfp(CRFPs Rpf);

        public bool AssignToUser(AssignUser assignUser);

        public List<RFPs> GetAllRFPs();
        public List<User> GetAllUsers();
        public bool updatesatusRfp(UpdateRfpStatus statusrfp);

        public List<RfpDocument> GetAllRFPrDocuments(int id);
    }
}
