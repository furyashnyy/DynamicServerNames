using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
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
                    if (_config.Debug)
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

            if (_config.CenterText && _config.UseAlignTag && !_config.BrowserSafeFormatting)
                resolvedName = $"<align=\"center\">{resolvedName}</align>";
            else if (_config.CenterText)
                resolvedName = CenterWithPadding(resolvedName, Math.Max(20, _config.CenterWidth));

            GameServer.Name = resolvedName;

            if (_config.Debug)
                Log.Debug($"[DynamicServerNames] Frame {_frameIndex}: {resolvedName}");
        }

        private string ResolveFrame(string frame)
        {
            string originalFrame = frame ?? string.Empty;
            string resolved = originalFrame;
            IReadOnlyList<string> links = GetConfiguredLinks();
            string link1 = GetLinkValue(links, 0, string.Empty);
            string link2 = GetLinkValue(links, 1, string.Empty);
            string link3 = GetLinkValue(links, 2, string.Empty);

            // If the author didn't include the {server_name} placeholder in the configured frame,
            // prepend a visible server name so every frame contains the server name.
            if (_config.AutoPrependServerName && !originalFrame.Contains("{server_name}"))
            {
                resolved = $"<color=#FF4444><b>{{server_name}}</b></color> " + resolved;
            }

            resolved = resolved.Replace("{server_name}", _config.ServerName ?? string.Empty);
            resolved = resolved.Replace("{tickrate}", GameServer.Tps.ToString("0.0", CultureInfo.InvariantCulture));
            resolved = resolved.Replace("{game_time}", FormatRoundTime(Exiled.API.Features.Round.ElapsedTime));
            resolved = resolved.Replace("{players}", Player.List.Count.ToString(CultureInfo.InvariantCulture));
            resolved = resolved.Replace("{max_players}", GameServer.MaxPlayerCount.ToString(CultureInfo.InvariantCulture));
            resolved = resolved.Replace("{admins}", Player.List.Count(player => player.RemoteAdminAccess).ToString(CultureInfo.InvariantCulture));
            resolved = resolved.Replace("{discord}", link1);
            resolved = resolved.Replace("{website}", link2);
            resolved = resolved.Replace("{donate}", link3);
            resolved = resolved.Replace("{links}", string.Join(" | ", links));

            for (int i = 0; i < 5; i++)
                resolved = resolved.Replace($"{{link{i + 1}}}", GetLinkValue(links, i, string.Empty));

            resolved = StripHiddenSegments(resolved);

            if (_config.BrowserSafeFormatting)
                resolved = StripRichTextTags(resolved);

            resolved = NormalizeSeparators(resolved);

            // Append an invisible server name at the end of every frame if enabled.
            if (_config.AppendHiddenName && !_config.BrowserSafeFormatting)
            {
                string hiddenValue = string.IsNullOrWhiteSpace(_config.HiddenName)
                    ? _config.ServerName ?? string.Empty
                    : _config.HiddenName;
                resolved = resolved + $" <size=0>{hiddenValue}</size>";
            }

            return resolved;
        }

        private static string StripRichTextTags(string value)
        {
            return Regex.Replace(
                value,
                "</?(?:color|size|align)(?:\\s*=\\s*(?:\"[^\"]*\"|#[0-9a-fA-F]+|[^\\s>]+))?\\s*>|</?(?:b|i|u|s)\\s*>",
                string.Empty,
                RegexOptions.IgnoreCase);
        }

        private static string StripHiddenSegments(string value)
        {
            // Some server browsers render <size=0> text literally, so remove hidden-size blocks.
            return Regex.Replace(value, @"<size\s*=\s*0\s*>.*?</size>", string.Empty, RegexOptions.IgnoreCase | RegexOptions.Singleline);
        }

        private static string NormalizeSeparators(string value)
        {
            string[] lines = value.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);

            for (int i = 0; i < lines.Length; i++)
            {
                string[] segments = lines[i].Split('|');
                List<string> nonEmptySegments = new List<string>();

                foreach (string segment in segments)
                {
                    if (HasVisibleContent(segment))
                        nonEmptySegments.Add(segment.Trim());
                }

                lines[i] = string.Join(" | ", nonEmptySegments);
            }

            return string.Join("\n", lines.Where(line => !string.IsNullOrWhiteSpace(line)));
        }

        private static bool HasVisibleContent(string text)
        {
            string withoutTags = Regex.Replace(text, @"<[^>]+>", string.Empty);
            return !string.IsNullOrWhiteSpace(withoutTags);
        }

        private static string CenterWithPadding(string value, int targetVisibleWidth)
        {
            string[] lines = value.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);

            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];
                int visibleLength = GetVisibleLength(line);

                if (visibleLength <= 0 || visibleLength >= targetVisibleWidth)
                    continue;

                int leftPad = (targetVisibleWidth - visibleLength) / 2;
                lines[i] = new string(' ', Math.Max(0, leftPad)) + line;
            }

            return string.Join("\n", lines);
        }

        private static int GetVisibleLength(string text)
        {
            string withoutTags = Regex.Replace(text, @"<[^>]+>", string.Empty);
            return withoutTags.Length;
        }

        private IReadOnlyList<string> GetConfiguredLinks()
        {
            List<string> links = new List<string>();

            foreach (string rawText in (_config.Links ?? new List<string>()).Take(5))
            {
                if (!TryParseTextLink(rawText, out string textUrl, out string textColor))
                    continue;

                if (_config.BrowserSafeFormatting)
                    links.Add(textUrl);
                else
                    links.Add(FormatColoredLink(textUrl, textColor));
            }

            return links.Where(link => !string.IsNullOrWhiteSpace(link)).Take(5).ToList();
        }

        private static bool TryParseTextLink(string rawText, out string url, out string color)
        {
            url = string.Empty;
            color = string.Empty;

            if (string.IsNullOrWhiteSpace(rawText))
                return false;

            string trimmed = rawText.Trim();
            int separatorIndex = trimmed.LastIndexOf(':');

            if (separatorIndex <= 0 || separatorIndex >= trimmed.Length - 1)
            {
                url = trimmed;
                return true;
            }

            string left = trimmed.Substring(0, separatorIndex).Trim();
            string right = trimmed.Substring(separatorIndex + 1).Trim();

            if (string.IsNullOrWhiteSpace(left))
                return false;

            url = left;
            color = right;
            return true;
        }

        private static string FormatColoredLink(string url, string color)
        {
            string trimmedUrl = (url ?? string.Empty).Trim();

            if (string.IsNullOrWhiteSpace(trimmedUrl))
                return string.Empty;

            string normalizedColor = NormalizeColor(color);

            if (string.IsNullOrWhiteSpace(normalizedColor))
                return trimmedUrl;

            return $"<color={normalizedColor}>{trimmedUrl}</color>";
        }

        private static string NormalizeColor(string color)
        {
            string normalized = (color ?? string.Empty).Trim();

            if (string.IsNullOrWhiteSpace(normalized))
                return string.Empty;

            if (string.Equals(normalized, "none", StringComparison.OrdinalIgnoreCase))
                return string.Empty;

            if (string.Equals(normalized, "null", StringComparison.OrdinalIgnoreCase))
                return string.Empty;

            return normalized;
        }

        private static string GetLinkValue(IReadOnlyList<string> links, int index, string fallback)
        {
            if (index >= 0 && index < links.Count)
                return links[index];

            return fallback ?? string.Empty;
        }

        private static string FormatRoundTime(TimeSpan elapsedTime)
        {
            int totalMinutes = (int)elapsedTime.TotalMinutes;

            return string.Format(CultureInfo.InvariantCulture, "{0:00}:{1:00}", totalMinutes, elapsedTime.Seconds);
        }
    }
}