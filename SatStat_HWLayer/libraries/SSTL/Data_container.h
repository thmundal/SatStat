#pragma once
#include "Subscribable.h"

namespace sstl
{
	/**
	*	Result is a class containing the name of a specific kind of data a sensor can read, as well as the actual data.
	*	The reason for this class is that one sensor might be able to measure more than one thing, so by adding multiple
	*	Result objects to that sensor would make it possible to name the different measurements, and contain the read values in a sensible way.
	*/
	template<typename T>
	class Data_container : public Subscribable
	{
	public:
		inline Data_container(const String& name)
			: m_name(name)
		{		
		}

		// Getters
		inline const String& get_name() const { return m_name; };
		inline data_types::Data<T>& get_data() { return m_data; };

		inline data_types::Types get_type() const override
		{
			return m_data.type;
		};

	private:
		String m_name;
		data_types::Data<T> m_data;
	};
}