namespace Identity.Permissions
{
    public static class Permissions
    {
        public static class Users
        {
            public const string View = "Permissions.Users.View";
            public const string Create = "Permissions.Users.Create";
            public const string Edit = "Permissions.Users.Edit";
            public const string ManageRoles = "Permissions.Users.ManageRoles";
            public const string ManageClaims = "Permissions.Users.ManageClaims";
            public const string Delete = "Permissions.Users.Delete";
        }

        public static class Roles
        {
            public const string View = "Permissions.Roles.View";
            public const string Create = "Permissions.Roles.Create";
            public const string Edit = "Permissions.Roles.Edit";
            public const string Delete = "Permissions.Roles.Delete";
            public const string ManageClaims = "Permissions.Roles.ManageClaims";

        }

        public static class OrganizationSetup
        {
            public const string View = "Permissions.OrganizationSetup.View";
            public const string Create = "Permissions.OrganizationSetup.Create";
            public const string Edit = "Permissions.OrganizationSetup.Edit";
            public const string Delete = "Permissions.OrganizationSetup.Delete";
            public const string Details = "Permissions.OrganizationSetup.Details";

        }
    }
}
