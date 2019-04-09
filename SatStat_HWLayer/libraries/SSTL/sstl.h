#pragma once
#include "Lists.h"

namespace sstl
{
	using types = data_types::Types;

	template<typename T>
	Data_container<T>* downcast(Subscribable* src)
	{
		return (Data_container<T>*)src;
	}
}
