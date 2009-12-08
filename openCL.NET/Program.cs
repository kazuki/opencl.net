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
using System.Runtime.InteropServices;

namespace openCL
{
	public class Program : HandleBase
	{
		Context _context;

		internal Program (Context context, IntPtr handle) : base (handle)
		{
			_context = context;
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
			return new Kernel (this, kernel);
		}

		public string GetBuildLog (Device device)
		{
			return Native.QueryInfoString (QueryType.ProgramBuild, _handle, device.Handle, ProgramBuildInfo.Log);
		}

		public string GetBuildOptions (Device device)
		{
			return Native.QueryInfoString (QueryType.ProgramBuild, _handle, device.Handle, ProgramBuildInfo.Options);
		}

		public BuildStatus GetBuildStatus (Device device)
		{
			return (BuildStatus)Native.QueryInfoInt32 (QueryType.ProgramBuild, _handle, device.Handle, ProgramBuildInfo.Status);
		}

		public uint ReferenceCount {
			get { return Native.QueryInfoUInt32 (QueryType.Program, _handle, ProgramInfo.ReferenceCount); }
		}

		public Context Context {
			get { return _context; }
		}

		public uint NumberOfDevices {
			get { return Native.QueryInfoUInt32 (QueryType.Program, _handle, ProgramInfo.NumDevices); }
		}

		public string ProgramSource {
			get { return Native.QueryInfoString (QueryType.Program, _handle, ProgramInfo.Source); }
		}

		public int[] BinarySizes {
			get {
				IntPtr[] sizes = Native.QueryInfoIntPtrArray (QueryType.Program, _handle, ProgramInfo.BinarySizes);
				int[] ret = new int[sizes.Length];
				for (int i = 0; i < sizes.Length; i ++)
					ret[i] = sizes[i].ToInt32 ();
				return ret;
			}
		}

		public byte[][] GetBinaries ()
		{
			int[] sizes = BinarySizes;
			byte[][] binaries = new byte[sizes.Length][];
			GCHandle[] handles = new GCHandle[sizes.Length];
			byte[] ptrs = new byte[IntPtr.Size * sizes.Length];
			for (int i = 0; i < binaries.Length; i ++) {
				binaries[i] = new byte[sizes[i]];
				handles[i] = GCHandle.Alloc (binaries[i], GCHandleType.Pinned);
				byte[] raw = Native.FromIntPtr (handles[i].AddrOfPinnedObject ());
				Array.Copy (raw, 0, ptrs, i * IntPtr.Size, IntPtr.Size);
			}
			try {
				Native.QueryInfoDirect (QueryType.Program, ptrs, _handle, ProgramInfo.Binaries);
			} finally {
				for (int i = 0; i < handles.Length; i ++)
					handles[i].Free ();
			}

			using (System.IO.FileStream strm = new System.IO.FileStream ("test.bc", System.IO.FileMode.Create)) {
				strm.Write (binaries[0], 0, binaries[0].Length);
			}

			return binaries;
		}
	}
}
