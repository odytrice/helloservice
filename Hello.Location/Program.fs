module Hello.Location.App

open System
open System.IO
open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Cors.Infrastructure
open Microsoft.AspNetCore.Hosting
open Microsoft.Extensions.Logging
open Microsoft.Extensions.DependencyInjection
open Giraffe
open Microsoft.AspNetCore.Server.Kestrel.Core
open Microsoft.Extensions.Configuration



// ---------------------------------
// Error handler
// ---------------------------------

let errorHandler (ex : Exception) (logger : ILogger) =
    logger.LogError(ex, "An unhandled exception has occurred while executing the request.")
    clearResponse >=> setStatusCode 500 >=> text ex.Message

// ---------------------------------
// Config and Main
// ---------------------------------

let configureKestrel (context: WebHostBuilderContext) (options: KestrelServerOptions) =
    let config = context.Configuration.GetSection("Kestrel")

    let port = config.GetValue<int> "Port"
    if port > 0 then
        options.ListenAnyIP(port)

let buildConfig (config : IConfigurationBuilder) =
    config.SetBasePath(System.IO.Directory.GetCurrentDirectory())
          .AddYamlFile("appsettings.yml", optional = false, reloadOnChange = true)
          .AddYamlFile("appsettings.user.yml", optional = true, reloadOnChange = true)
          .AddEnvironmentVariables()
    |> ignore

let configureCors (builder : CorsPolicyBuilder) =
    builder.WithOrigins("http://localhost:8080")
           .AllowAnyMethod()
           .AllowAnyHeader()
           |> ignore

let configureApp (app : IApplicationBuilder) =
    let env = app.ApplicationServices.GetService<IHostingEnvironment>()
    (match env.IsDevelopment() with
    | true  -> app.UseDeveloperExceptionPage()
    | false -> app.UseGiraffeErrorHandler errorHandler)
        .UseCors(configureCors)
        .UseStaticFiles()
        .UseGiraffe(Handlers.webApp)

let configureServices (services : IServiceCollection) =
    services.AddCors()    |> ignore
    services.AddGiraffe() |> ignore

let configureLogging (builder : ILoggingBuilder) =
    builder.AddConsole().AddDebug() |> ignore

[<EntryPoint>]
let main _ =
    let contentRoot = Directory.GetCurrentDirectory()
    let webRoot     = Path.Combine(contentRoot, "WebRoot")
    WebHostBuilder()
        .UseKestrel(Action<_,_> configureKestrel)
        .UseContentRoot(contentRoot)
        .UseIISIntegration()
        .UseWebRoot(webRoot)
        .ConfigureAppConfiguration(buildConfig)
        .Configure(Action<IApplicationBuilder> configureApp)
        .ConfigureServices(configureServices)
        .ConfigureLogging(configureLogging)
        .Build()
        .Run()
    0