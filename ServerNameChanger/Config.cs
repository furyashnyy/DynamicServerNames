using System.Collections.Generic;
using Exiled.API.Interfaces;

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
        public bool Debug { get; set; }

        /// <summary>
        /// Gets or sets the base server name used by the {server_name} placeholder.
        /// </summary>
        public string ServerName { get; set; } = "My SCP:SL Server";

        /// <summary>
        /// Gets or sets the interval in seconds between server name rotations.
        /// </summary>
        public int RotationInterval { get; set; } = 5;

        /// <summary>
        /// Gets or sets a value indicating whether the resolved frame should be centered.
        /// </summary>
        public bool CenterText { get; set; } = true;

        /// <summary>
        /// Gets or sets the Discord invite or website address used by the {discord} placeholder.
        /// </summary>
        public string DiscordUrl { get; set; } = "example.com";

        /// <summary>
        /// Gets or sets the website URL used by the {website} placeholder.
        /// </summary>
        public string WebsiteUrl { get; set; } = "example1.com";

        /// <summary>
        /// Gets or sets the donation URL used by the {donate} placeholder.
        /// </summary>
        public string DonateUrl { get; set; } = "example2.com";

        /// <summary>
        /// Gets or sets the list of server name frames used for rotation.
        /// </summary>
        public List<string> Frames { get; set; } = new List<string>
        {
            "<color=#FF4444><b>{server_name}</b></color> | <color=#00FF88>NoRules</color>\n<color=#00FF00>[TPS: {tickrate}]</color>  [Game: {game_time}]  [Players: {players}/{max_players}]  [Staff: {admins}]",
            "<color=#FF4444><b>{server_name}</b></color> | <color=#00FF88>NoRules</color>\n<color=#00BFFF>[Discord: {discord}]</color>  <color=#00BFFF>[Website: {website}]</color>  <color=#FFD700>[Donate: {donate}]</color>",
            "<color=#FF4444><b>{server_name}</b></color>  <color=#AAAAAA>|</color>  <color=#00FF00>{players}</color><color=#AAAAAA>/</color><color=#FFFFFF>{max_players}</color> <color=#AAAAAA>players online</color>  <color=#FF9900>[Staff online: {admins}]</color>",
            "<color=#FF4444><b>{server_name}</b></color>  <color=#AAAAAA>Server TPS: </color><color=#00FF00>{tickrate}</color>  <color=#AAAAAA>|</color>  Round time: <color=#00BFFF>{game_time}</color>",
            "<b><color=#FF4444>{server_name}</color></b>  <color=#FFD700>★ Come play with us! ★</color>  <color=#AAAAAA>{players}/{max_players} players</color>  |  <color=#00BFFF>{discord}</color>"
        };
    }
}