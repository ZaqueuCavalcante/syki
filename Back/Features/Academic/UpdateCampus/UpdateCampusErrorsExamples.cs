﻿namespace Syki.Back.Features.Academic.UpdateCampus;

public class UpdateCampusErrorsExamples : IMultipleExamplesProvider<ErrorOut>
{
    public IEnumerable<SwaggerExample<ErrorOut>> GetExamples()
    {
        yield return new CampusNotFound().ToSwaggerExampleErrorOut();
    }
}
