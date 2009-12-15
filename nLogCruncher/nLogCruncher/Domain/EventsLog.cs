﻿#region Copyright

// The contents of this file are subject to the Mozilla Public License
//  Version 1.1 (the "License"); you may not use this file except in compliance
//  with the License. You may obtain a copy of the License at
//  
//  http://www.mozilla.org/MPL/
//  
//  Software distributed under the License is distributed on an "AS IS"
//  basis, WITHOUT WARRANTY OF ANY KIND, either express or implied. See the
//  License for the specific language governing rights and limitations under 
//  the License.
//  
//  The Initial Developer of the Original Code is Robert Smyth.
//  Portions created by Robert Smyth are Copyright (C) 2008.
//  
//  All Rights Reserved.

#endregion

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Threading;


namespace NoeticTools.nLogCruncher.Domain
{
    public static class EventsLog
    {
        public static readonly ObservableCollection<IEventContext> Contexts = new ObservableCollection<IEventContext>();
        public static readonly ObservableCollection<IEventLevel> Levels = new ObservableCollection<IEventLevel>();

        private static readonly List<IStateListener<EventsLogChanged>> listeners =
            new List<IStateListener<EventsLogChanged>>();

        public static readonly ObservableCollection<ILogEvent> LogEvents = new ObservableCollection<ILogEvent>();
        private static readonly IEventContext rootContext = new EventContext("Root", null, 0);
        private static readonly TimeSpan updatePeriod = TimeSpan.FromSeconds(0.3);
        private static MessageQueue messageQueue;
        private static DispatcherTimer tickTimer;
        private static UDPListener udpListener;

        public static void ClearAll()
        {
            LogEvents.Clear();
            Levels.Clear();
            Levels.Add(new EventLevel("Error"));
            Levels.Add(new EventLevel("Warn"));
            Levels.Add(new EventLevel("Info"));
            Levels.Add(new EventLevel("Debug"));
            Levels.Add(new EventLevel("Trace"));
            rootContext.Clear();
        }

        public static void Start()
        {
            Contexts.Add(rootContext);
            messageQueue = new MessageQueue();
            udpListener = new UDPListener();
            udpListener.Start(messageQueue);

            tickTimer = new DispatcherTimer {Interval = updatePeriod};
            tickTimer.Tick += tickTimer_Tick;
            tickTimer.Start();
        }

        public static void Stop()
        {
            tickTimer.Stop();
            udpListener.Stop();
        }

        public static void AddListener(IStateListener<EventsLogChanged> listener)
        {
            listeners.Add(listener);
        }

        private static void tickTimer_Tick(object sender, EventArgs e)
        {
            if (messageQueue.HasMessages)
            {
                var messages = messageQueue.Dequeue();

                foreach (var message in messages)
                {
                    var logEvent = new LogEvent(message, rootContext, Levels);

                    if (logEvent.IsControlMessage)
                    {
                        var handled = false;

                        if (logEvent.Message.ToLower().Contains("events"))
                        {
                            handled = true;
                            LogEvents.Clear();
                        }

                        if (logEvent.Message.ToLower().Contains("reset"))
                        {
                            handled = true;
                            ClearAll();
                        }

                        if (!handled)
                        {
                            LogEvents.Add(logEvent);
                        }
                    }
                    else
                    {
                        LogEvents.Add(logEvent);
                    }
                }

                OnChanged();
            }
        }

        private static void OnChanged()
        {
            foreach (var listener in listeners)
            {
                listener.OnChange();
            }
        }
    }
}