# Hello Micro Services

Hello World.. Using Micro Services 😎

![Hello Microservices](docs/images/hellomicroservices.png)


This is meant to show an example of a micro-services application. It's written in F# using ASP.NET Core [Giraffe](https://github.com/giraffe-fsharp/Giraffe)


## Architecture
This application has a front-end and two micro-service backends namely

- App Service
- Location Service
- Person Service

### Person Service

This micro service returns a random person's name each time it's invoked

### Location Service

This micro service returns a random city and state each time it's invoked

### App Service

This is the front end that returns the greeting using data from both the Person service and the Location Service