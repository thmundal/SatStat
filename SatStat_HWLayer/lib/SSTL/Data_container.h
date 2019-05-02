#pragma once
#include "Subscribable.h"

namespace sstl
{
	/**
	*	Data_container is a generic class extending the Subscribable class.
	*	It contains the name of a specific kind of data a sensor can read, as well as the actual data.
	*/
	template<typename T>
	class Data_container : public Subscribable
	{
	public:
		/**
		*	Constructor. Setting m_name.
		*/
		inline Data_container(const String& name)
			: m_name(name) {}

		/**
		*	Returns the value of the m_data object.
		*/
		inline T& get_data() { return m_data.val; };

		/**
		*	Override of Subscribable's get_type method.
		*	Returns the type of the m_data object.
		*/
		inline const data_types::Types& get_type() const override
		{
			return m_data.type;
		};

		/**
		*	Override of Subscribable's get_name method.
		*	Returns the data identifier.
		*/
		inline const String& get_name() const override
		{
			return m_name;
		}

		/**
		*	Sets the value of m_data.
		*/
		inline void set_data(const T& val) { m_data.val = val; };

	private:
		String m_name;
		data_types::Data<T> m_data;
	};
}