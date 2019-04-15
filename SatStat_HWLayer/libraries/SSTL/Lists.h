#pragma once
#include "Data_container.h"
#include "../LinkedList/LinkedList.h"

namespace sstl
{
	/**
	*	Encapsulating class in anonymous namespace to prevent
	*	multiple definitions of static members.
	*/			
	class Lists
	{
	public:
		// Pure virtual destructor to make the class abstract.
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

	template<typename T>
	inline void Lists::add_entry(const String & key)
	{
		data_list.append(key, new Data_container<T>(key));
	}

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