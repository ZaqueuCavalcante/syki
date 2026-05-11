# Features — Agrupamento por Contexto

- SSO Multi-Tenant
- Database Modeling + Indexing
- Software Architecture
- Caching (Fusion Cache + Redis)
- RBAC (Role-Based Access Control)
- API Design + Docs
- Software Testing
- Load Balancing
- CAP Theorem
- Message Queues
- Rate Limiting
- WebSockets & SSE
- Fault Tolerance
- Monitoring
- Authentication & Authorization
- Data Structures + Algorithms
- Blob Storage
- Webhooks





## Academic (51 features → 13 grupos)

```
Academic/
├── Campi/              (7) CreateCampus, UpdateCampus, GetCampi, GetCampusClasses, GetCampusCourseOfferings, GetCampusStudents, GetCampusTeachers
├── Courses/            (4) CreateCourse, GetCourses, GetCoursesWithCurriculums, GetCoursesWithDisciplines
├── Curricula/          (3) CreateCourseCurriculum, GetCourseCurriculums, GetCourseDisciplines
├── CourseOfferings/    (2) CreateCourseOffering, GetCourseOfferings
├── Disciplines/        (3) CreateDiscipline, GetDisciplines, AddDisciplinePreRequisites
├── AcademicPeriods/    (3) CreateAcademicPeriod, GetAcademicPeriods, GetCurrentAcademicPeriod
├── EnrollmentPeriods/  (3) CreateEnrollmentPeriod, GetEnrollmentPeriods, UpdateEnrollmentPeriod
├── Teachers/           (5) CreateTeacher, GetTeachers, GetAcademicTeacher, AssignCampiToTeacher, AssignDisciplinesToTeacher
├── Students/           (2) CreateStudent, GetStudents
├── Classes/            (6) CreateClass, GetAcademicClass, GetAcademicClasses, StartClasses, FinalizeClasses, ReleaseClassesForEnrollment
├── Classrooms/         (4) CreateClassroom, GetClassrooms, AssignClassToClassroom, GetClassroomAgenda
├── Notifications/      (2) CreateNotification, GetNotifications
├── Webhooks/           (6) CreateWebhookSubscription, GetWebhooks, GetWebhookSubscription, GetWebhookCall, CallWebhooks, ReprocessWebhookCall
└── Insights/           (1) GetAcademicInsights
```

## Adm (12 features → 6 grupos)

```
Adm/
├── Commands/       (4) GetCommand, GetCommands, GetCommandsSummary, ReprocessCommand
├── Batches/        (3) GetBatch, GetBatches, GetBatchesSummary
├── FeatureFlags/   (2) GetFeatureFlags, SetupFeatureFlags
├── Institutions/   (1) GetInstitutions
├── Users/          (1) GetUsers
└── Insights/       (1) GetAdmInsights
```

## Cross (23 features → 4 grupos)

```
Cross/
├── Auth/           (7) Login, LoginMfa, Logout, SetupMfa, GetMfaKey, SendResetPasswordToken, ResetPassword
├── Users/          (5) CreatePendingUserRegister, FinishUserRegister, GetUserAccount, UpdateUserAccount, CreateUser
├── Notifications/  (3) GetUserNotifications, ViewNotifications, LinkOldNotifications
└── System/         (8) Health, Home, LoadFeatureFlags, SeedInstitutionData, SeedInstitutionTeachersCommand, CreateInstitution, SignIn, CreatePreSignedUrlForUpload
```

## Student (16 features → 4 grupos)

```
Student/
├── Classes/        (5) GetStudentCurrentClasses, GetStudentClass, GetStudentClassActivities, GetStudentClassActivity, GetStudentClassFrequency
├── Enrollment/     (3) CreateStudentEnrollment, GetCurrentEnrollmentPeriod, GetStudentEnrollmentClasses
├── Notes/          (5) GetStudentAverageNote, GetStudentNotes, GetStudentFrequencies, GetStudentFrequency, CreateClassActivityWork
└── Agenda/         (3) GetStudentAgenda, GetStudentInsights, GetStudentDisciplines
```

## Teacher (16 features → 4 grupos)

```
Teacher/
├── Classes/        (5) GetTeacherClasses, GetTeacherCurrentClasses, GetTeacherClass, GetTeacherClassLessons, GetTeacherClassStudents
├── Activities/     (6) CreateClassActivity, GetTeacherClassActivities, GetTeacherClassActivity, AddClassActivityNote, AddStudentClassActivityNote, GetClassNotesRemainingWeights
├── Attendance/     (2) CreateLessonAttendance, GetTeacherLessonAttendances
└── Agenda/         (3) SetSchedulingPreferences, GetTeacherAgenda, GetTeacherInsights
```
