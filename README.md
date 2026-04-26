# DynamicServerNames

DynamicServerNames is an EXILED plugin for SCP: Secret Laboratory that rotates the server browser name on a timer while preserving SCP:SL rich text formatting.

Repository: [github.com/furyashnyy/DynamicServerNames](https://github.com/furyashnyy/DynamicServerNames)

Discord: [discord.gg/aapjvcvd9m](https://discord.gg/aapjvcvd9m)

## What it does

The plugin updates `Server.Name` every `rotation_interval` seconds. Each frame supports live placeholders, colored text, bold text, and optional centered layout.

## Installation

1. Install .NET Framework 4.8 developer targeting support.
2. Restore dependencies with `dotnet restore`.
3. Build the plugin with `dotnet build ServerNameChanger/ServerNameChanger.csproj`.
4. Copy the resulting DLL from `ServerNameChanger/bin/Debug/net48/DynamicServerNames.dll` into your EXILED plugins folder.
5. Start the server once so EXILED generates the config file.

Default config location:

`%AppData%\EXILED\Configs\Plugins\DynamicServerNames\<server-port>.yml`

## Configuration guide

Use quoted strings for values that contain spaces, punctuation, or rich text tags.

```yaml
is_enabled: true
debug: false

server_name: "My SCP:SL Server"
rotation_interval: 5
center_text: true

discord_url: "discord.gg/aapjvcvd9m"
website_url: "github.com/furyashnyy/DynamicServerNames"
donate_url: "example.com/donate"

frames:
	- "<color=#FF4444><b>{server_name}</b></color> | <color=#00FF88>NoRules</color>\n<color=#00FF00>[TPS: {tickrate}]</color>  [Game: {game_time}]  [Players: {players}/{max_players}]  [Staff: {admins}]"
	- "<color=#FF4444><b>{server_name}</b></color> | <color=#00FF88>NoRules</color>\n<color=#00BFFF>[Discord: {discord}]</color>  <color=#00BFFF>[Website: {website}]</color>  <color=#FFD700>[Donate: {donate}]</color>"
```

### Field reference

`is_enabled` enables or disables the plugin.

`debug` enables debug logging. When true, each frame change is written to the log.

`server_name` is the base label used by `{server_name}`. Write it as a normal string, for example `"My SCP:SL Server"`.

`rotation_interval` controls the delay between frames in seconds. Values below `1` are clamped to `1`.

`center_text` wraps every resolved frame in `<align="center">...</align>` when enabled.

`discord_url`, `website_url`, and `donate_url` should be plain text strings. You can use a domain, invite, or full URL depending on how you want the browser text to look.

`frames` is the rotating message list. Each item should be a quoted string. You can use Unity rich text tags such as `<color=#RRGGBB>`, `<b>`, and line breaks with `\n`.

### Placeholders

`{server_name}` - Replaced with the configured `server_name` value.

`{tickrate}` - Current server tickrate rounded to one decimal place.

`{game_time}` - Round time in `MM:SS` format.

`{players}` - Current player count.

`{max_players}` - Maximum server player slots.

`{admins}` - Number of players with Remote Admin access.

`{discord}` - Replaced with `discord_url`.

`{website}` - Replaced with `website_url`.

`{donate}` - Replaced with `donate_url`.

## Build

```bash
dotnet restore
dotnet build ServerNameChanger/ServerNameChanger.csproj
```

The compiled DLL is placed under `bin/Debug/net48/` by default.