namespace Escc.WebAuthorMonitoring.WebApi.Model
{
    class Users
    {

        public string Name { get; set; }
        public string EmailAddress { get; set; }
        public string UserName { get; set; }
        public int WebAuthorId { get; set; }

        public Users(string name, string emailAddress, string userName, int webAuthorId)
        {
            Name = name;
            EmailAddress = emailAddress;
            UserName = userName;
            WebAuthorId = webAuthorId;
        }
    }
}
