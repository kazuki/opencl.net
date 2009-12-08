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
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace openCL
{
	public class Kernel : HandleBase
	{
		Program _prog;
		List<GCHandle> _pinnedObjects = new List<GCHandle> ();

		internal Kernel (Program prog, IntPtr handle) : base (handle)
		{
			_prog = prog;
		}

		protected override void Dispose (bool disposing)
		{
			for (int i = 0; i < _pinnedObjects.Count; i ++)
				_pinnedObjects[i].Free ();
			_pinnedObjects.Clear ();
			Native.clReleaseKernel (_handle);
		}

		public void SetArgument (uint arg_index, Memory mem)
		{
			SetArgument (arg_index, mem.Handle, IntPtr.Size);
		}

		public void SetArgument (uint arg_index, object value, int size)
		{
			GCHandle handle = GCHandle.Alloc (value, GCHandleType.Pinned);
			_pinnedObjects.Add (handle);
			OpenCLException.Check (Native.clSetKernelArg (_handle, arg_index, new IntPtr (size), handle.AddrOfPinnedObject ()));
		}

		public void SetLocalDataShare (uint arg_index, int size)
		{
			OpenCLException.Check (Native.clSetKernelArg (_handle, arg_index, new IntPtr (size), IntPtr.Zero));
		}

		public string FunctionName {
			get { return Native.QueryInfoString (QueryType.Kernel, _handle, KernelInfo.FunctionName); }
		}

		public uint NumberOfArguments {
			get { return Native.QueryInfoUInt32 (QueryType.Kernel, _handle, KernelInfo.NumArgs); }
		}

		public uint ReferenceCount {
			get { return Native.QueryInfoUInt32 (QueryType.Kernel, _handle, KernelInfo.ReferenceCount); }
		}

		public Context Context {
			get { return _prog.Context; }
		}

		public Program Program {
			get { return _prog; }
		}

		public int GetWorkGroupSize (Device device)
		{
			return Native.QueryInfoSize (QueryType.KernelWorkGroup, _handle, device.Handle, KernelWorkGroupInfo.WorkGroupSize).ToInt32 ();
		}

		public int[] GetCompileWorkGroupSize (Device device)
		{
			IntPtr[] temp = Native.QueryInfoIntPtrArray (QueryType.KernelWorkGroup, _handle, device.Handle, KernelWorkGroupInfo.CompileWorkGroupSize);
			int[] ret = new int[temp.Length];
			for (int i = 0; i < temp.Length; i ++)
				ret[i] = temp[i].ToInt32 ();
			return ret;
		}

		public ulong GetLocalMemorySize (Device device)
		{
			return Native.QueryInfoUInt64 (QueryType.KernelWorkGroup, _handle, device.Handle, KernelWorkGroupInfo.LocalMemSize);
		}
	}
}
