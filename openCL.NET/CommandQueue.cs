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
	public sealed class CommandQueue : HandleBase
	{
		internal CommandQueue (IntPtr handle) : base (handle)
		{
		}

		protected override void Dispose (bool disposing)
		{
			Native.clReleaseCommandQueue (_handle);
		}

		#region ReadBufer
		public void ReadBuffer (Memory buf, int buf_offset, object dst, int dst_offset, int size)
		{
			ReadBuffer (buf, buf_offset, dst, dst_offset, size, null);
		}

		public void ReadBuffer (Memory buf, int buf_offset, object dst, int dst_offset, int size, EventHandle[] wait_list)
		{
			GCHandle handle = GCHandle.Alloc (dst, GCHandleType.Pinned);
			try {
				IntPtr dst_ptr = new IntPtr (handle.AddrOfPinnedObject ().ToInt64 () + dst_offset);
				IntPtr[] waits = wait_list == null ? null : EventHandle.ToIntPtrArray (wait_list);
				uint num_waits = waits == null ? 0 : (uint)waits.Length;
				int errcode = Native.clEnqueueReadBuffer (_handle, buf.Handle, CL_Bool.True, new IntPtr (buf_offset), new IntPtr (size),
					dst_ptr, num_waits, waits, IntPtr.Zero);
				OpenCLException.Check (errcode);
			} finally {
				handle.Free ();
			}
		}

		public void ReadBufferAsync (Memory buf, int buf_offset, object dst, int dst_offset, int size, out EventHandle eventHandle)
		{
			ReadBufferAsync (buf, buf_offset, dst, dst_offset, size, null, out eventHandle);
		}

		public void ReadBufferAsync (Memory buf, int buf_offset, object dst, int dst_offset, int size, EventHandle[] wait_list, out EventHandle eventHandle)
		{
			GCHandle handle = GCHandle.Alloc (dst, GCHandleType.Pinned);
			try {
				IntPtr dst_ptr = new IntPtr (handle.AddrOfPinnedObject ().ToInt64 () + dst_offset);
				IntPtr[] waits = EventHandle.ToIntPtrArray (wait_list);
				uint num_waits = waits == null ? 0 : (uint)waits.Length;
				IntPtr event_handle;
				int errcode = Native.clEnqueueReadBuffer (_handle, buf.Handle, CL_Bool.False, new IntPtr (buf_offset), new IntPtr (size),
					dst_ptr, num_waits, waits, out event_handle);
				OpenCLException.Check (errcode);
				eventHandle = new EventHandle (event_handle);
			} finally {
				handle.Free ();
			}
		}
		#endregion

		#region WriteBufer
		public void WriteBufer (Memory buf, int buf_offset, object src, int src_offset, int size)
		{
			WriteBufer (buf, buf_offset, src, src_offset, size, null);
		}

		public void WriteBufer (Memory buf, int buf_offset, object src, int src_offset, int size, EventHandle[] wait_list)
		{
			GCHandle handle = GCHandle.Alloc (src, GCHandleType.Pinned);
			try {
				IntPtr src_ptr = new IntPtr (handle.AddrOfPinnedObject ().ToInt64 () + src_offset);
				IntPtr[] waits = wait_list == null ? null : EventHandle.ToIntPtrArray (wait_list);
				uint num_waits = waits == null ? 0 : (uint)waits.Length;
				int errcode = Native.clEnqueueWriteBuffer (_handle, buf.Handle, CL_Bool.True, new IntPtr (buf_offset), new IntPtr (size),
					src_ptr, num_waits, waits, IntPtr.Zero);
				OpenCLException.Check (errcode);
			} finally {
				handle.Free ();
			}
		}

		public void WriteBufferAsync (Memory buf, int buf_offset, object src, int src_offset, int size, out EventHandle eventHandle)
		{
			WriteBufferAsync (buf, buf_offset, src, src_offset, size, null, out eventHandle);
		}

		public void WriteBufferAsync (Memory buf, int buf_offset, object src, int src_offset, int size, EventHandle[] wait_list, out EventHandle eventHandle)
		{
			GCHandle handle = GCHandle.Alloc (src, GCHandleType.Pinned);
			try {
				IntPtr src_ptr = new IntPtr (handle.AddrOfPinnedObject ().ToInt64 () + src_offset);
				IntPtr[] waits = EventHandle.ToIntPtrArray (wait_list);
				uint num_waits = waits == null ? 0 : (uint)waits.Length;
				IntPtr event_handle;
				int errcode = Native.clEnqueueWriteBuffer (_handle, buf.Handle, CL_Bool.False, new IntPtr (buf_offset), new IntPtr (size),
					src_ptr, num_waits, waits, out event_handle);
				OpenCLException.Check (errcode);
				eventHandle = new EventHandle (event_handle);
			} finally {
				handle.Free ();
			}
		}
		#endregion

		#region Execute
		public void Execute (Kernel kernel)
		{
			Execute (kernel, null);
		}

		public void Execute (Kernel kernel, int[] global_work_offset, int[] global_work_size, int[] local_work_size)
		{
			Execute (kernel, global_work_offset, global_work_size, local_work_size, null);
		}

		public void Execute (Kernel kernel, EventHandle[] wait_list)
		{
			EventHandle eventHandle;
			ExecuteAsync (kernel, wait_list, out eventHandle);
			eventHandle.WaitOne ();
		}

		public void Execute (Kernel kernel, int[] global_work_offset, int[] global_work_size, int[] local_work_size, EventHandle[] wait_list)
		{
			EventHandle eventHandle;
			ExecuteAsync (kernel, global_work_offset, global_work_size, local_work_size, wait_list, out eventHandle);
			eventHandle.WaitOne ();
		}

		public void ExecuteAsync (Kernel kernel, out EventHandle eventHandle)
		{
			ExecuteAsync (kernel, null, out eventHandle);
		}

		public void ExecuteAsync (Kernel kernel, int[] global_work_offset, int[] global_work_size, int[] local_work_size, out EventHandle eventHandle)
		{
			ExecuteAsync (kernel, global_work_offset, global_work_size, local_work_size, null, out eventHandle);
		}

		public void ExecuteAsync (Kernel kernel, EventHandle[] wait_list, out EventHandle eventHandle)
		{
			IntPtr[] waits = wait_list == null ? null : EventHandle.ToIntPtrArray (wait_list);
			uint num_waits = waits == null ? 0 : (uint)waits.Length;
			IntPtr event_handle;
			OpenCLException.Check (Native.clEnqueueTask (_handle, kernel.Handle, num_waits, waits, out event_handle));
			eventHandle = new EventHandle (event_handle);
		}

		public void ExecuteAsync (Kernel kernel, int[] global_work_offset, int[] global_work_size, int[] local_work_size, EventHandle[] wait_list, out EventHandle eventHandle)
		{
			if (global_work_size == null || local_work_size == null || global_work_size.Length != local_work_size.Length)
				throw new ArgumentException ();

			uint work_dim = (uint)global_work_size.Length;
			IntPtr[] goffsets = null;
			IntPtr[] gthreads = new IntPtr[work_dim];
			IntPtr[] lthreads = new IntPtr[work_dim];
			for (int i = 0; i < work_dim; i ++) {
				gthreads[i] = new IntPtr (global_work_size[i]);
				lthreads[i] = new IntPtr (local_work_size[i]);
			}

			IntPtr[] waits = wait_list == null ? null : EventHandle.ToIntPtrArray (wait_list);
			uint num_waits = waits == null ? 0 : (uint)waits.Length;
			IntPtr event_handle;
			OpenCLException.Check (Native.clEnqueueNDRangeKernel (_handle, kernel.Handle, work_dim, goffsets, gthreads, lthreads, num_waits, waits, out event_handle));
			eventHandle = new EventHandle (event_handle);
		}
		#endregion
	}
}
