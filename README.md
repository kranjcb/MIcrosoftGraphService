# Microsoft Graph Service

A .NET 9.0 service for Microsoft Graph API operations with named pipe communication.

## Description

This project provides a service that handles Microsoft Graph API operations through a named pipe server architecture. It includes async pipe communication capabilities and example usage patterns.

## Features

- Named pipe server/client communication
- Microsoft Graph API integration
- JSON message serialization
- Logging with NLog
- Async operations support

## Project Structure

```
├── Program.cs              # Main entry point
├── Server.cs               # Main server implementation
├── ServerFunctions.cs      # Server function implementations
├── Model/
│   ├── RequestMessage.cs   # Request message model
│   └── ResponseMessage.cs  # Response message model
├── Shared/
│   ├── PipeClientAsync.cs  # Async pipe client
│   └── PipeServerAsync.cs  # Async pipe server
└── ExampleUsage/
    └── MicosoftGraphUsages.cs # Usage examples
```

## Requirements

- .NET 9.0
- Newtonsoft.Json 13.0.3
- NLog 6.0.3

## Building

```bash
dotnet build
```

## Running

```bash
dotnet run
```

## Usage

The service starts a named pipe server that can handle Microsoft Graph API requests. See the `ExampleUsage` folder for implementation examples.

## Configuration

Configure logging and other settings as needed for your environment.

## License

[Specify your license here]
