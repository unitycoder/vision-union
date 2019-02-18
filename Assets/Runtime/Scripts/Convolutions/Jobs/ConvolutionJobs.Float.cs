﻿using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using VisionUnion.Organization;

namespace VisionUnion.Jobs
{
    /*
    [BurstCompile]
    public struct ByteWithFloatConvolveJob : IConvolutionJob<float, byte>
    {
        [ReadOnly] public Convolution<float> Convolution;
        [ReadOnly] public ImageData<byte> Input;
        [WriteOnly] public ImageData<float> Output;

        public ByteWithFloatConvolveJob(Convolution<float> convolution, 
            ImageData<byte> input, ImageData<float> output)
        {
            Convolution = convolution;
            Input = input;
            Output = output;
        }

        public void SetConvolution(Convolution<float> convolution)
        {
            Convolution = convolution;
        }

        public void SetData(Convolution<float> convolution, ImageData<byte> input, ImageData<float> output)
        {
            Convolution = convolution;
            Input = input;
            Output = output;
        }

        public void Execute()
        {
            Convolution.Convolve(Input, Output);
        }
    }
    
    [BurstCompile]
    public struct ShortWithFloatConvolveJob : IConvolutionJob<float, short>
    {
        [ReadOnly] public Convolution<float> Convolution;
        [ReadOnly] public ImageData<short> Input;
        [WriteOnly] public ImageData<float> Output;

        public ShortWithFloatConvolveJob(Convolution<float> convolution, 
            ImageData<short> input, ImageData<float> output)
        {
            Convolution = convolution;
            Input = input;
            Output = output;
        }
        
        public void SetConvolution(Convolution<float> convolution)
        {
            Convolution = convolution;
        }

        public void SetData(Convolution<float> convolution, ImageData<short> input, ImageData<float> output)
        {
            Convolution = convolution;
            Input = input;
            Output = output;
        }

        public void Execute()
        {
            Convolution.Convolve(Input, Output);
        }
    }
    */
    
    [BurstCompile]
    // TODO - better names for these jobs!
    public struct FloatWithFloatConvolveJob : IJob
    {
        [ReadOnly] public Convolution2D<float> Convolution;
        [ReadOnly] public Image<float> Input;
        [WriteOnly] public Image<float> Output;

        public FloatWithFloatConvolveJob(Convolution2D<float> convolution, 
            Image<float> input, Image<float> output)
        {
            Convolution = convolution;
            Input = input;
            Output = output;
        }
        
        public void Execute()
        {
            Convolution.Convolve(Input, Output);
        }
    }
}