#pragma once

namespace data_types
{
	enum class Types
	{
		t_int, t_float, t_double
	};

	template <Types T> struct Data;

	template <>
	struct Data<Types::t_int>
	{
		const Types type = Types::t_int;
		int val;
	};

	template <>
	struct Data<Types::t_float>
	{
		const Types type = Types::t_float;
		float val;
	};

	template <>
	struct Data<Types::t_double>
	{
		const Types type = Types::t_double;
		double val;
	};
}