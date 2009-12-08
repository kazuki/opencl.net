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
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace openCL
{
	static class Native
	{
		const string DLL = "OpenCL";

		#region 4.1 Querying Platform Info
		[DllImport (DLL)]
		public extern static int clGetPlatformIDs (
			uint num_entries,
			IntPtr[] platforms,
			out uint num_platforms
		);

		[DllImport (DLL)]
		public extern static int clGetPlatformInfo (
			IntPtr platform,
			PlatformInfo param_name,
			IntPtr param_value_size,
			byte[] param_value,
			out IntPtr param_value_size_ret
		);
		#endregion

		#region 4.2 Querying Devices
		[DllImport (DLL)]
		public extern static int clGetDeviceIDs (
			IntPtr platform,
			DeviceType device_type,
			uint num_entries,
			IntPtr[] devices,
			out uint num_devices
		);

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
		public extern static IntPtr clCreateContext (
			IntPtr[] properties,
			uint num_devices,
			IntPtr[] devices,
			IntPtr pfn_notify,
			IntPtr user_data,
			out int errcode_ret);

		[DllImport (DLL)]
		public extern static IntPtr clCreateContextFromType (
			IntPtr[] properties,
			DeviceType device_type,
			IntPtr pfn_notify,
			IntPtr user_data,
			out int errcode_ret);

		[DllImport (DLL)]
		public extern static int clRetainContext (IntPtr context);

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
		public extern static int clRetainCommandQueue (IntPtr command_queue);

		[DllImport (DLL)]
		public extern static int clReleaseCommandQueue (IntPtr command_queue);

		[DllImport (DLL)]
		public extern static int clGetCommandQueueInfo (
			IntPtr command_queue,
			CommandQueueInfo param_name,
			IntPtr param_value_size,
			byte[] param_value,
			out IntPtr param_value_size_ret
		);
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
		public extern static int clEnqueueCopyBuffer (
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
		public extern static int clEnqueueCopyBuffer (
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
		public extern static int clRetainMemObject (IntPtr memobj);

		[DllImport (DLL)]
		public extern static int clReleaseMemObject (IntPtr memobj);

		[DllImport (DLL)]
		public extern static IntPtr clEnqueueMapBuffer (
			IntPtr command_queue,
			IntPtr buffer,
			CL_Bool blocking_map,
			MapFlags map_flags,
			IntPtr offset,
			IntPtr cb,
			uint num_events_in_wait_list,
			IntPtr[] event_wait_list,
			out IntPtr event_wait,
			out int errcode_ret);

		[DllImport (DLL)]
		public extern static IntPtr clEnqueueMapBuffer (
			IntPtr command_queue,
			IntPtr buffer,
			CL_Bool blocking_map,
			MapFlags map_flags,
			IntPtr offset,
			IntPtr cb,
			uint num_events_in_wait_list,
			IntPtr[] event_wait_list,
			IntPtr event_wait,
			out int errcode_ret);

		[DllImport (DLL)]
		public extern static int clEnqueueUnmapMemObject (
			IntPtr command_queue,
			IntPtr memobj,
			IntPtr mapped_ptr,
			uint num_events_in_wait_list,
			IntPtr[] event_wait_list,
			out IntPtr event_wait);

		[DllImport (DLL)]
		public extern static int clEnqueueUnmapMemObject (
			IntPtr command_queue,
			IntPtr memobj,
			IntPtr mapped_ptr,
			uint num_events_in_wait_list,
			IntPtr[] event_wait_list,
			IntPtr event_wait);

		[DllImport (DLL)]
		public extern static int clGetMemObjectInfo (
			IntPtr memobj,
			MemInfo param_name,
			IntPtr param_value_size,
			byte[] param_value,
			out IntPtr param_value_size_ret);
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
		public extern static int clRetainProgram (IntPtr program);

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

		[DllImport (DLL)]
		public extern static int clGetProgramInfo (
			IntPtr program,
			ProgramInfo param_name,
			IntPtr param_value_size,
			byte[] param_value,
			out IntPtr param_value_size_ret
		);

		[DllImport (DLL)]
		public extern static int clGetProgramBuildInfo (
			IntPtr program,
			IntPtr device,
			ProgramBuildInfo param_name,
			IntPtr param_value_size,
			byte[] param_value,
			out IntPtr param_value_size_ret
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

		[DllImport (DLL)]
		public extern static int clGetKernelInfo (
			IntPtr kernel,
			KernelInfo param_name,
			IntPtr param_value_size,
			byte[] param_value,
			out IntPtr param_value_size_ret
		);

		[DllImport (DLL)]
		public extern static int clGetKernelWorkGroupInfo (
			IntPtr kernel,
			IntPtr device,
			KernelWorkGroupInfo param_name,
			IntPtr param_value_size,
			byte[] param_value,
			out IntPtr param_value_size_ret
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
		public extern static int clEnqueueTask (
			IntPtr command_queue,
			IntPtr kernel,
			uint num_events_in_wait_list,
			IntPtr[] event_wait_list,
			IntPtr event_wait
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
			IntPtr event_wait
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

		#region Helper Static Functions
		public static IntPtr ToIntPtr (byte[] data, int offset)
		{
			if (IntPtr.Size == 4) {
				return new IntPtr (BitConverter.ToInt32 (data, offset * 4));
			} else {
				return new IntPtr (BitConverter.ToInt64 (data, offset * 8));
			}
		}

		public static byte[] FromIntPtr (IntPtr ptr)
		{
			if (IntPtr.Size == 4) {
				return BitConverter.GetBytes (ptr.ToInt32 ());
			} else {
				return BitConverter.GetBytes (ptr.ToInt64 ());
			}
		}

		static MethodInfo GetMethodInfo (QueryType type)
		{
			string name;
			switch (type) {
				case QueryType.Platform: name = "clGetPlatformInfo"; break;
				case QueryType.Device: name = "clGetDeviceInfo"; break;
				case QueryType.Context: name = "clGetContextInfo"; break;
				case QueryType.CommandQueue: name = "clGetCommandQueueInfo"; break;
				case QueryType.Memory: name = "clGetMemObjectInfo"; break;
				case QueryType.Program: name = "clGetProgramInfo"; break;
				case QueryType.ProgramBuild: name = "clGetProgramBuildInfo"; break;
				case QueryType.Kernel: name = "clGetKernelInfo"; break;
				case QueryType.KernelWorkGroup: name = "clGetKernelWorkGroupInfo"; break;
				default: throw new ArgumentException ();
			}
			return typeof (Native).GetMethod (name, BindingFlags.Static | BindingFlags.Public);
		}

		public static void QueryInfoDirect (QueryType type, byte[] buf, params object[] args)
		{
			MethodInfo mi = GetMethodInfo (type);
			object[] args2 = new object[args.Length + 3];
			Array.Copy (args, 0, args2, 0, args.Length);
			args2[args.Length] = new IntPtr (buf.Length);
			args2[args.Length + 1] = buf;
			args2[args.Length + 2] = null;
			OpenCLException.Check ((int)mi.Invoke (null, args2));
		}

		public static byte[] QueryInfo (QueryType type, params object[] args)
		{
			MethodInfo mi = GetMethodInfo (type);
			object[] args2 = new object[args.Length + 3];
			Array.Copy (args, 0, args2, 0, args.Length);
			args2[args.Length] = IntPtr.Zero;
			args2[args.Length + 1] = null;
			args2[args.Length + 2] = null;
			OpenCLException.Check ((int)mi.Invoke (null, args2));
			IntPtr size = (IntPtr)args2[args.Length + 2];
			byte[] value = new byte[size.ToInt32 ()];
			args2[args.Length] = size;
			args2[args.Length + 1] = value;
			OpenCLException.Check ((int)mi.Invoke (null, args2));
			return value;
		}

		public static string QueryInfoString (QueryType type, params object[] args)
		{
			return QueryInfoString (type, Encoding.ASCII, args);
		}

		public static string QueryInfoString (QueryType type, Encoding encoding, params object[] args)
		{
			return encoding.GetString (QueryInfo (type, args)).TrimEnd ('\0');
		}

		public static bool QueryInfoBoolean (QueryType type, params object[] args)
		{
			uint ret = QueryInfoUInt32 (type, args);
			if (ret == 0)
				return false;
			if (ret == 1)
				return true;
			throw new Exception ();
		}

		public static uint QueryInfoUInt32 (QueryType type, params object[] args)
		{
			byte[] buf = new byte[4];
			QueryInfoDirect (type, buf, args);
			return BitConverter.ToUInt32 (buf, 0);
		}

		public static int QueryInfoInt32 (QueryType type, params object[] args)
		{
			byte[] buf = new byte[4];
			QueryInfoDirect (type, buf, args);
			return BitConverter.ToInt32 (buf, 0);
		}

		public static long QueryInfoInt64 (QueryType type, params object[] args)
		{
			byte[] buf = new byte[8];
			QueryInfoDirect (type, buf, args);
			return BitConverter.ToInt64 (buf, 0);
		}

		public static ulong QueryInfoUInt64 (QueryType type, params object[] args)
		{
			byte[] buf = new byte[8];
			QueryInfoDirect (type, buf, args);
			return BitConverter.ToUInt64 (buf, 0);
		}

		public static IntPtr QueryInfoSize (QueryType type, params object[] args)
		{
			byte[] buf = new byte[IntPtr.Size];
			QueryInfoDirect (type, buf, args);
			return ToIntPtr (buf, 0);
		}

		public static IntPtr QueryInfoIntPtr (QueryType type, params object[] args)
		{
			return QueryInfoSize (type, args);
		}

		public static IntPtr[] QueryInfoIntPtrArray (QueryType type, params object[] args)
		{
			byte[] buf = Native.QueryInfo (type, args);
			IntPtr[] array = new IntPtr[buf.Length / IntPtr.Size];
			for (int i = 0; i < array.Length; i++)
				array[i] = Native.ToIntPtr (buf, i);
			return array;
		}
		#endregion
	}

	public enum QueryType
	{
		Platform,
		Device,
		Context,
		CommandQueue,
		Memory,
		Program,
		ProgramBuild,
		Kernel,
		KernelWorkGroup,
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

	public enum PlatformInfo : uint
	{
		Profile = 0x0900,
		Version = 0x0901,
		Name = 0x0902,
		Vendor = 0x0903,
		Extensions = 0x0904
	}

	public enum DeviceInfo : uint
	{
		Type = 0x1000,
		VendorID = 0x1001,
		MaxComputeUnits = 0x1002,
		MaxWorkItemDimensions = 0x1003,
		MaxWorkGroupSize = 0x1004,
		MaxWorkItemSizes = 0x1005,
		PreferredVectorWidthChar = 0x1006,
		PreferredVectorWidthShort = 0x1007,
		PreferredVectorWidthInt = 0x1008,
		PreferredVectorWidthLong = 0x1009,
		PreferredVectorWidthFloat = 0x100A,
		PreferredVectorWidthDouble = 0x100B,
		MaxClockFrequency = 0x100C,
		AddressBits = 0x100D,
		MaxReadImageArgs = 0x100E,
		MaxWriteImageArgs = 0x100F,
		MaxMemAllocSize = 0x1010,
		Image2dMaxWidth = 0x1011,
		Image2dMaxHeight = 0x1012,
		Image3dMaxWidth = 0x1013,
		Image3dMaxHeight = 0x1014,
		Image3dMaxDepth = 0x1015,
		ImageSupport = 0x1016,
		MaxParameterSize = 0x1017,
		MaxSamplers = 0x1018,
		MemBaseAddrAlign = 0x1019,
		MinDataTypeAlignSize = 0x101A,
		SingleFpConfig = 0x101B,
		GlobalMemCacheType = 0x101C,
		GlobalMemCacheLineSize = 0x101D,
		GlobalMemCacheSize = 0x101E,
		GlobalMemSize = 0x101F,
		MaxConstantBufferSize = 0x1020,
		MaxConstantArgs = 0x1021,
		LocalMemType = 0x1022,
		LocalMemSize = 0x1023,
		ErrorCorrectionSupport = 0x1024,
		ProfilingTimerResolution = 0x1025,
		EndianLittle = 0x1026,
		Available = 0x1027,
		CompilerAvailable = 0x1028,
		ExecutionCapabilities = 0x1029,
		QueueProperties = 0x102A,
		Name = 0x102B,
		Vendor = 0x102C,
		DriverVersion = 0x102D,
		Profile = 0x102E,
		Version = 0x102F,
		Extentions = 0x1030,
		Platform = 0x1031,
	}

	[Flags]
	public enum DeviceFpConfig : long
	{
		Denorm = (1 << 0),
		Inf_NaN = (1 << 1),
		RoundToNearest = (1 << 2),
		RoundToZero = (1 << 3),
		RoundToInf = (1 << 4),
		FMA = (1 << 5),
	}

	public enum DeviceMemCacheType : uint
	{
		None = 0,
		ReadOnlyCache = 1,
		ReadWriteCache = 2
	}

	public enum DeviceLocalMemType : uint
	{
		Local = 1,
		Global = 2
	}

	[Flags]
	public enum DeviceExecCapabilities : long
	{
		Kernel = (1 << 0),
		NativeKernel = (1 << 1),
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

	public enum CommandQueueInfo : uint
	{
		Context = 0x1090,
		Device = 0x1091,
		ReferenceCount = 0x1092,
		Properties = 0x1093
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

	public enum MemObjectType : uint
	{
		Buffer = 0x10F0,
		Image2d = 0x10F1,
		Image3d = 0x10F2
	}

	public enum MemInfo : uint
	{
		Type = 0x1100,
		Flags = 0x1101,
		Size = 0x1102,
		HostPtr = 0x1103,
		MapCount = 0x1104,
		ReferenceCount = 0x1105,
		Context = 0x1106,
	}

	public enum CL_Bool : uint
	{
		False = 0,
		True = 1
	}

	[Flags]
	public enum MapFlags : long
	{
		Read = 1 << 0,
		Write = 1 << 1
	}

	public enum ProgramInfo : uint
	{
		ReferenceCount = 0x1160,
		Context = 0x1161,
		NumDevices = 0x1162,
		Devices = 0x1163,
		Source = 0x1164,
		BinarySizes = 0x1165,
		Binaries = 0x1166,
	}

	public enum ProgramBuildInfo : uint
	{
		Status = 0x1181,
		Options = 0x1182,
		Log = 0x1183,
	}

	public enum BuildStatus : int
	{
		Success = 0,
		None = -1,
		Error = -2,
		InProgress = -3,
	}

	public enum KernelInfo : uint
	{
		FunctionName = 0x1190,
		NumArgs = 0x1191,
		ReferenceCount = 0x1192,
		Context = 0x1193,
		Program = 0x1194,
	}

	public enum KernelWorkGroupInfo : uint
	{
		WorkGroupSize = 0x11B0,
		CompileWorkGroupSize = 0x11B1,
		LocalMemSize = 0x11B2,
	}
}
