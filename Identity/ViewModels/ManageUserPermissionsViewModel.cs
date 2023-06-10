using Identity.Helpers;
using System.Collections.Generic;

namespace Identity.ViewModels
{
    public class ManageUserPermissionsViewModel
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string PermissionValue { get; set; }
        public IList<ManageUserClaimViewModel> ManagePermissionsViewModel { get; set; }
    }
}
