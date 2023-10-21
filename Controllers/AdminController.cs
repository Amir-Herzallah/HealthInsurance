using HealthInsurance.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Mail;
using System.Net.Mime;
using System.Net;
using iTextSharp.text.pdf;
using iTextSharp.text;

namespace HealthInsurance.Controllers
{
    public class AdminController : Controller
    {
        private readonly ModelContext _context;
        private readonly IWebHostEnvironment webHostEnvironment;
        public AdminController(ModelContext context, IWebHostEnvironment webHostEnvironment)
        {
            this._context = context;
            this.webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            ViewBag.id = HttpContext.Session.GetInt32("Id");
            ViewBag.name = HttpContext.Session.GetString("Name");
            ViewBag.email = HttpContext.Session.GetString("Email");
            ViewBag.phoneNumber = HttpContext.Session.GetString("PhoneNumber");
            ViewBag.profilePic = HttpContext.Session.GetString("ProfilePic");
            ViewBag.UsersCount = _context.Users.Count();
            ViewBag.SubsCount = _context.Subscriptions.Count();
            ViewBag.CurrentDate = DateTime.Now;
            ViewBag.userLoginId = HttpContext.Session.GetInt32("userLoginId");
            ViewBag.userLoginName = HttpContext.Session.GetString("userLoginName");
            ViewBag.userLoginEmail = HttpContext.Session.GetString("userLoginEmail");
            return View();
        }
        public IActionResult RegUsers()
        {
            var usersInfo = _context.Users.Include(s => s.Subscription.Beneficiaries).ToList();
            ViewBag.id = HttpContext.Session.GetInt32("Id");
            ViewBag.name = HttpContext.Session.GetString("Name");
            ViewBag.email = HttpContext.Session.GetString("Email");
            ViewBag.phoneNumber = HttpContext.Session.GetString("PhoneNumber");
            ViewBag.profilePic = HttpContext.Session.GetString("ProfilePic");
            ViewBag.UsersCount = _context.Users.Count();
            ViewBag.SubsCount = _context.Subscriptions.Count();
            ViewBag.CurrentDate = DateTime.Now;
            ViewBag.userLoginId = HttpContext.Session.GetInt32("userLoginId");
            ViewBag.userLoginName = HttpContext.Session.GetString("userLoginName");
            ViewBag.userLoginEmail = HttpContext.Session.GetString("userLoginEmail");
            return View(usersInfo);
        }
        //GET
        public IActionResult SubSearch()
        {
            ViewBag.id = HttpContext.Session.GetInt32("Id");
            ViewBag.name = HttpContext.Session.GetString("Name");
            ViewBag.email = HttpContext.Session.GetString("Email");
            ViewBag.phoneNumber = HttpContext.Session.GetString("PhoneNumber");
            ViewBag.profilePic = HttpContext.Session.GetString("ProfilePic");
            ViewBag.UsersCount = _context.Users.Count();
            ViewBag.SubsCount = _context.Subscriptions.Count();
            ViewBag.CurrentDate = DateTime.Now;
            ViewBag.userLoginId = HttpContext.Session.GetInt32("userLoginId");
            ViewBag.userLoginName = HttpContext.Session.GetString("userLoginName");
            ViewBag.userLoginEmail = HttpContext.Session.GetString("userLoginEmail");
            var model = _context.Users.Include(p => p.Subscription).Where(x => x.Subscription != null).ToList();
            return View(model);
        }

        [HttpPost]
        public IActionResult SubSearch(DateTime? startDate, DateTime? endDate)
        {
            ViewBag.id = HttpContext.Session.GetInt32("Id");
            ViewBag.name = HttpContext.Session.GetString("Name");
            ViewBag.email = HttpContext.Session.GetString("Email");
            ViewBag.phoneNumber = HttpContext.Session.GetString("PhoneNumber");
            ViewBag.profilePic = HttpContext.Session.GetString("ProfilePic");
            ViewBag.UsersCount = _context.Users.Count();
            ViewBag.SubsCount = _context.Subscriptions.Count();
            ViewBag.CurrentDate = DateTime.Now;
            ViewBag.userLoginId = HttpContext.Session.GetInt32("userLoginId");
            ViewBag.userLoginName = HttpContext.Session.GetString("userLoginName");
            ViewBag.userLoginEmail = HttpContext.Session.GetString("userLoginEmail");

            var model = _context.Users.Include(p => p.Subscription).Where(x => x.Subscription.Status != null).ToList();
            if (startDate == null && endDate == null)
            {
                return View(model);
            }
            else if (startDate != null && endDate == null)
            {
                var result = model.Where(x => x.Subscription.StartDate.Date >= startDate);
                return View(result);
            }

            else if (startDate == null && endDate != null)
            {
                var result = model.Where(x => x.Subscription.StartDate.Date <= endDate);
                return View(result);
            }
            else
            {
                var result = model.Where
                    (x => x.Subscription.StartDate.Date >= startDate && x.Subscription.StartDate.Date <= endDate);

                return View(result);
            }
        }
        public IActionResult AdminManageBeneficiaries()
        {
            ViewBag.id = HttpContext.Session.GetInt32("Id");
            ViewBag.name = HttpContext.Session.GetString("Name");
            ViewBag.email = HttpContext.Session.GetString("Email");
            ViewBag.phoneNumber = HttpContext.Session.GetString("PhoneNumber");
            ViewBag.profilePic = HttpContext.Session.GetString("ProfilePic");
            ViewBag.UsersCount = _context.Users.Count();
            ViewBag.SubsCount = _context.Subscriptions.Count();
            ViewBag.CurrentDate = DateTime.Now;
            ViewBag.BeneId = HttpContext.Session.GetInt32("BeneId");
            ViewBag.BeneName = HttpContext.Session.GetString("BeneName");
            ViewBag.BeneRelToSub = HttpContext.Session.GetString("BeneRelToSub");
            ViewBag.userLoginId = HttpContext.Session.GetInt32("userLoginId");
            ViewBag.userLoginName = HttpContext.Session.GetString("userLoginName");
            ViewBag.userLoginEmail = HttpContext.Session.GetString("userLoginEmail");
            // Retrieve a list of beneficiaries from the database
            var beneficiaries = _context.Beneficiaries.ToList();

            ViewBag.BeneId = HttpContext.Session.GetInt32("BeneId");
            ViewBag.BeneName = HttpContext.Session.GetString("BeneName");
            ViewBag.BeneRelToSub = HttpContext.Session.GetString("BeneRelToSub");
            ViewBag.userLoginId = HttpContext.Session.GetInt32("userLoginId");
            ViewBag.userLoginName = HttpContext.Session.GetString("userLoginName");
            ViewBag.userLoginEmail = HttpContext.Session.GetString("userLoginEmail");
            return View(beneficiaries);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateBeneficiaryStatus(int beneficiaryId, string newStatus)
        {
            // Retrieve the beneficiary from the database
            var beneficiary = _context.Beneficiaries.FirstOrDefault(b => b.Id == beneficiaryId);
            var sub = _context.Subscriptions.FirstOrDefault(s => s.Id == beneficiary.Subscriptionid);
            var user = _context.Users.Where(x => x.Id == sub.Userid ).FirstOrDefault();

            HttpContext.Session.SetInt32("Id", (Int32)user.Id);
            HttpContext.Session.SetString("Name", user.Username);
            HttpContext.Session.SetString("Email", user.Email);
            HttpContext.Session.SetString("PhoneNumber", user.PhoneNumber);
            HttpContext.Session.SetString("ProfilePic", user.ProfilePictureUrl);

            HttpContext.Session.SetInt32("BeneId", (int)beneficiary.Id);
            HttpContext.Session.SetString("BeneName", beneficiary.Name);
            HttpContext.Session.SetString("BeneRelToSub", beneficiary.RelationshipToSubscriber);

            ViewBag.BeneId = HttpContext.Session.GetInt32("BeneId");
            ViewBag.BeneName = HttpContext.Session.GetString("BeneName");
            ViewBag.BeneRelToSub = HttpContext.Session.GetString("BeneRelToSub");
            ViewBag.id = HttpContext.Session.GetInt32("Id");
            ViewBag.name = HttpContext.Session.GetString("Name");
            ViewBag.email = HttpContext.Session.GetString("Email");
            ViewBag.CurrentDate = DateTime.Now;

            if (beneficiary != null && beneficiary.BeneficiaryImagePath != null)
            {
                // Update the status to the new status (e.g., "Accepted" or "Rejected")
                beneficiary.Status = newStatus;

                //// Retrieve the subscriber's email from the database
                //var subEmail = _context.Users
                //    .Where(u => u.Id == beneficiary.Subscription.Userid)
                //    .Select(u => u.Email)
                //    .FirstOrDefault();

                if (beneficiary.Status == "Accepted")
                {
                    using (SmtpClient smtpClient = new SmtpClient("smtp.gmail.com"))
                    {
                        smtpClient.Port = 587;
                        smtpClient.UseDefaultCredentials = false;
                        smtpClient.Credentials = new NetworkCredential("amirherzalla8@gmail.com", "ccthxdcuhqqudgvi"); // Replace with your Gmail email and App Password
                        smtpClient.EnableSsl = true;

                        MailMessage mailMessage = new MailMessage
                        {
                            From = new MailAddress(ViewBag.email),
                            Subject = "Beneficiary Added!",
                            Body = "Congrats! you have added a beneficiary successfully."
                        };

                        mailMessage.To.Add("amir.herzalla123@gmail.com"); // Use the retrieved email

                        // Create the PDF invoice
                        var pdfFileName = GenerateInvoicePDF(ViewBag.name, ViewBag.CurrentDate, 50.0, ViewBag.BeneName, ViewBag.BeneRelToSub); // Pass the required data for the invoice

                        // Attach the PDF to the email
                        mailMessage.Attachments.Add(new Attachment(pdfFileName, MediaTypeNames.Application.Pdf));

                        smtpClient.Send(mailMessage);
                    }
                }

                // Save changes to the database
                await _context.SaveChangesAsync();
            }

            ViewBag.userLoginId = HttpContext.Session.GetInt32("userLoginId");
            ViewBag.userLoginName = HttpContext.Session.GetString("userLoginName");
            ViewBag.userLoginEmail = HttpContext.Session.GetString("userLoginEmail");

            return RedirectToAction("AdminManageBeneficiaries");
        }

        public string GenerateInvoicePDF(string customerName, DateTime currentDate, double amount, string beneName, string beneRelToSub)
        {
            // Create a new document with borders
            iTextSharp.text.Document document = new iTextSharp.text.Document(PageSize.A4, 50, 50, 50, 50);

            // Generate a unique file name for the PDF
            string pdfFileName = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N") + ".pdf");

            using (FileStream fs = new FileStream(pdfFileName, FileMode.Create))
            {
                PdfWriter writer = PdfWriter.GetInstance(document, fs);
                document.Open();

                // Add a border around the page
                PdfContentByte cb = writer.DirectContent;
                float margin = 30; // Adjust the margin as needed
                cb.SetLineWidth(2);
                cb.Rectangle(margin, margin, document.PageSize.Width - 2 * margin, document.PageSize.Height - 2 * margin);
                cb.Stroke();

                // Add a logo inside the border at the top center
                string logoPath = Path.Combine(webHostEnvironment.WebRootPath + "/HomeAssets/img/icon/" + "icon-02-primary.png");
                iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(logoPath);
                logo.SetAbsolutePosition((document.PageSize.Width - logo.ScaledWidth) / 2, document.PageSize.Height - 130); // Adjust the position as needed
                document.Add(logo);

                // Add some space
                for (int i = 0; i < 3; i++)
                {
                    document.Add(new Paragraph(" "));
                }
                // Add content to the PDF inside the border (e.g., customer name, date, amount)
                iTextSharp.text.Paragraph heading = new Paragraph("Subscription Details", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 16f));
                heading.Alignment = Element.ALIGN_CENTER;
                for (int i = 0; i < 2; i++)
                {
                    document.Add(new Paragraph(" "));
                }
                document.Add(heading);
                document.Add(new Paragraph(" "));
                document.Add(new Paragraph($"Customer Name: {customerName}"));
                document.Add(new Paragraph($"Date: {currentDate.ToShortDateString()}"));
                document.Add(new Paragraph($"Amount Paid: ${amount}"));
                document.Add(new Paragraph($"Beneficiary Name: {beneName}"));
                document.Add(new Paragraph($"Beneficiary Relationship to Subscriber: {beneRelToSub}"));

                document.Close();
            }

            return pdfFileName;
        }
    }
}
