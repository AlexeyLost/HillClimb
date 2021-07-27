using System;

namespace GAME.Managers
{
    public class EventsManager
    {
        private static EventsManager instance;
        
        public BikeEventBus BikeEvents { get; }
        public ControlEventBus ControlEvents { get; }
        public MainEventBus MainEvents { get; }

        public static EventsManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new EventsManager();

                return instance;
            }
        }

        public EventsManager()
        {
            BikeEvents = new BikeEventBus();
            ControlEvents = new ControlEventBus();
            MainEvents = new MainEventBus();
        }
        
        public class BikeEventBus
        {
            public Action BikeUpsideDown;
            public Action<float> Wheelie;
            public Action WheelieStopped;
            public Action<float> DistanceUpdated;
        }
        
        public class ControlEventBus
        {
            public Action PlayerTappedRight;
            public Action PlayerTappedLeft;
            public Action PlayerTappedUp;
        }
        
        public class MainEventBus
        {
            public Action UnsubscribeAll;
            public Action Update;
            public Action GameStarted;
        }
    }
}