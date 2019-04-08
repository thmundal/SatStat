#pragma once
#include "Data_container.h"
#include "../LinkedList.h"

namespace sstl
{	
	class Lists
	{
	public:
		using map = LinkedList<String, Subscribable*>;

		// Pure virtual destructor to make the class abstract.
		virtual ~Lists() = 0;

		// Subscription
		static bool subscribe(const String& key);
		static void unsubscribe(const String& key);

		// Add
		static void append_data(const String& key, Subscribable* data);

		// Getters
		static Subscribable* get_sub(const String& key);
		static Subscribable* get_data(const String& key);
		static map& get_sub_list();
		static map& get_data_list();

		// Setter
		template<typename T>
		static bool set_data(const String& key, const T& val);

	private:
		// Maps
		static map sub_list;
		static map data_list;
	};

	Lists::map Lists::sub_list;
	Lists::map Lists::data_list;

	inline bool Lists::subscribe(const String& key)
	{
		if (data_list[key] != nullptr)
		{
			sub_list.append(key, data_list[key]);
			return true;
		}		
		return false;
	}

	inline void Lists::unsubscribe(const String& key)
	{
		sub_list.erase(key);
	}

	inline void Lists::append_data(const String& key, Subscribable* data)
	{
		data_list.append(key, data);
	}

	inline Subscribable* Lists::get_sub(const String& key)
	{
		return sub_list[key];
	}

	inline Subscribable* Lists::get_data(const String& key)
	{
		return data_list[key];
	}

	inline Lists::map& Lists::get_sub_list()
	{
		return sub_list;
	}

	inline Lists::map & Lists::get_data_list()
	{
		return data_list;
	}

	template<typename T>
	inline bool Lists::set_data(const String& key, const T& val)
	{
		Subscribable* sub = sub_list[key];

		if (sub != nullptr)
		{
			Data_container<T>* dc = (Data_container<T>*)sub;
			auto& data = dc->get_data();
			data.val = val;

			return true;
		}

		return false;
	}
}