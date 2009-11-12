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
using openCL;

namespace Test
{
	class AppLoader
	{
		static void Main (string[] args)
		{
			int bytes = 1024;
			uint[] initValues = new uint[bytes / 4];
			uint[] actual = new uint[initValues.Length];
			using (Context context = new Context (DeviceType.GPU))
			using (CommandQueue queue = context.CreateCommandQueue (context.Devices[0], CommandQueueProperties.Default))
			using (Memory mem = context.CreateBuffer (MemoryFlags.ReadWrite, bytes))
			using (Program prog = context.CreateProgram (OpenCL_TestProgram)) {
				prog.Build (new Device[] {context.Devices[0]}, null);
				EventHandle write_event;
				queue.WriteBufferAsync (mem, 0, initValues, 0, bytes, out write_event);
				using (Kernel kernel = prog.CreateKernel ("test")) {
					kernel.SetArgument (0, mem);
					queue.Execute (kernel, null, new int[]{bytes / 2}, new int[] {64}, new EventHandle[] {write_event});
				}
				queue.ReadBuffer (mem, 0, actual, 0, bytes);
			}
			for (int i = 0; i < bytes / 4; i += 128) {
				for (int j = 0; j < 64; j ++)
					if (actual[i + j * 2] != i / 128 || actual[i + j * 2 + 1] != j) {
						Console.WriteLine ("err");
						i = bytes;
						break;
					}
			}
			Console.WriteLine ("cmpl.");
			Console.ReadLine ();
		}

		static string OpenCL_TestProgram =
@"__kernel void test (__global uint *state) {
	size_t group_id = get_group_id (0);
	size_t local_id = get_local_id (0);
	state[get_global_id(0) * 2] = group_id;
	state[get_global_id(0) * 2 + 1] = local_id;
}";
	}
}
