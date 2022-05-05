using System.Reactive.Linq;

namespace VersionizeRx
{
    public static class VRx
    {
        public static VRxProperty<(TLeft, TRight)> Combine<TLeft, TRight>(VRxProperty<TLeft> left, VRxProperty<TRight> right)
            => new VRxProperty<(TLeft, TRight)>(
                ObservableEx.CombineLatest(left._source, right._source)
                .Where(_ => _.First.Version == _.Second.Version)
            .Select(_ => ((Left: _.First.Value, _.Second.Value), _.First.Version)));
    }
}
