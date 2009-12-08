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
		Context _context;
		Device _device;

		internal CommandQueue (Context context, Device device, IntPtr handle) : base (handle)
		{
			_context = context;
			_device = device;
		}

		protected override void Dispose (bool disposing)
		{
			Native.clReleaseCommandQueue (_handle);
		}

		#region ReadBufer
		public void ReadBuffer (Memory buf, int buf_offset, object dst, int dst_offset, int size)
		{
			ReadBufferInternal (true, buf, buf_offset, dst, dst_offset, size, null);
		}

		public void ReadBuffer (Memory buf, int buf_offset, object dst, int dst_offset, int size, EventHandle[] wait_list)
		{
			ReadBufferInternal (true, buf, buf_offset, dst, dst_offset, size, wait_list);
		}

		public void ReadBufferAsync (Memory buf, int buf_offset, object dst, int dst_offset, int size)
		{
			ReadBufferInternal (false, buf, buf_offset, dst, dst_offset, size, null);
		}

		public void ReadBufferAsync (Memory buf, int buf_offset, object dst, int dst_offset, int size, EventHandle[] wait_list)
		{
			ReadBufferInternal (false, buf, buf_offset, dst, dst_offset, size, wait_list);
		}

		public void ReadBufferAsync (Memory buf, int buf_offset, object dst, int dst_offset, int size, EventHandle[] wait_list, out EventHandle eventHandle)
		{
			ReadBufferInternal (false, true, buf, buf_offset, dst, dst_offset, size, wait_list, out eventHandle);
		}

		void ReadBufferInternal (bool blocking, Memory buf, int buf_offset, object dst, int dst_offset, int size, EventHandle[] wait_list)
		{
			EventHandle eventHandle;
			ReadBufferInternal (blocking, false, buf, buf_offset, dst, dst_offset, size, wait_list, out eventHandle);
		}

		void ReadBufferInternal (bool blocking, bool outputEventHandle, Memory buf, int buf_offset, object dst, int dst_offset, int size, EventHandle[] wait_list, out EventHandle eventHandle)
		{
			IntPtr dst_handle;
			GCHandle dst_gc_handle;
			if (dst is IntPtr) {
				dst_handle = (IntPtr)dst;
				dst_gc_handle = new GCHandle ();
			} else {
				dst_gc_handle = GCHandle.Alloc (dst, GCHandleType.Pinned);
				dst_handle = dst_gc_handle.AddrOfPinnedObject ();
			}

			try {
				IntPtr dst_ptr = new IntPtr (dst_handle.ToInt64 () + dst_offset);
				IntPtr[] waits = EventHandle.ToIntPtrArray (wait_list);
				uint num_waits = waits == null ? 0 : (uint)waits.Length;
				CL_Bool is_blocking = (blocking ? CL_Bool.True : CL_Bool.False);
				if (outputEventHandle) {
					IntPtr event_handle;
					OpenCLException.Check (Native.clEnqueueReadBuffer (_handle, buf.Handle, is_blocking, new IntPtr (buf_offset), new IntPtr (size),
						dst_ptr, num_waits, waits, out event_handle));
					eventHandle = new EventHandle (event_handle);
				} else {
					OpenCLException.Check (Native.clEnqueueReadBuffer (_handle, buf.Handle, is_blocking, new IntPtr (buf_offset), new IntPtr (size),
						dst_ptr, num_waits, waits, IntPtr.Zero));
					eventHandle = null;
				}
			} finally {
				if (dst_gc_handle.IsAllocated)
					dst_gc_handle.Free ();
			}
		}
		#endregion

		#region WriteBufer
		public void WriteBuffer (Memory buf, int buf_offset, object src, int src_offset, int size)
		{
			WriteBufferInternal (true, buf, buf_offset, src, src_offset, size, null);
		}

		public void WriteBuffer (Memory buf, int buf_offset, object src, int src_offset, int size, EventHandle[] wait_list)
		{
			WriteBufferInternal (true, buf, buf_offset, src, src_offset, size, wait_list);
		}

		public void WriteBufferAsync (Memory buf, int buf_offset, object src, int src_offset, int size)
		{
			WriteBufferInternal (false, buf, buf_offset, src, src_offset, size, null);
		}

		public void WriteBufferAsync (Memory buf, int buf_offset, object src, int src_offset, int size, EventHandle[] wait_list)
		{
			WriteBufferInternal (false, buf, buf_offset, src, src_offset, size, wait_list);
		}

		public void WriteBufferAsync (Memory buf, int buf_offset, object src, int src_offset, int size, EventHandle[] wait_list, out EventHandle eventHandle)
		{
			WriteBufferInternal (false, true, buf, buf_offset, src, src_offset, size, null, out eventHandle);
		}

		void WriteBufferInternal (bool blocking, Memory buf, int buf_offset, object src, int src_offset, int size, EventHandle[] wait_list)
		{
			EventHandle eventHandle;
			WriteBufferInternal (blocking, false, buf, buf_offset, src, src_offset, size, wait_list, out eventHandle);
		}

		void WriteBufferInternal (bool blocking, bool outputEventHandle, Memory buf, int buf_offset, object src, int src_offset, int size, EventHandle[] wait_list, out EventHandle eventHandle)
		{
			IntPtr src_handle;
			GCHandle src_gc_handle;
			if (src is IntPtr) {
				src_handle = (IntPtr)src;
				src_gc_handle = new GCHandle ();
			} else {
				src_gc_handle = GCHandle.Alloc (src, GCHandleType.Pinned);
				src_handle = src_gc_handle.AddrOfPinnedObject ();
			}

			try {
				IntPtr src_ptr = new IntPtr (src_handle.ToInt64 () + src_offset);
				IntPtr[] waits = EventHandle.ToIntPtrArray (wait_list);
				uint num_waits = waits == null ? 0 : (uint)waits.Length;
				CL_Bool is_blocking = (blocking ? CL_Bool.True : CL_Bool.False);
				if (outputEventHandle) {
					IntPtr event_handle;
					OpenCLException.Check (Native.clEnqueueWriteBuffer (_handle, buf.Handle, is_blocking, new IntPtr (buf_offset), new IntPtr (size),
						src_ptr, num_waits, waits, out event_handle));
					eventHandle = new EventHandle (event_handle);
				} else {
					OpenCLException.Check (Native.clEnqueueWriteBuffer (_handle, buf.Handle, is_blocking, new IntPtr (buf_offset), new IntPtr (size),
						src_ptr, num_waits, waits, IntPtr.Zero));
					eventHandle = null;
				}
			} finally {
				if (src_gc_handle.IsAllocated)
					src_gc_handle.Free ();
			}
		}
		#endregion

		#region CopyBufer
		public void CopyBufer (Memory src, int src_offset, Memory dst, int dst_offset, int size)
		{
			CopyBufer (src, src_offset, dst, dst_offset, size, null);
		}

		public void CopyBufer (Memory src, int src_offset, Memory dst, int dst_offset, int size, EventHandle[] wait_list)
		{
			EventHandle wait;
			CopyBuferInternal (true, src, src_offset, dst, dst_offset, size, wait_list, out wait);
			wait.WaitOne ();
		}

		public void CopyBuferAsync (Memory src, int src_offset, Memory dst, int dst_offset, int size, EventHandle[] wait_list)
		{
			EventHandle dummy;
			CopyBuferInternal (false, src, src_offset, dst, dst_offset, size, wait_list, out dummy);
		}

		public void CopyBuferAsync (Memory src, int src_offset, Memory dst, int dst_offset, int size, EventHandle[] wait_list, out EventHandle eventHandle)
		{
			CopyBuferInternal (true, src, src_offset, dst, dst_offset, size, wait_list, out eventHandle);
		}

		void CopyBuferInternal (bool outputEventHandle, Memory src, int src_offset, Memory dst, int dst_offset, int size, EventHandle[] wait_list, out EventHandle eventHandle)
		{
			IntPtr[] waits = EventHandle.ToIntPtrArray (wait_list);
			uint num_waits = waits == null ? 0 : (uint)waits.Length;
			if (outputEventHandle) {
				IntPtr event_handle;
				OpenCLException.Check (Native.clEnqueueCopyBuffer (_handle, src.Handle, dst.Handle, new IntPtr (src_offset), new IntPtr (dst_offset),
					new IntPtr (size), num_waits, waits, out event_handle));
				eventHandle = new EventHandle (event_handle);
			} else {
				OpenCLException.Check (Native.clEnqueueCopyBuffer (_handle, src.Handle, dst.Handle, new IntPtr (src_offset), new IntPtr (dst_offset),
					new IntPtr (size), num_waits, waits, IntPtr.Zero));
				eventHandle = null;
			}
		}
		#endregion

		#region Mapping / Unmapping
		public IntPtr Mapping (Memory buffer, MapFlags flags, int offset, int cb)
		{
			return MappingAsync (true, buffer, flags, offset, cb);
		}

		public IntPtr MappingAsync (Memory buffer, MapFlags flags, int offset, int cb)
		{
			return MappingAsync (false, buffer, flags, offset, cb);
		}

		IntPtr MappingAsync (bool blocking, Memory buffer, MapFlags flags, int offset, int cb)
		{
			int errcode;
			IntPtr ptr = Native.clEnqueueMapBuffer (_handle, buffer.Handle, blocking ? CL_Bool.True : CL_Bool.False,
				flags, new IntPtr (offset), new IntPtr (cb), 0, null, IntPtr.Zero, out errcode);
			OpenCLException.Check (errcode);
			return ptr;
		}

		public void Unmapping (Memory buffer, IntPtr mapped)
		{
			OpenCLException.Check (Native.clEnqueueUnmapMemObject (_handle, buffer.Handle, mapped, 0, null, IntPtr.Zero));
		}
		#endregion

		#region Execute
		public void Execute (Kernel kernel)
		{
			Execute (kernel, null);
		}

		public void Execute (Kernel kernel, int global_work_offset, int global_work_size, int local_work_size)
		{
			Execute (kernel, global_work_offset, global_work_size, local_work_size, null);
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

		public void Execute (Kernel kernel, int global_work_offset, int global_work_size, int local_work_size, EventHandle[] wait_list)
		{
			EventHandle eventHandle;
			ExecuteAsync (kernel, global_work_offset, global_work_size, local_work_size, wait_list, out eventHandle);
			eventHandle.WaitOne ();
		}

		public void Execute (Kernel kernel, int[] global_work_offset, int[] global_work_size, int[] local_work_size, EventHandle[] wait_list)
		{
			EventHandle eventHandle;
			ExecuteAsync (kernel, global_work_offset, global_work_size, local_work_size, wait_list, out eventHandle);
			eventHandle.WaitOne ();
		}

		public void ExecuteAsync (Kernel kernel)
		{
			ExecuteAsync (kernel, null);
		}

		public void ExecuteAsync (Kernel kernel, EventHandle[] wait_list)
		{
			EventHandle dummy;
			ExecuteInternal (false, kernel, wait_list, out dummy);
		}

		public void ExecuteAsync (Kernel kernel, EventHandle[] wait_list, out EventHandle eventHandle)
		{
			ExecuteInternal (true, kernel, wait_list, out eventHandle);
		}

		public void ExecuteAsync (Kernel kernel, int global_work_offset, int global_work_size, int local_work_size)
		{
			ExecuteAsync (kernel, global_work_offset, global_work_size, local_work_size, null);
		}

		public void ExecuteAsync (Kernel kernel, int[] global_work_offset, int[] global_work_size, int[] local_work_size)
		{
			ExecuteAsync (kernel, global_work_offset, global_work_size, local_work_size, null);
		}

		public void ExecuteAsync (Kernel kernel, int global_work_offset, int global_work_size, int local_work_size, EventHandle[] wait_list)
		{
			ExecuteAsync (kernel, new int[] {global_work_offset}, new int[] {global_work_size}, new int[] {local_work_size}, wait_list);
		}

		public void ExecuteAsync (Kernel kernel, int[] global_work_offset, int[] global_work_size, int[] local_work_size, EventHandle[] wait_list)
		{
			EventHandle dummy;
			ExecuteInternal (false, kernel, global_work_offset, global_work_size, local_work_size, wait_list, out dummy);
		}

		public void ExecuteAsync (Kernel kernel, int global_work_offset, int global_work_size, int local_work_size, EventHandle[] wait_list, out EventHandle eventHandle)
		{
			ExecuteAsync (kernel, new int[] {global_work_offset}, new int[] {global_work_size}, new int[] {local_work_size}, wait_list, out eventHandle);
		}
		
		public void ExecuteAsync (Kernel kernel, int[] global_work_offset, int[] global_work_size, int[] local_work_size, EventHandle[] wait_list, out EventHandle eventHandle)
		{
			ExecuteInternal (true, kernel, global_work_offset, global_work_size, local_work_size, wait_list, out eventHandle);
		}

		void ExecuteInternal (bool outputEventHandle, Kernel kernel, EventHandle[] wait_list, out EventHandle eventHandle)
		{
			IntPtr[] waits = wait_list == null ? null : EventHandle.ToIntPtrArray (wait_list);
			uint num_waits = waits == null ? 0 : (uint)waits.Length;
			if (outputEventHandle) {
				IntPtr event_handle;
				OpenCLException.Check (Native.clEnqueueTask (_handle, kernel.Handle, num_waits, waits, out event_handle));
				eventHandle = new EventHandle (event_handle);
			} else {
				OpenCLException.Check (Native.clEnqueueTask (_handle, kernel.Handle, num_waits, waits, IntPtr.Zero));
				eventHandle = null;
			}
		}

		void ExecuteInternal (bool outputEventHandle, Kernel kernel, int[] global_work_offset, int[] global_work_size, int[] local_work_size, EventHandle[] wait_list, out EventHandle eventHandle)
		{
			if (global_work_size == null || local_work_size == null || global_work_size.Length != local_work_size.Length)
				throw new ArgumentException ();

			uint work_dim = (uint)global_work_size.Length;
			IntPtr[] goffsets = null;
			IntPtr[] gthreads = new IntPtr[work_dim];
			IntPtr[] lthreads = new IntPtr[work_dim];
			for (int i = 0; i < work_dim; i++) {
				gthreads[i] = new IntPtr (global_work_size[i]);
				lthreads[i] = new IntPtr (local_work_size[i]);
			}

			IntPtr[] waits = wait_list == null ? null : EventHandle.ToIntPtrArray (wait_list);
			uint num_waits = waits == null ? 0 : (uint)waits.Length;
			if (outputEventHandle) {
				IntPtr event_handle;
				OpenCLException.Check (Native.clEnqueueNDRangeKernel (_handle, kernel.Handle, work_dim, goffsets, gthreads, lthreads, num_waits, waits, out event_handle));
				eventHandle = new EventHandle (event_handle);
			} else {
				OpenCLException.Check (Native.clEnqueueNDRangeKernel (_handle, kernel.Handle, work_dim, goffsets, gthreads, lthreads, num_waits, waits, IntPtr.Zero));
				eventHandle = null;
			}
		}
		#endregion

		#region Flush/Finish
		public void Flush ()
		{
			OpenCLException.Check (Native.clFlush (_handle));
		}

		public void Finish ()
		{
			OpenCLException.Check (Native.clFinish (_handle));
		}
		#endregion

		#region Info
		public Context Context {
			get { return _context; }
		}

		public Device Device {
			get { return _device; }
		}

		public uint ReferenceCount {
			get { return Native.QueryInfoUInt32 (QueryType.CommandQueue, _handle, CommandQueueInfo.ReferenceCount); }
		}

		public CommandQueueProperties Properties {
			get { return (CommandQueueProperties)Native.QueryInfoInt64 (QueryType.CommandQueue, _handle, CommandQueueInfo.Properties); }
		}
		#endregion
	}
}
