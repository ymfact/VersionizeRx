namespace VersionizeRx
{
    using System.Reactive.Linq;

    public static class VRx
    {
        public static VRxProperty<(T1 Value1, T2 Value2)> Combine<T1, T2>(
            VRxProperty<T1> source1, VRxProperty<T2> source2)
            => new VRxProperty<(T1, T2)>(
                ObservableEx
                .CombineLatest(source1.Raw, source2.Raw)
                .Where(_ => _.First.Version == _.Second.Version)
            .Select(_ => ((_.First.Value, _.Second.Value), _.First.Version)));

        public static VRxProperty<(T1 Value1, T2 Value2, T3 Value3)> Combine<T1, T2, T3>(
            VRxProperty<T1> source1, VRxProperty<T2> source2, VRxProperty<T3> source3)
            => new VRxProperty<(T1, T2, T3)>(
                ObservableEx
                .CombineLatest(source1.Raw, source2.Raw, source3.Raw)
                .Where(_ => _.First.Version == _.Second.Version && _.Second.Version == _.Third.Version)
            .Select(_ => ((_.First.Value, _.Second.Value, _.Third.Value), _.First.Version)));

        public static VRxProperty<(T1 Value1, T2 Value2, T3 Value3, T4 Value4)> Combine<T1, T2, T3, T4>(
            VRxProperty<T1> source1, VRxProperty<T2> source2, VRxProperty<T3> source3, VRxProperty<T4> source4)
            => new VRxProperty<(T1, T2, T3, T4)>(
                ObservableEx
                .CombineLatest(source1.Raw, source2.Raw, source3.Raw, source4.Raw)
                .Where(_ => _.First.Version == _.Second.Version && _.Third.Version == _.Fourth.Version
                && _.First.Version == _.Third.Version)
            .Select(_ => ((_.First.Value, _.Second.Value, _.Third.Value, _.Fourth.Value), _.First.Version)));
    }
}
