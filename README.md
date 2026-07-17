# Train Journey Tracker

A real-time IoT train journey tracking application using nanoFramework and an e-ink display to show the current station and journey status.

## Overview

Train Journey Tracker is an embedded device application that connects to a backend API to fetch real-time train journey information and displays it on a low-power e-ink display. Perfect for commuters who want a dedicated device showing their current train stop without draining battery power.

## Features

- **Real-time Journey Updates**: Polls a backend API for current station and journey status
- **E-ink Display**: Uses a Ssd1680 e-ink display for low-power consumption and readability
- **WiFi Connectivity**: Connects via WiFi to fetch journey data
- **Smart Refresh**: Only updates the display when the station changes, minimizing power consumption
- **nanoFramework**: Lightweight .NET runtime optimized for IoT and embedded devices

## Hardware Requirements

- **Microcontroller**: Device compatible with nanoFramework
- **Display**: Ssd1680 E-ink display (122x250 pixels)
- **Communication**: SPI interface for display communication
- **GPIO Pins**: 
  - Pin 2: Data/Command
  - Pin 4: Reset
  - Pin 5: Busy
  - Pin 15: SPI Chip Select
- **WiFi Module**: Integrated or external WiFi connectivity

## Prerequisites

- [nanoFramework](https://www.nanoframework.net/) development environment
- Visual Studio 2022+ with nanoFramework extension
- Backend API endpoint running at `https://localhost:7227/api/journey/status`
- WiFi network credentials

## Installation

1. Clone the repository:
   ```bash
   git clone https://github.com/S-iba/TrainJourneySln.git
   cd TrainJourneySln
   ```

2. Open the solution in Visual Studio:
   ```bash
   start TrainJourneySln.slnx
   ```

3. Update WiFi credentials in `Program.cs`:
   ```csharp
   WifiNetworkHelper.ConnectDhcp("YOUR_SSID", "YOUR_PASSWORD");
   ```

4. Update API endpoint if necessary:
   ```csharp
   private const string ApiBaseUrl = "https://your-api-server/api/journey/status";
   ```

5. Deploy to your nanoFramework device

## API Response Format

The backend API should return JSON in the following format:

```json
{
  "stageId": 1,
  "stageName": "Central Station",
  "sequencePosition": 3
}
```

## Usage

Once deployed and running:

1. Device connects to WiFi network
2. Periodically requests journey status from the backend API
3. When station changes, updates the e-ink display with the new station name
4. Display enters low-power mode after refresh to preserve battery life

## Project Structure

```
TrainJourneySln/
├── TrainJourneyNode/
│   ├── Program.cs          # Main application entry point
│   └── [device files]
├── TrainJourneySln.slnx    # Solution file
└── README.md               # This file
```

## Technologies Used

- **Framework**: nanoFramework (.NET for IoT devices)
- **Language**: C# 9.0
- **Target**: .NET 10
- **Communication**: HTTP/HTTPS, SPI, GPIO
- **Display Driver**: Ssd1680 e-ink display
- **JSON**: nanoFramework.Json for serialization

## Future Enhancements

- [ ] Display graphics/map visualization
- [ ] Multiple station indicators
- [ ] Estimated arrival times
- [ ] Battery status monitoring
- [ ] Error recovery and retry logic
- [ ] Local caching of journey data

## Troubleshooting

- **WiFi Connection Issues**: Verify SSID and password in `Program.cs`
- **API Errors**: Check backend API is running and accessible
- **Display Not Updating**: Verify SPI pins and GPIO configuration match hardware setup
- **Network Timeouts**: Increase polling interval in the main loop

## License


