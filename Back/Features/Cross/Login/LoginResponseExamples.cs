namespace Syki.Back.Features.Cross.Login;

public class LoginResponseExamples : IMultipleExamplesProvider<LoginOut>
{
    public IEnumerable<SwaggerExample<LoginOut>> GetExamples()
    {
        yield return SwaggerExample.Create(
			"JWT",
			new LoginOut
			{
				AccessToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJqdGkiOiJkYzEyZDgzZi0zNzNjLTQzYTItOTdiMC0yMTg3NTFjMThhZTIiLCJzdWIiOiIwMTkzNWViMi1lNjA3LTc5OGMtOTlhMy04ZDdjYTQ3NzE2ZGYiLCJyb2xlIjoiQWNhZGVtaWMiLCJuYW1lIjoiYWNhZGVtaWNvQHN5a2kuY29tIiwiZW1haWwiOiJhY2FkZW1pY29Ac3lraS5jb20iLCJpbnN0aXR1dGlvbiI6ImJjZDI1YTQ3LTE3YjItNGNlYS04ZDZlLWQ1NTA0NzRkNmFlYSIsIm5iZiI6MTczMjQ2MDYxMSwiZXhwIjoxNzMyODIwNjExLCJpYXQiOjE3MzI0NjA2MTEsImlzcyI6InN5a2ktYXBpLWRldmVsb3BtZW50IiwiYXVkIjoic3lraS1hcGktZGV2ZWxvcG1lbnQifQ.MT99F1dOIAUJRNzqoF6YcQJqXfAxpS5mV-8U8JP_abE"
			}
		);
    }
}
