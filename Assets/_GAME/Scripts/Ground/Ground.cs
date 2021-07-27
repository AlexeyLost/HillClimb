using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GAME.Managers;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GAME.Grounds
{
    public class Ground
    {
        private GroundView view;

        private Camera mainCam;
        private Vector3 startPos;
        private Vector3 lastPos;
        private float deltaX;
        private float screenWidth;
        private bool started;

        private List<Vector3> groundPoints;


        public Ground()
        {
            EventsManager.Instance.MainEvents.UnsubscribeAll += UnsubscribeAll;
            mainCam = Camera.main;
            screenWidth = Screen.width;
            deltaX = screenWidth / 2;
            view = SpawnManager.Instance.GetObjectView(nameof(GroundView)) as GroundView;
            startPos = mainCam.ScreenToWorldPoint(new Vector3(0, Screen.height / 2, 0));
            startPos.z = 0;
            lastPos = mainCam.ScreenToWorldPoint(new Vector3(screenWidth, Screen.height / 2, 0));
            lastPos.z = 0;
            groundPoints = new List<Vector3>
            {
                startPos,
                lastPos
            };
            SetGroundPoints(groundPoints);

            started = true;
            GroundGenerator();
        }

        private void SetGroundPoints(List<Vector3> _points)
        {
            view.lineRend.positionCount = _points.Count;
            view.lineRend.SetPositions(_points.ToArray());
            RecalculateCollider(_points);
        }

        private void AddGroundPoint(List<Vector3> _points, Vector3 _pos)
        {
            view.lineRend.positionCount++;
            view.lineRend.SetPosition(_points.Count - 1, _pos);
            RecalculateCollider(_points);
        }

        private void RecalculateCollider(List<Vector3> _points)
        {
            List<Vector2> v2Points = new List<Vector2>();
            foreach (var curPoint in _points)
            {
                v2Points.Add(curPoint);
            }

            view.col.points = v2Points.ToArray();
        }

        private void GenerateNewGroundPoint()
        {
            lastPos += Vector3.right * Random.Range(8f, 14f) + Vector3.up * Random.Range(-7f, 7f);
            groundPoints.Add(lastPos);
            AddGroundPoint(groundPoints, lastPos);
        }

        private bool NeedToSpawnGround()
        {
            return lastPos.x < mainCam.ScreenToWorldPoint(new Vector3(screenWidth + deltaX, 0, 0)).x;
        }

        private void DespawnOldPoints()
        {
            List<Vector3> pointsToRemove = new List<Vector3>();
            foreach (var curPoint in groundPoints)
            {
                if (curPoint.x < mainCam.ScreenToWorldPoint(new Vector3(0 - screenWidth * 1.5f, 0, 0)).x) 
                    pointsToRemove.Add(curPoint);
            }

            foreach (var curPoint in pointsToRemove)
            {
                groundPoints.Remove(curPoint);
            }
            SetGroundPoints(groundPoints);
        }
        
        private async void GroundGenerator()
        {
            while (started)
            {
                if (NeedToSpawnGround())
                {
                    if (groundPoints.Count > 10)
                    {
                        DespawnOldPoints();
                    }
                    GenerateNewGroundPoint();
                }

                await Task.Delay(TimeSpan.FromSeconds(0.2f));
            }
        }

        private void UnsubscribeAll()
        {
            started = false;
            EventsManager.Instance.MainEvents.UnsubscribeAll -= UnsubscribeAll;
        }
    }
}