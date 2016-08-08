using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Kinect;
using Microsoft.Kinect.Input;
using Microsoft.Kinect.Toolkit.Input;

namespace Photobooth_PPTIK.Helper
{
    class EngagementManager : IKinectEngagementManager
    {
        private KinectSensor sensor;

        public event EventHandler Engaged;
        public event EventHandler Disengaged;

        private bool changed;
        private ulong? trackingId;
        BodyFrameReader reader;
        private Body[] bodies;

        public EngagementManager(KinectSensor sensor)
        {
            this.sensor = sensor;
        }

        public IReadOnlyList<BodyHandPair> KinectManualEngagedHands
        {
            get
            {
                return (KinectCoreWindow.KinectManualEngagedHands);
            }
        }

        public bool EngagedBodyHandPairsChanged()
        {
            return (this.changed);
        }

        public void StartManaging()
        {
            this.bodies = new Body[this.sensor.BodyFrameSource.BodyCount];
            this.reader = this.sensor.BodyFrameSource.OpenReader();
            this.reader.FrameArrived += OnFrameArrived;

        }

        public void StopManaging()
        {
            this.reader.FrameArrived -= OnFrameArrived;
            this.reader.Dispose();
            this.bodies = null;
        }

        void OnFrameArrived(object sender, BodyFrameArrivedEventArgs args)
        {
            if (args.FrameReference != null)
            {
                using (var frame = args.FrameReference.AcquireFrame())
                {
                    frame.GetAndRefreshBodyData(this.bodies);

                    // we want the first body that is tracked and where the 2 joints I'm interested
                    // in are tracked and where the hand is above the shoulder.
                    var first =
                      this.bodies.FirstOrDefault(
                        b => b.IsTracked
                          && AreTracked(b, JointType.HandRight, JointType.ShoulderRight)
                          && VerticalDistance(b, JointType.HandRight, JointType.ShoulderRight) >= 0.0d);

                    // got one? check to see if we need to trigger engagement.
                    if (first != null)
                    {
                        this.EnsureEngaged(first.TrackingId);
                    }
                    else
                    {
                        // not got one? check to see if we need to clear engagement.
                        this.EnsureNotEngaged();
                    }
                }
            }
        }

        
        private void EnsureEngaged(ulong trackingId)
        {
            if (this.trackingId != trackingId)
            {
                this.trackingId = trackingId;

                this.changed = true;

                KinectCoreWindow.SetKinectOnePersonManualEngagement(
                  new BodyHandPair(trackingId, HandType.RIGHT));

                if (this.Engaged != null)
                {
                    this.Engaged(this, EventArgs.Empty);
                }
            }
        }

        private void EnsureNotEngaged()
        {
            if (this.trackingId != null)
            {
                this.changed = true;

                this.trackingId = null;

                KinectCoreWindow.SetKinectOnePersonManualEngagement(null);

                if (this.Disengaged != null)
                {
                    this.Disengaged(this, EventArgs.Empty);
                }
            }
        }

        static bool AreTracked(Body body, params JointType[] joints)
        {
            return (joints.All(j => body.Joints[j].TrackingState == TrackingState.Tracked));
        }
        static double VerticalDistance(Body body, JointType jointOne, JointType jointTwo)
        {
            return (body.Joints[jointOne].Position.Y - body.Joints[jointTwo].Position.Y);
        }
    }
}
