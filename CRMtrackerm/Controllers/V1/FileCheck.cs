using CrmTracker.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CrmTracker.Models.EntityModels;
using System.Net.Http.Headers;
using CrmTracker.DapperORM;
using Dapper;
using System.Net.Mail;
using MimeKit;
using MailKit.Security;
using System.Reflection.Metadata;
using iTextSharp.text.pdf;

namespace CrmTracker.Controllers.V1
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileCheck : ControllerBase
    {
        ILoggerManager log = null;
        CRMtrackerDapperContext cdc;
        List<RfpDocument> documents;
        byte[] bytes;

        public FileCheck(ILoggerManager log, CRMtrackerDapperContext cdc)
        {
            this.log = log;
            this.cdc = cdc;

        }
        [HttpPost]
        [Route("/[controller]/V1/file")]
        // geting data from form
        // adding new enquiry

        public void Addfile([FromForm] file file)
        {
            log.LogInfo("ADDING NEW Enquiry ");

            using var memoryStream = new MemoryStream();
            file.file1.CopyToAsync(memoryStream);
            var a = memoryStream.ToArray();
            string s = Convert.ToBase64String(a);




            //byte[] bytes = Convert.FromBase64String(s);

            var email = new MimeMessage();
            var builder = new BodyBuilder();

            email.From.Add(MailboxAddress.Parse("manu7babu@gmail.com"));
            email.To.Add(MailboxAddress.Parse("manu7babu@gmail.com"));
            email.Subject = "thanks for your request";
            email.Body = new TextPart(MimeKit.Text.TextFormat.Text) { Text = "thanks for your request we will get back to you" };



            //to get all  enquiry documents  from database basedon enquiry id
            var query = "select * from \"RFPDocuments\" where rfpr_id="  ;//id;
            log.LogInfo(query);
            using (var conn = cdc.CreateConnection())
            {
                log.LogInfo("Get All  enquiries documents from DB using Dapper");
                documents = (List<RfpDocument>)conn.Query<RfpDocument>(query);
                //retrun the list that contains enquiry records

            }
            foreach(var Doc in documents)
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
            smtp.Authenticate("manu7babu@gmail.com", "mujjxpfvfxuwnqpf");
            smtp.Send(email);
            smtp.Disconnect(true);
        }



    }

}
