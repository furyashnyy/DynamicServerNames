# DynamicServerNames
[🇺🇸](README.md) | [🇷🇺](README-RU.md)

Плагин для EXILED, который по таймеру меняет имя сервера в браузере и поддерживает плейсхолдеры для живых данных.

Репозиторий: [github.com/furyashnyy/DynamicServerNames](https://github.com/furyashnyy/DynamicServerNames)

Discord: [discord.gg/aapjvcvd9m](https://discord.gg/aapjvcvd9m)

## Возможности

- Ротация `Server.Name` по таймеру.
- Плейсхолдеры для тикрейта, времени раунда, игроков, админов и ссылок.
- Центровка через align или визуальная центровка пробелами.
- Режим browser-safe для чистого отображения в списке серверов.
- Автовосстановление конфига, если файл пустой.

## Установка

1. Установите поддержку таргетинга .NET Framework 4.8.
2. Восстановите зависимости командой `dotnet restore`.
3. Соберите плагин командой `dotnet build DynamicServerNames/DynamicServerNames.csproj`.
4. Скопируйте `DynamicServerNames/bin/Debug/net48/DynamicServerNames.dll` в папку плагинов EXILED.
5. Запустите сервер один раз, чтобы EXILED создал конфиг.

Путь конфига по умолчанию:

`%AppData%\EXILED\Configs\Plugins\DynamicServerNames\<порт-сервера>.yml`

Если файл пустой, плагин пересоздаст его с рабочими значениями при загрузке.

## Быстрый пример конфига

Строки с пробелами и тегами пишите в кавычках.

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

## Значения полей

`is_enabled` включает или выключает плагин.

`debug` включает логирование кадров.

`server_name` базовое имя для `{server_name}`.

`rotation_interval` интервал между кадрами в секундах. Значения меньше `1` становятся `1`.

`auto_prepend_server_name` добавляет `{server_name}` в кадры, где его нет.

`center_text` включает центровку. Поведение зависит от `use_align_tag` и `center_width`.

`use_align_tag` добавляет `<align="center">...</align>`, если `browser_safe_formatting` выключен.

`browser_safe_formatting` удаляет rich text-теги и отключает скрытое имя. Рекомендуется для списка серверов.

`center_width` целевая видимая ширина для визуальной центровки в browser-safe режиме.

`append_hidden_name` добавляет невидимое имя через `<size=0>...</size>`, если browser-safe режим выключен.

`hidden_name` текст для скрытого имени. Пустое значение использует `server_name`.

`links` список до 5 элементов. Поддерживаемые форматы:
- `url` (без цвета)
- `url:color` (цвет применяется, если browser-safe выключен)
- `url:none` или `url:null` (без цвета)

`frames` список сообщений. Используйте строки в кавычках и `\n` для переноса строк. Rich text доступен только при `browser_safe_formatting: false`.

## Плейсхолдеры

`{server_name}` - значение из `server_name`.

`{tickrate}` - текущий tickrate, одна десятичная.

`{game_time}` - время раунда `MM:SS`.

`{players}` - текущее количество игроков.

`{max_players}` - максимум слотов.

`{admins}` - количество игроков с Remote Admin доступом.

`{links}` - все непустые ссылки через ` | `.

`{link1}`..`{link5}` - отдельные элементы `links`.

## Сборка

```bash
dotnet restore
dotnet build DynamicServerNames/DynamicServerNames.csproj
```

Выходные DLL:
- `DynamicServerNames/bin/Debug/net48/DynamicServerNames.dll`
- `DynamicServerNames/bin/Release/net48/DynamicServerNames.dll`