#pragma once
#include "Data_types.h"

namespace sstl
{
	/**
	*	Subscribable is a sort of interface that allow us to store objects of generic type that inherits/extends this interface in e.g. lists.
	*/
	class Subscribable
	{
	public:
		/**
		*	As other classes will inherit this one, we need a virtual destructor to let the compiler know that
		*	a polymorphic object instantiated through a Subscribable type might have it's own destructor that has to be called upon deletion.
		*/
		virtual ~Subscribable() {};

		/**
		*	Returns the type of the polymorphed object to be able to downcast to the correct type when needed.
		*/
		virtual const data_types::Types& get_type() const = 0;

		/**
		*	Returns the name of the polymorphed object.
		*/
		virtual const String& get_name() const = 0;
	};
}