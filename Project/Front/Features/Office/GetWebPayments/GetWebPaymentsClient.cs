using Exato.Shared.Features.Office.GetWebPayments;

namespace Exato.Front.Features.Office.GetWebPayments;

public class GetWebPaymentsClient(HttpClient http) : IOfficeClient
{
    public async Task<GetWebPaymentsOut> Get(GetWebPaymentsIn data)
    {
        return await http.GetFromJsonAsync<GetWebPaymentsOut>("office/web-payments".AddQueryString(data), HttpConfigs.JsonOptions) ?? new();
    }
}
