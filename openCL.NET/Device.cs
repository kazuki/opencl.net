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
using System.Text;

namespace openCL
{
	public class Device
	{
		IntPtr _handle;
		Dictionary<DeviceInfo, object> _devInfoCache = new Dictionary<DeviceInfo,object> ();

		internal Device (IntPtr handle)
		{
			_handle = handle;
		}

		int GetContextInfo (DeviceInfo paramName, out byte[] value, out int size)
		{
			IntPtr size_ret;
			int state = Native.clGetDeviceInfo (_handle, paramName, IntPtr.Zero, null, out size_ret);
			if (state != 0) {
				size = 0;
				value = null;
				return state;
			}
			size = size_ret.ToInt32 ();
			value = new byte[size];
			return Native.clGetDeviceInfo (_handle, paramName, size_ret, value, out size_ret);
		}

		internal IntPtr Handle {
			get { return _handle; }
		}

		public string Name {
			get {
				if (_devInfoCache.ContainsKey (DeviceInfo.Name))
					return (string)_devInfoCache[DeviceInfo.Name];
				byte[] value;
				int size;
				OpenCLException.Check (GetContextInfo (DeviceInfo.Name, out value, out size));
				string str = Encoding.ASCII.GetString (value, 0, size);
				_devInfoCache[DeviceInfo.Name] = str;
				return str;
			}
		}
	}
}
