using HealthInsurance.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Mail;
using System.Net.Mime;
using System.Net;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.Text;

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
            string? requests = _context.Beneficiaries.Where(b => b.Status == "Pending").Count().ToString();
            ViewBag.requests = requests;
            return View();
        }
        public IActionResult RegUsers()
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

            var usersInfo = _context.Users.Include(s => s.Subscription.Beneficiaries).ToList();
         
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
            var beneficiary = _context.Beneficiaries.FirstOrDefault(b => b.Id == beneficiaryId);
            var sub = _context.Subscriptions.FirstOrDefault(s => s.Id == beneficiary.Subscriptionid);
            var user = _context.Users.FirstOrDefault(x => x.Id == sub.Userid);

            HttpContext.Session.SetInt32("Id", (int)user.Id);
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
            var SubDate = sub.StartDate;

            if (beneficiary != null && beneficiary.BeneficiaryImagePath != null)
            {
                beneficiary.Status = newStatus;

                if (beneficiary.Status == "Accepted")
                {
                    using (SmtpClient smtpClient = new SmtpClient("smtp.gmail.com"))
                    {
                        smtpClient.Port = 587;
                        smtpClient.UseDefaultCredentials = false;
                        smtpClient.Credentials = new NetworkCredential("amirherzalla8@gmail.com", "ccthxdcuhqqudgvi");
                        smtpClient.EnableSsl = true;

                        MailMessage mailMessage = new MailMessage
                        {
                            From = new MailAddress("amirherzalla8@gmail.com"),
                            Subject = "Beneficiary Added!",
                            Body = "Congrats! you have added a beneficiary successfully."
                        };

                        mailMessage.To.Add(ViewBag.Email);

                        var subBeneList = _context.Subscriptions
                            .Include(b => b.Beneficiaries)
                            .Where(s => s.Id == beneficiary.Subscriptionid)
                            .ToList();

                        var beneficiaryInfo = new StringBuilder();

                        foreach (var subscription in subBeneList)
                        {
                            foreach (var bene in subscription.Beneficiaries)
                            {
                                beneficiaryInfo.AppendLine($"Beneficiary Name: {bene.Name}");
                                beneficiaryInfo.AppendLine($"Beneficiary Relationship to Subscriber: {bene.RelationshipToSubscriber}");
                                beneficiaryInfo.AppendLine(" ");
                            }
                        }

                        var pdfFileName = GenerateBenePDF(ViewBag.name, SubDate, 50.0, beneficiaryInfo.ToString());
                        mailMessage.Attachments.Add(new Attachment(pdfFileName, MediaTypeNames.Application.Pdf));

                        smtpClient.Send(mailMessage);
                    }
                }
                await _context.SaveChangesAsync();
            }

            ViewBag.userLoginId = HttpContext.Session.GetInt32("userLoginId");
            ViewBag.userLoginName = HttpContext.Session.GetString("userLoginName");
            ViewBag.userLoginEmail = HttpContext.Session.GetString("userLoginEmail");

            return RedirectToAction("AdminManageBeneficiaries");
        }

        public string GenerateBenePDF(string customerName, DateTime currentDate, double amount, string beneficiaryInfo)
        {
            iTextSharp.text.Document document = new iTextSharp.text.Document(PageSize.A4, 50, 50, 50, 50);

            string pdfFileName = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N") + ".pdf");

            using (FileStream fs = new FileStream(pdfFileName, FileMode.Create))
            {
                PdfWriter writer = PdfWriter.GetInstance(document, fs);
                document.Open();

                PdfContentByte cb = writer.DirectContent;
                float margin = 30;
                cb.SetLineWidth(2);
                cb.Rectangle(margin, margin, document.PageSize.Width - 2 * margin, document.PageSize.Height - 2 * margin);
                cb.Stroke();

                string logoPath = Path.Combine(webHostEnvironment.WebRootPath + "/HomeAssets/img/icon/" + "icon-02-primary.png");
                iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(logoPath);
                logo.SetAbsolutePosition((document.PageSize.Width - logo.ScaledWidth) / 2, document.PageSize.Height - 130); 
                document.Add(logo);

                for (int i = 0; i < 3; i++)
                {
                    document.Add(new Paragraph(" "));
                }

                iTextSharp.text.Paragraph heading = new Paragraph("Subscription Details", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 16f));
                heading.Alignment = Element.ALIGN_CENTER;
                for (int i = 0; i < 2; i++)
                {
                    document.Add(new Paragraph(" "));
                }
                document.Add(heading);
                document.Add(new Paragraph(" "));
                document.Add(new Paragraph($"Customer Name: {customerName}"));
                document.Add(new Paragraph($"Subscription Date: {currentDate.ToShortDateString()}"));
                document.Add(new Paragraph($"Amount Paid: ${amount}"));
                document.Add(new Paragraph(" "));
                document.Add(new Paragraph(beneficiaryInfo));

                document.Close();
            }

            return pdfFileName;
        }

        public IActionResult AdminManageTestimonials()
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
            ViewBag.id = HttpContext.Session.GetInt32("testiId");
            ViewBag.name = HttpContext.Session.GetString("testiName");
            ViewBag.email = HttpContext.Session.GetString("testiEmail");
            ViewBag.CurrentDate = DateTime.Now;

            var testimonials = _context.Testimonials.Include(u => u.User);

            return View(testimonials);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateTestimonialStatus(int userid, string newStatus)
        {
            var testimonials = _context.Testimonials.Include(u => u.User).FirstOrDefault(t => t.Id == userid);

            HttpContext.Session.SetInt32("testiId", (Int32)testimonials.Userid);
            HttpContext.Session.SetString("testiName", testimonials.User.Username);
            HttpContext.Session.SetString("testiEmail", testimonials.User.Email);
            HttpContext.Session.SetString("testiPhoneNumber", testimonials.User.PhoneNumber);
            HttpContext.Session.SetString("testiProfilePic", testimonials.User.ProfilePictureUrl);

            ViewBag.testiId = HttpContext.Session.GetInt32("testiId");
            ViewBag.testiName = HttpContext.Session.GetString("testiName");
            ViewBag.testiEmail = HttpContext.Session.GetString("testiEmail");
            ViewBag.CurrentDate = DateTime.Now;

            if (testimonials != null)
            {
                testimonials.Status = newStatus;
                await _context.SaveChangesAsync();
            }

            ViewBag.userLoginId = HttpContext.Session.GetInt32("userLoginId");
            ViewBag.userLoginName = HttpContext.Session.GetString("userLoginName");
            ViewBag.userLoginEmail = HttpContext.Session.GetString("userLoginEmail");

            return RedirectToAction("AdminManageTestimonials");
        }
        public IActionResult ReportStatistics()
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
            var usersTable = _context.Users.ToList();
            var subtable = _context.Subscriptions.ToList();
            var testTable = _context.Testimonials.ToList();
            var beneTable = _context.Beneficiaries.ToList();


            var model = from s in subtable
                        join u in usersTable on s.Userid equals u.Id
                        join t in testTable on u.Id equals t.Userid
                        join b in beneTable on s.Id equals b.Subscriptionid
                        select new JoinTables { users = u, subscriptions = s, beneficiaries = b, testimonials = t };

            var result = Tuple.Create
                <IEnumerable<Users>,
                IEnumerable<Subscriptions>,
                IEnumerable<JoinTables>,
                IEnumerable<Testimonials>,
                IEnumerable<Beneficiaries>>
                (usersTable, subtable, model, testTable, beneTable);


            return View(result);
        }

        [HttpPost]
        public IActionResult ReportStatistics(DateTime? startDate, DateTime? endDate)
        {
            var usersTable = _context.Users.ToList();
            var subtable = _context.Subscriptions.ToList();
            var testTable = _context.Testimonials.ToList();
            var beneTable = _context.Beneficiaries.ToList();


            var model = from s in subtable
                        join u in usersTable on s.Userid equals u.Id
                        join t in testTable on u.Id equals t.Userid
                        join b in beneTable on s.Id equals b.Subscriptionid
                        select new JoinTables { users = u, subscriptions = s, beneficiaries = b, testimonials = t };

            var result = Tuple.Create
                <IEnumerable<Users>,
                IEnumerable<Subscriptions>,
                IEnumerable<JoinTables>,
                IEnumerable<Testimonials>,
                IEnumerable<Beneficiaries>>
                (usersTable, subtable, model, testTable, beneTable);

            return View(result);

        }
    }

}

