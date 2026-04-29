using Exiled.API.Interfaces;
using System.Collections.Generic;

namespace DynamicServerNames
{
    /// <summary>
    /// Plugin configuration for DynamicServerNames.
    /// </summary>
    public sealed class Config : IConfig
    {
        /// <summary>
        /// Gets or sets a value indicating whether the plugin is enabled.
        /// </summary>
        public bool IsEnabled { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether debug logging is enabled.
        /// </summary>
        public bool Debug { get; set; } = true;

        /// <summary>
        /// Gets or sets the base server name used by the {server_name} placeholder.
        /// </summary>
        public string ServerName { get; set; } = "My SCP:SL Server";

        /// <summary>
        /// Gets or sets a value indicating whether to auto-prepend {server_name} to frames
        /// that do not include the placeholder.
        /// </summary>
        public bool AutoPrependServerName { get; set; }

        /// <summary>
        /// Gets or sets the interval in seconds between server name rotations.
        /// </summary>
        public int RotationInterval { get; set; } = 5;

        /// <summary>
        /// Gets or sets a value indicating whether the resolved frame should be centered.
        /// </summary>
        public bool CenterText { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to use the rich-text align tag for centering.
        /// Disable this for server browser compatibility.
        /// </summary>
        public bool UseAlignTag { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether browser-safe formatting should be used.
        /// When enabled, rich-text tags are stripped and visual centering is done with spacing.
        /// </summary>
        public bool BrowserSafeFormatting { get; set; } = true;

        /// <summary>
        /// Gets or sets the target visible width used for browser-safe visual centering.
        /// </summary>
        public int CenterWidth { get; set; } = 64;

        /// <summary>
        /// Gets or sets custom links in URL-to-color format.
        /// Example item: "discord.gg/1234:gray".
        /// Empty string items are ignored.
        /// </summary>
        public List<string> Links { get; set; } = new List<string>
        {
            "discord.gg/aapjvcvd9m:gray",
            string.Empty,
            string.Empty,
            string.Empty,
            string.Empty,
        };

        /// <summary>
        /// Gets or sets the list of server name frames used for rotation.
        /// </summary>
        public System.Collections.Generic.List<string> Frames { get; set; } = new System.Collections.Generic.List<string>
        {
            "<color=#00BFFF><b>Thanks for using DynamicServerNames</b></color>\n<color=#00BFFF>https://github.com/furyashnyy</color>\n<color=#00BFFF>https://discord.gg/aapjvcvd9m</color>\n<color=#00BFFF>https://t.me/furyashnyy</color>",
            "<color=#FF4444><b>{server_name}</b></color> | <color=#00FF88>NoRules</color>\n<color=#00FF00>[TPS: {tickrate}]</color>  [Game: {game_time}]  [Players: {players}/{max_players}]  [Staff: {admins}]",
            "<color=#FF4444><b>{server_name}</b></color> | <color=#00FF88>NoRules</color>\n<color=#AAAAAA>{links}</color>",
            "<color=#FF4444><b>{server_name}</b></color>  <color=#AAAAAA>|</color>  <color=#00FF00>{players}</color><color=#AAAAAA>/</color><color=#FFFFFF>{max_players}</color> <color=#AAAAAA>players online</color>  <color=#FF9900>[Staff online: {admins}]</color>",
            "<color=#FF4444><b>{server_name}</b></color>  <color=#AAAAAA>Server TPS: </color><color=#00FF00>{tickrate}</color>  <color=#AAAAAA>|</color>  Round time: <color=#00BFFF>{game_time}</color>",
            "<b><color=#FF4444>{server_name}</color></b>  <color=#FFD700>★ Come play with us! ★</color>  <color=#AAAAAA>{players}/{max_players} players</color>  |  <color=#AAAAAA>Admins: {admins}</color>  |  {links}"
        };

        /// <summary>
        /// Gets or sets a value indicating whether to append an invisible hidden server name (size=0) to every frame.
        /// This can be useful for server browser matching while keeping the visible text unchanged.
        /// </summary>
        public bool AppendHiddenName { get; set; }

        /// <summary>
        /// Gets or sets the hidden name text appended when <see cref="AppendHiddenName" /> is enabled.
        /// When empty, the visible server name is used.
        /// </summary>
        public string HiddenName { get; set; } = string.Empty;
    }
}