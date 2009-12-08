// 
// Copyright (c) 2009, Kazuki Oikawa
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//

using System;
using System.Diagnostics;
using openCL;

namespace Test
{
	class AppLoader
	{
		static void Main (string[] args)
		{
			Platform[] platforms = Platform.GetPlatforms ();
			for (int i = 0; i < platforms.Length; i++) {
				Console.WriteLine ("Platform={0}", i);
				Console.WriteLine ("  Name       = {0}", platforms[i].Name);
				Console.WriteLine ("  Vendor     = {0}", platforms[i].Vendor);
				Console.WriteLine ("  Version    = {0}", platforms[i].Version);
				Console.WriteLine ("  Profile    = {0}", platforms[i].Profile);
				Console.WriteLine ("  Extensions = {0}", platforms[i].Extensions);
				Device[] devices = platforms[0].GetDevices (DeviceType.All);
				for (int j = 0; j < devices.Length; j++) {
					Console.WriteLine ("  Device={0}", j);
					Console.WriteLine ("    Name   = {0}", devices[j].Name);
					Console.WriteLine ("    Vendor = {0}", devices[j].Vendor);
					Console.WriteLine ("    MaxComputeUnits = {0}", devices[j].MaxComputeUnits);
				}
			}
			
			int blockSize = 16;
			int matrixSize = 1024;
			int matrixBytes = matrixSize * matrixSize * 4;
			float[] matrixA = new float[matrixSize * matrixSize];
			float[] matrixB = new float[matrixSize * matrixSize];
			float[] matrixR = new float[matrixSize * matrixSize];
			float[] matrixCPU = new float[matrixSize * matrixSize];

			int[] global_threads = new int[] {matrixSize, matrixSize};
			int[] local_threads = new int[] {blockSize, blockSize};
			int blockCacheBytes = blockSize * blockSize * 4;

			Random rnd = new Random ();
			for (int i = 0; i < matrixA.Length; i ++) {
				matrixA[i] = (float)rnd.NextDouble ();
				matrixB[i] = (float)rnd.NextDouble ();
			}

			Stopwatch sw;
			Device device;
			using (Context context = new Context (DeviceType.GPU))
			using (CommandQueue queue = context.CreateCommandQueue (device = context.Devices[0], CommandQueueProperties.Default))
			using (Memory memA = context.CreateBuffer (MemoryFlags.ReadOnly, matrixBytes))
			using (Memory memB = context.CreateBuffer (MemoryFlags.ReadOnly, matrixBytes))
			using (Memory memC = context.CreateBuffer (MemoryFlags.WriteOnly, matrixBytes))
			using (Program prog = context.CreateProgram (OpenCL_TestProgram, device, null))
			using (Kernel kernel = prog.CreateKernel ("multMatrix")) {
				kernel.SetArgument (0, memA);
				kernel.SetArgument (1, memB);
				kernel.SetArgument (2, memC);
				kernel.SetLocalDataShare (3, blockCacheBytes);
				kernel.SetLocalDataShare (4, blockCacheBytes);
				kernel.SetArgument (5, matrixSize, 4);
				kernel.SetArgument (6, blockSize, 4);

				queue.WriteBuffer (memA, 0, matrixA, 0, matrixBytes);
				queue.WriteBuffer (memB, 0, matrixB, 0, matrixBytes);
				
				sw = Stopwatch.StartNew ();
				queue.Execute (kernel, null, global_threads, local_threads);
				sw.Stop ();
				Console.WriteLine ("GPU: {0:f4}ms", sw.Elapsed.TotalMilliseconds);

				queue.ReadBuffer (memC, 0,  matrixR, 0, matrixBytes);
			}

			sw = Stopwatch.StartNew ();
			for (int i = 0; i < matrixSize; i ++) {
				for (int j = 0; j < matrixSize; j ++) {
					for (int k = 0; k < matrixSize; k ++)
						matrixCPU[i * matrixSize + j] += matrixA[i * matrixSize + k] * matrixB[k * matrixSize + j];
				}
			}
			sw.Stop ();
			Console.WriteLine ("CPU: {0:f4}ms", sw.Elapsed.TotalMilliseconds);

			for (int i = 0; i < matrixR.Length; i ++) {
				if (matrixR[i] != matrixCPU[i]) {
					Console.WriteLine ("Error");
					return;
				}
			}
			Console.WriteLine ("OK");
		}

		static string OpenCL_TestProgram =
@"__kernel void multMatrix (__global float *a, __global float *b, __global float *c,
                            __local float *local_a, __local float *local_b, const int width, const int blockSize) {
	int bx = get_group_id(0);
	int by = get_group_id(1);
	int tx = get_local_id(0); 
	int ty = get_local_id(1);
	int n = width / blockSize;
	float sum = 0;

	for (int i = 0; i < n; i++) {
		int globalIdx0 = i  * blockSize + tx;
		int globalIdy0 = by * blockSize + ty;
		int globalId0  = globalIdy0 * width + globalIdx0;
		int globalIdx1 = bx * blockSize + tx;
		int globalIdy1 = i  * blockSize + ty;
		int globalId1  = globalIdy1 * width + globalIdx1;
		local_a[ty * blockSize + tx] = a[globalId0];
		local_b[ty * blockSize + tx] = b[globalId1];
		barrier(CLK_LOCAL_MEM_FENCE);

		for (int k = 0; k < blockSize; k++)
			sum += local_a[ty * blockSize + k] * local_b[k * blockSize + tx];
		barrier(CLK_LOCAL_MEM_FENCE);
	}

	int x  = get_global_id(0);
	int y  = get_global_id(1);
	int outIndex = y * width + x;
	c[outIndex] = sum;
}";
	}
}
