using System;
using System.Collections.Generic;
using System.Text;

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
