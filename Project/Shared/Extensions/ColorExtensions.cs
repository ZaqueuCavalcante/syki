namespace Exato.Shared.Extensions;

public static class ColorExtensions
{
    private static readonly List<string> Colors =
    [
        "#E6194B","#3CB44B","#FFE119","#4363D8","#F58231",
        "#911EB4","#46F0F0","#F032E6","#BCF60C","#FABEBE",
        "#008080","#E6BEFF","#9A6324","#FFFAC8","#800000",
        "#AAFFC3","#808000","#FFD8B1","#000075","#808080",
        "#A9A9A9","#FF69B4","#B0E0E6","#7FFFD4","#D2691E",
        "#6495ED","#DC143C","#008000","#00CED1","#FFD700",
        "#4B0082","#ADFF2F","#FF4500","#2E8B57","#DA70D6",
        "#40E0D0","#9932CC","#FF1493","#7FFF00","#FF8C00",
        "#1E90FF","#C71585","#00FA9A","#FF6347","#8B0000",
        "#00BFFF","#696969","#CD5C5C","#20B2AA","#87CEEB"
    ];

    public static string PickColor(int index)
    {
        if (index > Colors.Count) return Colors.PickRandom();

        return Colors[index];
    }
}
