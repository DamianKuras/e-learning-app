using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Aggregates.UserProfileAggregate
{
    public class UserProfile
    {
        private UserProfile()
        {
        }
        public Guid Id { get; private set; }
        public string IdentityId { get; private set; }

        public BasicInfo BasicInfo { get; private set; }
        public DateTime Created { get; private set; }
        public DateTime LastUpdated { get; private set; }
        public static UserProfile CreateUserProfile(string identityId, BasicInfo basicInfo)
        {
            var UserProfile = new UserProfile
            {
                IdentityId = identityId,
                BasicInfo = basicInfo,
                Created = DateTime.UtcNow,
                LastUpdated = DateTime.UtcNow,
            };
            return UserProfile;
        }
        public void UpdateBasicInfo(BasicInfo basicInfo)
        {
            BasicInfo = basicInfo;
            LastUpdated = DateTime.UtcNow;
        }
    }
}
