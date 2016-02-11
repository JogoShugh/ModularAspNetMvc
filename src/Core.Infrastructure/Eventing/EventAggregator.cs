using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace Core.Infrastructure.Eventing
{
    public class EventAggregator
    {
        // TODO: I know, I know singletons are bad
        private static EventAggregator _instance = new EventAggregator();

        public static EventAggregator Instance
        {
            get { return _instance; }
        }

        private ISubject<object> _subject = new Subject<object>();

        public IObservable<TEvent> GetEvent<TEvent>()
        {
            return _subject.AsObservable().OfType<TEvent>();
        }

        public void Subscribe<TEvent>(Action<TEvent> action)
        {
            GetEvent<TEvent>().Subscribe(action);
        }

        public void Publish<TEvent>(TEvent @event)
        {
            _subject.OnNext(@event);
        }
    }
}
