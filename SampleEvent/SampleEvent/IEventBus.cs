﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleEvent
{
    public interface IEventBus
    {
        void Publish<TSampleEvent>(TSampleEvent sampleEvent) where TSampleEvent : ISampleEvent;
    }
}