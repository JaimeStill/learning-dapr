﻿using Dapr.Client;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MyFrontEnd.Pages;

public class IndexModel : PageModel
{
    private readonly DaprClient _daprClient;

    public IndexModel(DaprClient daprClient)
    {
        _daprClient = daprClient;
    }

    public async Task OnGet()
    {
        IEnumerable<WeatherForecast>? forecasts = await _daprClient
            .InvokeMethodAsync<IEnumerable<WeatherForecast>>(
                HttpMethod.Get,
                "MyBackEnd",
                "weatherforecast"
            );

        ViewData["WeatherForecastData"] = forecasts;        
    }
}
