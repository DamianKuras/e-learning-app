using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Aggregates.UserProfileAggregate
{
    public class BasicInfo
    {
        private BasicInfo()
        {
        }
        public string FirstName { get; private set; }
        public string LastName { get;private set; }
        public string EmailAddress { get;private set; }  


        public static BasicInfo CreateBasicInfo(string firstName,string lastName, string emailAddress)
        {
            BasicInfo basicInfo = new BasicInfo
            {
                FirstName = firstName,
                LastName = lastName,
                EmailAddress = emailAddress,
            };
            return basicInfo;
        }


    }
}
