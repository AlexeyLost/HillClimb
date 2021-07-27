using System;
using System.Threading.Tasks;
using GAME.Grounds;
using GAME.Managers;
using UnityEngine;

namespace GAME.Bikes
{
    public class Bike
    {
        public BikeView view;


        private bool tapRight;
        private bool tapLeft;
        private float wheelieTime;
        private bool wheelieStarted;
        private bool started;
        private float distance;
        
        public Bike()
        {
            view = SpawnManager.Instance.GetObjectView(nameof(BikeView)) as BikeView;
            EventsManager.Instance.ControlEvents.PlayerTappedRight += MotorForward;
            EventsManager.Instance.ControlEvents.PlayerTappedLeft += MotorBackward;
            EventsManager.Instance.ControlEvents.PlayerTappedUp += MotorToZero;
            view.forwardWheelTrigger.CollisionExit += ForwardWheelExitCollider;
            view.forwardWheelTrigger.CollisionEnter += ForwardWheelEnterCollider;
            view.headTrigger.CollisionEnter += HeadCollided;
            EventsManager.Instance.MainEvents.UnsubscribeAll += UnsubscribeAll;
            EventsManager.Instance.MainEvents.Update += Update;
            started = true;
            UpdateDistance();
        }


        private void MotorForward()
        {
            JointMotor2D backMotor = view.backWheelJoint.motor;
            backMotor.motorSpeed = 600f;
            view.backWheelJoint.motor = backMotor;
        }

        private void MotorBackward()
        {
            JointMotor2D backMotor = view.backWheelJoint.motor;
            backMotor.motorSpeed = 0f;
            view.backWheelJoint.motor = backMotor;
            JointMotor2D forwardMotor = view.forwardWheelJoint.motor;
            forwardMotor.motorSpeed = 0f;
            view.forwardWheelJoint.motor = forwardMotor;
        }

        private void MotorToZero()
        {
            JointMotor2D backMotor = view.backWheelJoint.motor;
            backMotor.motorSpeed = 0f;
            view.backWheelJoint.motor = backMotor;
            view.backWheelJoint.useMotor = false;
            JointMotor2D forwardMotor = view.forwardWheelJoint.motor;
            forwardMotor.motorSpeed = 0f;
            view.forwardWheelJoint.motor = forwardMotor;
            view.forwardWheelJoint.useMotor = false;
        }

        private void ForwardWheelExitCollider(Collision2D coll2D)
        {
            if (coll2D.gameObject.GetComponent<GroundView>())
            {
                wheelieStarted = true;
                wheelieTime = 0;
            }
        }

        private void ForwardWheelEnterCollider(Collision2D coll2D)
        {
            if (coll2D.gameObject.GetComponent<GroundView>())
            {
                wheelieStarted = false;
                wheelieTime = 0;
                EventsManager.Instance.BikeEvents.WheelieStopped?.Invoke();
            }
        }
        
        private void HeadCollided(Collision2D coll2d)
        {
            if (coll2d.gameObject.GetComponent<GroundView>())
            {
                EventsManager.Instance.BikeEvents.BikeUpsideDown?.Invoke();
            }
        }

        private void Update()
        {
            if (wheelieStarted)
            {
                wheelieTime += Time.deltaTime;
                if (wheelieTime > 1f)
                {
                    EventsManager.Instance.BikeEvents.Wheelie?.Invoke(wheelieTime);
                }
            }
        }

        private async void UpdateDistance()
        {
            while (started)
            {
                if (view.bodyTransform.transform.position.x > distance)
                {
                    distance = view.bodyTransform.position.x;
                    EventsManager.Instance.BikeEvents.DistanceUpdated?.Invoke(distance);
                }
                await Task.Delay(TimeSpan.FromSeconds(0.5f));
            }
        }
        
        private void UnsubscribeAll()
        {
            started = false;
            EventsManager.Instance.ControlEvents.PlayerTappedRight -= MotorForward;
            EventsManager.Instance.ControlEvents.PlayerTappedLeft -= MotorBackward;
            EventsManager.Instance.ControlEvents.PlayerTappedUp -= MotorToZero;
            view.forwardWheelTrigger.CollisionExit -= ForwardWheelExitCollider;
            view.headTrigger.CollisionEnter -= HeadCollided;
            view.forwardWheelTrigger.CollisionEnter -= ForwardWheelEnterCollider;
            EventsManager.Instance.MainEvents.Update -= Update;
            EventsManager.Instance.MainEvents.UnsubscribeAll -= UnsubscribeAll;
        }
    }
}