﻿namespace Syki.Back.Features.Cross.SendResetPasswordToken;

public class SendResetPasswordTokenErrorsExamples : IMultipleExamplesProvider<ErrorOut>
{
    public IEnumerable<SwaggerExample<ErrorOut>> GetExamples()
    {
        yield return new UserNotFound().ToSwaggerExampleErrorOut();
    }
}
