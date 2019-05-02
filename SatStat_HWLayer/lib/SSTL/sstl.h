#pragma once
#include "Lists.h"

namespace sstl
{
	using types = data_types::Types;

	/**
	*	Downcasts the given Subscribable object to a Data_container object of the given generic type.
	*/
	template<typename T>
	Data_container<T>* downcast(Subscribable* src)
	{
		return (Data_container<T>*)src;
	}
}
