namespace Syki.Back.Domain.Enums;

public enum SsoProviderType
{
    [Description("Azure AD")]
    AzureAd = 0,

    [Description("Google Workspace")]
    GoogleWorkspace = 1,

    [Description("Okta")]
    Okta = 2,

    [Description("Auth0")]
    Auth0 = 3,

    [Description("Custom OIDC")]
    CustomOidc = 4,
}
