global using OneOf;
global using Syki.Shared;
global using Syki.Front.Auth;
global using Syki.Front.Markers;
global using Syki.Front.Configs;
global using Syki.Front.Constants;
global using Syki.Front.Extensions;

global using Syki.Front.Features.Academic.CreateAcademicPeriod;
global using Syki.Front.Features.Academic.CreateEnrollmentPeriod;
global using Syki.Front.Features.Academic.GetAcademicPeriods;
global using Syki.Front.Features.Academic.GetCampi;
global using Syki.Front.Features.Academic.GetAcademicClasses;
global using Syki.Front.Features.Academic.GetCourseCurriculums;
global using Syki.Front.Features.Academic.GetCourseDisciplines;
global using Syki.Front.Features.Academic.GetCourseOfferings;
global using Syki.Front.Features.Academic.GetCourses;
global using Syki.Front.Features.Academic.CrossLogin;
global using Syki.Front.Features.Academic.GetCoursesWithCurriculums;
global using Syki.Front.Features.Academic.GetCoursesWithDisciplines;
global using Syki.Front.Features.Academic.GetDisciplines;
global using Syki.Front.Features.Academic.GetEnrollmentPeriods;
global using Syki.Front.Features.Academic.GetNotifications;
global using Syki.Front.Features.Academic.GetStudents;
global using Syki.Front.Features.Academic.GetTeachers;
global using Syki.Front.Features.Academic.GetAcademicClass;
global using Syki.Front.Features.Academic.UpdateEnrollmentPeriod;
global using Syki.Front.Features.Academic.CreateClass;
global using Syki.Front.Features.Academic.StartClasses;
global using Syki.Front.Features.Academic.FinalizeClasses;
global using Syki.Front.Features.Academic.ReleaseClassesForEnrollment;

global using Syki.Front.Features.Adm.GetUsers;
global using Syki.Front.Features.Adm.GetInstitutions;
global using Syki.Front.Features.Adm.SetupFeatureFlags;
global using Syki.Front.Features.Adm.GetDomainEventsSummary;
global using Syki.Front.Features.Adm.GetDomainEvents;
global using Syki.Front.Features.Adm.GetCommand;
global using Syki.Front.Features.Adm.GetBatches;
global using Syki.Front.Features.Adm.GetBatch;
global using Syki.Front.Features.Adm.GetBatchesSummary;
global using Syki.Front.Features.Adm.ReprocessCommand;
global using Syki.Front.Features.Adm.GetCommandsSummary;

global using Syki.Front.Features.Cross.CreatePendingUserRegister;
global using Syki.Front.Features.Cross.FinishUserRegister;
global using Syki.Front.Features.Cross.GetUserNotifications;
global using Syki.Front.Features.Cross.Health;
global using Syki.Front.Features.Cross.Login;
global using Syki.Front.Features.Cross.LoginMfa;
global using Syki.Front.Features.Cross.ResetPassword;
global using Syki.Front.Features.Cross.SendResetPasswordToken;

global using Syki.Front.Features.Student.CreateStudentEnrollment;
global using Syki.Front.Features.Student.GetCurrentEnrollmentPeriod;
global using Syki.Front.Features.Student.GetStudentAgenda;
global using Syki.Front.Features.Student.GetStudentDisciplines;
global using Syki.Front.Features.Student.GetStudentEnrollmentClasses;
global using Syki.Front.Features.Student.GetStudentNotes;
global using Syki.Front.Features.Student.GetStudentActivities;
global using Syki.Front.Features.Student.CreateClassActivityWork;
global using Syki.Front.Features.Student.GetStudentFrequencies;
global using Syki.Front.Features.Student.GetStudentFrequency;
global using Syki.Front.Features.Student.GetStudentAverageNote;
global using Syki.Front.Features.Student.GetStudentCurrentClasses;
global using Syki.Front.Features.Student.GetStudentClass;

global using Syki.Front.Features.Teacher.GetTeacherClass;
global using Syki.Front.Features.Teacher.GetTeacherAgenda;
global using Syki.Front.Features.Teacher.GetTeacherClasses;
global using Syki.Front.Features.Teacher.GetTeacherClassLessons;
global using Syki.Front.Features.Teacher.GetTeacherClassActivity;
global using Syki.Front.Features.Teacher.GetTeacherCurrentClasses;
global using Syki.Front.Features.Teacher.GetTeacherClassActivities;
global using Syki.Front.Features.Teacher.AddStudentClassActivityNote;
global using Syki.Front.Features.Teacher.GetTeacherLessonAttendances;
global using Syki.Front.Features.Teacher.GetClassNotesRemainingWeights;

global using System.Net.Http.Json;
global using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
