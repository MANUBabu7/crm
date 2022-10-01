using Dapper;

using CrmTracker.DapperORM;
using CrmTracker.Contracts;
using CrmTracker.Models.EntityModels;
using CrmTracker.Models.ResourceModels;

namespace CrmTracker.Repository
{
    public class CustomerRepo : IcustomerMasterEntrty
    {
        CRMtrackerDapperContext cdc;
        ILoggerManager log = null;
        public CustomerRepo(CRMtrackerDapperContext cdc, ILoggerManager log)
        {
            this.cdc = cdc;
            this.log = log;
        }
        public bool NewCustomer(NewCustomer newcus)
        {
            // adding new customer to database
            try
            {
                using (var conn = cdc.CreateConnection())
                {

                    conn.Open();
                    // opening connection
                    //inserting values into customers
                    int nor = conn.Execute("insert into customers (customer_name, customer_type, customer_mobile,  customer_email , customer_address, customer_company ,customer_location,customer_website ,customer_status ) values (@customer_name, @customer_type, @customer_mobile,  @customer_email , @customer_address, @customer_company ,@customer_location,@customer_website ,@customer_status )", newcus );
                    if (nor == 1)
                    {
                        //on addiding returning ture
                        log.LogInfo("new customer is added ");
                        return true;
                    }
                    else
                    {
                        //ifnot added returning flase
                        log.LogError("insert statement statement is not excuited");

                        return false;
                    }
                }

            }
            catch (Exception)
            {

                throw;
            }
        }
        public bool EditCustomer(Customer editcus)
            //editing records in database
        {
            try
            {
                using (var conn = cdc.CreateConnection())
                {

                    conn.Open();
                    //updating edited values to customers table in data base
                    int nor = conn.Execute("update customers set customer_name=@customer_name, customer_type=@customer_type, customer_mobile=@customer_mobile,  customer_email=@customer_email , customer_address=@customer_address, customer_company=@customer_company , customer_location=customer_location,customer_website=@customer_website , customer_status=@customer_status where customer_id=@eid", new { eid = editcus.customer_id ,customer_name=editcus.customer_name, customer_type=editcus.customer_type , customer_mobile=editcus.customer_mobile,  customer_email=editcus.customer_email,  customer_address=editcus.customer_address, customer_company=editcus.customer_company,  customer_location=editcus.customer_location, customer_website=editcus.customer_website, customer_status=editcus.customer_status });
                    if (nor == 1)
                    {
                        log.LogInfo("Executing update setting edited values ");
                        //on edited returing true
                        return true;
                    }
                    else
                    {
                        log.LogError("edit statement is not excuited");
                        //ifnot edited returning flase
                        return false;
                    }
                }

            }
            catch (Exception)
            {

                throw;
            }
            
        }
        public bool DeleteCustomer(int delete)
        {// soft deleting customer updating status to delete
            try
            {

                using (var conn = cdc.CreateConnection())
                {
                    var str = delete;
                    conn.Open();
                    //opening connection to database
                    int nor = conn.Execute("update customers set customer_status='Deleted' where customer_id=@eid", new { eid = str });
                    /// executing update query
                    if (nor == 1)
                    {
                        return true;
                        // on update returning true 
                        log.LogInfo("Executing update setting customerStatus to delete ");
                    }
                    else
                    {
                        return false;
                        log.LogError("update statement for deletion not excuited");
                        //if not return flase
                    }
                }
            }
            catch (Exception )
            {
                // throwing error
                throw;
            }        
        }
        public List<Customer> getAllCustomer()
        {
            try
            {
                //to get all customers from database
                var query = "select *from customers";
                log.LogInfo(query);
                using (var conn = cdc.CreateConnection())
                {
                    log.LogInfo("Get All customers from DB using Dapper");
                    List<Customer> customers = (List<Customer>)conn.Query<Customer>(query);
                    //retrun the list that contains customer records
                    return customers.ToList();

                }

            }
            catch (Exception)
            {
                //error in loading data from data base
                log.LogError("error in loading data from data base");

                throw;
             
            }

        }
    }
}
