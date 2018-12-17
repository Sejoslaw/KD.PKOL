using System.Runtime.Serialization;
using System.ServiceModel;

namespace KD.PKOL.Models.MessageService
{
    [ServiceContract]
    public interface IMessageService
    {
        [OperationContract]
        MessageResponse Send(MessageRequest message);
    }

    [DataContract]
    public class MessageRequest
    {
        [DataMember]
        public string Subject { get; set; }
        [DataMember]
        public string Message { get; set; }
        [DataMember]
        public Recipient Recipient { get; set; }
    }

    [DataContract]
    public class Recipient
    {
        [DataMember]
        public string FirstName { get; set; }
        [DataMember]
        public string LastName { get; set; }
        [DataMember]
        public LegalForm LegalForm { get; set; }
        [DataMember]
        public Contact[] Contacts { get; set; }
    }

    [DataContract]
    public class Contact
    {
        [DataMember]
        public ContactType ContactType { get; set; }
        [DataMember]
        public string Value { get; set; }
    }

    [DataContract]
    public class MessageResponse
    {
        [DataMember]
        public ReturnCode ReturnCode { get; set; }
        [DataMember]
        public string ErrorMessage { get; set; }
    }

    [DataContract]
    public enum ReturnCode
    {
        [EnumMember(Value = "Success")]
        Success,
        [EnumMember(Value = "ValidationError")]
        ValidationError,
        [EnumMember(Value = "InternalError")]
        InternalError
    }

    [DataContract]
    public enum LegalForm
    {
        [EnumMember(Value = "Person")]
        Person,
        [EnumMember(Value = "Company")]
        Company
    }

    [DataContract]
    public enum ContactType
    {
        [EnumMember(Value = "Mobile")]
        Mobile,
        [EnumMember(Value = "Fax")]
        Fax,
        [EnumMember(Value = "Email")]
        Email,
        [EnumMember(Value = "OfficePhone")]
        OfficePhone,
        [EnumMember(Value = "OfficeFax")]
        OfficeFax,
        [EnumMember(Value = "OfficeEmail")]
        OfficeEmail
    }
}
