# dotnet-identity-user
ASP.NET Core Razor Pages web apps that use Entity Framework (EF) Core for data access.
+ Razor Pages
+ Unit Testing
+ Data Layer Testing
+ Integration Testing
+ SignalR on local or Azure

### Requirements
+ Docker Compose
+ ASP NET Core 2.2 & Entity Framework Identity User
+ Sql Server
+ SignR(Chat Message)

### Usage
+ Start Sql Server and Web App
    ```
    cd devops
    docker-compose up
    ```

+ Using Azure SignalR Service
    - Set UseAzureSignalR=true in appsetting.json
    - Set Azure ConnectionString (Endpoint=https://[instant-name].service.signalr.net;AccessKey=[your-key];Version=1.0;)
    ```
    cd src/IndetityUsers
    dotnet user-secrets set Azure:SignalR:ConnectionString "<Your connection string>"
    ```

    - Run app
    ```
    cd src/IndetityUsers
    dotnet build
    dotnet run
    ```

+ Register User & Login (IdentityUser)

+ Send Message
    - ![Send Message](./imgs/send-msg.jpg)


+ Receive Message
    - ![Receive Message](./imgs/room-receive-msg.jpg)

### Reference
+ [Using Azure SignalR Service](https://docs.microsoft.com/en-us/azure/azure-signalr/signalr-quickstart-dotnet-core)
