using Exato.Web;

namespace Exato.Back.Features.Web.AccountManagementCompany;

public class GetCompanyUserService(WebDbContext webCtx) : IWebService
{
    public async Task<ExatoWebResponseWrapper> Get(Guid userUid)
    {
        var result = new GetCompanyUserResponse();

        var user = await webCtx.Users.FirstOrDefaultAsync(x => x.UserUid == userUid.ToString());
        if (user == null) return ExatoWebResponseWrapper.NewError(ExatoWebErrorCodes.OnlyCompanyAdminsCanDoIt);

        var userCompanies = await webCtx.WebUserCompanies.Where(x => x.UserId == user.Id).ToListAsync();
        if (userCompanies == null || userCompanies.Count == 0) return ExatoWebResponseWrapper.NewError(ExatoWebErrorCodes.InvalidUserData);

        var userExternalId = userCompanies.FirstOrDefault(x => x.CompanyId == null)?.UserExternalId;
        if (userExternalId == null) return ExatoWebResponseWrapper.NewError(ExatoWebErrorCodes.InvalidUserData);

        result.Uid = Guid.Parse(user.UserUid);
        result.ExternalId = Guid.Parse(userExternalId);
        result.Name = user.Name;
        result.Email = user.EmailMain;
        result.Claims = user.ExtraClaims?.ToArray() ?? [];
        result.CreationDate = user.CreationDate;

        var lastActivity = await webCtx.ActivitiesLogs.Where(x => x.UserId == user.Id)
            .OrderByDescending(x => x.EventDate)
            .FirstOrDefaultAsync();
        result.LastAccessDate = lastActivity?.EventDate;

        var ids = userCompanies.Where(x => x.CompanyId != null).Select(x => x.CompanyId).ToList() ?? [];

        var companies = await webCtx.Companies.Where(x => ids.Contains(x.Id)).ToListAsync();

        foreach (var company in companies)
        {
            result.Companies.Add(new() { ExternalId = company.ExternalId.ToString() ?? "-", Name = company.Name });
        }

        return ExatoWebResponseWrapper.NewSuccess(result);
    }
}
