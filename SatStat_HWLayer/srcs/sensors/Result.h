#pragma once
#include "Arduino.h"
#include "Data_types.h"

/**
*	The data namespace holds everything related to subscribable sensor data.
*/
namespace data
{	
	/**
	*	Subscribable is a sort of interface that allow us to store objects of generic type that inherits this interface in e.g. lists.
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
		virtual data_types::Types get_type() const = 0;
	};

	/**
	*	Result is a class containing the name of a specific kind of data a sensor can read, as well as the actual data.
	*	The reason for this class is that one sensor might be able to measure more than one thing, so by adding multiple
	*	Result objects to that sensor would make it possible to name the different measurements, and contain the read values in a sensible way.
	*/
	template<data_types::Types T>
	class Result : public Subscribable
	{
	public:
		inline Result(const String& name)
			: m_name(name) {};

		// Getters
		inline const String& get_name() const { return m_name; };
		inline data_types::Data<T>& get_data() { return m_data; };

		inline data_types::Types get_type() const override
		{ 
			return T; 
		};

	private:
		String m_name;
		data_types::Data<T> m_data;
	};

	// Typedefs to make life easier.
	using types = data_types::Types;

	using result_int = Result<types::t_int>*;
	using result_float = Result<types::t_float>*;
	using result_double = Result<types::t_double>*;
	
	template<typename T>
	constexpr auto& val_of(T& x) { return x->get_data().val; }
}