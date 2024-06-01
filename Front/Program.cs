using Syki.Front.Configs;

var builder = WasmConfigs.CreateHostBuilder(args);

builder.AddMudConfigs();
builder.AddHttpConfigs();
builder.AddAuthConfigs();
builder.AddAcademicServicesConfigs();
builder.AddAdmServicesConfigs();
builder.AddCrossServicesConfigs();
builder.AddStudentServicesConfigs();
builder.AddTeacherServicesConfigs();
builder.AddSellerServicesConfigs();
builder.AddLocalStorageConfigs();

await builder.Build().RunAsync();
