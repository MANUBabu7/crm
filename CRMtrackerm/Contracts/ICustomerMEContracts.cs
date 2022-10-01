using CrmTracker.Models.EntityModels;
using CrmTracker.Models.ResourceModels;

namespace CrmTracker.Contracts
{
    public interface IcustomerMasterEntrty
    {
        public bool NewCustomer(NewCustomer newcus);
        public bool EditCustomer(Customer editcus);
        public bool DeleteCustomer(int delete);
        public List<Customer> getAllCustomer();
    }
}
