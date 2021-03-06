﻿using NUnit.Framework;
using UnityEngine;

namespace VisionUnion.Tests
{
	public class PadTests
	{
		[TestCaseSource(typeof(GetSamePadCases), "Uniform")]
		public void GetSamePadCases<T>(Image<byte> input, Convolution2D<T> convolution, Padding expected)
			where T: struct
		{
			var output = Pad.GetSamePad(input, convolution);
			Debug.Log(output.ToString());
			Assert.IsTrue(expected.Equals(output));
		}
		
		[TestCaseSource(typeof(OutputPadImages), "ConstantCases")]
		public void ZeroPaddingCases(Image<byte> input, Padding pad, Image<byte> expected)
		{
			var output = Pad.Constant(input, pad);
			output.Print();
			
			AssertPadValuesAtBounds(output, pad, 0);
			output.Buffer.AssertDeepEqual(expected.Buffer);
			output.Dispose();
		}
		
		[TestCaseSource(typeof(OutputPadImages), "ConstantCases")]
		public void ConstantPaddingCases(Image<byte> input, Padding pad, Image<byte> expected)
		{
			const byte value = 7;
			var output = Pad.Constant(input, pad, value);
			output.Print();
			
			AssertPadValuesAtBounds(output, pad, value);
			output.Dispose();
		}

		[TestCaseSource(typeof(PadConvolutionInputCases), "Uniform")]
		public void ConvolveWithSameZeroPad<T>(Image<byte> input, Convolution2D<T> convolution, 
			Image<byte> expected)
			where T: struct
		{
			var paddedInput = Pad.ConvolutionInput(input, convolution);
			expected.Buffer.AssertDeepEqual(paddedInput.Buffer);
		}

		static void AssertPadValuesAtBounds(Image<byte> output, Padding pad, byte value)
		{
			if (pad.top > 1 || pad.left > 1)
			{
				var firstValue = output.Buffer[0];
				Assert.AreEqual(value, firstValue);
			}

			if (pad.bottom <= 1 && pad.right <= 1) 
				return;
			
			var lastValue = output.Buffer[output.Buffer.Length - 1];
			Assert.AreEqual(value, lastValue);
		}
	}
}