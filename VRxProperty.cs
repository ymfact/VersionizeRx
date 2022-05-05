namespace VersionizeRx
{
    using System;
    using System.Collections.Generic;
    using System.Reactive.Linq;

    public readonly struct VRxProperty<TValue>
    {
        internal readonly IObservable<(TValue Value, uint Version)> Raw;

        public VRxProperty(IObservable<TValue> root)
        {
            var version = 0u;
            this.Raw = root.Select(value =>
            {
                version += 1;
                return (Value: value, Version: version);
            }).Publish().RefCount();
        }

        public VRxProperty(IObservable<(TValue Value, uint Version)> source)
            => this.Raw = source;

        public IObservable<TValue> Observable
            => this.Raw.Select(_ => _.Value);

        public VRxProperty<TReturn> Select<TReturn>(Func<TValue, TReturn> selector)
            => new VRxProperty<TReturn>(this.Raw.Select(_ => selector(_.Value)));

        public VRxProperty<TValue> DistinctUntilChanged()
            => new VRxProperty<TValue>(this.Raw.DistinctUntilChanged());

        public VRxProperty<TValue> DistinctUntilChanged(IEqualityComparer<TValue> valueComparer)
            => new VRxProperty<TValue>(this.Raw.DistinctUntilChanged(new EqualityComparer(valueComparer)));

        public VRxProperty<TValue> DistinctUntilChanged<TKey>(Func<TValue, TKey> keySelector)
            => new VRxProperty<TValue>(this.Raw.DistinctUntilChanged(tuple => keySelector(tuple.Value)));

        public VRxProperty<TValue> DistinctUntilChanged<TKey>(Func<TValue, TKey> keySelector, IEqualityComparer<TKey> comparer)
            => new VRxProperty<TValue>(this.Raw.DistinctUntilChanged(tuple => keySelector(tuple.Value), comparer));

        private readonly struct EqualityComparer : IEqualityComparer<(TValue Value, uint Version)>
        {
            private readonly IEqualityComparer<TValue> valueComparer;

            public EqualityComparer(IEqualityComparer<TValue> valueComparer)
                => this.valueComparer = valueComparer;

            bool IEqualityComparer<(TValue Value, uint Version)>.Equals((TValue Value, uint Version) x, (TValue Value, uint Version) y)
                => this.valueComparer.Equals(x.Value, y.Value);

            int IEqualityComparer<(TValue Value, uint Version)>.GetHashCode((TValue Value, uint Version) obj)
                => this.valueComparer.GetHashCode(obj.Value);
        }
    }

    public static class VRxProperty
    {
        public static VRxProperty<TValue> ToVRxProperty<TValue>(this IObservable<TValue> root)
            => new VRxProperty<TValue>(root);
    }
}
