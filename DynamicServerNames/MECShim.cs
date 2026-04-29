using System;
using System.Collections.Concurrent;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Exiled.API.Features;

namespace MEC
{
    /// <summary>
    /// Lightweight coroutine handle used by the timing shim.
    /// </summary>
    public readonly struct CoroutineHandle
    {
        internal CoroutineHandle(Guid id)
        {
            Id = id;
        }

        internal Guid Id { get; }
    }

    /// <summary>
    /// MEC-compatible timing helpers used by the plugin.
    /// </summary>
    public static class Timing
    {
        private static readonly object SyncRoot = new object();
        private static readonly ConcurrentDictionary<Guid, CoroutineState> RunningCoroutines = new ConcurrentDictionary<Guid, CoroutineState>();

        /// <summary>
        /// Starts a coroutine that yields wait times as floating-point seconds.
        /// </summary>
        /// <param name="coroutine">The coroutine to execute.</param>
        /// <returns>A handle that can be used to stop the coroutine.</returns>
        public static CoroutineHandle RunCoroutine(IEnumerator<float> coroutine)
        {
            if (coroutine == null)
                throw new ArgumentNullException(nameof(coroutine));

            Guid id = Guid.NewGuid();
            CoroutineState state = new CoroutineState(coroutine);
            RunningCoroutines[id] = state;
            state.Schedule(id, 0f);
            return new CoroutineHandle(id);
        }

        /// <summary>
        /// Stops the specified coroutine.
        /// </summary>
        /// <param name="handle">The coroutine handle to stop.</param>
        public static void KillCoroutines(CoroutineHandle handle)
        {
            if (!RunningCoroutines.TryRemove(handle.Id, out CoroutineState? state))
                return;

            state.Dispose();
        }

        /// <summary>
        /// Returns the wait time used by MEC-style coroutines.
        /// </summary>
        /// <param name="seconds">The number of seconds to wait.</param>
        /// <returns>The supplied wait time.</returns>
        public static float WaitForSeconds(float seconds) => seconds;

        private static bool Advance(Guid id)
        {
            if (!RunningCoroutines.TryGetValue(id, out CoroutineState? state))
                return false;

            bool movedNext;
            float waitTime = 0f;

            try
            {
                movedNext = state.Coroutine.MoveNext();
                if (movedNext)
                    waitTime = state.Coroutine.Current;
            }
            catch (Exception exception)
            {
                Log.Error($"[MEC] Coroutine failed: {exception}");
                KillCoroutines(new CoroutineHandle(id));
                return false;
            }

            if (!movedNext)
            {
                KillCoroutines(new CoroutineHandle(id));
                return false;
            }

            state.Schedule(id, waitTime);
            return true;
        }

        private sealed class CoroutineState
        {
            private readonly object timerLock = new object();

            public CoroutineState(IEnumerator<float> coroutine)
            {
                Coroutine = coroutine;
            }

            public IEnumerator<float> Coroutine { get; }

            public Timer? Timer { get; private set; }

            public void Schedule(Guid id, float waitTime)
            {
                lock (timerLock)
                {
                    if (Timer == null)
                    {
                        Timer = new Timer(_ => Advance(id), null, Timeout.Infinite, Timeout.Infinite);
                    }

                    int dueTimeMilliseconds = waitTime <= 0f ? 0 : (int)Math.Ceiling(waitTime * 1000f);
                    Timer.Change(dueTimeMilliseconds, Timeout.Infinite);
                }
            }

            public void Dispose()
            {
                lock (timerLock)
                {
                    Timer?.Dispose();
                    Timer = null;
                }
            }
        }
    }
}