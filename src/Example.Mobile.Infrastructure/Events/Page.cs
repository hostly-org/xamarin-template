using System;
using System.Collections.Generic;
using System.Text;

namespace Example.Mobile.Infrastructure.Events
{
    public sealed class Page
    {
        public long Offset { get; }
        public long PreviousOffset { get; }
        public IEnumerable<IEvent> Events { get; }

        public Page(long offset, long previousOffset, IEnumerable<IEvent> events)
        {
            if (events == null)
                throw new ArgumentNullException(nameof(events));

            Offset = offset;
            PreviousOffset = previousOffset;
            Events = events;
        }
    }
}
