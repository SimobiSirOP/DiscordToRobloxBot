using System.Runtime.CompilerServices;

namespace DiscordToRobloxBan.RobloxAPI.Helpers;

#pragma warning disable IDE0130, MA0003
internal static class ObjectExtensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static T ThrowIfNull<T>(this T? value) => value ?? throw new ArgumentNullException(null);
}