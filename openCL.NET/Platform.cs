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
	public class Platform
	{
		IntPtr _handle;

		internal Platform (IntPtr handle)
		{
			_handle = handle;
		}

		public static Platform[] GetPlatforms ()
		{
			uint num_platforms;
			OpenCLException.Check (Native.clGetPlatformIDs (0, null, out num_platforms));
			IntPtr[] ids = new IntPtr[num_platforms];
			OpenCLException.Check (Native.clGetPlatformIDs (num_platforms, ids, out num_platforms));
			Platform[] platforms = new Platform[ids.Length];
			for (int i = 0; i < platforms.Length; i ++)
				platforms[i] = new Platform (ids[i]);
			return platforms;
		}

		public Device[] GetDevices (DeviceType type)
		{
			uint num_devices;
			OpenCLException.Check (Native.clGetDeviceIDs (_handle, type, 0, null, out num_devices));
			IntPtr[] ids = new IntPtr[num_devices];
			OpenCLException.Check (Native.clGetDeviceIDs (_handle, type, num_devices, ids, out num_devices));
			Device[] devices = new Device[ids.Length];
			for (int i = 0; i < devices.Length; i++)
				devices[i] = new Device (ids[i]);
			return devices;
		}

		public string Profile {
			get { return Native.QueryInfoString (QueryType.Platform, _handle, PlatformInfo.Profile); }
		}

		public string Version {
			get { return Native.QueryInfoString (QueryType.Platform, _handle, PlatformInfo.Version); }
		}

		public string Name {
			get { return Native.QueryInfoString (QueryType.Platform, _handle, PlatformInfo.Name); }
		}

		public string Vendor {
			get { return Native.QueryInfoString (QueryType.Platform, _handle, PlatformInfo.Vendor); }
		}

		public string Extensions {
			get { return Native.QueryInfoString (QueryType.Platform, _handle, PlatformInfo.Extensions); }
		}
	}
}
