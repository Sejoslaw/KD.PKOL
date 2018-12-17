using KD.PKOL.Db;
using KD.PKOL.Models.Dtos;
using KD.PKOL.Models.MessageService;
using NLog;
using System;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace KD.PKOL.Services
{
    public class MessageService : IMessageService
    {
        private static ILogger LOGGER = LogManager.GetCurrentClassLogger();

        private IDbContext Context { get; }

        public MessageService()
        {
            this.Context = DependencyContainer.INSTANCE.Resolve<IDbContext>();
        }

        public MessageResponse Send(MessageRequest request)
        {
            if (request == null)
            {
                return null;
            }

            MessageResponse response = new MessageResponse();

            if (request?.Recipient?.LegalForm == LegalForm.Person)
            {
                LOGGER.Info("Handling person form.");
                this.HandlePerson(request, response);
            }
            else if (request?.Recipient?.LegalForm == LegalForm.Company)
            {
                LOGGER.Info("Handling company form.");
                this.HandleCompany(request, response);
            }
            else
            {
                LOGGER.Error($"Unknown Legal Form type: { request?.Recipient?.LegalForm }");
                response.ErrorMessage = $"Unknown Legal Form type: { request?.Recipient?.LegalForm }";
                response.ReturnCode = ReturnCode.ValidationError;
            }

            return response;
        }

        private void HandlePerson(MessageRequest request, MessageResponse response)
        {
            if (string.IsNullOrWhiteSpace(request?.Recipient?.FirstName) || string.IsNullOrWhiteSpace(request?.Recipient?.LastName))
            {
                LOGGER.Error("Fields FirstName and LastName are required.");
                response.ErrorMessage = "Fields FirstName and LastName are required.";
                response.ReturnCode = ReturnCode.ValidationError;
                return;
            }

            Contact email = request?.Recipient?.Contacts?.FirstOrDefault(contact => contact?.ContactType == ContactType.Email);

            this.IsEmailValid(request, email, response);
        }

        private void HandleCompany(MessageRequest request, MessageResponse response)
        {
            if (string.IsNullOrWhiteSpace(request?.Recipient?.LastName))
            {
                LOGGER.Error("Field LastName is required.");
                response.ErrorMessage = "Field LastName is required.";
                response.ReturnCode = ReturnCode.ValidationError;
                return;
            }

            Contact email = request?.Recipient?.Contacts?.FirstOrDefault(contact => contact?.ContactType == ContactType.OfficeEmail);

            this.IsEmailValid(request, email, response);
        }

        private bool IsEmailValid(MessageRequest request, Contact email, MessageResponse response)
        {
            if (email == null)
            {
                LOGGER.Error("At least one Contact Type must contain a valid email.");
                response.ErrorMessage = "At least one Contact Type must contain a valid email.";
                response.ReturnCode = ReturnCode.ValidationError;
                return false;
            }

            try
            {
                MailAddress emailAddress = new MailAddress(email?.Value);

                LOGGER.Info("Request is valid.");
                response.ErrorMessage = "Request is valid.";
                response.ReturnCode = ReturnCode.Success;

                this.SendValidationEmail(email?.Value, request);

                return true;
            }
            catch (FormatException)
            {
                LOGGER.Error($"Invalid email address: { email?.Value }");
                response.ErrorMessage = $"Invalid email address: { email?.Value }";
                response.ReturnCode = ReturnCode.ValidationError;
                return false;
            }
        }

        private void SendValidationEmail(string email, MessageRequest request)
        {
            Task.Run(() =>
            {
                LOGGER.Info($"Sending email to: { email }...");

                SmtpClient client = new SmtpClient
                {
                    Port = Settings.PORT,
                    Host = Settings.HOST,
                    EnableSsl = Settings.SSL,
                    Timeout = Settings.TIMEOUT,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(Settings.USERNAME, Settings.PASSWORD)
                };

                MailMessage mail = new MailMessage(Settings.EMAILFROM, email, request?.Subject, request?.Message)
                {
                    BodyEncoding = Encoding.UTF8,
                    DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure
                };

                try
                {
                    client.Send(mail);
                    LOGGER.Info($"Email sent to: { email }.");

                    this.SaveEmailInDatabase(email, mail);
                }
                catch (Exception ex)
                {
                    LOGGER.Error($"Error when sending email to: { email }.");
                    LOGGER.Error(ex);

                    this.SaveEmailInDatabase(email, mail, ex);
                }
            });
        }

        private void SaveEmailInDatabase(string email, MailMessage mail, Exception exception = null)
        {
            try
            {
                MessageDto dto = new MessageDto
                {
                    Id = Guid.NewGuid(),
                    Address = email,
                    Message = mail?.Body,
                    SendTime = DateTime.Now,
                    ErrorMessage = exception?.Message,
                    ErrorTag = exception?.HResult.ToString()
                };

                LOGGER.Info($"Saving email '{ email }' in database...");
                this.Context.Create(dto);
                this.Context.Save();
                LOGGER.Info($"Email '{ email }' saved in database.");
            }
            catch (Exception ex)
            {
                LOGGER.Error($"Error when creating record in database.");
                LOGGER.Error(ex);
            }
        }
    }
}
