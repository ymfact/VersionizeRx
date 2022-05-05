using System;
using System.Collections.Generic;
using System.Reactive.Linq;

namespace VersionizeRx
{
    public readonly struct VRxProperty<TValue>
    {
        internal readonly IObservable<(TValue Value, uint Version)> _observable;
        public IObservable<TValue> Observable => _observable.Select(_ => _.Value);

        public VRxProperty(IObservable<TValue> root)
        {
            var version = 0u;
            _observable = root.Select(value =>
            {
                version += 1;
                return (Value: value, Version: version);
            }).Publish().RefCount();
        }

        public VRxProperty(IObservable<(TValue, uint)> source)
            => _observable = source;

        public VRxProperty<TReturn> Select<TReturn>(Func<TValue, TReturn> selector)
            => new VRxProperty<TReturn>(_observable.Select(_ => selector(_.Value)));

        public VRxProperty<TValue> DistinctUntilChanged()
            => new VRxProperty<TValue>(_observable.DistinctUntilChanged());

        public VRxProperty<TValue> DistinctUntilChanged(IEqualityComparer<TValue> valueComparer)
            => new VRxProperty<TValue>(_observable.DistinctUntilChanged(new EqualityComparer(valueComparer)));

        public VRxProperty<TValue> DistinctUntilChanged<TKey>(Func<TValue, TKey> keySelector)
            => new VRxProperty<TValue>(_observable.DistinctUntilChanged(tuple => keySelector(tuple.Value)));

        public VRxProperty<TValue> DistinctUntilChanged<TKey>(Func<TValue, TKey> keySelector, IEqualityComparer<TKey> comparer)
            => new VRxProperty<TValue>(_observable.DistinctUntilChanged(tuple => keySelector(tuple.Value), comparer));

        private readonly struct EqualityComparer : IEqualityComparer<(TValue Value, uint Version)>
        {
            private readonly IEqualityComparer<TValue> _valueComparer;

            public EqualityComparer(IEqualityComparer<TValue> valueComparer)
                => _valueComparer = valueComparer;

            bool IEqualityComparer<(TValue Value, uint Version)>.Equals((TValue Value, uint Version) x, (TValue Value, uint Version) y)
                => _valueComparer.Equals(x.Value, y.Value);

            int IEqualityComparer<(TValue Value, uint Version)>.GetHashCode((TValue Value, uint Version) obj)
                => _valueComparer.GetHashCode(obj.Value);
        }
    }

    public static class VRxProperty
    {
        public static VRxProperty<TValue> ToVRxProperty<TValue>(this IObservable<TValue> root) => new VRxProperty<TValue>(root);
    }
}
