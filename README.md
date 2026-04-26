# DynamicServerNames

A lightweight EXILED plugin for SCP: Secret Laboratory that rotates the server browser name on a timer and keeps the text fully rich-text aware.

## English

### What it does

DynamicServerNames changes `Server.Name` every `rotation_interval` seconds using an EXILED coroutine flow. Each frame supports live placeholders and SCP:SL rich text tags, so you can build a clean server browser message with colors, bold text, and centered layout.

### Installation

1. Install .NET Framework 4.8 developer targeting support.
2. Restore dependencies with `dotnet restore`.
3. Build the plugin with `dotnet build ServerNameChanger/ServerNameChanger.csproj`.
4. Copy the resulting DLL from `ServerNameChanger/bin/Debug/net48/DynamicServerNames.dll` into your EXILED plugins folder.
5. Start the server once so EXILED generates the config file.

Default plugin config location:

`%AppData%\EXILED\Configs\Plugins\DynamicServerNames\<server-port>.yml`

### Configuration guide

Use quoted strings for all text values that contain spaces or rich text tags.

```yaml
is_enabled: true
debug: false

server_name: "My SCP:SL Server"
rotation_interval: 5
center_text: true

discord_url: "discord.gg/example"
website_url: "example.com"
donate_url: "example.com/donate"

frames:
  - "<color=#FF4444><b>{server_name}</b></color> | <color=#00FF88>NoRules</color>\n<color=#00FF00>[TPS: {tickrate}]</color>  [Game: {game_time}]  [Players: {players}/{max_players}]  [Staff: {admins}]"
  - "<color=#FF4444><b>{server_name}</b></color> | <color=#00FF88>NoRules</color>\n<color=#00BFFF>[Discord: {discord}]</color>  <color=#00BFFF>[Website: {website}]</color>  <color=#FFD700>[Donate: {donate}]</color>"
```

Field notes:

`server_name` is the base label used by `{server_name}`. Write it as a normal string, for example `"My SCP:SL Server"`.

`rotation_interval` is the delay between frames in seconds. Values below `1` are clamped to `1`.

`center_text` wraps every resolved frame in `<align="center">...</align>` when enabled.

`discord_url`, `website_url`, and `donate_url` should be plain text strings. You can use a domain, invite, or full URL depending on how you want the browser text to look.

`frames` is the rotating message list. Each item should be a quoted string. You can use Unity rich text tags such as `<color=#RRGGBB>`, `<b>`, and line breaks with `\n`.

### Placeholder reference

`{server_name}` - Replaced with the configured `server_name` value.

`{tickrate}` - Current server tickrate rounded to one decimal place.

`{game_time}` - Round time in `MM:SS` format.

`{players}` - Current player count.

`{max_players}` - Maximum server player slots.

`{admins}` - Number of players with Remote Admin access.

`{discord}` - Replaced with `discord_url`.

`{website}` - Replaced with `website_url`.

`{donate}` - Replaced with `donate_url`.

### Build

```bash
dotnet restore
dotnet build ServerNameChanger/ServerNameChanger.csproj
```

The compiled DLL is placed under `bin/Debug/net48/` by default.

## Русский

### Что делает плагин

DynamicServerNames меняет `Server.Name` каждые `rotation_interval` секунд через coroutine EXILED. Каждый кадр поддерживает живые плейсхолдеры и rich text SCP:SL, поэтому можно собрать красивое сообщение для браузера сервера с цветами, жирным шрифтом и центрированием.

### Установка

1. Установите поддержку таргетинга .NET Framework 4.8.
2. Восстановите зависимости командой `dotnet restore`.
3. Соберите плагин командой `dotnet build ServerNameChanger/ServerNameChanger.csproj`.
4. Скопируйте DLL из `ServerNameChanger/bin/Debug/net48/DynamicServerNames.dll` в папку плагинов EXILED.
5. Запустите сервер один раз, чтобы EXILED создал конфиг.

Путь конфигурации по умолчанию:

`%AppData%\EXILED\Configs\Plugins\DynamicServerNames\<порт-сервера>.yml`

### Настройка конфига

Все текстовые значения, в которых есть пробелы или rich text-теги, лучше писать в кавычках.

```yaml
is_enabled: true
debug: false

server_name: "My SCP:SL Server"
rotation_interval: 5
center_text: true

discord_url: "discord.gg/example"
website_url: "example.com"
donate_url: "example.com/donate"

frames:
  - "<color=#FF4444><b>{server_name}</b></color> | <color=#00FF88>NoRules</color>\n<color=#00FF00>[TPS: {tickrate}]</color>  [Game: {game_time}]  [Players: {players}/{max_players}]  [Staff: {admins}]"
  - "<color=#FF4444><b>{server_name}</b></color> | <color=#00FF88>NoRules</color>\n<color=#00BFFF>[Discord: {discord}]</color>  <color=#00BFFF>[Website: {website}]</color>  <color=#FFD700>[Donate: {donate}]</color>"
```

Описание полей:

`server_name` - базовое имя сервера, которое подставляется в `{server_name}`. Пишите обычной строкой, например `"My SCP:SL Server"`.

`rotation_interval` - интервал между кадрами в секундах. Значения меньше `1` автоматически станут `1`.

`center_text` - если включено, каждый кадр будет обёрнут в `<align="center">...</align>`.

`discord_url`, `website_url`, `donate_url` - обычные строки. Можно указывать домен, invite или полный URL, как вам удобнее.

`frames` - список вращающихся сообщений. Каждый элемент должен быть строкой в кавычках. Можно использовать теги Unity rich text, например `<color=#RRGGBB>`, `<b>` и переносы строк через `\n`.

### Список плейсхолдеров

`{server_name}` - подставляет значение из `server_name`.

`{tickrate}` - текущий tickrate сервера, округлённый до одной десятичной.

`{game_time}` - время раунда в формате `MM:SS`.

`{players}` - текущее число игроков.

`{max_players}` - максимальное количество слотов сервера.

`{admins}` - количество игроков с доступом Remote Admin.

`{discord}` - значение `discord_url`.

`{website}` - значение `website_url`.

`{donate}` - значение `donate_url`.

### Сборка

```bash
dotnet restore
dotnet build ServerNameChanger/ServerNameChanger.csproj
```

По умолчанию DLL собирается в `bin/Debug/net48/`.