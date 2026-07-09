namespace Syki.Back.Domain.Enums;

/// <summary>
/// See <see cref="FeatureGroup"/> for grouping logic
/// </summary>
public enum UserActivityType
{
    #region Identity

    EmailPasswordLogin_Success = 0,
    EmailPasswordLogin_UserNotFound = 1,
    EmailPasswordLogin_WrongPassword = 2,
    EmailPasswordLogin_SsoRequired = 3,
    EmailPasswordLogin_LockedOut = 4,
    EmailPasswordLogin_RequiresTwoFactor = 5,

    SsoOidcLogin_Success = 100,
    SsoOidcLogin_UserNotFound = 101,
    SsoOidcLogin_EmptyEmail = 102,
    SsoOidcLogin_DomainNotConfigured = 103,
    SsoOidcLogin_RemoteFailure = 104,

    TwoFactorLogin_Success = 200,
    TwoFactorLogin_LockedOut = 201,
    TwoFactorLogin_InvalidToken = 202,

    SetupTwoFactor_Success = 300,
    SetupTwoFactor_InvalidToken = 301,

    ResetPassword_Success = 400,
    ResetPassword_TokenNotFound = 401,
    ResetPassword_TokenIsUsed = 402,
    ResetPassword_TokenIsExpired = 403,
    ResetPassword_InvalidToken = 404,
    ResetPassword_WeakPassword = 405,

    Logout_Success = 500,

    CreateRole_Success = 600,
    CreateRole_InvalidRolePermissions = 601,

    UpdateRole_Success = 700,
    UpdateRole_InvalidRolePermissions = 701,

    CreateSsoConfiguration_Success = 800,

    UpdateSsoConfiguration_Success = 900,

    SendResetPasswordToken_Success = 1000,
    SendResetPasswordToken_UserNotFound = 1001,

    MagicLinkLogin_Success = 1100,
    MagicLinkLogin_TokenNotFound = 1101,
    MagicLinkLogin_TokenIsUsed = 1102,
    MagicLinkLogin_TokenIsExpired = 1103,

    #endregion

    #region Users

    RegisterUser_Success = 10_000,
    RegisterUser_EmailAlreadyUsed = 10_001,
    RegisterUser_EstudUserProvisioned = 10_002,

    #endregion
}
