using System.Reactive.Linq;

namespace VersionizeRx
{
    public static class VRx
    {
        public static VRxProperty<(T1, T2)> Combine<T1, T2>(
            VRxProperty<T1> _1, VRxProperty<T2> _2)
            => new VRxProperty<(T1, T2)>(
                ObservableEx
                .CombineLatest(_1._observable, _2._observable)
                .Where(_ => _.First.Version == _.Second.Version)
            .Select(_ => ((_.First.Value, _.Second.Value), _.First.Version)));

        public static VRxProperty<(T1, T2, T3)> Combine<T1, T2, T3>(
            VRxProperty<T1> _1, VRxProperty<T2> _2, VRxProperty<T3> _3)
            => new VRxProperty<(T1, T2, T3)>(
                ObservableEx
                .CombineLatest(_1._observable, _2._observable, _3._observable)
                .Where(_ => _.First.Version == _.Second.Version && _.Second.Version == _.Third.Version)
            .Select(_ => ((_.First.Value, _.Second.Value, _.Third.Value), _.First.Version)));

        public static VRxProperty<(T1, T2, T3, T4)> Combine<T1, T2, T3, T4>(
            VRxProperty<T1> _1, VRxProperty<T2> _2, VRxProperty<T3> _3, VRxProperty<T4> _4)
            => new VRxProperty<(T1, T2, T3, T4)>(
                ObservableEx
                .CombineLatest(_1._observable, _2._observable, _3._observable, _4._observable)
                .Where(_ => _.First.Version == _.Second.Version && _.Third.Version == _.Fourth.Version
                && _.First.Version == _.Third.Version)
            .Select(_ => ((_.First.Value, _.Second.Value, _.Third.Value, _.Fourth.Value), _.First.Version)));
    }
}
