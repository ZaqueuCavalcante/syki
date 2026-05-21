namespace Syki.Back.Auth.Permissions;

public enum PermissionGroup
{
    [Description("Identity")]
    Identity = 0,

    [Description("Users")]
    Users = 1,

    [Description("Campi")]
    Campi = 2,

    [Description("Disciplines")]
    Disciplines = 3,

    [Description("Courses")]
    Courses = 4,

    [Description("Teachers")]
    Teachers = 5,

    [Description("Students")]
    Students = 6,
}
