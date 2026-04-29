using System;
using Exiled.API.Features;
using ServerEvents = Exiled.Events.Handlers.Server;

namespace DynamicServerNames
{
    /// <summary>
    /// Main plugin entry point for DynamicServerNames.
    /// </summary>
    public sealed class Plugin : Exiled.API.Features.Plugin<Config>
    {
        private NameRotator? _nameRotator;

        /// <inheritdoc />
        public override string Author => "furyashnyy";

        /// <inheritdoc />
        public override string Name => "DynamicServerNames";

        /// <inheritdoc />
        public override string Prefix => "DynamicServerNames";

        /// <inheritdoc />
        public override Version Version => new Version(1, 0, 0);

        /// <inheritdoc />
        public override void OnEnabled()
        {
            base.OnEnabled();

            _nameRotator = new NameRotator(Config);
            ServerEvents.WaitingForPlayers += OnWaitingForPlayers;
            _nameRotator.Start();

            int interval = Math.Max(1, Config.RotationInterval);

            if (Config.Debug)
            {
                Log.Info($"[DynamicServerNames] loaded. {Config.Frames.Count} frames, interval {interval}s");

                if (Config.Frames.Count == 0)
                    Log.Warn("[DynamicServerNames] No frames configured. Rotation will not start.");
            }
        }

        /// <inheritdoc />
        public override void OnDisabled()
        {
            ServerEvents.WaitingForPlayers -= OnWaitingForPlayers;

            _nameRotator?.Stop();
            _nameRotator = null;

            base.OnDisabled();
        }

        private void OnWaitingForPlayers()
        {
            _nameRotator?.Start();
        }
    }
}