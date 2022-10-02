using Dapper;
using CrmTracker.DapperORM;
using CrmTracker.Contracts;
using CrmTracker.Models.EntityModels;
using CrmTracker.Models.ResourceModels;
using System.Web;
using System.Net.Mail;
using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;

namespace CrmTracker.Repository
{
    public class EnquiriesRepo : IEnquiry
    {
        CRMtrackerDapperContext cdc;
        ILoggerManager log = null;
        string toemail;
        public EnquiriesRepo(CRMtrackerDapperContext cdc, ILoggerManager log)
        {
            this.cdc = cdc;
            this.log = log;
        }

        public bool NewEnquiry(NewEnquiries enquiries)
        {
            log.LogInfo("creating enquireis in data base");
            
                using (var conn = cdc.CreateConnection())
                {
                    
                    var query = "insert into enquiries( enquiry_date, customer_id, enquiry_subject, enquiry_desc, status) values (current_date,@customer_id,@enquiry_subject, @enquiry_desc ,'pending') returning enquiry_id;";
                    int enquiry_id = conn.QueryFirstOrDefault<int>(query, enquiries);
                    foreach (var documents in enquiries.enquiries_documents)
                    {
                        string qry = "insert into enquiries_documents (enquiry_id, document_path, document_desc) values (@enquiry_id, @document_path, @document_desc);";
                    int count = conn.Execute(qry, new { enquiry_id, @document_path = documents.document_path, @document_desc = documents.document_desc });
                     }
                var query1 = "select customer_email from customers where customer_id="+enquiries.customer_id;
                  var  emailq = conn.QueryFirstOrDefault(query1);
                 toemail = emailq.customer_email;
                }

                
            log.LogInfo("sending mail to customer");

            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse("manu7babu@gmail.com"));
            email.To.Add(MailboxAddress.Parse(toemail));
            email.Subject = "thanks for your request";
            email.Body =new TextPart(MimeKit.Text.TextFormat.Text) { Text= "thanks for your request we will get back to you" };
            
            using var smtp = new MailKit.Net.Smtp.SmtpClient();
            smtp.Connect("smtp.gmail.com",587,SecureSocketOptions.StartTls);
            smtp.Authenticate("manu7babu@gmail.com", "mujjxpfvfxuwnqpf");
            smtp.Send(email);
            smtp.Disconnect(true);
            return true;
        }
         public List<Enquiry> getAllEnquires()
        {
            try
            {
                //to get all active enquiries from database
                var query = "select *from enquiries where status='pending'";
                log.LogInfo(query);
                using (var conn = cdc.CreateConnection())
                {
                    log.LogInfo("Get All active enquiries from DB using Dapper");
                    List<Enquiry> enquiries = (List<Enquiry>)conn.Query<Enquiry>(query);
                    //retrun the list that contains enquiry records
                    return enquiries.ToList();

                }

            }
            catch (Exception)
            {
                //error in loading data from data base
                log.LogError("error in loading data from data base");

                throw;

            }
        }
        public List<Rfpcategory> getAllRfpcategory()
        {
            try
            {
                //to get all active enquiries from database
                var query = "select *from  \"RFPCategories\";";
                log.LogInfo(query);
                using (var conn = cdc.CreateConnection())
                {
                    log.LogInfo("Get All active enquiries from DB using Dapper");
                    List<Rfpcategory> rfpcategories = (List<Rfpcategory>)conn.Query<Rfpcategory>(query);
                    //retrun the list that contains enquiry records
                    return rfpcategories.ToList();

                }

            }
            catch (Exception)
            {
                //error in loading data from data base
                log.LogError("error in loading data from data base");

                throw;

            }

        }

        public List<EnquiryDocument> getAllEnquiresDocuments(int id)
        {
            try
            {
                //to get all  enquiry documents  from database basedon enquiry id
                var query = "select *from enquiries_documents where enquiry_id=" + id;
                log.LogInfo(query);
                using (var conn = cdc.CreateConnection())
                {
                    log.LogInfo("Get All  enquiries documents from DB using Dapper");
                    List<EnquiryDocument> documents = (List<EnquiryDocument>)conn.Query<EnquiryDocument>(query);
                    //retrun the list that contains enquiry records
                    return documents.ToList();

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
