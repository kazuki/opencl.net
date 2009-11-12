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

namespace openCL
{
	public class Program : HandleBase
	{
		public Program (IntPtr handle) : base (handle)
		{
		}

		protected override void Dispose (bool disposing)
		{
			Native.clReleaseProgram (_handle);
		}

		public void Build (Device[] devices, string options)
		{
			IntPtr[] device_list = new IntPtr[devices.Length];
			for (int i = 0; i < device_list.Length; i++)
				device_list[i] = devices[i].Handle;
			int errcode = Native.clBuildProgram (_handle,
				(uint)devices.Length, device_list, options, IntPtr.Zero, IntPtr.Zero);
			OpenCLException.Check (errcode);
		}

		public Kernel CreateKernel (string name)
		{
			int errcode;
			IntPtr kernel = Native.clCreateKernel (_handle, name, out errcode);
			OpenCLException.Check (errcode);
			return new Kernel (kernel);
		}
	}
}
