# DynamicServerNames

Плагин для EXILED, который по таймеру меняет имя сервера в браузере и поддерживает rich text SCP:SL.

## Что делает плагин

DynamicServerNames меняет `Server.Name` каждые `rotation_interval` секунд через coroutine EXILED. Каждый кадр поддерживает плейсхолдеры и rich text SCP:SL, так что можно сделать аккуратное имя сервера с цветами, жирным текстом и центровкой.

## Установка

1. Установите поддержку таргетинга .NET Framework 4.8.
2. Восстановите зависимости командой `dotnet restore`.
3. Соберите плагин командой `dotnet build DynamicServerNames/DynamicServerNames.csproj`.
4. Скопируйте DLL из `DynamicServerNames/bin/Debug/net48/DynamicServerNames.dll` в папку плагинов EXILED.
5. Запустите сервер один раз, чтобы EXILED создал конфиг.

Путь конфига по умолчанию:

`%AppData%\EXILED\Configs\Plugins\DynamicServerNames\<порт-сервера>.yml`

Если файл пустой, плагин пересоздаст его с рабочими значениями при загрузке.

## Настройка конфига

Все строковые значения с пробелами и rich text-тегами пишите в кавычках.

```yaml
is_enabled: true
debug: true

server_name: "My SCP:SL Server"
rotation_interval: 5
center_text: true

links:
  - "discord.gg/aapjvcvd9m:gray"
  - ""
  - ""
  - ""
  - ""

frames:
  - "<color=#FF4444><b>{server_name}</b></color> | <color=#00FF88>NoRules</color>\n<color=#00FF00>[TPS: {tickrate}]</color>  [Game: {game_time}]  [Players: {players}/{max_players}]  [Staff: {admins}]"
  - "<color=#FF4444><b>{server_name}</b></color> | <color=#00FF88>NoRules</color>\n<color=#AAAAAA>Админы: {admins}</color> | {links}"
```

### Значения полей

`server_name` - базовое имя сервера для `{server_name}`. Пример: `"My SCP:SL Server"`.

`rotation_interval` - интервал между кадрами в секундах. Значения меньше `1` автоматически станут `1`.

`center_text` - если включено, каждый кадр будет обёрнут в `<align="center">...</align>`.

`links` - список до 5 строк в формате `url:color`.
- Пустые строки игнорируются.
- Если цвет `none`, он считается как `gray`.
- Пример: `"discord.gg/aapjvcvd9m:gray"`.

`frames` - список сообщений для вращения. Каждый элемент должен быть строкой в кавычках. Можно использовать теги Unity rich text, например `<color=#RRGGBB>`, `<b>` и переносы строк через `\n`.

## Плейсхолдеры

`{server_name}` - значение из `server_name`.

`{tickrate}` - текущий tickrate сервера, округлённый до одной десятичной.

`{game_time}` - время раунда в формате `MM:SS`.

`{players}` - текущее количество игроков.

`{max_players}` - максимальное количество слотов сервера.

`{admins}` - количество игроков с доступом Remote Admin.

`{links}` - все непустые ссылки через ` | ` без хвостового разделителя.

`{link1}`..`{link5}` - отдельные элементы списка `links`.

## Сборка

```bash
dotnet restore
dotnet build DynamicServerNames/DynamicServerNames.csproj
```

Собранная DLL будет в `bin/Debug/net48/`.