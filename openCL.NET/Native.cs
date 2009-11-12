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
	static class Native
	{
		const string DLL = "opencl.dll";

		#region 4.1 Querying Platform Info
		#endregion

		#region 4.2 Querying Devices
		[DllImport (DLL)]
		public extern static int clGetDeviceInfo (
			IntPtr device,
			DeviceInfo param_name,
			IntPtr param_value_size,
			byte[] param_value,
			out IntPtr param_value_size_ret);
		#endregion

		#region 4.3 Contexts
		[DllImport (DLL)]
		public extern static IntPtr clCreateContextFromType (
			IntPtr[] properties,
			DeviceType device_type,
			IntPtr pfn_notify,
			IntPtr user_data,
			out int errcode_ret);

		[DllImport (DLL)]
		public extern static int clReleaseContext (IntPtr context);

		[DllImport (DLL)]
		public extern static int clGetContextInfo (
			IntPtr context,
			ContextInfo param_name,
			IntPtr param_value_size,
			byte[] param_value,
			out IntPtr param_value_size_ret);
		#endregion

		#region 5. The OpenCL Runtime
		#region 5.1 Command Queues
		[DllImport (DLL)]
		public extern static IntPtr clCreateCommandQueue (
			IntPtr context,
			IntPtr device,
			CommandQueueProperties properties,
			out int errcode_ret);

		[DllImport (DLL)]
		public extern static int clReleaseCommandQueue (IntPtr command_queue);
		#endregion

		#region 5.2 Memory Objects
		[DllImport (DLL)]
		public extern static IntPtr clCreateBuffer (
			IntPtr context,
			MemoryFlags flags,
			IntPtr size,
			IntPtr host_ptr,
			out int errcode_ret);

		[DllImport (DLL)]
		public extern static int clEnqueueReadBuffer (
			IntPtr command_queue,
			IntPtr buffer,
			CL_Bool blocking_read,
			IntPtr offset,
			IntPtr cb,
			IntPtr ptr,
			uint num_events_in_wait_list,
			IntPtr[] event_wait_list,
			out IntPtr event_wait);

		[DllImport (DLL)]
		public extern static int clEnqueueReadBuffer (
			IntPtr command_queue,
			IntPtr buffer,
			CL_Bool blocking_read,
			IntPtr offset,
			IntPtr cb,
			IntPtr ptr,
			uint num_events_in_wait_list,
			IntPtr[] event_wait_list,
			IntPtr event_wait);

		[DllImport (DLL)]
		public extern static int clEnqueueWriteBuffer (
			IntPtr command_queue,
			IntPtr buffer,
			CL_Bool blocking_write,
			IntPtr offset,
			IntPtr cb,
			IntPtr ptr,
			uint num_events_in_wait_list,
			IntPtr[] event_wait_list,
			out IntPtr event_wait);

		[DllImport (DLL)]
		public extern static int clEnqueueWriteBuffer (
			IntPtr command_queue,
			IntPtr buffer,
			CL_Bool blocking_write,
			IntPtr offset,
			IntPtr cb,
			IntPtr ptr,
			uint num_events_in_wait_list,
			IntPtr[] event_wait_list,
			IntPtr event_wait);

		[DllImport (DLL)]
		public extern static int clEnqueueWriteBuffer (
			IntPtr command_queue,
			IntPtr src_buffer,
			IntPtr dst_buffer,
			IntPtr src_offset,
			IntPtr dst_offset,
			IntPtr cb,
			uint num_events_in_wait_list,
			IntPtr[] event_wait_list,
			out IntPtr event_wait);

		[DllImport (DLL)]
		public extern static int clEnqueueWriteBuffer (
			IntPtr command_queue,
			IntPtr src_buffer,
			IntPtr dst_buffer,
			IntPtr src_offset,
			IntPtr dst_offset,
			IntPtr cb,
			uint num_events_in_wait_list,
			IntPtr[] event_wait_list,
			IntPtr event_wait);

		[DllImport (DLL)]
		public extern static int clReleaseMemObject (IntPtr memobj);
		#endregion

		#region 5.3 ...
		#endregion

		#region 5.4 Program Objects
		[DllImport (DLL)]
		public extern static IntPtr clCreateProgramWithSource (
			IntPtr context,
			uint count,
			IntPtr strings,
			IntPtr lengths,
			out int errcode_ret
		);

		[DllImport (DLL)]
		public extern static int clReleaseProgram (IntPtr program);

		[DllImport (DLL)]
		public extern static int clBuildProgram (
			IntPtr program,
			uint num_devices,
			IntPtr[] device_list,
			[MarshalAs(UnmanagedType.LPStr)]string options,
			IntPtr pfn_notify,
			IntPtr user_data
		);
		#endregion

		#region 5.5 Kernel Objects
		[DllImport (DLL)]
		public extern static IntPtr clCreateKernel (
			IntPtr program,
			[MarshalAs(UnmanagedType.LPStr)]string kernel_name,
			out int errcode_ret
		);

		[DllImport (DLL)]
		public extern static int clReleaseKernel (IntPtr kernel);

		[DllImport (DLL)]
		public extern static int clSetKernelArg (
			IntPtr kernel,
			uint arg_index,
			IntPtr arg_size,
			IntPtr arg_value
		);
		#endregion

		#region 5.6 Executing Kernels
		[DllImport (DLL)]
		public extern static int clEnqueueTask (
			IntPtr command_queue,
			IntPtr kernel,
			uint num_events_in_wait_list,
			IntPtr[] event_wait_list,
			out IntPtr event_wait
		);

		[DllImport (DLL)]
		public extern static int clEnqueueNDRangeKernel (
			IntPtr command_queue,
			IntPtr kernel,
			uint work_dim,
			IntPtr[] global_work_offset,
			IntPtr[] global_work_size,
			IntPtr[] local_work_size,
			uint num_events_in_wait_list,
			IntPtr[] event_wait_list,
			out IntPtr event_wait
		);
		#endregion

		#region 5.7 Event Object
		[DllImport (DLL)]
		public extern static int clWaitForEvents (
			uint num_events,
			IntPtr[] event_list
		);

		[DllImport (DLL)]
		public extern static int clReleaseEvent (IntPtr event_handle);
		#endregion
		#endregion

		public static IntPtr ToIntPtr (byte[] data, int offset)
		{
			if (IntPtr.Size == 4) {
				return new IntPtr (BitConverter.ToInt32 (data, offset * 4));
			} else {
				return new IntPtr (BitConverter.ToInt64 (data, offset * 8));
			}
		}
	}

	[Flags]
	public enum DeviceType : long
	{
		Default = (1 << 0),
		CPU = (1 << 1),
		GPU = (1 << 2),
		Accelerator = (1 << 3),
		All = 0xFFFFFFFF,
	}

	public enum DeviceInfo : uint
	{
		Type = 0x1000,
		VendorID = 0x1001,
		MaxComputeUnits = 0x1002,
		MaxWorkItemDimensions = 0x1003,
		MaxWorkGroupSize = 0x1004,
		MaxWorkItemSizes = 0x1005,
		PREFERRED_VECTOR_WIDTH_CHAR = 0x1006,
		PREFERRED_VECTOR_WIDTH_SHORT = 0x1007,
		PREFERRED_VECTOR_WIDTH_INT = 0x1008,
		PREFERRED_VECTOR_WIDTH_LONG = 0x1009,
		PREFERRED_VECTOR_WIDTH_FLOAT = 0x100A,
		PREFERRED_VECTOR_WIDTH_DOUBLE = 0x100B,
		MAX_CLOCK_FREQUENCY = 0x100C,
		ADDRESS_BITS = 0x100D,
		MAX_READ_IMAGE_ARGS = 0x100E,
		MAX_WRITE_IMAGE_ARGS = 0x100F,
		MAX_MEM_ALLOC_SIZE = 0x1010,
		IMAGE2D_MAX_WIDTH = 0x1011,
		IMAGE2D_MAX_HEIGHT = 0x1012,
		IMAGE3D_MAX_WIDTH = 0x1013,
		IMAGE3D_MAX_HEIGHT = 0x1014,
		IMAGE3D_MAX_DEPTH = 0x1015,
		IMAGE_SUPPORT = 0x1016,
		MAX_PARAMETER_SIZE = 0x1017,
		MAX_SAMPLERS = 0x1018,
		MEM_BASE_ADDR_ALIGN = 0x1019,
		MIN_DATA_TYPE_ALIGN_SIZE = 0x101A,
		SINGLE_FP_CONFIG = 0x101B,
		GLOBAL_MEM_CACHE_TYPE = 0x101C,
		GLOBAL_MEM_CACHELINE_SIZE = 0x101D,
		GLOBAL_MEM_CACHE_SIZE = 0x101E,
		GLOBAL_MEM_SIZE = 0x101F,
		MAX_CONSTANT_BUFFER_SIZE = 0x1020,
		MAX_CONSTANT_ARGS = 0x1021,
		LOCAL_MEM_TYPE = 0x1022,
		LOCAL_MEM_SIZE = 0x1023,
		ERROR_CORRECTION_SUPPORT = 0x1024,
		PROFILING_TIMER_RESOLUTION = 0x1025,
		ENDIAN_LITTLE = 0x1026,
		AVAILABLE = 0x1027,
		COMPILER_AVAILABLE = 0x1028,
		EXECUTION_CAPABILITIES = 0x1029,
		QUEUE_PROPERTIES = 0x102A,
		Name = 0x102B,
		VENDOR = 0x102C,
		DriverVersion = 0x102D,
		PROFILE = 0x102E,
		Version = 0x102F,
		EXTENSIONS = 0x1030,
		PLATFORM = 0x1031,
	}

	public enum ContextInfo : uint
	{
		ReferenceCount = 0x1080,
		Devices = 0x1081,
		Properties = 0x1082,
	}

	[Flags]
	public enum CommandQueueProperties : long
	{
		Default = 0,
		OutOfOrderExecMode = (1 << 0),
		Profiling = (1 << 1),
	}

	[Flags]
	public enum MemoryFlags : long
	{
		ReadWrite = (1 << 0),
		WriteOnly = (1 << 1),
		ReadOnly = (1 << 2),
		UseHostPtr = (1 << 3),
		AllocHostPtr = (1 << 4),
		CopyHostPtr = (1 << 5),
	}

	public enum CL_Bool : uint
	{
		False = 0,
		True = 1
	}
}
