#pragma once
#include "Func_ptr.h"
#include "../../lib/LinkedList/LinkedList.h"
#include "Request_functions.h"

/**
*	Class containing two lists of available requests.
*	The reason for having two lists is that some requests takes parameters, and some don't.
*	By having two lists, it's possible to append both kinds of requests without knowing if it takes parameters or not.
*	It also makes it possible to retreive them from the correct list by checking if it exists at all,
*	and whether or not it is in the list with requests that has parameters.
*/
class Request_container
{
public:	
	// Constructor
	Request_container();

	// Checks
	bool exists(const String& key);
	bool has_params(const String& key);

	// Getters
	Func_ptr<void>& get_no_param(const String& key);
	Func_ptr<void, Json_container<JsonObject>&>& get_with_param(const String& key);

private:
	// Append methods
	void append(const String& key, const Func_ptr<void>& func);
	void append(const String& key, const Func_ptr<void, Json_container<JsonObject>&>& func);

	LinkedList<String, Func_ptr<void>> no_param;
	LinkedList<String, Func_ptr<void, Json_container<JsonObject>&>> with_param;
};

