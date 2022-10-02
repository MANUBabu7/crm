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

        public RfpsRepo(CRMtrackerDapperContext cdc, ILoggerManager log, IConfiguration configuration)
        {
            this.cdc = cdc;
            this.log = log;
            _configuration = configuration;
            fromemail = _configuration["email:from"];
            password = _configuration["email:password"];

        }

        public bool NewRfp(RFPs Rfp)
        {
            log.LogInfo("");

            using (var conn = cdc.CreateConnection())
            {
                //to get all  enquiry documents  from database basedon enquiry id
                var query = "select *from enquiries_documents where enquiry_id=" + Rfp.rfpr_enqr_id;
                log.LogInfo(query);

                log.LogInfo("Get All  enquiries documents from DB using Dapper");
                edocuments = (List<EnquiryDocument>)conn.Query<EnquiryDocument>(query);
                //retrun the list that contains enquiry records



                var query1 = "insert into \"RFPs\" (rfpr_enqr_id,rfpr_assignedto,rfpr_subject,rfpr_intro_note,rpfr_rfpc_id,rfpr_created_ausr_id,rfpr_ludatetime,rfpr_status) values (@rfpr_enqr_id,@rfpr_assignedto,@rfpr_subject,@rfpr_intro_note,@rpfr_rfpc_id,@rfpr_created_ausr_id,@CurrentDate,@rfpr_status) returning rfpr_id;";
                int rfpr_id = conn.QueryFirstOrDefault<int>(query1, new { rfpr_enqr_id = Rfp.rfpr_enqr_id, rfpr_assignedto = Rfp.rfpr_assignedto, rfpr_subject = Rfp.rfpr_subject, rfpr_intro_note = Rfp.rfpr_intro_note, rpfr_rfpc_id = Rfp.rpfr_rfpc_id, rfpr_created_ausr_id = Rfp.rfpr_created_ausr_id, CurrentDate = DateTime.Now, rfpr_status = Rfp.rfpr_status });
                foreach (var documents in edocuments)
                {
                    // converting enquiry documents to 
                    string qry = "insert into \"RFPDocuments\" (rfpr_id,rfpd_documentpath,rfpd_version,rpfd_reviewed_by,rfpd_desc) values (@rfpr_id,@rfpd_documentpath,'version1',@rpfd_reviewed_by,@rfpd_desc);";
                    int count = conn.Execute(qry, new { rfpr_id = rfpr_id, rfpd_documentpath = documents.document_path, rpfd_reviewed_by = Rfp.rfpr_created_ausr_id, rfpd_desc = documents.document_desc });
                }

                var query3 = "select ausr_email from \"AdminUsers\" where ausr_id=@id;"; 
                var emailq = conn.QueryFirstOrDefault(query3 ,new {id= Rfp.rfpr_assignedto });
                string toemail = emailq.ausr_email;

                var email = new MimeMessage();
                var builder = new BodyBuilder();

                email.From.Add(MailboxAddress.Parse(fromemail));
                email.To.Add(MailboxAddress.Parse(toemail));
                email.Subject = "thanks for your request";
                email.Body = new TextPart(MimeKit.Text.TextFormat.Text) { Text = "thanks for your request we will get back to you" };

                //to get all  enquiry documents  from database basedon enquiry id
                var query2 = "select * from \"RFPDocuments\" where rfpr_id=" + rfpr_id;
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
