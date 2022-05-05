using System;
using System.Reactive.Linq;

namespace VersionizeRx
{
    public readonly struct VRxProperty<TValue>
    {
        internal readonly IObservable<(TValue Value, uint Version)> _source;
        public IObservable<TValue> Observable => _source.Select(_ => _.Value);

        public VRxProperty(IObservable<TValue> root)
        {
            var version = 0u;
            _source = root.Select(value =>
            {
                version += 1;
                return (Value: value, Version: version);
            }).Publish().RefCount();
        }

        public VRxProperty(IObservable<(TValue, uint)> source) => _source = source;

        public VRxProperty<TValue> Where(Func<TValue, bool> predicate)
            => new VRxProperty<TValue>(_source.Where(_ => predicate(_.Value)));

        public VRxProperty<TReturn> Select<TReturn>(Func<TValue, TReturn> selector)
            => new VRxProperty<TReturn>(_source.Select(_ => selector(_.Value)));
    }

    public static class VRxProperty
    {
        public static VRxProperty<TValue> ToVRxProperty<TValue>(this IObservable<TValue> root) => new VRxProperty<TValue>(root);
    }
}
