namespace LibSM64
{
    public enum SM64TerrainType
    {
        Grass  = 0x0000,
        Stone  = 0x0001,
        Snow   = 0x0002,
        Sand   = 0x0003,
        Spooky = 0x0004,
        Water  = 0x0005,
        Slide  = 0x0006,
    }

    public enum SM64SurfaceType
    {
        Default          = 0x0000,// Environment default
        Burning          = 0x0001,// Lava / Frostbite (in SL), but is used mostly for Lava
        Hangable         = 0x0005,// Ceiling that Mario can climb on
        Slow             = 0x0009,// Slow down Mario, unused
        VerySlippery     = 0x0013,// Very slippery, mostly used for slides
        Slippery         = 0x0014,// Slippery
        NotSlippery      = 0x0015,// Non-slippery, climbable
        ShallowQuicksand = 0x0021,// Shallow Quicksand (depth of 10 units)
        DeepQuicksand    = 0x0022,// Quicksand (lethal, slow, depth of 160 units)
        InstantQuicksand = 0x0023,// Quicksand (lethal, instant)
        Ice              = 0x002E,// Slippery Ice, in snow levels and THI's water floor
        Hard             = 0x0030,// Hard floor (Always has fall damage)
        HardSlippery     = 0x0035,// Hard and slippery (Always has fall damage)
        HardVerySlippery = 0x0036,// Hard and very slippery (Always has fall damage)
        HardNotSlippery  = 0x0037,// Hard and Non-slippery (Always has fall damage)
        VerticalWind     = 0x0038,// Death at bottom with vertical wind
    }
}