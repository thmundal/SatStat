#pragma once
#include "Lists.h"

namespace sstl
{
	// Declaration of static members.
	LinkedList<String, Subscribable*> Lists::sub_list;
	LinkedList<String, Subscribable*> Lists::data_list;

	/**
	*	Copies an object from the data_list to the sub_list.
	*/
	bool Lists::subscribe(const String& key)
	{
		if (data_list[key] != nullptr)
		{
			sub_list.append(key, data_list[key]);
			return true;
		}
		return false;
	}

	/**
	*	Erases an object from the sub_list.
	*	The object is still present in the data_list.
	*/
	void Lists::unsubscribe(const String& key)
	{		
		sub_list.erase(key);
	}

	/**
	*	Appends an object to the data_list.
	*/
	void Lists::append_data(const String& key, Subscribable* data)
	{
		data_list.append(key, data);
	}

	/**
	*	Returns a specific object from the sub_list.
	*/
	Subscribable* Lists::get_sub(const String& key)
	{
		return sub_list[key];
	}

	/**
	*	Returns a specific object from the data_list.
	*/
	Subscribable* Lists::get_data(const String& key)
	{
		return data_list[key];
	}

	/**
	*	Returns the entire sub_list.
	*/
	LinkedList<String, Subscribable*>& Lists::get_sub_list()
	{
		return sub_list;
	}

	/**
	*	Returns the entire data_list.
	*/
	LinkedList<String, Subscribable*>& Lists::get_data_list()
	{
		return data_list;
	}
}