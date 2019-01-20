﻿using System;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;

// Luma vs "relative luminance" and their RGB->greyscale conversions
// are covered at:  https://en.wikipedia.org/wiki/Luma_(video)
namespace VisionUnion.Jobs
{
    // default weights for relative luminance calculation
    static class LuminanceWeights
    {
        public static Color48 Float
        {
            get { return new Color48(0.2126f, 0.7152f, 0.0722f); }
        }

        public static Color24 Byte
        {
            get
            {
                var red = (byte)(Float.r * 255f);
                var green = (byte)(Float.g * 255f);
                var blue = (byte)(Float.b * 255f);
                return new Color24(red, green, blue);
            }
        }
    }
    
    [BurstCompile]
    public struct GreyscaleByLuminanceFloatJob : IJobParallelFor
    {
        public Color48 Weights;
        
        [ReadOnly] public NativeArray<Color48> InputTexture;
    
        [WriteOnly] public NativeArray<float> Grayscale;

        public GreyscaleByLuminanceFloatJob(NativeArray<Color48> input, 
            NativeArray<float> grayscale, 
            Color48 weights)
        {
            InputTexture = input;
            Grayscale = grayscale;
            Weights = weights;
            if (Weights.Equals(default(Color48)))
                Weights = LuminanceWeights.Float;
        }
        
        public void Execute(int index)
        {
            var p = InputTexture[index];
            Grayscale[index] = p.r * Weights.r + p.g * Weights.g + p.b * Weights.b;
        }
    }

    [BurstCompile]
    public struct GreyscaleLuminanceByteJob : IJobParallelFor
    {
        public Color24 Weights;
        
        [ReadOnly] public NativeArray<Color24> InputTexture;
    
        [WriteOnly] public NativeArray<byte> Grayscale;
        
        public GreyscaleLuminanceByteJob(NativeArray<Color24> input, 
            NativeArray<byte> grayscale, 
            Color24 weights)
        {
            InputTexture = input;
            Grayscale = grayscale;
            Weights = weights;
            if (Weights.Equals(default(Color24)))
                Weights = LuminanceWeights.Byte;
        }

        public void Execute(int index)
        {
            var p = InputTexture[index];
            Grayscale[index] = Convert.ToByte(p.r * Weights.r + p.g * Weights.g + p.b * Weights.b);
        }
    }
}