using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Hemlock.Models
{
    public class Permission
    {
        [Key]
        public int Bit { get; set; }
        public string PermissionType { get; set; }
    }
}