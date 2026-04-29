using System;
using System.IO;
using System.Text;
using Exiled.API.Features;

namespace DynamicServerNames
{
    internal static class ConfigBootstrap
    {
        private const string PluginName = "DynamicServerNames";

        [System.Runtime.CompilerServices.ModuleInitializer]
        internal static void Initialize()
        {
            try
            {
                string configPath = GetConfigPath();

                if (string.IsNullOrWhiteSpace(configPath))
                    return;

                string? directoryPath = Path.GetDirectoryName(configPath);

                if (!string.IsNullOrWhiteSpace(directoryPath))
                    Directory.CreateDirectory(directoryPath);

                if (File.Exists(configPath))
                {
                    string existing = File.ReadAllText(configPath, Encoding.UTF8);

                    if (!string.IsNullOrWhiteSpace(existing))
                        return;
                }

                File.WriteAllText(configPath, BuildDefaultConfig(), new UTF8Encoding(encoderShouldEmitUTF8Identifier: false));
            }
            catch
            {
                // Config repair must never block the plugin from loading.
            }
        }

        private static string GetConfigPath()
        {
            string configRoot = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "EXILED",
                "Configs",
                "Plugins",
                PluginName);

            int? serverPort = GetServerPort();
            string? fileName = serverPort.HasValue && serverPort.Value > 0 ? $"{serverPort.Value}.yml" : null;

            if (!string.IsNullOrWhiteSpace(fileName))
                return Path.Combine(configRoot, fileName);

            string[] configFiles = Directory.Exists(configRoot)
                ? Directory.GetFiles(configRoot, "*.yml", SearchOption.TopDirectoryOnly)
                : Array.Empty<string>();

            if (configFiles.Length == 1)
                return configFiles[0];

            return Path.Combine(configRoot, "7777.yml");
        }

        private static int? GetServerPort()
        {
            try
            {
                object? portValue = typeof(Server).GetProperty("Port")?.GetValue(null);

                if (portValue is int port)
                    return port;

                if (portValue is short shortPort)
                    return shortPort;

                if (portValue is byte bytePort)
                    return bytePort;
            }
            catch
            {
            }

            return null;
        }

        private static string BuildDefaultConfig()
        {
            return
                "is_enabled: true\n" +
                "debug: true\n" +
                "server_name: \"My SCP:SL Server\"\n" +
                "rotation_interval: 5\n" +
                "auto_prepend_server_name: false\n" +
                "center_text: false\n" +
                "use_align_tag: false\n" +
                "browser_safe_formatting: true\n" +
                "center_width: 64\n" +
                "append_hidden_name: false\n" +
                "hidden_name: \"\"\n" +
                "links:\n" +
                "  - \"discord.gg/aapjvcvd9m\"\n" +
                "  - \"\"\n" +
                "  - \"\"\n" +
                "  - \"\"\n" +
                "  - \"\"\n" +
                "frames:\n" +
                "  - |-\n" +
                "    <color=#00BFFF><b>Thanks for using DynamicServerNames</b></color>\n" +
                "    <color=#00BFFF>https://github.com/furyashnyy</color>\n" +
                "    <color=#00BFFF>https://discord.gg/aapjvcvd9m</color>\n" +
                "    <color=#00BFFF>https://t.me/furyashnyy</color>\n" +
                "  - |-\n" +
                "    <color=#FF4444><b>{server_name}</b></color> | <color=#00FF88>NoRules</color>\n" +
                "    <color=#00FF00>[TPS: {tickrate}]</color>  [Game: {game_time}]  [Players: {players}/{max_players}]  [Staff: {admins}]\n" +
                "  - |-\n" +
                "    <color=#FF4444><b>{server_name}</b></color> | <color=#00FF88>NoRules</color>\n" +
                "    {links}\n" +
                "  - |-\n" +
                "    <color=#FF4444><b>{server_name}</b></color>  <color=#AAAAAA>|</color>  <color=#00FF00>{players}</color><color=#AAAAAA>/</color><color=#FFFFFF>{max_players}</color> <color=#AAAAAA>players online</color>  <color=#FF9900>[Staff online: {admins}]</color>\n" +
                "  - |-\n" +
                "    <color=#FF4444><b>{server_name}</b></color>  <color=#AAAAAA>Server TPS: </color><color=#00FF00>{tickrate}</color>  <color=#AAAAAA>|</color>  Round time: <color=#00BFFF>{game_time}</color>\n" +
                "  - |-\n" +
                "    <b><color=#FF4444>{server_name}</color></b>  <color=#FFD700>★ Come play with us! ★</color>  <color=#AAAAAA>{players}/{max_players} players</color>  |  <color=#AAAAAA>Admins: {admins}</color>  |  {links}\n";
        }
    }
}