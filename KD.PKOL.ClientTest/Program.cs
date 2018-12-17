using KD.PKOL.TestClient;
using System;

namespace KD.PKOL.ClientTest
{
    class Program
    {
        static void Main(string[] args)
        {
            MessageServiceClient client = new MessageServiceClient();

            MessageRequest request = new MessageRequest()
            {
                Message = "Example Message",
                Subject = "Example Subject",
                Recipient = new Recipient
                {
                    FirstName = "Krzysztof",
                    LastName = "Dobrzyński",
                    LegalForm = LegalForm.Person,
                    Contacts = new Contact[]
                    {
                        new Contact
                        {
                            ContactType = ContactType.Email,
                            Value = "user@gmail.com"
                        }
                    }
                }
            };

            MessageResponse response = client.Send(request);

            Console.WriteLine(response);
        }
    }
}
