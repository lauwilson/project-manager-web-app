using System;

namespace Hemlock.Models.Enum
{
    [Flags]
    public enum PermissionsEnum
    {
    CanViewOwnActivity = (1 << 0),
    CanAddEditOwnActivity = (1 << 1),
    CanViewAllActivity = (1 << 2),
    CanAddEditAllActivity = (1 << 3),
    ManageUsers = (1 << 4),
    ManageProjects = (1 << 5),
    ManageCategories = (1 << 6),
    ManageStaff = (1 << 7),
    CanDeleteActivity = (1 << 8),
    Admin = (CanViewOwnActivity |
            CanAddEditOwnActivity |
            CanViewAllActivity |
            CanAddEditAllActivity |
            ManageUsers |
            ManageProjects |
            ManageCategories |
            ManageStaff |
            CanDeleteActivity),
    User = (CanViewOwnActivity | CanAddEditOwnActivity | CanDeleteActivity)
    }
}