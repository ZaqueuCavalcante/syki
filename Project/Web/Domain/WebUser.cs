using Exato.Shared.Enums;
using Exato.Shared.Extensions;

namespace Exato.Web.Domain;

public class WebUser
{
    public int Id { get; set; }

    public string UserUid { get; set; }

    public bool Active { get; set; }

    public int? AddressId { get; set; }

    public string? Password { get; set; }

    public string? PasswordSeed { get; set; }

    public string? PasswordAlgorithm { get; set; }

    /// <summary>
    /// Ver <see cref="ExatoWebOnboardStatus"/>
    /// </summary>
    public int OnboardStatus { get; set; }

    public DateTime CreationDate { get; set; }

    public DateTime? LastPasswordUpdateDate { get; set; }

    public int PaymentMode { get; set; }

    public int WrongPasswordAttempts { get; set; }

    public bool SoftDelete { get; set; }

    public string? LastAccessIp { get; set; }

    public string? Cpf { get; set; }

    public string? PictureFilePath { get; set; }

    public string? Name { get; set; }

    public int RetryAttemptsQuiz { get; set; }

    public string? MotherName { get; set; }

    public DateTime? Birthday { get; set; }

    public int? PromotionalCodeId { get; set; }

    public int? IndicatedByUserCompanyId { get; set; }

    public bool? BonusForIndicatorGiven { get; set; }

    public string? EmailMain { get; set; }

    public string? Comments { get; set; }

    public List<string>? ExtraClaims { get; set; }

    public DateTime? TwoFactorRequiredAt { get; set; }

    public bool? OnboardCompletedWithoutValidation { get; set; }

    public bool? BonusForAddingPhone { get; set; }

    public bool? CameFromChatbot { get; set; }

    public Guid? LastImpersonatedCompanyUid { get; set; }

    public Guid? PartnerKeyId { get; set; }

    public bool? CameFromRegisterPostPaid { get; set; }

    public WebUser() { }

    public WebUser(string name, string email, string? cpf, List<ExatoWebClaims> claims)
    {
        Name = name;
        EmailMain = email;
        Cpf = cpf.OnlyNumbers().HasValue() ? cpf.OnlyNumbers() : null;
        UserUid = Guid.NewGuid().ToString();
        PaymentMode = 0;
        WrongPasswordAttempts = 0;
        CreationDate = DateTime.Now;
        SoftDelete = false;
        RetryAttemptsQuiz = 0;
        ExtraClaims = claims.ToPermissions();
        Active = false;

        OnboardStatus = cpf.OnlyNumbers().HasValue() ?
            ExatoWebOnboardStatus.Completed.ToInt() : ExatoWebOnboardStatus.Waiting.ToInt();

        // TODO: Password fields
        Password = "$2a$12$G63gTJRJvRKTeT9pZAxIduRWn1T2MVwtRrW2wM/PWtwq9LGiTcvhO";
        PasswordSeed = "$2a$12$G63gTJRJvRKTeT9pZAxIdu";
        PasswordAlgorithm = null;
    }

    public void EditarCadastro(string name, string email, string? cpf)
    {
        Name = name;
        EmailMain = email;
        Cpf = cpf.OnlyNumbers().HasValue() ? cpf.OnlyNumbers() : null;

        OnboardStatus = cpf.OnlyNumbers().HasValue() ?
            ExatoWebOnboardStatus.Completed.ToInt() : ExatoWebOnboardStatus.Waiting.ToInt();
    }

    public ExatoWebOnboardStatus GetOnboardStatus()
    {
        return OnboardStatus.IntToEnum<ExatoWebOnboardStatus>();
    }

    public void DoSoftDelete()
    {
        Active = false;
        SoftDelete = true;
    }
}
