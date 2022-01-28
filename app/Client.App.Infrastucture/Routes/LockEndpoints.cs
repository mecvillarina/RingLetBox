using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.App.Infrastructure.Routes
{
    public static class LockEndpoints
    {
        public const string GetAll = "api/lock/getAll";
        public const string GetAllBySender = "api/lock/getAllBySender/{0}";
        public const string AddNewLock = "api/lock/addNew";
        public const string ClaimLock = "api/lock/claim";
        public const string RevokeLock = "api/lock/revoke";
        public const string RefundRevokedLock = "api/lock/refundRevoked";
    }
}
