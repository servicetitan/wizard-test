namespace WizardTest.Core;

public static class KvTarjan
{
    public static IEnumerable<TValue> GetOrdered<TKey, TValue>(
        TKey rootKey, Func<TKey, TValue> map, Func<TValue, IEnumerable<TKey>> next) where TKey : notnull
    {
        IEnumerable<TValue> DfsPostOrder(TKey key, IDictionary<TKey, bool> marks)
        {
            if (!marks.TryAdd(key, false)) {
                if (marks[key]) {
                    yield break;
                }

                throw new Exception("Cycle detected");
            }

            var value = map(key);

            foreach (var childKey in next(value))
            foreach (var childValue in DfsPostOrder(childKey, marks)) {
                yield return childValue;
            }

            marks[key] = true;
            yield return value;
        }

        var sorted = DfsPostOrder(rootKey, new Dictionary<TKey, bool>());
        return sorted;
    }
}
