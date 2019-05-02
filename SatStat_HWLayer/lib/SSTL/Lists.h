#pragma once
#include "Data_container.h"
#include "../LinkedList/LinkedList.h"

namespace sstl
{
	/**
	*	This class holds two lists, both containing pointers to a Subscribable object.
	*	Upon creation of a specific sensor object, the data it's able to read is added to the data_list.
	*	When data is subscribed to, it get's copied from the data_list into the sub_list.
	*	When unsubscribing to data, it get's erased from the sub_list, but remains in the data_list to allow resubscription.
	*/			
	class Lists
	{
	public:
		/**
		*	Pure virtual destructor to make the class abstract.
		*/
		virtual ~Lists() = 0;

		// Subscription
		static bool subscribe(const String& key);
		static void unsubscribe(const String& key);

		// Add
		template<typename T> static void add_entry(const String& key);
		static void append_data(const String& key, Subscribable* data);

		// Setter
		template<typename T>
		static bool set_data(const String& key, const T& val);

		// Getters
		static Subscribable* get_sub(const String& key);
		static Subscribable* get_data(const String& key);
		static LinkedList<String, Subscribable*>& get_sub_list();
		static LinkedList<String, Subscribable*>& get_data_list();

	private:
		// Maps
		static LinkedList<String, Subscribable*> sub_list;
		static LinkedList<String, Subscribable*> data_list;
	};

	/**
	*	Adds an empty Data_container of the given type to the data_list with the given key.
	*/
	template<typename T>
	inline void Lists::add_entry(const String & key)
	{
		data_list.append(key, new Data_container<T>(key));
	}

	/**
	*	Sets the value of an existing entry in the list to the given value of generic type.
	*/
	template<typename T>
	inline bool Lists::set_data(const String& key, const T& val)
	{		
		Subscribable* dl = data_list[key];

		if (dl != nullptr)
		{
			Data_container<T>* dc = (Data_container<T>*)dl;
			auto& data = dc->get_data();
			data = val;

			return true;
		}

		return false;
	}
	
}