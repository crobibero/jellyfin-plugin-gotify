<h1 align="center">Jellyfin Gotify Plugin</h1>
<h3 align="center">Part of the <a href="https://jellyfin.org/">Jellyfin Project</a></h3>

## About

The Jellyfin Gotify plugin can be used for sending notifications to a hosted <a href="https://gotify.net/">Gotify server.</a>

## Build & Installation Process

1. Clone this repository
2. Ensure you have .NET Core SDK setup and installed
3. Build the plugin with following command:

```
dotnet publish --configuration Release --output bin
```

4. Place the resulting `Jellyfin.Plugin.Gotify.dll` file in a folder called `plugins/` inside your Jellyfin installation / data directory.

### Screenshot

<img src=screenshot.png>
