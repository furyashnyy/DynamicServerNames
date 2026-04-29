# DynamicServerNames
[🇺🇸](README.md) | [🇷🇺](README-RU.md)

![](https://komarev.com/ghpvc/?username=your-github-username)

DynamicServerNames is an EXILED plugin for SCP: Secret Laboratory that rotates the server browser name on a timer and supports placeholders for live data.

Repository: [github.com/furyashnyy/DynamicServerNames](https://github.com/furyashnyy/DynamicServerNames)

Discord: [discord.gg/aapjvcvd9m](https://discord.gg/aapjvcvd9m)

## Features

- Rotates `Server.Name` on a configurable interval.
- Placeholders for tickrate, round time, players, admins, and links.
- Optional centering with align tags or manual padding.
- Browser-safe mode to strip rich text for cleaner server list output.
- Config auto-repair when the file is empty.

## Installation

1. Install .NET Framework 4.8 developer targeting support.
2. Restore dependencies with `dotnet restore`.
3. Build the plugin with `dotnet build DynamicServerNames/DynamicServerNames.csproj`.
4. Copy `DynamicServerNames/bin/Debug/net48/DynamicServerNames.dll` into your EXILED plugins folder.
5. Start the server once so EXILED generates the config file.

Default config location:

`%AppData%\EXILED\Configs\Plugins\DynamicServerNames\<server-port>.yml`

If that file is empty, the plugin recreates it with valid defaults on load.

## Quick config example

Use quotes for values that contain spaces or tags.

```yaml
is_enabled: true
debug: true

server_name: "My SCP:SL Server"
rotation_interval: 5
auto_prepend_server_name: false

center_text: false
use_align_tag: false
browser_safe_formatting: true
center_width: 64

append_hidden_name: false
hidden_name: ""

links:
  - "discord.gg/aapjvcvd9m"
  - "eklmn.arnos.dev"
  - ""
  - ""
  - ""

frames:
  - "{server_name} | TPS: {tickrate} | {game_time}"
  - "{server_name} | {players}/{max_players} | {links}"
```

## Configuration reference

`is_enabled` enables or disables the plugin.

`debug` enables debug logging. When true, each frame change is written to the log.

`server_name` is the base label used by `{server_name}`.

`rotation_interval` controls the delay between frames in seconds. Values below `1` are clamped to `1`.

`auto_prepend_server_name` adds `{server_name}` to frames that do not include it.

`center_text` centers each frame. See `use_align_tag` and `center_width` for behavior.

`use_align_tag` wraps each frame with `<align="center">...</align>` when enabled and `browser_safe_formatting` is false.

`browser_safe_formatting` removes rich text tags and disables hidden-name append. Recommended for the server list.

`center_width` is the target visible width for manual centering when browser-safe formatting is enabled.

`append_hidden_name` appends an invisible name with `<size=0>...</size>` when browser-safe formatting is false.

`hidden_name` is the text used for hidden name append. Empty means `server_name`.

`links` is a list of up to 5 items. Provide plain URLs.

`frames` is the rotating message list. Use quoted strings and `\n` for new lines. Rich text tags are allowed only when `browser_safe_formatting` is false.

## Placeholders

`{server_name}` - Configured server name.

`{tickrate}` - Current server tickrate, one decimal place.

`{game_time}` - Round time in `MM:SS`.

`{players}` - Current player count.

`{max_players}` - Maximum player slots.

`{admins}` - Players with Remote Admin access.

`{links}` - Non-empty link entries joined by ` | `.

`{link1}`..`{link5}` - Individual link entries.

## Build

```bash
dotnet restore
dotnet build DynamicServerNames/DynamicServerNames.csproj
```

Outputs:
- `DynamicServerNames/bin/Debug/net48/DynamicServerNames.dll`
- `DynamicServerNames/bin/Release/net48/DynamicServerNames.dll`
