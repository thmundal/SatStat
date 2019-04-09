#pragma once
#include "Lists.h"

namespace sstl
{
	LinkedList<String, Subscribable*> Lists::sub_list;
	LinkedList<String, Subscribable*> Lists::data_list;

	bool Lists::subscribe(const String& key)
	{
		if (data_list[key] != nullptr)
		{
			sub_list.append(key, data_list[key]);
			return true;
		}
		return false;
	}

	void Lists::unsubscribe(const String& key)
	{
		sub_list.erase(key);
	}

	void Lists::append_data(const String& key, Subscribable* data)
	{
		data_list.append(key, data);
	}

	Subscribable* Lists::get_sub(const String& key)
	{
		return sub_list[key];
	}

	Subscribable* Lists::get_data(const String& key)
	{
		return data_list[key];
	}

	LinkedList<String, Subscribable*>& Lists::get_sub_list()
	{
		return sub_list;
	}

	LinkedList<String, Subscribable*>& Lists::get_data_list()
	{
		return data_list;
	}
}