#pragma once
#include "Data_container.h"
#include "../LinkedList.h"

namespace sstl
{
	/**
	*	Encapsulating class in anonymous namespace to prevent
	*	multiple definitions of static members.
	*/
	namespace
	{			
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

			// Getters
			static Subscribable* get_sub(const String& key);
			static Subscribable* get_data(const String& key);
			static LinkedList<String, Subscribable*>& get_sub_list();
			static LinkedList<String, Subscribable*>& get_data_list();

			// Setter
			template<typename T>
			static bool set_data(const String& key, const T& val);

		private:
			// Maps
			static LinkedList<String, Subscribable*> sub_list;
			static LinkedList<String, Subscribable*> data_list;
		};

		LinkedList<String, Subscribable*> Lists::sub_list;
		LinkedList<String, Subscribable*> Lists::data_list;

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

		inline LinkedList<String, Subscribable*>& Lists::get_sub_list()
		{
			return sub_list;
		}

		inline LinkedList<String, Subscribable*>& Lists::get_data_list()
		{
			return data_list;
		}

		template<typename T>
		inline void Lists::add_entry(const String & key)
		{
			data_list.append(key, new Data_container<T>(key));
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
}