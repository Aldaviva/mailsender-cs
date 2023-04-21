namespace MailSender;

// ReSharper disable InconsistentNaming - library functions that are supposed to look like Linq methods, which user UpperCase naming.
public static class Extensions {

    public static IEnumerable<T> Compact<T>(this IEnumerable<T?> source) where T: class {
        return source.Where(item => item != null)!;
    }

    public static IEnumerable<T> Compact<T>(this IEnumerable<T?> source) where T: struct {
        return source.Where(item => item != null).Cast<T>();
    }

    public static string? EmptyToNull(this string? str) {
        return string.IsNullOrWhiteSpace(str) ? null : str;
    }

}