#pragma once
#include "Arduino.h"

namespace sstl
{
	namespace data_types
	{
		enum Types
		{
			t_int, t_float, t_double
		};

		template <typename T> struct Data;

		template <>
		struct Data<int>
		{
			const Types type = Types::t_int;
			int val;
		};

		template <>
		struct Data<float>
		{
			const Types type = Types::t_float;
			float val;
		};

		template <>
		struct Data<double>
		{
			const Types type = Types::t_double;
			double val;
		};
	}
}