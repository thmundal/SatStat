#pragma once
#include "Arduino.h"

namespace sstl
{
	namespace data_types
	{
		/**
		*	Enumerator for the different data types.
		*/
		enum Types
		{
			t_int, t_float, t_double
		};

		/**
		*	Declaration of the generic Data struct.
		*	The different definitions of this struct holds the data type as well as the value.
		*/
		template <typename T> struct Data;

		/**
		*	Data struct for integers.
		*/
		template <>
		struct Data<int>
		{
			const Types type = Types::t_int;
			int val;
		};

		/**
		*	Data struct for floats.
		*/
		template <>
		struct Data<float>
		{
			const Types type = Types::t_float;
			float val;
		};

		/**
		*	Data struct for doubles.
		*/
		template <>
		struct Data<double>
		{
			const Types type = Types::t_double;
			double val;
		};
	}
}