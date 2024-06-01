global using Syki.Shared;
global using Syki.Front.Auth;
global using Syki.Front.Extensions;

global using Syki.Front.Features.Academic.CreateAcademicPeriod;
global using Syki.Front.Features.Academic.CreateCampus;
global using Syki.Front.Features.Academic.CreateClass;
global using Syki.Front.Features.Academic.CreateCourse;
global using Syki.Front.Features.Academic.CreateCourseCurriculum;
global using Syki.Front.Features.Academic.CreateCourseOffering;
global using Syki.Front.Features.Academic.CreateDiscipline;
global using Syki.Front.Features.Academic.CreateEnrollmentPeriod;
global using Syki.Front.Features.Academic.CreateNotification;
global using Syki.Front.Features.Academic.CreateStudent;
global using Syki.Front.Features.Academic.CreateTeacher;
global using Syki.Front.Features.Academic.GetAcademicInsights;
global using Syki.Front.Features.Academic.GetAcademicPeriods;
global using Syki.Front.Features.Academic.GetCampi;
global using Syki.Front.Features.Academic.GetClasses;
global using Syki.Front.Features.Academic.GetCourseCurriculums;
global using Syki.Front.Features.Academic.GetCourseDisciplines;
global using Syki.Front.Features.Academic.GetCourseOfferings;
global using Syki.Front.Features.Academic.GetCourses;
global using Syki.Front.Features.Academic.GetCoursesWithCurriculums;
global using Syki.Front.Features.Academic.GetCoursesWithDisciplines;
global using Syki.Front.Features.Academic.GetDisciplines;
global using Syki.Front.Features.Academic.GetNotifications;
global using Syki.Front.Features.Academic.GetStudents;
global using Syki.Front.Features.Academic.GetTeachers;
global using Syki.Front.Features.Academic.UpdateCampus;
global using Syki.Front.Features.Academic.GetEnrollmentPeriods;

global using Syki.Front.Features.Adm.GetAdmInsights;
global using Syki.Front.Features.Adm.GetInstitutions;
global using Syki.Front.Features.Adm.GetUsers;

global using Syki.Front.Features.Cross.CreatePendingUserRegister;
global using Syki.Front.Features.Cross.FinishUserRegister;
global using Syki.Front.Features.Cross.GetMfaKey;
global using Syki.Front.Features.Cross.GetUserNotifications;
global using Syki.Front.Features.Cross.Login;
global using Syki.Front.Features.Cross.LoginMfa;
global using Syki.Front.Features.Cross.ResetPassword;
global using Syki.Front.Features.Cross.SendResetPasswordToken;
global using Syki.Front.Features.Cross.SetupMfa;
global using Syki.Front.Features.Cross.ViewNotifications;

global using Syki.Front.Features.Student.CreateStudentEnrollment;
global using Syki.Front.Features.Student.GetCurrentEnrollmentPeriod;
global using Syki.Front.Features.Student.GetStudentAgenda;
global using Syki.Front.Features.Student.GetStudentDisciplines;
global using Syki.Front.Features.Student.GetStudentEnrollmentClasses;
global using Syki.Front.Features.Student.GetStudentInsights;

global using Syki.Front.Features.Teacher.GetTeacherAgenda;
global using Syki.Front.Features.Teacher.GetTeacherClasses;
global using Syki.Front.Features.Teacher.GetTeacherInsights;

global using Syki.Front.Features.Seller.GetSellerCourseOfferings;

global using System.Net.Http.Json;
global using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
