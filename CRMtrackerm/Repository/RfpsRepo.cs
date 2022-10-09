using Dapper;
using CrmTracker.DapperORM;
using CrmTracker.Contracts;
using CrmTracker.Models.EntityModels;
using static CrmTracker.Models.EntityModels.RFPs;
using CrmTracker.Models.ResourceModels;
using MimeKit;
using MailKit.Security;

namespace CrmTracker.Repository
{
    public class RfpsRepo : IRfpContract
    {
        CRMtrackerDapperContext cdc;
        ILoggerManager log = null;
        List<EnquiryDocument> edocuments;
        private List<RfpDocument> documents;
        private readonly IConfiguration _configuration;
        private readonly string fromemail;
        private readonly string password;
       public int rfpr_enqr_id;

        public string rfpr_subject;
        public  string rfpr_intro_note;
        public string rpfr_rfpc_id;
        public string rfpr_created_ausr_id;

        public RfpsRepo(CRMtrackerDapperContext cdc, ILoggerManager log, IConfiguration configuration)
        {
            this.cdc = cdc;
            this.log = log;
            _configuration = configuration;
            fromemail = _configuration["email:from"];
            password = _configuration["email:password"];

        }

        public bool updatesatusRfp(UpdateRfpStatus statusrfp)
        {

            using (var conn = cdc.CreateConnection())
            {


                string qry = "update \"RFPs\"  SET rfpr_status = @status where rfpr_id=@id;";
                int count = conn.Execute(qry, new { @status = statusrfp.Status, @id = statusrfp.rfpr_id });

                return true;
            }

        }
        public bool AssignToUser(AssignUser assignUser)
        {
            using (var conn = cdc.CreateConnection())
            {


                string qry = "update \"RFPs\"  SET rfpr_assignedto = @user where rfpr_id=@id;";
                int count = conn.Execute(qry, new { @user = assignUser.ausr_id, @id = assignUser.rfpr_id });
                var query3 = "select ausr_email from \"AdminUsers\" where ausr_id=@id;";
                var emailq = conn.QueryFirstOrDefault(query3, new { id = assignUser.ausr_id });
                string toemail = emailq.ausr_email;

                var email = new MimeMessage();
                var builder = new BodyBuilder();

                email.From.Add(MailboxAddress.Parse(fromemail));
                email.To.Add(MailboxAddress.Parse(toemail));
                email.Subject = "thanks for your request";
                email.Body = new TextPart(MimeKit.Text.TextFormat.Text) { Text = "thanks for your request we will get back to you" };

                //to get all  enquiry documents  from database basedon enquiry id
                var query2 = "select * from \"RFPDocuments\" where rfpr_id=" + assignUser.rfpr_id;
                log.LogInfo(query2);

                log.LogInfo("Get All  enquiries documents from DB using Dapper");
                documents = (List<RfpDocument>)conn.Query<RfpDocument>(query2);
                //retrun the list that contains enquiry records


                foreach (var Doc in documents)
                {
                    byte[] bytes = Convert.FromBase64String(Doc.rfpd_documentpath);
                    builder.Attachments.Add("myFile.txt", bytes, new ContentType("application", "txt"));



                }
                email.Body = builder.ToMessageBody();

                //foreach (var attachment in attachments)
                //{
                //    builder.Attachments.Add(attachment);
                //}
                using var smtp = new MailKit.Net.Smtp.SmtpClient();
                smtp.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
                smtp.Authenticate(fromemail, password);
                smtp.Send(email);
                smtp.Disconnect(true);

                return true;
            }
        }
        public bool NewRfp(CRFPs Rfp)
        {
          


            log.LogInfo("");
            
            using (var conn = cdc.CreateConnection())
            {
                foreach (var rfps in Rfp.RfpDoc)
                {
                    rfpr_enqr_id = rfps.enquiry_id;
                    rfpr_subject = rfps.rfpsubject;
                    rfpr_intro_note = rfps.rfpintro;
                    rpfr_rfpc_id = rfps.Rfpcatg;

                    rfpr_created_ausr_id = "us1";

                }




                    var query1 = "insert into \"RFPs\" (rfpr_enqr_id,rfpr_assignedto,rfpr_subject,rfpr_intro_note,rpfr_rfpc_id,rfpr_created_ausr_id,rfpr_ludatetime,rfpr_status) values (@rfpr_enqr_id,'Not Assigned',@rfpr_subject,@rfpr_intro_note,@rpfr_rfpc_id,@rfpr_created_ausr_id,@CurrentDate,'pending') returning rfpr_id;";
                int rfpr_id = conn.QueryFirstOrDefault<int>(query1, new { rfpr_enqr_id = rfpr_enqr_id, rfpr_subject = rfpr_subject, rfpr_intro_note = rfpr_intro_note, rpfr_rfpc_id = rpfr_rfpc_id, rfpr_created_ausr_id = rfpr_created_ausr_id, CurrentDate = DateTime.Now });
                foreach (var rfps in Rfp.RfpDoc)
                {
                    // converting enquiry documents to 
                    string qry = "insert into \"RFPDocuments\" (rfpr_id,rfpd_documentpath,rfpd_version,rpfd_reviewed_by,rfpd_desc) values (@rfpr_id,@rfpd_documentpath,'version1','',@rfpd_desc);";
                    int count = conn.Execute(qry, new { rfpr_id = rfpr_id, rfpd_documentpath = rfps.document_path, rfpd_desc = rfps.document_desc });
                }




            }
            return true;
        }

        public List<RFPs> GetAllRFPs()
        {

            try
            {
                //to get all active enquiries from database
                var query = "select *from \"RFPs\";";
                log.LogInfo(query);
                using (var conn = cdc.CreateConnection())
                {
                    log.LogInfo("Get All active enquiries from DB using Dapper");
                    List<RFPs> enquiries = (List<RFPs>)conn.Query<RFPs>(query);
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
        public List<User> GetAllUsers()
        {

            try
            {
                //to get all active enquiries from database
                var query = "select *from \"AdminUsers\";";
                log.LogInfo(query);
                using (var conn = cdc.CreateConnection())
                {
                    log.LogInfo("Get All active enquiries from DB using Dapper");
                    List<User> AllUsers = (List<User>)conn.Query<User>(query);
                    //retrun the list that contains enquiry records
                    return AllUsers.ToList();

                }

            }
            catch (Exception)
            {
                //error in loading data from data base
                log.LogError("error in loading data from data base");

                throw;

            }
        }
        public List<RfpDocument> GetAllRFPrDocuments(int id)
        {
            try
            {
                //to get all  enquiry documents  from database basedon enquiry id
                var query = "select * from \"RFPDocuments\" where rfpr_id=" + id;
                log.LogInfo(query);
                using (var conn = cdc.CreateConnection())
                {
                    log.LogInfo("Get All  enquiries documents from DB using Dapper");
                    List<RfpDocument> documents = (List<RfpDocument>)conn.Query<RfpDocument>(query);
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
