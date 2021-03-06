﻿using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;
using VisionUnion.Organization;

namespace VisionUnion
{
    public static class Pad
    {
        public static Image<T> Image<T>(Image<T> input, Padding padding, PadMode mode, 
            T constantValue = default(T), Allocator allocator = Allocator.Persistent)
            where T: struct
        {
            var output = default(Image<T>);
            switch (mode)
            {
                // constant is the only method supported so far
                case PadMode.Constant:
                    output = Constant(input, padding, constantValue, allocator);
                    break;
            }

            return output;
        }
        
        public static Image<TImage> ConvolutionInput<TImage, TConvolution>(Image<TImage> input, 
            ParallelConvolutions<TConvolution> convolutions, 
            ConvolutionPadMode mode = ConvolutionPadMode.Same,
            TImage constantValue = default(TImage), 
            Allocator allocator = Allocator.Persistent)
            where TImage: struct
            where TConvolution : struct
        {
            var output = default(Image<TImage>);
            var firstConvolution = convolutions.Sequences[0].Convolutions[0];
            switch (mode)
            {
                case ConvolutionPadMode.Same:
                    var padding = GetSamePad(input, firstConvolution);
                    output = Constant(input, padding, constantValue, allocator);
                    break;
                case ConvolutionPadMode.Valid:
                    output = Constant(input, new Padding(0), constantValue, allocator);
                    break;
            }

            return output;
        }
        
        public static Image<TImage> ConvolutionInput<TImage, TConvolution>(Image<TImage> input, 
            Convolution2D<TConvolution> convolution, 
            ConvolutionPadMode mode = ConvolutionPadMode.Same,
            TImage constantValue = default(TImage), 
            Allocator allocator = Allocator.Persistent)
            where TImage: struct
            where TConvolution : struct
        {
            var output = default(Image<TImage>);
            switch (mode)
            {
                case ConvolutionPadMode.Same:
                    var padding = GetSamePad(input, convolution);
                    output = Constant(input, padding, constantValue, allocator);
                    break;
                case ConvolutionPadMode.Valid:
                    output = Constant(input, new Padding(0), constantValue, allocator);
                    break;
            }

            return output;
        }

        public static Padding GetSamePad<TInput, TConvolution>(Image<TInput> input, 
            Convolution2D<TConvolution> convolution)
            where TInput: struct
            where TConvolution: struct
        {
            var strides = convolution.Stride;
            var kernel = convolution.Kernel2D;

            var outHeight = (int)math.ceil((float)(input.Height - kernel.Height + 1) / strides.y);
            var outWidth = (int)math.ceil((float)(input.Width - kernel.Width + 1) / strides.x);
            
            var padding = new Padding();
            var hDiff = input.Height - outHeight;
            var wDiff = input.Width - outWidth;
            if (hDiff % 2 == 0)
            {
                var halfDiff = hDiff / 2;
                padding.left += halfDiff;
                padding.right += halfDiff;
            }
            else
            {
                var halfDiff = (int)math.floor(hDiff / 2);
                padding.left += halfDiff;
                padding.right += halfDiff + 1;
            }
            if (wDiff % 2 == 0)
            {
                var halfDiff = wDiff / 2;
                padding.top += halfDiff;
                padding.bottom += halfDiff;
            }
            else
            {
                var halfDiff = (int)math.floor(wDiff / 2);
                padding.top += halfDiff;
                padding.bottom += halfDiff + 1;
            }

            return padding;
        }
        
        public static int GetSizeDifference(int width, int height, Padding pad)
        {
            var newHeight = height + pad.top + pad.bottom;
            var newWidth = width + pad.left + pad.right;
            var newSize = newHeight * newWidth;
            return width * height - newSize;
        }
        
        public static Vector2Int GetNewSize(int width, int height, Padding pad)
        {
            var newHeight = height + pad.top + pad.bottom;
            var newWidth = width + pad.left + pad.right;
            return new Vector2Int(newWidth, newHeight);
        }
        
        public static Image<T> Constant<T>(Image<T> input, Padding pad, T value = default(T), 
            Allocator allocator = Allocator.Persistent)
            where T: struct
        {
            var newWidth = input.Width + pad.left + pad.right;
            var newHeight = input.Height + pad.top + pad.bottom;
           
            var newImage = new Image<T>(newWidth, newHeight, allocator);
            var outBuffer = newImage.Buffer;
            var inBuffer = input.Buffer;

            var inputIndex = 0;
            var outIndex = 0;
            for (var i = 0; i < pad.top; i++)                                     // top pad
            {
                for (var c = 0; c < newImage.Width; c++)
                {
                    outBuffer[outIndex] = value;
                    outIndex++;
                }
            }

            var contentRowEnd = newHeight - pad.bottom;
            for (var r = pad.top; r < contentRowEnd; r++)
            {
                var contentColumnEnd = newWidth - pad.right;
                for (var i = 0; i < pad.left; i++)                               // left pad
                {
                    outBuffer[outIndex] = value;
                    outIndex++;
                }
                for (var i = pad.left; i < contentColumnEnd; i++)                // original content
                {
                    outBuffer[outIndex] = inBuffer[inputIndex];
                    outIndex++;
                    inputIndex++;
                }
                for (var i = contentColumnEnd; i < newWidth; i++)                // right pad
                {
                    outBuffer[outIndex] = value;
                    outIndex++;
                }
            }
            
            for (var r = contentRowEnd; r < newHeight; r++)                      // bottom pad
            {
                for (var c = 0; c < newImage.Width; c++)
                {
                    outBuffer[outIndex] = value;
                    outIndex++;
                }
            }

            return newImage;
        }
        
        public static void Constant<T>(Image<T> input, Image<T> output, 
            Padding pad, T value = default(T))
            where T: struct
        {
            var newWidth = input.Width + pad.left + pad.right;
            var newHeight = input.Height + pad.top + pad.bottom;
            var outBuffer = output.Buffer;
            var inBuffer = input.Buffer;
            var inputIndex = 0;
            var outIndex = 0;
            for (var i = 0; i < pad.top; i++)                                     // top pad
            {
                for (var c = 0; c < output.Width; c++)
                {
                    outBuffer[outIndex] = value;
                    outIndex++;
                }
            }

            var contentRowEnd = newHeight - pad.bottom;
            for (var r = pad.top; r < contentRowEnd; r++)
            {
                var contentColumnEnd = newWidth - pad.right;
                for (var i = 0; i < pad.left; i++)                               // left pad
                {
                    outBuffer[outIndex] = value;
                    outIndex++;
                }
                for (var i = pad.left; i < contentColumnEnd; i++)                // original content
                {
                    outBuffer[outIndex] = inBuffer[inputIndex];
                    outIndex++;
                    inputIndex++;
                }
                for (var i = contentColumnEnd; i < newWidth; i++)                // right pad
                {
                    outBuffer[outIndex] = value;
                    outIndex++;
                }
            }
            
            for (var r = contentRowEnd; r < newHeight; r++)                      // bottom pad
            {
                for (var c = 0; c < output.Width; c++)
                {
                    outBuffer[outIndex] = value;
                    outIndex++;
                }
            }
        }
    }
}