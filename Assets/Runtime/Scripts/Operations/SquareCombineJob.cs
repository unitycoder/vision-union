﻿using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

namespace VisionUnion.Jobs
{
    [BurstCompile]
    public struct SquareCombineJob : IJobParallelFor
    {
        [ReadOnly] public Image<float> A;
        [ReadOnly] public Image<float> B;
        [WriteOnly] public Image<float> Output;

        public void Execute(int index)
        {
            var x = A.Buffer[index];
            var y = B.Buffer[index];
            Output.Buffer[index] = math.sqrt(x * x + y * y);
        }
    }
}