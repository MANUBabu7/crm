

using CrmTracker.Models.EntityModels;
using static CrmTracker.Models.EntityModels.RFPs;

namespace CrmTracker.Contracts
{
    public interface IRfpContract
    {
        public bool NewRfp(RFPs Rpf);

        public List<RFPs> GetAllRFPs();
        public List<RfpDocument> GetAllRFPrDocuments(int id);
    }
}
