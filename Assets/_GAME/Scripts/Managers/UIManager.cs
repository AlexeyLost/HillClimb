using System;
using System.Collections;
using TMPro;
using UnityEngine;

namespace GAME.Managers
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI distanceLbl;
        [SerializeField] private TextMeshProUGUI wheelieLbl;

        private Coroutine wheelieLblCoroutine;
        
        private void OnEnable()
        {
            EventsManager.Instance.MainEvents.GameStarted += InitUI;
            EventsManager.Instance.BikeEvents.Wheelie += UpdateWheelieLbl;
            EventsManager.Instance.BikeEvents.WheelieStopped += HideWheelieLbl;
            EventsManager.Instance.BikeEvents.DistanceUpdated += UpdateDistanceLbl;
        }

        private void OnDisable()
        {
            EventsManager.Instance.MainEvents.GameStarted -= InitUI;
            EventsManager.Instance.BikeEvents.Wheelie -= UpdateWheelieLbl;
            EventsManager.Instance.BikeEvents.WheelieStopped -= HideWheelieLbl;
            EventsManager.Instance.BikeEvents.DistanceUpdated -= UpdateDistanceLbl;
        }

        private void InitUI()
        {
            distanceLbl.text = "Distance: 0";
            wheelieLbl.text = "Wheelie: 0s";
            wheelieLbl.alpha = 0;
        }

        private void UpdateWheelieLbl(float sec)
        {
            decimal deSec = (decimal) sec;
            wheelieLbl.text = "Wheelie: " + Math.Round(deSec, 1) + "s";
            if (wheelieLblCoroutine == null)
                wheelieLblCoroutine = StartCoroutine(UpdateWheelieLblState(true));
        }

        private void HideWheelieLbl()
        {
            if (wheelieLblCoroutine != null) StopCoroutine(wheelieLblCoroutine);
            wheelieLblCoroutine = StartCoroutine(UpdateWheelieLblState(false));
        }

        private IEnumerator UpdateWheelieLblState(bool show)
        {
            bool condition = show ? wheelieLbl.alpha < 1 : wheelieLbl.alpha > 0;
            while (condition)
            {
                wheelieLbl.alpha += show ? 0.1f : -0.1f;
                yield return new WaitForSeconds(0.03f);
            }

            wheelieLblCoroutine = null;
        }

        private void UpdateDistanceLbl(float dist)
        {
            distanceLbl.text = "Distance: " + Mathf.FloorToInt(dist);
        }
    }
}