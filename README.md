# Playwright .NET 7.0 Project

## Overview

This project leverages Microsoft Playwright, a powerful tool for automated end-to-end testing of web applications, using the .NET 7.0 framework. It is designed to be used in Visual Studio Code with the Playwright Test extension to enhance development efficiency and testing accuracy.

## Prerequisites

- [.NET 7.0 SDK](https://dotnet.microsoft.com/download/dotnet/7.0): Ensure you have the latest version of the .NET 7.0 SDK installed.
- [Visual Studio Code](https://code.visualstudio.com/): A lightweight but powerful source code editor that runs on your desktop.
- [Playwright Test for Visual Studio Code Extension](https://marketplace.visualstudio.com/items?itemName=ms-playwright.playwright): This extension simplifies writing, running, and debugging Playwright tests directly within VS Code.

## Getting Started

open projct in visual studio code and run the following command in terminal

```bash
dotnet restore
```

```bash
dotnet build
```

## Running Tests

```bash
dotnet test
```

You can also install the Playwright Test for Visual Studio Code extension and run the tests directly from the test explorer.

UI tests have been written so they dont use headless mode, you can change this in the test settings.
