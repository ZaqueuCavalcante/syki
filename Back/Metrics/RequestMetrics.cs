using System.Collections.Concurrent;

namespace Syki.Back.Metrics;

public class RequestMetrics
{
    public Guid Id { get; set; }
    public DateTime Start { get; set; }
    public DateTime Stop { get; set; }
    public ResumeData Resume { get; set; }
    public List<RequestData> Requests { get; set; }

    public RequestMetrics()
    {
        Id = Guid.CreateVersion7();
        Start = DateTime.UtcNow;
    }

    public void Save(List<RequestData> requests)
    {
        Stop = DateTime.UtcNow;
        Requests = requests;
        Resume = new();
        foreach (var request in requests)
        {
            Resume.Total += request.Values["Total"];
            Resume.Post += request.Values.GetValueOrDefault("POST");
            Resume.Put += request.Values.GetValueOrDefault("PUT");
            Resume.Get += request.Values.GetValueOrDefault("GET");
        }
    }
}

public class ResumeData
{
    public int Total { get; set; }
    public int Post { get; set; }
    public int Put { get; set; }
    public int Get { get; set; }
}

public class RequestData
{
    public string Endpoint { get; set; }
    public ConcurrentDictionary<string, int> Values { get; set; }
}
