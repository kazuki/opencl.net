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
using System.Text;

namespace openCL
{
	public class Context : HandleBase
	{
		Device[] _devices = null;

		public Context (DeviceType deviceType) : this (null, deviceType)
		{
		}

		public Context (Platform platform, DeviceType deviceType) : base (IntPtr.Zero)
		{
			if (platform == null)
				platform = Platform.GetPlatforms ()[0];
			int errcode;
			IntPtr[] properties = new IntPtr[] {
				new IntPtr ((int)ContextProperties.Platform),
				platform.Handle,
				IntPtr.Zero
			};
			_handle = Native.clCreateContextFromType (properties, deviceType, IntPtr.Zero, IntPtr.Zero, out errcode);
			OpenCLException.Check (errcode);
		}

		public Context (Device device) : this (null, new Device[] {device})
		{
		}

		public Context (Device[] devices) : this (null, devices)
		{
		}

		public Context (Platform platform, Device[] devices) : base (IntPtr.Zero)
		{
			if (platform == null)
				platform = Platform.GetPlatforms ()[0];
			int errcode;
			IntPtr[] properties = new IntPtr[] {
				new IntPtr ((int)ContextProperties.Platform),
				platform.Handle,
				IntPtr.Zero
			};
			_handle = Native.clCreateContext (properties, (uint)devices.Length,
				HandleBase.ToHandleArray<Device> (devices), IntPtr.Zero, IntPtr.Zero, out errcode);
			OpenCLException.Check (errcode);
		}

		protected override void Dispose (bool disposing)
		{
			Native.clReleaseContext (_handle);
		}

		public CommandQueue CreateCommandQueue (Device device, CommandQueueProperties properties)
		{
			int errcode;
			IntPtr command_queue = Native.clCreateCommandQueue (_handle, device.Handle, properties, out errcode);
			OpenCLException.Check (errcode);
			return new CommandQueue (this, device, command_queue);
		}

		public Memory CreateBuffer (MemoryFlags flags, int size)
		{
			int errcode;
			IntPtr membuf = Native.clCreateBuffer (_handle, flags, new IntPtr (size), IntPtr.Zero, out errcode);
			OpenCLException.Check (errcode);
			return new Memory (this, membuf);
		}

		public Memory CreateBuffer (MemoryFlags flags, int size, GCHandle hostPtr)
		{
			int errcode;
			IntPtr membuf = Native.clCreateBuffer (_handle, flags, new IntPtr (size), hostPtr.AddrOfPinnedObject (), out errcode);
			OpenCLException.Check (errcode);
			return new Memory (this, membuf);
		}

		public Program CreateProgram (string src)
		{
			byte[] raw = Encoding.UTF8.GetBytes (src);
			GCHandle handle = GCHandle.Alloc (raw, GCHandleType.Pinned);
			GCHandle handle2 = GCHandle.Alloc (handle.AddrOfPinnedObject (), GCHandleType.Pinned);
			GCHandle handle3 = GCHandle.Alloc (raw.Length, GCHandleType.Pinned);
			try {
				int errcode;
				IntPtr prog = Native.clCreateProgramWithSource (_handle, 1, handle2.AddrOfPinnedObject (),
					handle3.AddrOfPinnedObject (), out errcode);
				OpenCLException.Check (errcode);
				return new Program (this, prog);
			} finally {
				handle.Free ();
				handle2.Free ();
				handle3.Free ();
			}
		}

		public Program CreateProgram (string src, Device device, string build_options)
		{
			return CreateProgram (src, new Device[] {device}, build_options);
		}

		public Program CreateProgram (string src, Device[] devices, string build_options)
		{
			Program prog = CreateProgram (src);
			try {
				prog.Build (devices, build_options);
			} finally {
				//Console.WriteLine (prog.GetBuildLog (devices[0]));
			}
			return prog;
		}

		public uint ReferenceCount {
			get { return Native.QueryInfoUInt32 (QueryType.Context, _handle, ContextInfo.ReferenceCount); }
		}

		public Device[] Devices {
			get {
				if (_devices == null) {
					IntPtr[] ptrs = Native.QueryInfoIntPtrArray (QueryType.Context, _handle, ContextInfo.Devices);
					Device[] devices = new Device[ptrs.Length];
					for (int i = 0; i < devices.Length; i ++)
						devices[i] = Device.CreateDevice (ptrs[i]);
					_devices = devices;
				}
				return _devices;
			}
		}
	}
}
