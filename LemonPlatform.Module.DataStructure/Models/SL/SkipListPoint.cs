﻿namespace LemonPlatform.Module.DataStructure.Models.SL
{
    public class SkipListPoint
    {
        public int X { get; set; }

        public int Y { get; set; }

        public int Key { get; set; }

        public SkipListPoint? Down { get; set; }
    }
}