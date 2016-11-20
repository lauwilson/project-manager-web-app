using System.Collections.Generic;
using System.Web.Mvc;

namespace Hemlock.Models
{
    public class EditEmployeeViewModel
    {
        public Employee Employee { get; set; }
        public IEnumerable<SelectListItem> ListOfPositions { get; set; }
        public bool HasAdminPrivileges { get; set; }
    }
}