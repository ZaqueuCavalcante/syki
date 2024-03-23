namespace Syki.Daemon.Emails;

public class BrevoEmailMessage
{
    public BrevoEmail Sender { get; set; }
    public List<BrevoEmail> To { get; set; }
    public string Subject { get; set; }
    public string HtmlContent { get; set; }

    public BrevoEmailMessage(string sender, string to, string subject, string content)
    {
        Sender = new BrevoEmail { Email = sender };
        To = [new BrevoEmail { Email = to }];
        Subject = subject;
        HtmlContent = content;
    }
}
