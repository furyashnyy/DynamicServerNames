using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Exiled.API.Features;
using MEC;
using GameServer = Exiled.API.Features.Server;

namespace DynamicServerNames
{
    /// <summary>
    /// Rotates the visible server name on a coroutine timer.
    /// </summary>
    public sealed class NameRotator
    {
        private readonly Config _config;
        private CoroutineHandle _handle;
        private bool _isRunning;
        private int _frameIndex = -1;

        /// <summary>
        /// Initializes a new instance of the <see cref="NameRotator" /> class.
        /// </summary>
        /// <param name="config">Plugin configuration.</param>
        public NameRotator(Config config)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
        }

        /// <summary>
        /// Starts the rotation coroutine if there are frames configured.
        /// </summary>
        public void Start()
        {
            Stop();

            if (_config.Frames == null || _config.Frames.Count == 0)
                return;

            _frameIndex = -1;
            _handle = Timing.RunCoroutine(Rotate());
            _isRunning = true;
        }

        /// <summary>
        /// Stops the rotation coroutine if it is currently running.
        /// </summary>
        public void Stop()
        {
            if (!_isRunning)
                return;

            Timing.KillCoroutines(_handle);
            _isRunning = false;
        }

        private IEnumerator<float> Rotate()
        {
            int interval = Math.Max(1, _config.RotationInterval);

            while (true)
            {
                try
                {
                    ApplyNextFrame();
                }
                catch (Exception exception)
                {
                    Log.Error($"[DynamicServerNames] Failed to update server name: {exception}");
                }

                yield return Timing.WaitForSeconds(interval);
            }
        }

        private void ApplyNextFrame()
        {
            if (_config.Frames == null || _config.Frames.Count == 0)
                return;

            _frameIndex = (_frameIndex + 1) % _config.Frames.Count;

            string resolvedName = ResolveFrame(_config.Frames[_frameIndex]);

            if (_config.CenterText)
                resolvedName = $"<align=\"center\">{resolvedName}</align>";

            GameServer.Name = resolvedName;

            if (_config.Debug)
                Log.Debug($"[ServerNameChanger] Frame {_frameIndex}: {resolvedName}");
        }

        private string ResolveFrame(string frame)
        {
            string resolved = frame ?? string.Empty;

            resolved = resolved.Replace("{server_name}", _config.ServerName ?? string.Empty);
            resolved = resolved.Replace("{tickrate}", GameServer.Tps.ToString("0.0", CultureInfo.InvariantCulture));
            resolved = resolved.Replace("{game_time}", FormatRoundTime(Exiled.API.Features.Round.ElapsedTime));
            resolved = resolved.Replace("{players}", Player.List.Count.ToString(CultureInfo.InvariantCulture));
            resolved = resolved.Replace("{max_players}", GameServer.MaxPlayerCount.ToString(CultureInfo.InvariantCulture));
            resolved = resolved.Replace("{admins}", Player.List.Count(player => player.RemoteAdminAccess).ToString(CultureInfo.InvariantCulture));
            resolved = resolved.Replace("{discord}", _config.DiscordUrl ?? string.Empty);
            resolved = resolved.Replace("{website}", _config.WebsiteUrl ?? string.Empty);
            resolved = resolved.Replace("{donate}", _config.DonateUrl ?? string.Empty);

            return resolved;
        }

        private static string FormatRoundTime(TimeSpan elapsedTime)
        {
            int totalMinutes = (int)elapsedTime.TotalMinutes;

            return string.Format(CultureInfo.InvariantCulture, "{0:00}:{1:00}", totalMinutes, elapsedTime.Seconds);
        }
    }
}