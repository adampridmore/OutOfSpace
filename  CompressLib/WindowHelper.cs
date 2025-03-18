namespace CompressLib;

//public static class WindowHelper {
    // public static void Window2<T>(this IEnumerable<T> list, Action<List<T>> function)
    // {
    //     // ReSharper disable PossibleMultipleEnumeration
    //     foreach (var (first, second) in list.Zip((list.Skip(1))))
    //     {
    //         var items = new List<T> { first, second };
    //         function(items);
    //     }
    // }
    //
    // public static IEnumerable<IEnumerable<T>> WindowSelectMany2<T>
    //     (this Func<IEnumerable<T>,List<T>> function, IEnumerable<T> list)
    // {
    //     // ReSharper disable PossibleMultipleEnumeration
    //     foreach (var (first, second) in list.Zip((list.Skip(1))))
    //     {
    //         var items = new List<T>() {first, second};
    //         
    //         yield return function(items);
    //     }
    // }
//}