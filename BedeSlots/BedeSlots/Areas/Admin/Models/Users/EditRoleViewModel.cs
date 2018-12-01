using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace BedeSlots.Web.Areas.Admin.Models.Users
{
    public class EditRoleViewModel
    {
        public string UserId { get; set; }

        public string UserName { get; set; }

        public string RoleId { get; set; }

        public ICollection<SelectListItem> Roles { get; set; }
    }
}
