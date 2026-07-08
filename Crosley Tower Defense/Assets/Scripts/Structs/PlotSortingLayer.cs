using System;

public enum PlotSortingLayer
{
    PlotLayer1,
    PlotLayer2,
    PlotLayer3,
    PlotLayer4,
    PlotLayer5,
    PlotLayer6,
    PlotLayer7,
}

public static class PlotSortingLayerExtensions
{
    public static string ToDisplayName(this PlotSortingLayer layer)
    {
        return layer switch
        {
            PlotSortingLayer.PlotLayer1 => "Plot Layer 1",
            PlotSortingLayer.PlotLayer2 => "Plot Layer 2",
            PlotSortingLayer.PlotLayer3 => "Plot Layer 3",
            PlotSortingLayer.PlotLayer4 => "Plot Layer 4",
            PlotSortingLayer.PlotLayer5 => "Plot Layer 5",
            PlotSortingLayer.PlotLayer6 => "Plot Layer 6",
            PlotSortingLayer.PlotLayer7 => "Plot Layer 7",
            _ => throw new ArgumentOutOfRangeException(nameof(layer), layer, null)
        };
    }
}