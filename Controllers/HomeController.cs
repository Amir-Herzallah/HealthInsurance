using HealthInsurance.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Net.Mail;
using System.Net;
using Microsoft.CodeAnalysis.Differencing;
using Microsoft.AspNetCore.Authorization.Policy;
using System.Reflection.Metadata;
using iTextSharp.text.pdf;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using System.Net.Mime;
using System.IO;

namespace HealthInsurance.Controllers
{
    public class HomeController : Controller
    {
        private readonly ModelContext _context;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, ModelContext context, IWebHostEnvironment webHostEnvironment)
        {
            _logger = logger;
            _context = context;
            this.webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            ViewBag.id = HttpContext.Session.GetInt32("Id");
            ViewBag.name = HttpContext.Session.GetString("Name");
            ViewBag.email = HttpContext.Session.GetString("Email");
            ViewBag.phoneNumber = HttpContext.Session.GetString("PhoneNumber");
            ViewBag.profilePic = HttpContext.Session.GetString("ProfilePic");
            ViewBag.CurrentDate = DateTime.Now;
            ViewBag.userLoginId = HttpContext.Session.GetInt32("userLoginId");
            ViewBag.userLoginName = HttpContext.Session.GetString("userLoginName");
            ViewBag.userLoginEmail = HttpContext.Session.GetString("userLoginEmail");

            var homePage = _context.HomePage.ToList();
            return View(homePage);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        // GET:
        public async Task<IActionResult> EditProfile(decimal? id)
        {
            ViewBag.id = HttpContext.Session.GetInt32("Id");
            ViewBag.name = HttpContext.Session.GetString("Name");
            ViewBag.email = HttpContext.Session.GetString("Email");
            ViewBag.phoneNumber = HttpContext.Session.GetString("PhoneNumber");
            ViewBag.profilePic = HttpContext.Session.GetString("ProfilePic");
            ViewBag.CurrentDate = DateTime.Now;
            ViewBag.userLoginId = HttpContext.Session.GetInt32("userLoginId");
            ViewBag.userLoginName = HttpContext.Session.GetString("userLoginName");
            ViewBag.userLoginEmail = HttpContext.Session.GetString("userLoginEmail");

            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var users = await _context.Users.FindAsync(id);
            if (users == null)
            {
                return NotFound();
            }
            ViewData["Roleid"] = new SelectList(_context.Roles, "Id", "Id", users.Roleid);
            return View(users);
        }

        // POST
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProfile(decimal id, [Bind("Id,Username,Password,Email,PhoneNumber,RegistrationDate,Roleid,ProfilePictureUrl,ProfilePictureFile")] Users users)
        {
            ViewBag.id = HttpContext.Session.GetInt32("Id");
            ViewBag.name = HttpContext.Session.GetString("Name");
            ViewBag.email = HttpContext.Session.GetString("Email");
            ViewBag.phoneNumber = HttpContext.Session.GetString("PhoneNumber");
            ViewBag.profilePic = HttpContext.Session.GetString("ProfilePic");
            ViewBag.CurrentDate = DateTime.Now;
            ViewBag.userLoginId = HttpContext.Session.GetInt32("userLoginId");
            ViewBag.userLoginName = HttpContext.Session.GetString("userLoginName");
            ViewBag.userLoginEmail = HttpContext.Session.GetString("userLoginEmail");

            if (id != users.Id)
            {
                return NotFound();
            }

            var existingUser = await _context.Users.FindAsync(users.Id);

            if (existingUser != null)
            {
                if (!string.IsNullOrEmpty(users.Username))
                {
                    existingUser.Username = users.Username;
                }
                if (!string.IsNullOrEmpty(users.Password))
                {
                    existingUser.Password = users.Password;
                }
                if (!string.IsNullOrEmpty(users.Email))
                {
                    existingUser.Email = users.Email;
                }
                if (!string.IsNullOrEmpty(users.PhoneNumber))
                {
                    existingUser.PhoneNumber = users.PhoneNumber;
                }
                if (!string.IsNullOrEmpty(users.ProfilePictureUrl))
                {
                    existingUser.ProfilePictureUrl = users.ProfilePictureUrl;
                }

                if (users.ProfilePictureFile != null)
                {
                    string wwwRootPath = webHostEnvironment.WebRootPath;
                    string fileName = Guid.NewGuid().ToString() + users.ProfilePictureFile.FileName;
                    string path = Path.Combine(wwwRootPath, "images", fileName);

                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await users.ProfilePictureFile.CopyToAsync(fileStream);
                    }

                    existingUser.ProfilePictureUrl = fileName;
                }

                _context.Update(existingUser);
                await _context.SaveChangesAsync();

                if (existingUser.Roleid == 1)
                {
                    return RedirectToAction("Index", "Admin");
                }
                else if (existingUser.Roleid == 2)
                {
                    TempData["EditSuccess"] = "You have edited your profile successfully!";
                    return RedirectToAction("EditProfile", "Home");
                }
            }
            else
            {
                return NotFound();
            }

            ViewData["Roleid"] = new SelectList(_context.Roles, "Id", "Id", users.Roleid);
            return View(users);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
       
        public IActionResult Subscriptions()
        {
            ViewBag.id = HttpContext.Session.GetInt32("Id");
            ViewBag.name = HttpContext.Session.GetString("Name");
            ViewBag.email = HttpContext.Session.GetString("Email");
            ViewBag.phoneNumber = HttpContext.Session.GetString("PhoneNumber");
            ViewBag.profilePic = HttpContext.Session.GetString("ProfilePic");
            ViewBag.CurrentDate = DateTime.Now;

            return View();
        }
        public IActionResult CardCheck()
        {
            ViewBag.id = HttpContext.Session.GetInt32("Id");
            ViewBag.name = HttpContext.Session.GetString("Name");
            ViewBag.email = HttpContext.Session.GetString("Email");
            ViewBag.phoneNumber = HttpContext.Session.GetString("PhoneNumber");
            ViewBag.profilePic = HttpContext.Session.GetString("ProfilePic");
            ViewBag.CurrentDate = DateTime.Now;
            ViewBag.userLoginId = HttpContext.Session.GetInt32("userLoginId");
            ViewBag.userLoginName = HttpContext.Session.GetString("userLoginName");
            ViewBag.userLoginEmail = HttpContext.Session.GetString("userLoginEmail");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CardCheck([Bind("CardNo, CardHolderName, Cvv")] Bank bank)
        {
            ViewBag.id = HttpContext.Session.GetInt32("Id");
            ViewBag.name = HttpContext.Session.GetString("Name");
            ViewBag.email = HttpContext.Session.GetString("Email");
            ViewBag.phoneNumber = HttpContext.Session.GetString("PhoneNumber");
            ViewBag.profilePic = HttpContext.Session.GetString("ProfilePic");
            ViewBag.CurrentDate = DateTime.Now;
            ViewBag.userLoginId = HttpContext.Session.GetInt32("userLoginId");
            ViewBag.userLoginName = HttpContext.Session.GetString("userLoginName");
            ViewBag.userLoginEmail = HttpContext.Session.GetString("userLoginEmail");

            var check = _context.Bank.FirstOrDefault(x => x.CardNo == bank.CardNo && x.CardHolderName == bank.CardHolderName && x.Cvv == bank.Cvv);

            if (check != null)
            {
                var card = await _context.Bank.Where(x => x.CardNo == bank.CardNo).FirstOrDefaultAsync();

                if (card.Balance >= 50)
                {
                    card.Balance -= 50;
                    card.PaymentMethod = "CreditCard";

                    var userId = HttpContext.Session.GetInt32("Id");
                    var subscription = _context.Subscriptions.FirstOrDefault(s => s.Userid == userId);

                    if (subscription == null)
                    {
                        subscription = new Subscriptions
                        {
                            Userid = userId,
                            StartDate = DateTime.Now,
                            Amount = 50,
                            Status = "Subscribed",
                        };
                        _context.Add(subscription);

                        _context.Update(card);
                    }
                    else
                    {
                        TempData["SubError"] = "You are already subscribed!";
                        return RedirectToAction("Subscriptions", "Home");
                    }

                    using (SmtpClient smtpClient = new SmtpClient("smtp.gmail.com"))
                    {
                        smtpClient.Port = 587;
                        smtpClient.UseDefaultCredentials = false;
                        smtpClient.Credentials = new NetworkCredential("amirherzalla8@gmail.com", "ccthxdcuhqqudgvi");
                        smtpClient.EnableSsl = true;

                        MailMessage mailMessage = new MailMessage
                        {
                            From = new MailAddress("amirherzalla8@gmail.com"),
                            Subject = "Invoice for Subscription",
                            Body = "Congratulations you have subscribed successfully! here is your payment details"
                        };

                        mailMessage.To.Add(ViewBag.email);

                        var pdfFileName = GenerateInvoicePDF(ViewBag.name, ViewBag.CurrentDate, 50.0); 
                        
                        mailMessage.Attachments.Add(new Attachment(pdfFileName, MediaTypeNames.Application.Pdf));

                        smtpClient.Send(mailMessage);
                    }

                    await _context.SaveChangesAsync();

                    TempData["PaymentSuccess"] = "Payment successful. Please check your email for the invoice.";

                    return RedirectToAction("Subscriptions", "Home");
                }
                else
                {
                    TempData["PaymentError"] = "Payment failed. Insufficient balance.";
                }
            }
            else
            {
                TempData["PaymentError"] = "Payment failed. Invalid card details.";
            }

            return RedirectToAction("Subscriptions", "Home");
        }
        public string GenerateInvoicePDF(string customerName, DateTime currentDate, double amount)
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
                logo.SetAbsolutePosition((document.PageSize.Width - logo.ScaledWidth) / 2, document.PageSize.Height - 130); // Adjust the position as needed
                document.Add(logo);

                for (int i = 0; i < 3; i++)
                {
                    document.Add(new Paragraph(" "));
                }

                Paragraph heading = new Paragraph("Invoice", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 16f));
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

                document.Close();
            }

            return pdfFileName;
        }

        public IActionResult AddBeneficiaries()
        {
            ViewBag.BeneId = HttpContext.Session.GetInt32("BeneId");
            ViewBag.BeneName = HttpContext.Session.GetString("BeneName");
            ViewBag.BeneRelToSub = HttpContext.Session.GetString("BeneRelToSub");
            ViewBag.id = HttpContext.Session.GetInt32("Id");
            ViewBag.name = HttpContext.Session.GetString("Name");
            ViewBag.email = HttpContext.Session.GetString("Email");
            ViewBag.phoneNumber = HttpContext.Session.GetString("PhoneNumber");
            ViewBag.profilePic = HttpContext.Session.GetString("ProfilePic");
            ViewBag.CurrentDate = DateTime.Now;
            ViewBag.userLoginId = HttpContext.Session.GetInt32("userLoginId");
            ViewBag.userLoginName = HttpContext.Session.GetString("userLoginName");
            ViewBag.userLoginEmail = HttpContext.Session.GetString("userLoginEmail");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddBeneficiaries([Bind("Subscriptionid,Name,DateOfBirth,Gender,RelationshipToSubscriber,Status,BeneficiaryImagePath,BeneficiaryImageFile,BeneficiaryCreationDate")] Beneficiaries beneficiaries)
        {
            ViewBag.id = HttpContext.Session.GetInt32("Id");
            ViewBag.name = HttpContext.Session.GetString("Name");
            ViewBag.email = HttpContext.Session.GetString("Email");
            ViewBag.phoneNumber = HttpContext.Session.GetString("PhoneNumber");
            ViewBag.profilePic = HttpContext.Session.GetString("ProfilePic");
            ViewBag.CurrentDate = DateTime.Now;

            beneficiaries.Status = "Pending";

            var userId = HttpContext.Session.GetInt32("Id");
            var subscription = _context.Subscriptions.FirstOrDefault(s => s.Userid == userId);

            if (subscription == null)
            {
                TempData["NotSubed"] = "You must be subscribed first!";
                return RedirectToAction("Subscriptions", "Home");
            }
            else
            {
                if (beneficiaries.BeneficiaryImageFile != null)
                {
                    string wwwRootPath = webHostEnvironment.WebRootPath;
                    string fileName = Guid.NewGuid().ToString() + beneficiaries.BeneficiaryImageFile.FileName;
                    string path = Path.Combine(wwwRootPath + "/images/" + fileName);

                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await beneficiaries.BeneficiaryImageFile.CopyToAsync(fileStream);
                    }

                    beneficiaries.BeneficiaryImagePath = fileName;
                }

                beneficiaries.BeneficiaryCreationDate = DateTime.Now.Date;
                HttpContext.Session.SetInt32("BeneId", (int)beneficiaries.Id);
                HttpContext.Session.SetString("BeneName", beneficiaries.Name);
                HttpContext.Session.SetString("BeneRelToSub", beneficiaries.RelationshipToSubscriber);

                var newBeneficiary = new Beneficiaries
                {
                    Subscriptionid = beneficiaries.Subscriptionid,
                    Name = beneficiaries.Name,
                    DateOfBirth = beneficiaries.DateOfBirth,
                    Gender = beneficiaries.Gender,
                    RelationshipToSubscriber = beneficiaries.RelationshipToSubscriber,
                    Status = beneficiaries.Status,
                    BeneficiaryImagePath = beneficiaries.BeneficiaryImagePath,
                    BeneficiaryCreationDate = beneficiaries.BeneficiaryCreationDate
                };

                subscription.Beneficiaries.Add(newBeneficiary);

                await _context.SaveChangesAsync();
            }
            TempData["BeneSuccess"] = "Your request has been successfully submitted. Please check your email for further instructions and updates.";
            return RedirectToAction("AddBeneficiaries", "Home");
        }

        // GET: Testimonials/Create
        public IActionResult AddTestimonials()
        {
            ViewBag.BeneId = HttpContext.Session.GetInt32("BeneId");
            ViewBag.BeneName = HttpContext.Session.GetString("BeneName");
            ViewBag.BeneRelToSub = HttpContext.Session.GetString("BeneRelToSub");
            ViewBag.id = HttpContext.Session.GetInt32("Id");
            ViewBag.name = HttpContext.Session.GetString("Name");
            ViewBag.email = HttpContext.Session.GetString("Email");
            ViewBag.phoneNumber = HttpContext.Session.GetString("PhoneNumber");
            ViewBag.profilePic = HttpContext.Session.GetString("ProfilePic");
            ViewBag.CurrentDate = DateTime.Now;
            ViewBag.userLoginId = HttpContext.Session.GetInt32("userLoginId");
            ViewBag.userLoginName = HttpContext.Session.GetString("userLoginName");
            ViewBag.userLoginEmail = HttpContext.Session.GetString("userLoginEmail");

            ViewData["Userid"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: Testimonials/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddTestimonials([Bind("Id,Userid,Rating,Commentt,SubmissionDate,Status")] Testimonials testimonials)
        {
            ViewBag.id = HttpContext.Session.GetInt32("Id");
            testimonials.Userid = ViewBag.id;

            testimonials.Status = "Pending";
            testimonials.SubmissionDate = DateTime.Now.Date;

            _context.Add(testimonials);
            await _context.SaveChangesAsync();


            ViewData["Userid"] = new SelectList(_context.Users, "Id", "Id", testimonials.Userid);
            TempData["TestimonialSuccess"] = "Your testimonial has been successfully submitted. It will be reviewed by our admins";

            return RedirectToAction("Index", "Testimonials");
        }

    }
}