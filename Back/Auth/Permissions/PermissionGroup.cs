namespace Estud.Back.Auth.Permissions;

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

    [Description("Periods")]
    Periods = 7,

    [Description("CourseCurriculums")]
    CourseCurriculums = 8,

    [Description("CourseOfferings")]
    CourseOfferings = 9,

    [Description("Classes")]
    Classes = 10,

    [Description("Webhooks")]
    Webhooks = 11,

    [Description("Notifications")]
    Notifications = 12,

    [Description("Classrooms")]
    Classrooms = 13,
}
