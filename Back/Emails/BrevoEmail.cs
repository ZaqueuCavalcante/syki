namespace Estud.Back.Emails;

public class BrevoEmail
{
    public string Name { get; set; }
    public string Email { get; set; }
}

public class BrevoEmailMessage
{
    public BrevoEmail Sender { get; set; }
    public List<BrevoEmail> To { get; set; }
    public string Subject { get; set; }
    public string HtmlContent { get; set; }

    public BrevoEmailMessage(string sender, string to, string subject, string content)
    {
        Sender = new BrevoEmail { Name = "Estud", Email = sender };
        To = [new BrevoEmail { Email = to }];
        Subject = subject;
        HtmlContent = content;
    }
}
