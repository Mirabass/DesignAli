using System;

namespace DA.Core.Models
{
    public interface ILoggedInUserModel
    {
        public DateTime CreatedDate { get; set; }
        public string? EmailAdress { get; set; }
        public string? FirstName { get; set; }
        public string? Id { get; set; }
        public string? LastName { get; set; }
        public string? Token { get; set; }

        void LogOffUser();
    }
}