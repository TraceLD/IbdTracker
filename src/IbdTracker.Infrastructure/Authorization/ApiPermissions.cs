using System.Collections.Generic;

namespace IbdTracker.Infrastructure.Authorization
{
    public static class ApiPermissions
    {
        public static IEnumerable<string> PermissionsList => new List<string>
        {
            "read:patient",
            "read:allpatients",
            "read:assignedpatients",
            "read:appointments",
            "write:appointments",
            "read:prescriptions",
            "read:bms",
            "write:bms",
            "read:meals",
            "write:meals",
            "read:pain",
            "write:pain",
            "write:notifications",
            "read:recommendations",
            "read:doctor",
            "write:doctor"
        };
    }
}