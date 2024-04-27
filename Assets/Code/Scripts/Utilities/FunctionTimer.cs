using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Collections;

namespace QuantumRevenant.Timer
{
    public class FunctionTimer
    {
        private static List<FunctionTimer> activeTimerList;
        private static GameObject initGameObject;
        private static void InitIfNeeded()
        {
            if (initGameObject != null)
                return;

            initGameObject = new GameObject("FunctionTimer_InitGameObject");
            activeTimerList = new List<FunctionTimer>();
        }

        public static FunctionTimer Create(Action action, float timer, string timerName = null)
        {
            InitIfNeeded();
            GameObject gameObject = new GameObject("FunctionTimer", typeof(MonoBehaviourHood));
            FunctionTimer functionTimer = new FunctionTimer(action, timer, timerName, gameObject);
            gameObject.GetComponent<MonoBehaviourHood>().onUpdate = functionTimer.Update;
            activeTimerList.Add(functionTimer);
            return functionTimer;
        }

        private static void RemoveTimer(FunctionTimer functionTimer)
        {
            InitIfNeeded();
            activeTimerList.Remove(functionTimer);
        }

        #region Play/Pause Timers
        public static void PlayPauseNullTimers(bool pause, bool mismatched = false, bool ignorePausedStatus = true, bool paused = true)
        { PlayPauseMatchTimer(pause, null, mismatched, ignorePausedStatus, paused); }
        public static void PlayPauseAllTimers(bool pause) { activeTimerList.ForEach(timer => timer.setPause(pause)); }
        public static void PlayPauseFirstTimer(bool pause, string timerName, bool mismatched = false, bool ignorePausedStatus = true, bool paused = true)
        { SelectFirstTimer(timerName, mismatched, ignorePausedStatus, paused)?.setPause(pause); }
        public static void PlayPauseMatchTimer(bool pause, string timerName, bool mismatched = false, bool ignorePausedStatus = true, bool paused = true)
        { SelectMatchTimers(timerName, mismatched, ignorePausedStatus, paused)?.ForEach(timer => timer.setPause(pause)); }
        #endregion

        #region Stop Timers
        public static void StopNullTimers(bool mismatched = false, bool ignorePausedStatus = true, bool paused = true)
        { StopMatchTimers(null, mismatched, ignorePausedStatus, paused); }
        public static void StopAllTimers() { activeTimerList.ForEach(timer => timer.DestroySelf()); }
        public static void StopFirstTimer(string timerName, bool mismatched = false, bool ignorePausedStatus = true, bool paused = true)
        { SelectFirstTimer(timerName, mismatched, ignorePausedStatus, paused)?.DestroySelf(); }
        public static void StopMatchTimers(string timerName, bool mismatched = false, bool ignorePausedStatus = true, bool paused = true)
        { SelectMatchTimers(timerName, mismatched, ignorePausedStatus, paused).ForEach(timer => timer.DestroySelf()); }

        #endregion
        #region Select Timers
        private static FunctionTimer SelectFirstTimer(string timerName, bool mismatched = false, bool ignorePausedStatus = true, bool paused = true)
        {
            return activeTimerList.FirstOrDefault(x => x.timerName == timerName ^ mismatched && (ignorePausedStatus || (x.isDisabled != paused)));
        }
        private static List<FunctionTimer> SelectMatchTimers(string timerName, bool mismatched = false, bool ignorePausedStatus = true, bool paused = true)
        {
            return activeTimerList
                        .Where(x => x.timerName == timerName ^ mismatched && (ignorePausedStatus || (x.isDisabled != paused)))
                        .ToList();
        }
        #endregion

        #region Internal Timer
        //Dummy class to access MonoBehaviour methods
        private class MonoBehaviourHood : MonoBehaviour
        {
            public Action onUpdate;
            private void Update() { if (onUpdate != null) onUpdate(); }
        }

        private Action action;
        private float timer;
        private string timerName;
        private GameObject gameObject;
        private bool isDisabled;
        private bool isReusable;
        private FunctionTimer(Action action, float timer, string timerName, GameObject gameObject, bool isReusable = false)
        {
            this.action = action;
            this.timer = timer;
            this.timerName = timerName;
            this.gameObject = gameObject;
            this.isReusable = isReusable;
            isDisabled = false;
        }

        private void Update()
        {
            if (isDisabled)
                return;

            timer -= Time.deltaTime;
            if (timer < 0)
            {
                action();
                setPause(!isReusable);

                if (!isReusable)
                    DestroySelf();
                else
                    timer = float.PositiveInfinity;
            }
        }

        public void setPause(bool paused) { isDisabled = paused; }

        public void changeTime(float timer) { this.timer = timer; }
        public float getTimer() { return timer; }

        public void DestroySelf()
        {
            isDisabled = true;
            if (isReusable)
            {
                Debug.Log($"Hey! estás intentando borrarme. Pero soy reutilizable. Simplemente me voy a desactivar. Verifica esto - {timerName}");
                return;
            }
            UnityEngine.Object.Destroy(gameObject);
            RemoveTimer(this);
        }

        public void NoObjectionDestroySelf()
        {
            isDisabled = true;
            UnityEngine.Object.Destroy(gameObject);
            RemoveTimer(this);
        }
        #endregion
    }

    public class CoroutinesTimer
    {
        public static CoroutinesTimer Create(Action action, float timer, string timerName = null)
        {
            GameObject gameObject = new GameObject("CoroutineTimer", typeof(MonoBehaviourHood));
            CoroutinesTimer coroutinesTimer = new CoroutinesTimer(action, timer, timerName, gameObject);
            gameObject.GetComponent<MonoBehaviourHood>().action = coroutinesTimer.action;
            gameObject.GetComponent<MonoBehaviourHood>().onEnd = coroutinesTimer.DestroySelf;
            return coroutinesTimer;
        }
        private class MonoBehaviourHood : MonoBehaviour
        {
            public Action action;
            public float timer;
            public Action onEnd;
            private void Start()
            {
                StartCoroutine(CoroutineTimer(timer, action));
            }
            IEnumerator CoroutineTimer(float timer, Action action)
            {
                yield return new WaitForSeconds(timer);
                action();
                onEnd();
            }
        }
        private Action action;
        private float timer;
        private string timerName;
        private GameObject gameObject;
        private bool isDisabled;
        private CoroutinesTimer(Action action, float timer, string timerName, GameObject gameObject)
        {
            this.action = action;
            this.timer = timer;
            this.timerName = timerName;
            this.gameObject = gameObject;
            isDisabled = false;
        }
        private void DestroySelf()
        {
            isDisabled = true;
            UnityEngine.Object.Destroy(gameObject);
        }
    }


}