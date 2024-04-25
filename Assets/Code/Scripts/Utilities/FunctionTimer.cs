using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace QuantumRevenant.Timer
{
    public class FunctionTimer
    {
        private static List<FunctionTimer> activeTimerList;
        private static GameObject initGameObject;
        private static void InitIfNeeded()
        {
            if (initGameObject == null)
            {
                initGameObject = new GameObject("FunctionTimer_InitGameObject");
                activeTimerList = new List<FunctionTimer>();
            }
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

        #endregion

        #region Stop Timers

        public static void StopNullTimers(bool mismatched = false) { StopMatchTimers(null, mismatched); }

        public static void StopFirstTimer(string timerName, bool mismatched = false)
        {
            activeTimerList.FirstOrDefault(x => x.timerName == timerName ^ mismatched)?.DestroySelf();
        }

        public static void StopMatchTimers(string timerName, bool mismatched = false)
        {
            activeTimerList
                        .Where(x => x.timerName == timerName ^ mismatched)
                        .ToList()
                        .ForEach(timer => timer.DestroySelf());
        }

        public static void StopAllTimers()
        {
            activeTimerList.ForEach(timer => timer.DestroySelf());
        }
        #endregion

        #region Internal Timer
        //Dummy class to access MonoBehaviour methods
        private class MonoBehaviourHood : MonoBehaviour
        {
            public Action onUpdate;
            private void Update()
            {
                if (onUpdate != null) onUpdate();
            }
        }

        private Action action;
        private float timer;
        private string timerName;
        private GameObject gameObject;
        private bool isDisabled;

        private FunctionTimer(Action action, float timer, string timerName, GameObject gameObject)
        {
            this.action = action;
            this.timer = timer;
            this.timerName = timerName;
            this.gameObject = gameObject;
            isDisabled = false;
        }

        public void Update()
        {
            if (isDisabled)
                return;

            timer -= Time.deltaTime;
            if (timer < 0)
            {
                action();
                DestroySelf();
            }
        }

        private void DestroySelf()
        {
            isDisabled = true;
            UnityEngine.Object.Destroy(gameObject);
            RemoveTimer(this);
        }
        #endregion
    }
}