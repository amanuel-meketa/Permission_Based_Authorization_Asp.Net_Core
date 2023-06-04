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

        public static class TestUser
        {
            public const string View = "Permissions.TestUser.View";
            public const string Create = "Permissions.TestUser.Create";
            public const string Edit = "Permissions.TestUser.Edit";
            public const string Delete = "Permissions.TestUser.Delete";
            public const string Details = "Permissions.TestUser.Details";

        }
    }
}
